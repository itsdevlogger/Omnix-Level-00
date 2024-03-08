using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


namespace Omnix.Utils
{
    public class UtilsObject : MonoBehaviour
    {
        public void DownloadAssetBundleFromURL(string url, Action<AssetBundle> success, Action<UnityWebRequest.Result> error) => StartCoroutine(DownloadAssetCour(url, success, error));

        public void DownloadImage(string url, Action<Sprite> success, Action<UnityWebRequest.Result> error)
        {
            StartCoroutine(DownloadImageCour(url, success, error));
        }

        public Coroutine StartCustomCoroutine(IEnumerator routine, Action onComplete) => StartCoroutine(CustomCoroutine(routine, onComplete));

        public void DownloadTexture(string url, Action<Texture> success, Action<UnityWebRequest.Result> error)
        {
            StartCoroutine(DownloadTextureCour(url, success, error));
        }

        IEnumerator DownloadAssetCour(string url, Action<AssetBundle> success, Action<UnityWebRequest.Result> error)
        {
            using (UnityWebRequest webRequest = UnityWebRequestAssetBundle.GetAssetBundle(url))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string[] pages = url.Split('/');
                int page = pages.Length - 1;

                switch (webRequest.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                    case UnityWebRequest.Result.ProtocolError:
                    {
                        error.Invoke(webRequest.result);
                        break;
                    }
                    case UnityWebRequest.Result.Success:
                    {
                        success.Invoke(DownloadHandlerAssetBundle.GetContent(webRequest));
                        break;
                    }
                }
            }

            Destroy(gameObject);
        }
        
        IEnumerator DownloadImageCour(string url, Action<Sprite> success, Action<UnityWebRequest.Result> error)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();

                if (uwr.result == UnityWebRequest.Result.Success)
                {
                    var texture = DownloadHandlerTexture.GetContent(uwr);
                    Sprite cardSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                    success.Invoke(cardSprite);
                }
                else
                {
                    error.Invoke(uwr.result);
                }
            }
            Destroy(gameObject);
        }
        
        IEnumerator CustomCoroutine(IEnumerator routine, Action onComplete)
        {
            yield return StartCoroutine(routine);
            onComplete?.Invoke();
            Destroy(gameObject);
        }

        IEnumerator DownloadTextureCour(string url, Action<Texture> success, Action<UnityWebRequest.Result> error)
        {
            using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
            {
                yield return uwr.SendWebRequest();
                if (uwr.result == UnityWebRequest.Result.Success)
                {
                    success.Invoke(DownloadHandlerTexture.GetContent(uwr));
                }
                else
                {
                    error.Invoke(uwr.result);
                }
            }
            Destroy(gameObject);
        }


    }
}