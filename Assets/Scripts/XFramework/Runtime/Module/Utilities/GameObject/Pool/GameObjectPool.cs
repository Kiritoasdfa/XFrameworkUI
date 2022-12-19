using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XFramework
{
    public class GameObjectPool : Singleton<GameObjectPool>, IDisposable
    {
        private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

        private Dictionary<string, GameObject> poolObjParentDict = new Dictionary<string, GameObject>();

        /// <summary>
        /// UI对象池位置
        /// </summary>
        private Transform uiHidden = null;

        /// <summary>
        /// 非UI对象池位置
        /// </summary>
        private Transform notUIHideen = null;

        public void Init()
        {
            
        }

        /// <summary>
        /// 取出
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public GameObject Fetch(string key)
        {
            GameObject obj = Dequeue(key);

            if (obj != null)
            {
                obj.SetViewActive(true);
            }

            return obj;
        }

        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void Recycle(string key, GameObject obj)
        {
            if (!obj)
                return;

            bool isUI = obj.transform as RectTransform;

            if (isUI)
                RecycleUI(key, obj);
            else
                RecycleNotUI(key, obj);

            Enqueue(key, obj);
        }

        private void RecycleUI(string key, GameObject obj)
        {
            try
            {
                if (uiHidden == null)
                {
                    //uiHidden = Common.Instance?.Get<Global>().UI.Find("Hidden");
                    uiHidden = GameObject.Find("UIPool")?.transform;
                    if (uiHidden == null)
                    {
                        Transform root = Common.Instance?.Get<Global>().GameRoot;
                        uiHidden = new GameObject("UIPool").transform;
                        uiHidden.gameObject.layer = LayerMask.NameToLayer("UI");
                        //uiHidden.SetViewActive(false);
                        uiHidden.SetParent(root, false);
                    }
                }

                if (!this.poolObjParentDict.TryGetValue(key, out var parentObj))
                {
                    parentObj = new GameObject(key);
                    parentObj.transform.SetParent(uiHidden, false);
                    this.poolObjParentDict.Add(key, parentObj);
                }

                obj.transform.SetParent(parentObj.transform, false);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void RecycleNotUI(string key, GameObject obj)
        {
            try
            {
                if (notUIHideen == null)
                {
                    Transform root = Common.Instance?.Get<Global>().GameRoot;
                    notUIHideen = root.Find("Pool");

                    if (notUIHideen == null)
                    {
                        GameObject pool = new GameObject("Pool");
                        pool.SetViewActive(false);
                        pool.transform.SetParent(root, false);
                        notUIHideen = pool.transform;
                    }
                }

                if (!this.poolObjParentDict.TryGetValue(key, out var parentObj))
                {
                    parentObj = new GameObject(key);
                    parentObj.transform.SetParent(notUIHideen, false);
                    this.poolObjParentDict.Add(key, parentObj);
                }

                obj.transform.SetParent(parentObj.transform, false);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private GameObject Dequeue(string key)
        {
            if (pool.TryGetValue(key, out Queue<GameObject> queue))
            {
                if (queue.Count > 0)
                {
                    GameObject obj = queue.Dequeue();
                    if (queue.Count == 0)
                    {
                        ObjectPool.Instance.Recycle(queue);
                        pool.Remove(key);
                    }
                    return obj;
                }
            }

            return null;
        }

        private void Enqueue(string key, GameObject obj)
        {
            if (!pool.TryGetValue(key, out Queue<GameObject> queue))
            {
                queue = ObjectPool.Instance.Fetch<Queue<GameObject>>();
                pool.Add(key, queue);
            }

            queue.Enqueue(obj);
        }

        public void Clear()
        {
            foreach (var item in pool)
            {
                var key = item.Key;
                var queue = item.Value;
                while (queue.Count > 0)
                {
                    GameObject obj = queue.Dequeue();
                    ResourcesManager.Instance?.Loader?.ReleaseInstance(obj);
                }

                if (this.poolObjParentDict.TryRemove(key, out var parentObj))
                {
                    Object.Destroy(parentObj);
                }
            }

            pool.Clear();
        }

        public void Dispose()
        {
            Clear();

            Instance = null;
        }
    }
}
