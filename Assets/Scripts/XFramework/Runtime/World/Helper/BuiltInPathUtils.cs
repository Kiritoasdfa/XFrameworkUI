using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFramework
{
    public static class BuiltInPathUtils
    {
        private static Dictionary<string, string> resPath = new Dictionary<string, string>();

        private const string UIPrefab = "UI/Prefabs";

        private const string UITexture = "UI/Textures";

        private const string SpriteAtlas = "SpriteAtlas";

        private const string AudioClip = "AudioClips";

        public static string GetUI(string key)
        {
            if (!resPath.TryGetValue(key, out string path))
            {
                path = $"{UIPrefab}{key}";
                resPath.Add(key, path);
            }

            return path;
        }

        public static string GetUITexture(string key)
        {
            if (!resPath.TryGetValue(key, out string path))
            {
                path = $"{UITexture}{key}";
                resPath.Add(key, path);
            }

            return path;
        }

        public static string GetSpriteAtlas(string key)
        {
            if (!resPath.TryGetValue(key, out string path))
            {
                path = $"{SpriteAtlas}{key}";
                resPath.Add(key, path);
            }

            return path;
        }

        public static string GetAudioClip(string key)
        {
            if (!resPath.TryGetValue(key, out string path))
            {
                path = $"{AudioClip}{key}";
                resPath.Add(key, path);
            }

            return path;
        }
    }
}
