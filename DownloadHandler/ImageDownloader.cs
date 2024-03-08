using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WebUtils
{
    public class ImageDownloader : MonoBehaviour
    {
        #region Unity Callbacks
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
        #endregion

        #region Downloader
        private IEnumerator DownloadImages(IEnumerable<DownloadArgs> urlsWithIndividualCallbacks, Action onAllDownloaded)
        {
            foreach (DownloadArgs args in urlsWithIndividualCallbacks)
            {
                InternalDownloader downloader = new InternalDownloader(args.URL);
                foreach (object item in downloader.Download()) yield return item;
                args.Callback?.Invoke(downloader.Result);
            }

            onAllDownloaded?.Invoke();
            Destroy(gameObject);
        }

        private IEnumerator DownloadImages<T>(IEnumerable<DownloadArgs<T>> urlsWithIndividualCallbacks, Action onAllDownloaded)
        {
            foreach (DownloadArgs<T> args in urlsWithIndividualCallbacks)
            {
                InternalDownloader downloader = new InternalDownloader(args.URL);
                foreach (object item in downloader.Download()) yield return item;
                args.Callback?.Invoke(downloader.Result, args.Data);
            }

            onAllDownloaded?.Invoke();
            Destroy(gameObject);
        }

        private IEnumerator DownloadImage<T>(string url, T data, Action<Sprite, T> callback)
        {
            InternalDownloader downloader = new InternalDownloader(url);
            foreach (object item in downloader.Download()) yield return item;
            callback?.Invoke(downloader.Result, data);
            Destroy(gameObject);
        }

        private IEnumerator DownloadImage(string url, Action<Sprite> callback)
        {
            InternalDownloader downloader = new InternalDownloader(url);
            foreach (object item in downloader.Download()) yield return item;
            callback?.Invoke(downloader.Result);
            Destroy(gameObject);
        }
        #endregion

        #region Statics
        /// <summary>
        /// Download a single image
        /// </summary>
        /// <param name="url">Image url</param>
        /// <param name="callback"> Callback when the download is completed </param>
        public static void Download(string url, Action<Sprite> callback)
        {
            #if UNITY_SERVER
            callback?.Invoke(null);
            #else
            if (string.IsNullOrEmpty(url))
                callback?.Invoke(null);
            else
            {
                ImageDownloader handler = new GameObject("Image Downloader").AddComponent<ImageDownloader>();
                handler.StartCoroutine(handler.DownloadImage(url, callback));
            }
            #endif
        }

        /// <summary>
        /// Downlaod a single image
        /// </summary>
        /// <typeparam name="T"> Type of user data </typeparam>
        /// <param name="url"> Image url </param>
        /// <param name="data"> User data, can be anything </param>
        /// <param name="callback"> Callback when the download is completed </param>
        public static void Download<T>(string url, T data, Action<Sprite, T> callback)
        {
            #if UNITY_SERVER
            callback?.Invoke(null, data);
            #else
            if (string.IsNullOrEmpty(url))
                callback?.Invoke(null, data);
            else
            {
                ImageDownloader handler = new GameObject("Image Downloader").AddComponent<ImageDownloader>();
                handler.StartCoroutine(handler.DownloadImage(url, data, callback));
            }
            #endif
        }

        /// <summary>
        /// Download multiple images
        /// </summary>
        /// <param name="urlsWithIndividualCallbacks"> urls of all the images to download. Each entry in this enumerable should be a tuple indicating (string url, Action&lt;Sprite&gt; onImageDownloadedFromGivenUrl) </param>
        /// <param name="onAllDownloaded">Callback when all images are downloaded</param>
        public static void Download(IEnumerable<DownloadArgs> urlsWithIndividualCallbacks, Action onAllDownloaded)
        {
            ImageDownloader handler = new GameObject("Image Downloader").AddComponent<ImageDownloader>();
            handler.StartCoroutine(handler.DownloadImages(urlsWithIndividualCallbacks, onAllDownloaded));
        }

        /// <summary>
        /// Download multiple images
        /// </summary>
        /// <param name="urlsWithIndividualCallbacks"> urls of all the images to download. Each entry in this enumerable should be a tuple indicating (string url, T userData, Action&lt;Sprite, T&gt; onImageDownloadedFromGivenUrl) </param>
        /// <param name="onAllDownloaded">Callback when all images are downloaded</param>
        public static void Download<T>(IEnumerable<DownloadArgs<T>> urlsWithIndividualCallbacks, Action onAllDownloaded)
        {
            ImageDownloader handler = new GameObject("Image Downloader").AddComponent<ImageDownloader>();
            handler.StartCoroutine(handler.DownloadImages(urlsWithIndividualCallbacks, onAllDownloaded));
        }
        #endregion
    }
}