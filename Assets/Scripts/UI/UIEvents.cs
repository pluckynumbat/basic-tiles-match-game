using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class to hold all the different UI events that can be raised in a session
/// </summary>
public static class UIEvents
{
    // raised by the UI Dialog Spawner in the scene when a dialog is displayed
    public delegate void DialogDisplayedHandler(UIDialogBase dialog);
    public static event DialogDisplayedHandler DialogDisplayedEvent;
    public static void RaiseDialogDisplayedEvent(UIDialogBase dialog)
    {
        DialogDisplayedEvent?.Invoke(dialog);
    }
    
    // raised by a UI Dialog in the scene when it is dismissed
    public delegate void DialogDismissedHandler(UIDialogBase dialog);
    public static event DialogDismissedHandler DialogDismissedEvent;
    public static void RaiseDialogDismissedEvent(UIDialogBase dialog)
    {
        DialogDismissedEvent?.Invoke(dialog);
    }
    
    // raised by a UI Level Select Node when the player presses it 
    public delegate void LevelSelectedHandler(string levelName);
    public static event LevelSelectedHandler LevelSelectedEvent;
    public static void RaiseLevelSelectedEvent(string levelName)
    {
        LevelSelectedEvent?.Invoke(levelName);
    }
    
    // raised by the Main Manager when it has processed level data from the level string
    public delegate void LevelDataLoadedHandler(LevelData levelData);
    public static event LevelDataLoadedHandler LevelDataLoadedEvent;
    public static void RaiseLevelDataLoadedEvent(LevelData levelData)
    {
        LevelDataLoadedEvent?.Invoke(levelData);
    }
    
    
}