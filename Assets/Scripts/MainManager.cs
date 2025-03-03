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
    }

    private void OnDestroy()
    {
        UIEvents.LevelSelectedEvent -= OnLevelSelected;
        UIEvents.PlayLevelRequestEvent -= OnPlayLevelRequest;
        GameEvents.LevelDataRequestEvent -= OnLevelDataRequest;
        GameEvents.LevelEndedEvent -= OnLevelEnded;
        UIEvents.LeaveLevelRequestEvent -= OnLeaveLevelRequest;
        UIEvents.RestartLevelRequestEvent -= OnRestartLevelRequest;
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
        
        //TODO: validate the properties inside level data if possible before using them / broadcasting it
        
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
        //TODO: validate the properties inside level data if possible before using them / broadcasting it
        GameEvents.RaiseLevelDataReadyEvent(levelToPlay);
    }
    
    // request the UI dialog spawner to spawn the level end dialog, and supply it with required parameters
    private void OnLevelEnded(bool won)
    {
        UIEvents.RaiseDialogDisplayRequestEvent(LEVEL_END_DIALOG_NAME, new object[] {won, isRandomModeEnabled});
    }
    
    // player wants to leave the level scene
    private void OnLeaveLevelRequest()
    {
        SceneManager.LoadScene(MAIN_SCENE_ID);
    }
    
    // player wants to restart their level
    private void OnRestartLevelRequest()
    {
        SceneManager.LoadScene(LEVEL_SCENE_ID);
    }
}