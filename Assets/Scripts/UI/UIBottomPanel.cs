using UnityEngine;

/// <summary>
/// A UI Panel that shows at the bottom of a level, providing
/// a quick way to Quit the level / Restart the level / (TODO:Toggle Audio mute)
/// </summary>
public class UIBottomPanel : MonoBehaviour
{
    // go to the main scene
    public void OnQuitButtonClicked()
    {
        UIEvents.RaiseLeaveLevelRequestEvent();
    }
    
    //reload the level scene
    public void OnRestartButtonClicked()
    {
        UIEvents.RaiseRestartLevelRequestEvent();
    }
    
    //toggle audio mute
    public void OnMuteButtonClicked()
    {
        //TODO: add functionality to toggle audio
    }
}
