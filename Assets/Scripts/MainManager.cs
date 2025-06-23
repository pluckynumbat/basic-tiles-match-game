using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The main manager gets loaded in the main scene and will persist throughout the session.
/// It is a singleton class, and it can provide data that needs to persist across scenes
/// like level to be loaded etc.
/// </summary>
public class MainManager : MonoBehaviour
{
    private const int MAIN_SCENE_ID = 0;
    private const int LEVEL_SCENE_ID = 1;
    
    private const string LEVEL_END_DIALOG_NAME = "LevelEndDialog";
    private const string LEVEL_PREVIEW_DIALOG_NAME = "LevelPreviewDialog";
    private const string ENTER_RANDOM_MODE_DIALOG_NAME = "EnterRandomModeDialog";
    
    
    //Singleton
    private static MainManager instance;
    public static MainManager Instance
    {
        get
        {
            return instance;
        }
    }
    
    // store the next level the player will play
    public LevelData levelToPlay;
    
    // store the last level that the game received from the server
    public LevelData levelFromServer;
    
    // check this when the player is in random mode
    public bool isRandomModeEnabled = false;
    public Random.State randomState; // needed in case player wants to restart a level in random mode
    
    //global volume setting
    public float lastGlobalVolume;

    // setting to enable / disable remote levels
    public bool enableRemoteLevels = false;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        UIEvents.LevelSelectedEvent -= OnLevelSelected;
        UIEvents.LevelSelectedEvent += OnLevelSelected;
        
        UIEvents.PlayLevelRequestEvent -= OnPlayLevelRequest;
        UIEvents.PlayLevelRequestEvent += OnPlayLevelRequest;
        
        GameEvents.LevelDataRequestEvent -= OnLevelDataRequest;
        GameEvents.LevelDataRequestEvent += OnLevelDataRequest;
        
        GameEvents.LevelEndedEvent -= OnLevelEnded;
        GameEvents.LevelEndedEvent += OnLevelEnded;
        
        UIEvents.LeaveLevelRequestEvent -= OnLeaveLevelRequest;
        UIEvents.LeaveLevelRequestEvent += OnLeaveLevelRequest;
        
        UIEvents.RestartLevelRequestEvent -= OnRestartLevelRequest;
        UIEvents.RestartLevelRequestEvent += OnRestartLevelRequest;
        
        UIEvents.RandomModeSelectedEvent -= OnRandomModeSelected;
        UIEvents.RandomModeSelectedEvent += OnRandomModeSelected;
        
        UIEvents.PlayRandomModeRequestEvent -= OnPlayRandomModeRequest;
        UIEvents.PlayRandomModeRequestEvent += OnPlayRandomModeRequest;
        
        UIEvents.ToggleMuteRequestEvent -= OnToggleMuteRequested;
        UIEvents.ToggleMuteRequestEvent += OnToggleMuteRequested;
 
        NetworkEvents.LevelReceivedFromServerEvent -= OnLevelReceivedFromServer;
        NetworkEvents.LevelReceivedFromServerEvent += OnLevelReceivedFromServer;
        
        UIEvents.RemoteLevelSelectedEvent -= OnRemoteLevelSelected;
        UIEvents.RemoteLevelSelectedEvent += OnRemoteLevelSelected;
    }

    private void OnDestroy()
    {
        UIEvents.LevelSelectedEvent -= OnLevelSelected;
        UIEvents.PlayLevelRequestEvent -= OnPlayLevelRequest;
        GameEvents.LevelDataRequestEvent -= OnLevelDataRequest;
        GameEvents.LevelEndedEvent -= OnLevelEnded;
        UIEvents.LeaveLevelRequestEvent -= OnLeaveLevelRequest;
        UIEvents.RestartLevelRequestEvent -= OnRestartLevelRequest;
        UIEvents.RandomModeSelectedEvent -= OnRandomModeSelected;
        UIEvents.PlayRandomModeRequestEvent -= OnPlayRandomModeRequest;
        UIEvents.ToggleMuteRequestEvent -= OnToggleMuteRequested;
        NetworkEvents.LevelReceivedFromServerEvent -= OnLevelReceivedFromServer;
        UIEvents.RemoteLevelSelectedEvent -= OnRemoteLevelSelected;
    }

    // a level select node was pressed in the main scene
    // load the level by level name and cache it as the probable level to play
    private void OnLevelSelected(string levelName)
    {
        levelToPlay = LevelJSONReader.ReadJSON(levelName);
        if (levelToPlay == null)
        {
            Debug.LogError($"Invalid level data for level name: {levelName}, abort");
            return;
        }
        
        //request display of the level preview dialog
        UIEvents.RaiseDialogDisplayRequestEvent(LEVEL_PREVIEW_DIALOG_NAME, new object[] {levelToPlay});
    }
    
    // the player pressed 'Play' on the level preview dialog, switch scenes to level scene for this level
    // (level data is already in levelToPlay at this point)
    private void OnPlayLevelRequest()
    {
        SceneManager.LoadScene(LEVEL_SCENE_ID);
    }
    
    // level scene has been loaded and the managers in it need the level data
    private void OnLevelDataRequest()
    {
        GameEvents.RaiseLevelDataReadyEvent(levelToPlay);
    }
    
    // request the UI dialog spawner to spawn the level end dialog, and supply it with required parameters
    private void OnLevelEnded(bool won)
    {
        UIEvents.RaiseDialogDisplayRequestEvent(LEVEL_END_DIALOG_NAME, new object[] {won, levelToPlay, isRandomModeEnabled });
    }
    
    // player wants to leave the level scene
    private void OnLeaveLevelRequest()
    {
        if (isRandomModeEnabled)
        {
            isRandomModeEnabled = false;
        }
        SceneManager.LoadScene(MAIN_SCENE_ID);
    }
    
    // player wants to restart their level
    // (if in random mode, load the saved random state, so that the level re-starts with the same random data)
    private void OnRestartLevelRequest()
    {
        if (isRandomModeEnabled)
        {
            Random.state = randomState;
        }
        SceneManager.LoadScene(LEVEL_SCENE_ID);
    }
    
    // player presssed the random mode button, request the Enter Random Mode Dialog to be shown
    private void OnRandomModeSelected()
    {
        UIEvents.RaiseDialogDisplayRequestEvent(ENTER_RANDOM_MODE_DIALOG_NAME, null);
    }
    
    
    // player wants to enter a random mode level:
    // generate a new level, set random mode flag, save random state, and load the level scene
    private void OnPlayRandomModeRequest()
    {
        levelToPlay = RandomLevelGenerator.GenerateRandomLevel();
        isRandomModeEnabled = true;
        randomState = Random.state;
        
        SceneManager.LoadScene(LEVEL_SCENE_ID);
    }

    // if muting, store volume, then mute
    // else, reset volume to what it was before muting
    private void OnToggleMuteRequested()
    {
        if (AudioListener.volume > 0)
        {
            lastGlobalVolume = AudioListener.volume;
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = lastGlobalVolume;
        }
    }

    private void OnLevelReceivedFromServer(string levelJSONString)
    {
        levelFromServer = LevelJSONReader.CreateLevelDataFromJSONString(levelJSONString);
        if (levelFromServer == null)
        {
            Debug.LogError($"Invalid level data for level received from server, abort");
            return;
        }
    }
    
}