using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework
{
    public abstract class LoopScrollRectComponent : UIComponent<LoopScrollRect>, ILoopScrollRectPrefabKey, LoopScrollDataSource, LoopScrollPrefabSource
    {
        private string key;

        public int TotalCount => this.Get().totalCount;

        public UI Content { get; private set; }

        protected override void EndInitialize()
        {
            base.EndInitialize();
            this.SetDefaultValue();
        }

        protected override void SetParentAfter()
        {
            base.SetParentAfter();

            if (this.Content is null)
            {
                if (this.Get() != null && this.Get().content != null)
                {
                    Content = this.parent.AddChild("Content", this.Get().content.gameObject, true);
                }
            }
            else
            {
                this.parent.AddChild(this.Content);
            }
        }

        protected override void Destroy()
        {
            this.RemoveAllValueChangedListeners();
            this.key = null;
            this.Get().ClearCells();
            this.SetDataSource(null);
            this.SetPrefabSource(null);
            this.Content = null;
            //this.SetTotalCount(0);
            base.Destroy();
        }

        public void SetPrefabSource(LoopScrollPrefabSource prefabSource)
        {
            this.Get().prefabSource = prefabSource;
        }

        public void SetDataSource(LoopScrollDataSource dataSource)
        {
            this.Get().dataSource = dataSource;
        }

        public void SetDefaultValue()
        {
            this.SetDataSource(this);
            this.SetPrefabSource(this);
        }

        public void SetKey(string key)
        {
            this.key = key;
        }

        public void SetTotalCount(int totalCount)
        {
            this.Get().totalCount = totalCount;
        }

        public void RefillCells(int startItem = 0, bool fillViewRect = false, float contentOffset = 0)
        {
            this.Get().RefillCells(startItem, fillViewRect, contentOffset);
        }

        public void RefillCellsFromEnd(int endItem = 0, bool alignStart = false)
        {
            this.Get().RefillCellsFromEnd(endItem, alignStart);
        }

        public void RefreshCells()
        {
            this.Get().RefreshCells();
        }

        public void SetVerticalNormalizedPosition(float value)
        {
            this.Get().verticalNormalizedPosition = value;
        }

        public void SetHorizontalNormalizedPosition(float value)
        {
            this.Get().horizontalNormalizedPosition = value;
        }

        public void AddValueChangedListener(UnityAction<Vector2> action)
        {
            this.Get().AddValueChangedListener(action);
        }

        public void RemoveValueChangedListener(UnityAction<Vector2> action)
        {
            this.Get().RemoveValueChangedListener(action);
        }

        public void RemoveAllValueChangedListeners()
        {
            this.Get().RemoveAllValueChangedListeners();
        }

        public void ValueChangedInvoke(Vector2 v2)
        {
            this.Get().ValueChangedInvoke(v2);
        }

        protected abstract void ProvideData(Transform transform, int index);

        protected abstract void ReturnObject(Transform transform);

        #region Interface

        public string Key => key;

        void LoopScrollDataSource.ProvideData(Transform transform, int idx)
        {
            this.ProvideData(transform, idx);
        }

        GameObject LoopScrollPrefabSource.GetObject(int index)
        {
            if (this.Key.IsNullOrEmpty())
                throw new ArgumentNullException("LoopScrollPrefabSource.GetObject获取对象失败，未设置Key");

            return ResourcesManager.Instantiate(this, this.Key, this.Get().content, true);
        }

        void LoopScrollPrefabSource.ReturnObject(Transform trans)
        {
            this.ReturnObject(trans);
            GameObject obj = trans.gameObject;
            ResourcesManager.ReleaseInstance(obj);
        }

        #endregion
    }

    public class LoopScrollRectComponent<T> : LoopScrollRectComponent where T : UI, new()
    {
        protected ILoopScrollRectProvide<T> provideData;

        protected Dictionary<int, T> children = new Dictionary<int, T>();

        protected override void Destroy()
        {
            base.Destroy();
            this.children.Clear();
            this.provideData = null;
        }

        public void SetProvideData(ILoopScrollRectProvide<T> provideData)
        {
            this.provideData = provideData;
        }

        public void SetProvideData(string key, ILoopScrollRectProvide<T> provideData)
        {
            this.SetKey(key);
            this.SetProvideData(provideData);
        }

        protected void RemoveChild(int instanceId)
        {
            if (this.children.TryRemove(instanceId, out var child))
            {
                child.Dispose();
            }
        }

        #region override

        protected override void ProvideData(Transform transform, int idx)
        {
            GameObject obj = transform.gameObject;
            int instanceId = obj.GetInstanceID();
            this.RemoveChild(instanceId);

            string name = instanceId.ToString();
            T child = UI.Create<T>(name, obj, true);        //这里创建之后并没有执行Awake，如果有需要可以在接收方法里自己调用Awake
            //this.Parent.AddChild(child);
            var list = this.Content.GetList();
            list.AddChild(child);
            this.children.Add(instanceId, child);
            this.provideData.ProvideData(child, idx);
            list.TryAddToDict(child);
        }

        protected override void ReturnObject(Transform trans)
        {
            GameObject obj = trans.gameObject;
            int instanceId = obj.GetInstanceID();
            this.RemoveChild(instanceId);
        }

        #endregion
    }

    public static class LoopScrollRectExtensions
    {
        public static LoopScrollRectComponent<T> GetLoopScrollRect<T>(this ILoopScrollRectProvide<T> self) where T : UI, new()
        {
            if (!(self is UI ui))
                return null;

            return self.GetLoopScrollRect(ui);
        }

        public static LoopScrollRectComponent<T> GetLoopScrollRect<T>(this ILoopScrollRectProvide<T> self, UI ui) where T : UI, new()
        {
            var comp = ui.GetLoopScrollRect<T>();
            return comp;
        }

        public static LoopScrollRectComponent<T> GetLoopScrollRect<T>(this UI self) where T : UI, new()
        {
            var baseComp = self.GetUIComponent<LoopScrollRectComponent>(false);
            if (baseComp != null)
                return baseComp as LoopScrollRectComponent<T>;

            var comp = self.TakeComponent<LoopScrollRectComponent<T>>(true);
            if (comp != null)
            {
                if (self is ILoopScrollRectProvide<T> provide)
                    comp.SetProvideData(provide);
            }

            return comp;
        }

        public static LoopScrollRectComponent<T> GetLoopScrollRect<T>(this UI self, string key) where T : UI, new()
        {
            var ui = self.GetFromKeyOrPath(key);
            return ui?.GetLoopScrollRect<T>();
        }

        public static void AddValueChangedListener(this LoopScrollRect self, UnityAction<Vector2> action)
        {
            if (action is null)
                return;

            self.onValueChanged.AddListener(action);
        }

        public static void RemoveValueChangedListener(this LoopScrollRect self, UnityAction<Vector2> action)
        {
            if (action is null)
                return;

            self.onValueChanged.RemoveListener(action);
        }

        public static void RemoveAllValueChangedListeners(this LoopScrollRect self)
        {
            self.onValueChanged.RemoveAllListeners();
        }

        public static void ValueChangedInvoke(this LoopScrollRect self, Vector2 v2)
        {
            self.onValueChanged.Invoke(v2);
        }
    }
}
