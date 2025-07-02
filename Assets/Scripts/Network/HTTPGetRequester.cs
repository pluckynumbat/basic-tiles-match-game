using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HTTPGetRequester : MonoBehaviour
{
    private const string RemoteLevelURI = "http://localhost:8090/level";
    private void Awake()
    {
        NetworkEvents.RemoteLevelRequestedEvent -= OnRemoteLevelRequested;
        NetworkEvents.RemoteLevelRequestedEvent += OnRemoteLevelRequested;
    }
    
    private void OnDestroy()
    {
        NetworkEvents.RemoteLevelRequestedEvent -= OnRemoteLevelRequested;
    }

    private void OnRemoteLevelRequested()
    {
        StartCoroutine(GetLevelRequest(RemoteLevelURI));
    }

   private IEnumerator GetLevelRequest(string uri)
   {
       UnityWebRequest webRequest = UnityWebRequest.Get(uri);
       yield return webRequest.SendWebRequest();
       
       switch (webRequest.result)
       {
           case UnityWebRequest.Result.Success:
               Debug.Log("Level Received: " + webRequest.downloadHandler.text);
               NetworkEvents.RaiseRemoteLevelReceivedEvent(webRequest.downloadHandler.text);
               break;
           
           default:
               Debug.LogError("Get Request Error: " + webRequest.error);
               break;
       }
   }
}
