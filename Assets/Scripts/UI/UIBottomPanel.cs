using UnityEngine;

/// <summary>
/// A UI Panel that shows at the bottom of a level main scene, providing
/// a quick way to Quit a level / Restart a level / Toggle Audio mute
/// in the main scene, this panel only has the mute button
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
        UIEvents.RaiseToggleMuteRequestEvent();
    }
}
