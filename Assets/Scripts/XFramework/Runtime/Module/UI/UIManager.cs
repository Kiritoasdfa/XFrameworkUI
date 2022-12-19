using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace XFramework
{
    public sealed class UIManager : CommonObject, IUpdate
    {
        private Dictionary<string, UI> allUIs = new Dictionary<string, UI>();

        private Dictionary<int, Transform> allLayers = new Dictionary<int, Transform>();

        private Stack<string> uiStack = new Stack<string>();

        private const string CancelKeyCode = "Cancel";

        protected override void Init()
        {
            Transform ui = Common.Instance.Get<Global>().UI;
            var reference = ui.Reference();
            allLayers.Add((int)UILayer.Low, reference.GetChild<Transform>("Low"));
            allLayers.Add((int)UILayer.Mid, reference.GetChild<Transform>("Mid"));
            allLayers.Add((int)UILayer.High, reference.GetChild<Transform>("High"));
        }

        protected override void Destroy()
        {
            Clear();
            allLayers.Clear();
        }

        private void InitUI(UI ui, UILayer layer)
        {
            if (ui is null)
                return;

            ui.SetLayer(layer);
            UI child = GetTop();
            if (child != null && !child.IsDisposed)
            {
                child.OnBlur();
            }
            uiStack.Push(ui.Name);
        }

        /// <summary>
        /// 创建UI并初始化
        /// </summary>
        /// <param name="uiType"></param>
        /// <param name="parentObj"></param>
        /// <param name="allowManagement">允许UIManager进行管理</param>
        /// <returns></returns>
        private UI CreateInner(string uiType, Transform parentObj, bool allowManagement)
        {
            Remove(uiType);
            var uiEvent = GetUIEventManager()?.Get(uiType);
            if (uiEvent is null)
                return null;

            if (allowManagement)
            {
                if (!uiEvent.AllowManagement)
                {
                    Log.Error($"尝试创建UIType为{uiType}的UI到UIManager，因为{uiType}是不受管理的");
                    return null;
                }
            }

            UI ui = uiEvent.OnCreate();
            if (ui is null)
                return null;
                
            GameObject obj = GetGameObject(ui, uiEvent, parentObj);
            ui.Bind(obj, uiType);

            if (allowManagement)
                allUIs.Add(uiType, ui);

            return ui;
        }

        /// <summary>
        /// 创建一个GameObject对象
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="uiEvent"></param>
        /// <param name="parentObj"></param>
        /// <returns></returns>
        private GameObject GetGameObject(XObject parent, AUIEvent uiEvent, Transform parentObj)
        {
            if (uiEvent is null)
                return null;

            string key = uiEvent.Key;
            if (key.IsNullOrEmpty())
                return null;

            if (!parentObj)
            {
                UILayer layer = UILayer.Low;
                if (uiEvent is IUILayer uiLayer)
                {
                    layer = uiLayer.Layer;
                }

                parentObj = GetUILayer(layer);
            }                

            bool isFromPool = uiEvent.IsFromPool;
            GameObject obj = ResourcesManager.Instantiate(parent, key, parentObj, isFromPool);

            return obj;
        }

        private UIEventManager GetUIEventManager()
        {
            return Common.Instance.Get<UIEventManager>();
        }

        public UI Create(string uiType, Transform parentObj, bool awake = true)
        {
            UI ui = CreateInner(uiType, parentObj, false);
            if (awake)
                ObjectHelper.Awake(ui);

            return ui;
        }

        public UI Create<P1>(string uiType, P1 p1, Transform parentObj)
        {
            UI ui = CreateInner(uiType, parentObj, false);
            ObjectHelper.Awake(ui, p1);

            return ui;
        }

        public UI Create(string uiType, UILayer layer)
        {
            Transform parentObj = GetUILayer(layer, UILayer.Low);
            UI ui = CreateInner(uiType, parentObj, true);

            InitUI(ui, layer);
            ObjectHelper.Awake(ui);

            return ui;
        }

        public UI Create<P1>(string uiType, P1 p1, UILayer layer)
        {
            Transform parentObj = GetUILayer(layer, UILayer.Low);
            UI ui = CreateInner(uiType, parentObj, true);

            InitUI(ui, layer);
            ObjectHelper.Awake(ui, p1);

            return ui;
        }

        public UI Create(string uiType)
        {
            UI ui = CreateInner(uiType, null, true);
            if (ui is null)
                return null;

            UILayer layer = UILayer.Low;
            if (GetUIEventManager()?.Get(uiType) is IUILayer uiLayer)
            {
                layer = uiLayer.Layer;
            }

            InitUI(ui, layer);
            ObjectHelper.Awake(ui);

            return ui;
        }

        public UI Create<P1>(string uiType, P1 p1)
        {
            UI ui = CreateInner(uiType, null, true);
            if (ui is null)
                return null;

            UILayer layer = UILayer.Low;
            if (GetUIEventManager()?.Get(uiType) is IUILayer uiLayer)
            {
                layer = uiLayer.Layer;
            }

            InitUI(ui, layer);
            ObjectHelper.Awake(ui, p1);

            return ui;
        }

        /// <summary>
        /// 获取UI层级对象
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public Transform GetUILayer(UILayer layer, UILayer defaultLayer = UILayer.Low)
        {
            allLayers.TryGetValue((int)layer, out Transform parent);
            return parent != null ? parent : allLayers[(int)defaultLayer];
        }

        /// <summary>
        /// 移除UI
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        public bool Remove(string uiType)
        {
            if (allUIs.TryRemove(uiType, out UI child))
            {
                GetUIEventManager()?.OnRemove(child);
                child?.Dispose();

                if (uiStack.Count > 0)
                {
                    string type = uiStack.Peek();
                    if (type == uiType)
                    {
                        uiStack.Pop();
                        while (uiStack.Count > 0)
                        {
                            type = uiStack.Peek();
                            child = Get(type);
                            if (child != null && !child.IsDisposed)
                            {
                                child.OnFocus();
                                break;
                            }
                            else
                            {
                                uiStack.Pop();
                            }
                        }
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取UI
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        public UI Get(string uiType)
        {
            TryGet(uiType, out UI ui);
            return ui;
        }

        /// <summary>
        /// 获取顶层UI
        /// </summary>
        /// <returns></returns>
        public UI GetTop()
        {
            if (uiStack.Count == 0)
                return null;

            string uiType = uiStack.Peek();
            return Get(uiType);
        }

        /// <summary>
        /// 尝试获取一个UI
        /// </summary>
        /// <param name="uiType"></param>
        /// <param name="ui"></param>
        /// <returns></returns>
        public bool TryGet(string uiType, out UI ui)
        {
            return allUIs.TryGetValue(uiType, out ui);
        }

        /// <summary>
        /// 是否包含UI
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        public bool Contain(string uiType)
        {
            return allUIs.ContainsKey(uiType);
        }

        /// <summary>
        /// 清除所有的UI
        /// </summary>
        public void Clear()
        {
            using var list = XList<string>.Create();
            list.AddRange(allUIs.Keys);
            foreach (var uiType in list)
            {
                Remove(uiType);
            }

            allUIs.Clear();
            uiStack.Clear();
        }

        public void Update()
        {
            if (Input.GetButtonDown(CancelKeyCode))
            { 
                UI child = GetTop();
                if (child != null && !child.IsDisposed)
                {
                    child.OnCancel();
                }
            }
        }
    }
}
