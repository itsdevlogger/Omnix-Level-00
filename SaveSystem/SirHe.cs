using System;
using System.IO;
using UnityEngine;

namespace SaveSystem
{
    public static class SirHe
    {
        public static string GetPath(string id)
        {
            var folder = Path.Combine(Application.persistentDataPath, "Data");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var filePath = Path.Combine(folder, id);
            return filePath;
        }

        public static string GetUniqueId()
        {
            string id = Guid.NewGuid().ToString();
            while (File.Exists(SirHe.GetPath(id)))
                id = Guid.NewGuid().ToString();
            return id;
        }
    }
}