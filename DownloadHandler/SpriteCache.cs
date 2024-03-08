using System.IO;
using UnityEngine;
namespace WebUtils
{
    public static class SpriteCache
    {
        private static readonly string CachePath = Path.Combine(Application.persistentDataPath, "image_downloader_cache");

        private static void ClearCache()
        {
            if (!Directory.Exists(CachePath))
            {
                Directory.CreateDirectory(CachePath);
                return;
            }
            if (PlayerPrefs.GetString("[LAST_IMAGE_CACHED_VERSION]") == Application.version)
            {
                return;
            }
            PlayerPrefs.SetString("[LAST_IMAGE_CACHED_VERSION]", Application.version);
            
            string[] files = Directory.GetFiles(CachePath);

            foreach (string file in files)
            {
                File.Delete(file);
            }

        }
        
        public static void Save(Sprite sprite, string name)
        {
            if (sprite == null) return;

            if (!Directory.Exists(CachePath))
            {
                Directory.CreateDirectory(CachePath);
            }

            Texture2D texture = sprite.texture;
            byte[] bytes = texture.EncodeToPNG();
            string filePath = Path.Combine(CachePath, name + ".png");
            File.WriteAllBytes(filePath, bytes);
        }

        public static bool TryLoad(string name, out Sprite sprite)
        {
            string filePath = Path.Combine(CachePath, name + ".png");

            if (File.Exists(filePath))
            {
                byte[] bytes = File.ReadAllBytes(filePath);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(bytes);
                sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                return sprite != null;
            }

            sprite = null;
            return false;
        }
    }
}

