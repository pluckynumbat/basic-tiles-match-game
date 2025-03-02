using UnityEngine;

/// <summary>
/// this UI class will load and spawn the required dialog on the screen
/// (only 1 dialog can be on the screen at a time)
/// </summary>
public class UIDialogSpawner : MonoBehaviour
{
    private const string UI_DIALOGS_DIRECTORY = "UIDialogs/";
    private const string LEVEL_PREVIEW_DIALOG_NAME = "LevelPreviewDialog";
    private const string LEVEL_END_DIALOG_NAME = "LevelEndDialog";
    
    private bool isDialogDisplayed = false;

    private void Awake()
    {   
        GameEvents.LevelEndedEvent -= OnLevelEnded;
        GameEvents.LevelEndedEvent += OnLevelEnded;
        
        UIEvents.DialogDismissedEvent -= OnDialogDismissed;
        UIEvents.DialogDismissedEvent += OnDialogDismissed;

        UIEvents.LevelDataLoadedEvent -= OnLevelDataLoaded;
        UIEvents.LevelDataLoadedEvent += OnLevelDataLoaded;
        
    }

    private void OnDestroy()
    {
        GameEvents.LevelEndedEvent -= OnLevelEnded;
        UIEvents.DialogDismissedEvent -= OnDialogDismissed;
        UIEvents.LevelDataLoadedEvent -= OnLevelDataLoaded;
    }

    // spawn the level end dialog
    private void OnLevelEnded(bool won)
    {
        UIDialogBase levelEndDialog = SpawnDialog(LEVEL_END_DIALOG_NAME);
        if (levelEndDialog == null)
        {
            Debug.LogError($"dialog could not be spawned: {LEVEL_END_DIALOG_NAME}");
            return;
        }
        levelEndDialog.Setup(won);
    }

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
        Object dialogObject = Instantiate(Resources.Load(UI_DIALOGS_DIRECTORY + dialogName, typeof(GameObject)), transform);
        GameObject dialogGameObject = dialogObject as GameObject;
        if (dialogGameObject != null)
        {
            dialog = dialogGameObject.GetComponent<UIDialogBase>();
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

    // spawn the level preview dialog and supply it with the loaded level data
    private void OnLevelDataLoaded(LevelData levelData)
    {
        UIDialogBase levelPreviewDialog = SpawnDialog(LEVEL_PREVIEW_DIALOG_NAME);
        if (levelPreviewDialog == null)
        {
            Debug.LogError($"dialog could not be spawned: {LEVEL_PREVIEW_DIALOG_NAME}");
            return;
        }
        levelPreviewDialog.Setup(levelData);
    }
}