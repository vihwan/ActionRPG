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
            var url = "< ǥ���� �̹����� �ּ�>";

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
                    //������ �߻��ϸ� OnError �޽����� ����
                    observer.OnError(new Exception(uwr.error));
                    yield break;
                }

                var result = ((DownloadHandlerTexture)uwr.downloadHandler).texture;

                //�����ϸ� OnNext/OnCompleted�� �����Ѵ�.
                observer.OnNext(result);
                observer.OnCompleted();
            }
        }
    }
}
