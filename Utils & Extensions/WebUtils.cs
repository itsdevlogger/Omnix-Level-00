using System;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Omnix.Utils
{
    public static class WebUtils
    {
        public static void DownloadAsset<T>(string url, string assetName, Action<T> success) where T : Object => DownloadAsset(url, assetName, success, null);
        public static void DownloadAssetBundle(string url, Action<AssetBundle> success) => DownloadAssetBundle(url, success, null);
        public static void DownloadImage(string url, System.Action<Sprite> success) => DownloadImage(url, success, null);
        public static void DownloadTexture(string url, System.Action<Texture> success) => DownloadTexture(url, success, null);

        public static void DownloadAsset<T>(string url, string assetName, Action<T> success, Action<UnityWebRequest.Result> error) where T : Object
        {
            UtilsObject go = new GameObject("DownloadingAssets").AddComponent<UtilsObject>();
            go.DownloadAssetBundleFromURL(url, (AssetBundle bundle) => OnAssetDownloaded<T>(bundle, assetName, success, error), error);
        }

        public static void DownloadAssetBundle(string url, Action<AssetBundle> success, Action<UnityWebRequest.Result> error)
        {
            UtilsObject go = new GameObject("DownloadingAssets").AddComponent<UtilsObject>();
            go.DownloadAssetBundleFromURL(url, success, error);
        }

        public static void DownloadImage(string url, System.Action<Sprite> success, Action<UnityWebRequest.Result> error)
        {
            GameObject obj = new GameObject();
            UtilsObject downloadImageHandler = obj.AddComponent<UtilsObject>();
            downloadImageHandler.DownloadImage(url, success, error);
        }
        
        public static void DownloadTexture(string url, System.Action<Texture> success, Action<UnityWebRequest.Result> error)
        {
            GameObject obj = new GameObject();
            UtilsObject downloadImageHandler = obj.AddComponent<UtilsObject>();
            downloadImageHandler.DownloadTexture(url, success, error);
        }
        
        private static void OnAssetDownloaded<T>(AssetBundle bundle, string assetName, Action<T> success, Action<UnityWebRequest.Result> error) where T : Object
        {
            T asset = bundle.LoadAsset<T>(assetName);
            if (asset == null)
                error?.Invoke(UnityWebRequest.Result.DataProcessingError);
            else
                success.Invoke(asset);
        }
    }
}