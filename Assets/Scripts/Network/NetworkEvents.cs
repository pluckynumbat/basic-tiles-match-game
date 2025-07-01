/// <summary>
/// Static class to hold all the different Network events that can be raised in a session
/// </summary>
public static class NetworkEvents
{
    public delegate void RemoteLevelRequestedHandler();
    public static event RemoteLevelRequestedHandler RemoteLevelRequestedEvent;
    public static void RaiseRemoteLevelRequestedEvent()
    {
        RemoteLevelRequestedEvent?.Invoke();
    }
    
    public delegate void LevelReceivedFromServerHandler(string levelJSONString);
    public static event LevelReceivedFromServerHandler LevelReceivedFromServerEvent;
    public static void RaiseLevelReceivedFromServerEvent(string levelJSONString)
    {
        LevelReceivedFromServerEvent?.Invoke(levelJSONString);
    }
}
