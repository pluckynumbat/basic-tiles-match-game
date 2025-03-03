using UnityEngine;

/// <summary>
/// UI element present in the main scene which lets the player select random mode
/// </summary>
public class UIRandomModeButton : MonoBehaviour
{
    public void OnRandomModeButtonClicked()
    {
        UIEvents.RaiseRandomModeSelectedEvent();
    }
}
