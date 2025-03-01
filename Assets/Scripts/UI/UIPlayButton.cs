using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Debug UI Class to help test scene transitions. Used to load the level scene
/// </summary>
public class UIPlayButton : MonoBehaviour
{
    // load the level scene
    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
}
