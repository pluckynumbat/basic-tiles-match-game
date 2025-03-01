using UnityEngine;

/// <summary>
/// this UI class will load and spawn the required dialog on the screen
/// (only 1 dialog can be on the screen at a time)
/// </summary>
public class UIDialogSpawner : MonoBehaviour
{
    private const string UI_DIALOGS_DIRECTORY = "UIDialogs/";
    
    private bool isDialogDisplayed = false;
    
    // load the required prefab from resources
    // get the UIDialogBase component attached to it
    // return a reference to that component
    private UIDialogBase SpawnDialog(string dialogName)
    {
        if (isDialogDisplayed)
        {
            Debug.LogError($"Can only display 1 dialog at a time, please check call to this function for spawning dialog with name {dialogName}");
            return null;
        }

        UIDialogBase dialog = null;
        Object obj = Instantiate(Resources.Load(UI_DIALOGS_DIRECTORY + dialogName, typeof(GameObject)), transform);
        GameObject go = obj as GameObject;
        if (go != null)
        {
            dialog = go.GetComponent<UIDialogBase>();
        }
        
        isDialogDisplayed = true;
        
        //also let other systems know that a dialog is being displayed
        UIEvents.RaiseDialogDisplayedEvent(dialog);
        
        return dialog;
    }

    // set the displayed flag to false
    private void OnDialogDismissed(UIDialogBase dialog)
    {
        isDialogDisplayed = false;
    }
}