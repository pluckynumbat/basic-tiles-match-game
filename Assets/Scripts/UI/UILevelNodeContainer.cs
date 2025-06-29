using UnityEngine;

/// <summary>
/// this script is to control the dynamic elements of the level node container like feature specific elements
/// </summary>
public class UILevelNodeContainer : MonoBehaviour
{
    public GameObject[] RemoteLevelGameObjects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (RemoteLevelGameObjects == null)
        {
            return;
        }

        bool enabled = MainManager.Instance.enableRemoteLevels;

        foreach (GameObject go in RemoteLevelGameObjects)
        {
            go.SetActive(enabled);
        }
    }
}
