using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Threading.Tasks;
using System.IO;

namespace XFramework
{
    public abstract class ResourcesLoader : IDisposable
    {
        public async virtual Task InitAsync()
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// ͬ��������Դ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract Object LoadAsset(string key, Type type);

        /// <summary>
        /// ͬ��������Դ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract T LoadAsset<T>(string key) where T : Object;

        /// <summary>
        /// �첽������Դ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract Task<T> LoadAssetAsync<T>(string key) where T : Object;

        /// <summary>
        /// �첽������Դ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract Task<Object> LoadAssetAsync(string key, Type type);

        /// <summary>
        /// ͬ�����ز�ʵ����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parent"></param>
        /// <param name="isFromPool"></param>
        /// <returns></returns>
        public abstract GameObject Instantiate(string key, Transform parent);

        /// <summary>
        /// ͬ�����ز�ʵ����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="isFromPool"></param>
        /// <returns></returns>
        public abstract GameObject Instantiate(string key, Transform parent, Vector3 position);

        /// <summary>
        /// ͬ�����ز�ʵ����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="quaternion"></param>
        /// <param name="isFromPool"></param>
        /// <returns></returns>
        public abstract GameObject Instantiate(string key, Transform parent, Vector3 position, Quaternion quaternion);

        /// <summary>
        /// �첽���ز�ʵ����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parent"></param>
        /// <param name="isFromPool"></param>
        /// <returns></returns>
        public abstract Task<GameObject> InstantiateAsync(string key, Transform parent);

        /// <summary>
        /// �첽���ز�ʵ����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="isFromPool"></param>
        /// <returns></returns>
        public abstract Task<GameObject> InstantiateAsync(string key, Transform parent, Vector3 position);

        /// <summary>
        /// �첽���ز�ʵ����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <param name="quaternion"></param>
        /// <param name="isFromPool"></param>
        /// <returns></returns>
        public abstract Task<GameObject> InstantiateAsync(string key, Transform parent, Vector3 position, Quaternion quaternion);

        /// <summary>
        /// �ͷ�ʵ��������
        /// </summary>
        /// <param name="obj"></param>
        public abstract void ReleaseInstance(GameObject obj);

        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        /// <param name="asset"></param>
        public abstract void ReleaseAsset(Object asset);

        /// <summary>
        /// �ͷ�δʹ�õ���Դ
        /// </summary>
        public abstract void UnloadUnusedAssets();

        public virtual void Dispose()
        {

        }
    }
}
