using UnityEngine;

/// <summary>
/// this script is to control the dynamic elements of the level node container like feature specific elements
/// </summary>
public class UILevelNodeContainer : MonoBehaviour
{
    public GameObject[] RemoteLevelGameObjects;
   
    private void Awake()
    {   
        NetworkEvents.RemoteLevelsStatusUpdateEvent -= OnRemoteLevelsStatusUpdate;
        NetworkEvents.RemoteLevelsStatusUpdateEvent += OnRemoteLevelsStatusUpdate;
    }
    void Start()
    {
        if (RemoteLevelGameObjects == null)
        {
            return;
        }
        
        foreach (GameObject go in RemoteLevelGameObjects)
        {
            if (go == null)
            {
                continue;
            }
            
            go.SetActive(false);
        }
    }
    
    private void OnDestroy()
    {
        NetworkEvents.RemoteLevelsStatusUpdateEvent -= OnRemoteLevelsStatusUpdate;
    }

    private void OnRemoteLevelsStatusUpdate(bool enable)
    {
        if (RemoteLevelGameObjects == null)
        {
            return;
        }

        foreach (GameObject go in RemoteLevelGameObjects)
        {
            if (go == null)
            {
                continue;
            }
            
            go.SetActive(enable);
        }
    }
}
