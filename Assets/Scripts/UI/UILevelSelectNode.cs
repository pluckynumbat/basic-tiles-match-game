using UnityEngine;

/// <summary>
/// A UI element on the main scene that the player presses to select a level they want to play
/// </summary>
public class UILevelSelectNode : MonoBehaviour
{
    public string levelToPlay;
    
    // raise the level selected event
    public void OnPlayButtonClicked()
    {
        UIEvents.RaiseLevelSelectedEvent(levelToPlay);
    }
}
