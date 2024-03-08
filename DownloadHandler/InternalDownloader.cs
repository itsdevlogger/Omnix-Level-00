using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
namespace WebUtils
{
    public class InternalDownloader
    {
        private static Dictionary<string, InternalDownloader> _currentlyDownloading = new Dictionary<string, InternalDownloader>();
        private static Dictionary<string, Sprite> _preloaded = new Dictionary<string, Sprite>();
        
        public Sprite Result { get; private set; }
        private string _url;
        private string _hash;

        public InternalDownloader(string url)
        {
            _url = url;
            _hash = CreateMD5(url);
        }

        public IEnumerable Download()
        {
            Result = null;

            if (_preloaded.ContainsKey(_hash))
            {
                Result = _preloaded[_hash];
                yield break;
            }
            
            // If the image is present in cache then return that
            if (SpriteCache.TryLoad(_hash, out Sprite spr))
            {
                Result = spr;
                SaveToPreloaded();
                yield break;
            }
            
            // If another Downloader is already downloading from this link
            if (_currentlyDownloading.TryGetValue(_hash, out InternalDownloader downloader))
            {
                // Wait till that downloader is finish
                while (downloader.Result == null)
                {
                    yield return null;
                }

                // Grab the downloaded image
                Result = downloader.Result;
                SaveToPreloaded();
                yield break;
            }

            // If no downloader is downloading from the link, then this register this downloader...
            _currentlyDownloading.Add(_hash, this);

            // Download the image
            using UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_url);
            yield return uwr.SendWebRequest();

            // If we encountered an error while downloading
            if (uwr.result == UnityWebRequest.Result.ProtocolError || uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.DataProcessingError)
            {
                Result = null;
                _currentlyDownloading.Remove(_hash);
                yield break;
            }

            // Get downloaded image
            Texture2D texture = DownloadHandlerTexture.GetContent(uwr);
            Result = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));

            // Save to cache
            SpriteCache.Save(Result, _hash);
            SaveToPreloaded();
            _currentlyDownloading.Remove(_hash);
            yield break;
        }

        private void SaveToPreloaded()
        {
            if (!_preloaded.ContainsKey(_hash) && Result != null)
            {
                _preloaded.Add(_hash, Result);
            }
        }
        
        private static string CreateMD5(string input)
        {
            using System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes);
        }
    }
}