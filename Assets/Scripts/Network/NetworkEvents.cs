/// <summary>
/// Static class to hold all the different Network events that can be raised in a session
/// </summary>
public static class NetworkEvents
{
    public delegate void LevelRequestedFromServerHandler();
    public static event LevelRequestedFromServerHandler LevelRequestedFromServerEvent;
    public static void RaiseLevelRequestedFromServerEvent()
    {
        LevelRequestedFromServerEvent?.Invoke();
    }
    
    public delegate void LevelReceivedFromServerHandler(string levelJSONString);
    public static event LevelReceivedFromServerHandler LevelReceivedFromServerEvent;
    public static void RaiseLevelReceivedFromServerEvent(string levelJSONString)
    {
        LevelReceivedFromServerEvent?.Invoke(levelJSONString);
    }
}
