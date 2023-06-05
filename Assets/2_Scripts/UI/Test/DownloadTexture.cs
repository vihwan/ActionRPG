using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UniRx;


namespace Test.UniRx
{
    public class DownloadTexture : MonoBehaviour
    {
        [SerializeField]
        private RawImage mRawImage;

        private void Start()
        {
            var url = "< 표시할 이미지의 주소>";

            GetTextureAsync(url);
        }
        private IObservable<Texture> GetTextureAsync(string url)
        {
            return Observable.FromCoroutine<Texture>(observer => 
            {
                return GetTextureCoroutine(observer, url);                    
            });
        }

        private IEnumerator GetTextureCoroutine(IObserver<Texture> observer, string url)
        {
            using (var uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();
                if(uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    //에러가 발생하면 OnError 메시지를 발행
                    observer.OnError(new Exception(uwr.error));
                    yield break;
                }

                var result = ((DownloadHandlerTexture)uwr.downloadHandler).texture;

                //성공하면 OnNext/OnCompleted를 발행한다.
                observer.OnNext(result);
                observer.OnCompleted();
            }
        }
    }
}
