using UnityEngine;

/// <summary>
/// The main manager gets loaded in the main scene and will persist throughout the session.
/// It is a singleton class, and it can provide data that needs to persist across scenes
/// like level to be loaded etc.
/// </summary>
public class MainManager : MonoBehaviour
{
    //Singleton
    private static MainManager instance;
    public static MainManager Instance
    {
        get
        {
            return instance;
        }
    }

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
    }
}