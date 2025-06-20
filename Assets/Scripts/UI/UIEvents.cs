using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class to hold all the different UI events that can be raised in a session
/// </summary>
public static class UIEvents
{
    // raised by an element when they want to display a dialog on the screen
    public delegate void DialogDisplayRequestHandler(string dialogName, object[] setupData);
    public static event DialogDisplayRequestHandler DialogDisplayRequestEvent;
    public static void RaiseDialogDisplayRequestEvent(string dialogName, object[] setupData)
    {
        DialogDisplayRequestEvent?.Invoke(dialogName, setupData);
    }
    
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
    
    // raised by UI elements to enter the level scene
    public delegate void PlayLevelRequestHandler();
    public static event PlayLevelRequestHandler PlayLevelRequestEvent;
    public static void RaisePlayLevelRequestEvent()
    {
        PlayLevelRequestEvent?.Invoke();
    }
    
    // raised by UI elements to leave the level scene
    public delegate void LeaveLevelRequestHandler();
    public static event LeaveLevelRequestHandler LeaveLevelRequestEvent;
    public static void RaiseLeaveLevelRequestEvent()
    {
        LeaveLevelRequestEvent?.Invoke();
    }
    
    // raise by a UI Random Mode Button when the player presses it 
    public delegate void RandomModeSelectedHandler();
    public static event RandomModeSelectedHandler RandomModeSelectedEvent;
    public static void RaiseRandomModeSelectedEvent()
    {
        RandomModeSelectedEvent?.Invoke();
    }
    
    // raised UI elements to start a new random level
    public delegate void PlayRandomModeRequestHandler();
    public static event PlayRandomModeRequestHandler PlayRandomModeRequestEvent;
    public static void RaisePlayRandomModeRequestEvent()
    {
        PlayRandomModeRequestEvent?.Invoke();
    }
    
    // raised by UI elements to reload the level scene
    public delegate void RestartLevelRequestHandler();
    public static event RestartLevelRequestHandler RestartLevelRequestEvent;
    public static void RaiseRestartLevelRequestEvent()
    {
        RestartLevelRequestEvent?.Invoke();
    }
    
    // raised by UI elements to toggle volume on/off (mute button)
    public delegate void ToggleMuteRequestHandler();
    public static event ToggleMuteRequestHandler ToggleMuteRequestEvent;
    public static void RaiseToggleMuteRequestEvent()
    {
        ToggleMuteRequestEvent?.Invoke();
    }
    
    // raised by the player level from server button when pressed
    public delegate void ServerLevelSelectedHandler();
    public static event ServerLevelSelectedHandler ServerLevelSelectedEvent;
    public static void RaiseServerLevelSelectedEvent()
    {
        ServerLevelSelectedEvent?.Invoke();
    }

}