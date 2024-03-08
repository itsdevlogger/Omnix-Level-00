using System;
using UnityEngine;
namespace WebUtils
{
    public readonly struct DownloadArgs
    {
        public readonly string URL;
        public readonly Action<Sprite> Callback;

        public DownloadArgs(string url, Action<Sprite> callback) : this()
        {
            URL = url;
            Callback = callback;
        }
    }
    
    public readonly struct DownloadArgs<T>
    {
        public readonly string URL;
        public readonly T Data;
        public readonly Action<Sprite, T> Callback;

        public DownloadArgs(string url, T data, Action<Sprite, T> callback)
        {
            URL = url;
            Data = data;
            Callback = callback;
        }
    }
}