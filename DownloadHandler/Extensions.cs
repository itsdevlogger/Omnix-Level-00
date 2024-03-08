using System;
using UnityEngine;
using UnityEngine.UI;

namespace WebUtils
{
    public static class Extensions
    {
        /// <summary> Download an image from the given URL and set it as the sprite of the provided Image </summary>
        public static void DownloadAndSet(this Image image, string url, Action<Sprite> callback = null)
        {
            IData holder = new ImageData() { image = image, action = callback };
            ImageDownloader.Download(url, holder, OnDownload);
        }

        /// <summary> Download an image from the given URL and set it as the sprite of the provided Image </summary>
        public static void DownloadAndSet<T>(this Image image, string url, T data, Action<Sprite, T> callback = null)
        {
            IData holder = new ImageData<T>(){ image = image, data = data, action = callback };
            ImageDownloader.Download(url, holder, OnDownload);
        }

        /// <summary> Download an image from the given URL and set it as the sprite of the provided SpriteRenderer </summary>
        public static void DownloadAndSet(this SpriteRenderer image, string url, Action<Sprite> callback = null)
        {
            IData holder = new SpriteRendererData() { image = image, action = callback };
            ImageDownloader.Download(url, holder, OnDownload);
        }

        /// <summary> Download an image from the given URL and set it as the sprite of the provided SpriteRenderer </summary>
        public static void DownloadAndSet<T>(this SpriteRenderer image, string url, T data, Action<Sprite, T> callback = null)
        {
            IData holder = new SpriteRendererData<T>() { image = image, data = data, action = callback };
            ImageDownloader.Download(url, holder, OnDownload);
        }

        /// <summary> Download an image from the given URL and set it as the sprite of the provided SpriteRenderer </summary>
        public static void DownloadAndSet(this RawImage image, string url, Action<Sprite> callback = null)
        {
            IData holder = new RawImageData() { image = image, action = callback };
            ImageDownloader.Download(url, holder, OnDownload);
        }

        /// <summary> Download an image from the given URL and set it as the sprite of the provided SpriteRenderer </summary>
        public static void DownloadAndSet<T>(this RawImage image, string url, T data, Action<Sprite, T> callback = null)
        {
            IData holder = new RawImageData<T>() { image = image, data = data, action = callback };
            ImageDownloader.Download(url, holder, OnDownload);
        }


        private static void OnDownload(Sprite sprite, IData holder)
        {
            holder.Done(sprite);
        }


        private interface IData
        {
            void Done(Sprite sprite);
        }

        private class ImageData : IData
        {
            public Image image;
            public Action<Sprite> action;

            public void Done(Sprite sprite)
            {
                image.sprite = sprite;
                action?.Invoke(sprite);
            }
        }

        private class ImageData<T> : IData
        {
            public Image image;
            public T data;
            public Action<Sprite, T> action;

            public void Done(Sprite sprite)
            {
                image.sprite = sprite;
                action?.Invoke(sprite, data);
            }
        }

        private class SpriteRendererData : IData
        {
            public SpriteRenderer image;
            public Action<Sprite> action;

            public void Done(Sprite sprite)
            {
                image.sprite = sprite;
                action?.Invoke(sprite);
            }
        }

        private class SpriteRendererData<T> : IData
        {
            public SpriteRenderer image;
            public T data;
            public Action<Sprite, T> action;

            public void Done(Sprite sprite)
            {
                image.sprite = sprite;
                action?.Invoke(sprite, data);
            }
        }

        private class RawImageData : IData
        {
            public RawImage image;
            public Action<Sprite> action;

            public void Done(Sprite sprite)
            {
                if (sprite != null) image.texture = sprite.texture;
                action?.Invoke(sprite);
            }
        }

        private class RawImageData<T> : IData
        {
            public RawImage image;
            public T data;
            public Action<Sprite, T> action;

            public void Done(Sprite sprite)
            {
                if (sprite != null) image.texture = sprite.texture;
                action?.Invoke(sprite, data);
            }
        }
    }
}