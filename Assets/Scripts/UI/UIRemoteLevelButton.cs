using UnityEngine;

/// <summary>
/// UI element present in the main scene which lets the player play a remote level
/// </summary>
public class UIRemoteLevelButton : MonoBehaviour
{
    public void OnRemoteLevelButtonClicked()
    {
        UIEvents.RaiseRemoteLevelSelectedEvent();
    }
}
