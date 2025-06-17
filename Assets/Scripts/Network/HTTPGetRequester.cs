public class HTTPGetRequester : MonoBehaviour
{
    private void Awake()
    {
        NetworkEvents.LevelRequestedFromServerEvent -= OnLevelRequestedFromServer;
        NetworkEvents.LevelRequestedFromServerEvent += OnLevelRequestedFromServer;
    }
    
    private void OnDestroy()
    {
        NetworkEvents.LevelRequestedFromServerEvent -= OnLevelRequestedFromServer;
    }

    private void OnLevelRequestedFromServer()
    {
    }

    private IEnumerator GetRequest(string uri)
    {
        
    }
}
