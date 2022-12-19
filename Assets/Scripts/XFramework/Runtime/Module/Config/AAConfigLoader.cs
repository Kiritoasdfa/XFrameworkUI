﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework
{
    public class AAConfigLoader : IConfigLoader
    {
        public async Task<Dictionary<string, byte[]>> LoadAllAsync()
        {
            Dictionary<string, byte[]> dict = new Dictionary<string, byte[]>();

            async Task Load(string fileName)
            {
                TextAsset asset = await ResourcesManager.Instance.Loader.LoadAssetAsync<TextAsset>(fileName);
                if (asset is null)
                    return;

                dict[fileName] = asset.bytes;
                ResourcesManager.Instance.Loader.ReleaseAsset(asset);
            }

            using var tasks = XList<Task>.Create();
            var types = TypesManager.Instance.GetTypes(typeof(ConfigAttribute));
            foreach (var type in types)
            {
                tasks.Add(Load(type.Name));
            }

            await Task.WhenAll(tasks);
            return dict;
        }

        public byte[] LoadOne(string name)
        {
            var textAsset = ResourcesManager.Instance.Loader.LoadAsset<TextAsset>(name);
            if (textAsset is null)
                return null;

            var bytes = textAsset.bytes;
            ResourcesManager.Instance.Loader.ReleaseAsset(textAsset);

            return bytes;
        }

        public async Task<byte[]> LoadOneAsync(string name)
        {
            var textAsset = await ResourcesManager.Instance.Loader.LoadAssetAsync<TextAsset>(name);
            if (textAsset is null)
                return null;

            var bytes = textAsset.bytes;
            ResourcesManager.Instance.Loader.ReleaseAsset(textAsset);

            return bytes;
        }
    }
}
