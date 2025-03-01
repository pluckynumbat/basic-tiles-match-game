using UnityEngine;

/// <summary>
/// Base class for all UI Dialogs to derive
/// </summary>
public abstract class UIDialogBase : MonoBehaviour
{
    public abstract void Setup(object data); // send data to the dialog via this method

    // let other systems know that the dialog was destroyed
    private void OnDestroy()
    {
        UIEvents.RaiseDialogDismissedEvent(this);
    }
}
