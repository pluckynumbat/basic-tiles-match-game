using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Debug UI Class to help test scene transitions. Used to load the level scene
/// </summary>
public class UIPlayButton : MonoBehaviour
{
    public string levelName;
    
    // raise the level selected event
    public void OnPlayButtonClicked()
    {
        UIEvents.RaiseLevelSelectedEvent(levelName);
    }
}
