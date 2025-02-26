using UnityEngine;

/// <summary>
/// Static class to hold all the different game events that can be raised in a session
/// </summary>
public static class GameEvents
{
    // raised by the input detector when player input is detected
    public delegate void InputDetectedHandler(Vector3 inputWorldPosition);
    public static event InputDetectedHandler InputDetectedEvent;
    public static void RaiseInputDetectedEvent(Vector3 inputWorldPosition)
    {
        InputDetectedEvent?.Invoke(inputWorldPosition);
    }
}
