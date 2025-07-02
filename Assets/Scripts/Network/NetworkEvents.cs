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
    
    public delegate void RemoteLevelReceivedHandler(string levelJSONString);
    public static event RemoteLevelReceivedHandler RemoteLevelReceivedEvent;
    public static void RaiseRemoteLevelReceivedEvent(string levelJSONString)
    {
        RemoteLevelReceivedEvent?.Invoke(levelJSONString);
    }
    
    public delegate void RemoteTestCompletedHandler(bool success);
    public static event RemoteTestCompletedHandler RemoteTestCompletedEvent;
    public static void RaiseRemoteTestCompletedEvent(bool success)
    {
        RemoteTestCompletedEvent?.Invoke(success);
    }

}
