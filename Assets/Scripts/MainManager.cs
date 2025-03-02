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
        
        UIEvents.LeaveLevelRequestEvent -= OnLeaveLevelRequest;
        UIEvents.LeaveLevelRequestEvent += OnLeaveLevelRequest;
    }

    private void OnDestroy()
    {
        UIEvents.LevelSelectedEvent -= OnLevelSelected;
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
        
        // let other systems in the main scene know that level data is loaded
        UIEvents.RaiseLevelDataLoadedEvent(levelToPlay);
    }
    
    // the player pressed 'Play' on the level preview dialog, switch scenes to level scene for this level
    // (level data is already in levelToPlay at this point)
    private void OnPlayLevelRequest()
    {
        SceneManager.LoadScene(LEVEL_SCENE_ID);
    }
    
    // player wants to leave the level scene
    private void OnLeaveLevelRequest()
    {
        SceneManager.LoadScene(MAIN_SCENE_ID);
    }
}