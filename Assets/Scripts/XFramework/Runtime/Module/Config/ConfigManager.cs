using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework
{
    public sealed class ConfigManager : CommonObject
    {
        /// <summary>
        /// �����ļ��� -> ����������
        /// </summary>
        private Dictionary<string, byte[]> configBytes = new Dictionary<string, byte[]>();

        /// <summary>
        /// �������� -> �����л������Ķ���
        /// </summary>
        private Dictionary<Type, object> configProtos = new Dictionary<Type, object>();

        /// <summary>
        /// �����Config���Ե�������
        /// <para>���� -> ����</para>
        /// </summary>
        private Dictionary<string, Type> configTypes = new Dictionary<string, Type>();

        private IConfigLoader loader;

        protected override void Init()
        {
            var types = TypesManager.Instance.GetTypes(typeof(ConfigAttribute));
            foreach (var type in types)
            {
                configTypes.Add(type.Name, type);
            }

            this.SetDefaultLoader();
        }

        public void SetDefaultLoader()
        {
            this.SetLoader(new UnityConfigLoader());
        }

        public void SetLoader(IConfigLoader loader)
        {
            this.loader = loader;
        }

        /// <summary>
        /// ������������
        /// </summary>
        /// <returns></returns>
        public async Task LoadAllConfigsAsync()
        {
            var tagId = this.TagId;
            configBytes = await loader.LoadAllAsync();
            if (tagId != this.TagId)
                return;

            await DeserializeConfigs();
        }

        /// <summary>
        /// ���ص�������
        /// </summary>
        /// <param name="configType"></param>
        /// <returns></returns>
        public async Task LoadOneConfigAsync(Type configType)
        {
            var tagId = this.TagId;
            var bytes = await loader.LoadOneAsync(configType.Name);
            if (tagId != this.TagId || bytes is null || bytes.Length == 0)
                return;

            await DeserializeAsync(configType, bytes);
        }

        /// <summary>
        /// ���ص�������
        /// </summary>
        /// <param name="configType"></param>
        public void LoadOneConfig(Type configType)
        {
            var bytes = loader.LoadOne(configType.Name);
            if (bytes != null)
            {
                object configObj = ProtobufHelper.FromBytes(bytes, configType);
                configProtos[configType] = configObj;
            }
        }

        /// <summary>
        /// �����л����е�����
        /// </summary>
        /// <returns></returns>
        private async Task DeserializeConfigs()
        {
            if (configTypes.Count == configProtos.Count)
                return;

            if (configBytes.Count == 0)
                return;

            using var tasks = XList<Task>.Create();
            foreach (var configInfo in configTypes)
            {
                string name = configInfo.Key;
                Type configType = configInfo.Value;
                if (configBytes.TryGetValue(name, out var bytes))
                {
                    if (configProtos.ContainsKey(configType))
                        continue;

                    tasks.Add(DeserializeAsync(configType, bytes));
                }
                else
                {
                    Log.Error($"���ü���ʧ�ܣ���Ϊ{name}�����������ļ�");
                }
            }

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// �����л�����
        /// </summary>
        /// <param name="configType"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private async Task DeserializeAsync(Type configType, byte[] bytes)
        {
            var task = Task.Run(() =>
            {
                object obj = ProtobufHelper.FromBytes(bytes, configType);
                return obj;
            });

            object configObj = await task;
            configProtos[configType] = configObj;
        }

        protected override void Destroy()
        {
            configBytes.Clear();
            configProtos.Clear();
            configTypes.Clear();
            loader = null;
        }
    }
}
