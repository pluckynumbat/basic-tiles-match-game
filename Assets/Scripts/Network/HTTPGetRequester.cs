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
       UnityWebRequest webRequest = UnityWebRequest.Get(uri);
       yield return webRequest.SendWebRequest();
       
       switch (webRequest.result)
       {
           case UnityWebRequest.Result.Success:
               Debug.Log("Level Received: " + webRequest.downloadHandler.text);
               break;
           
           default:
               Debug.LogError("Get Request Error: " + webRequest.error);
               break;
       }
   }
}
