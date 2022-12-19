using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XFramework
{
    public interface IUIBehaviour
    {
        UIBehaviour UIBehaviour { get; }
    }

    public abstract class UComponent : UI.Component, IAwake<UnityEngine.Component>
    {
        public abstract void Initialize(Component comp);
    }

    [UIComponentFlag]
    public abstract class UComponent<T> : UComponent where T : UnityEngine.Component
    {
        protected T unityComponent;

        public sealed override void Initialize(Component comp)
        {
            this.unityComponent = comp as T;
            this.EndInitialize();
        }

        protected virtual void EndInitialize()
        {
            
        }

        public T Get() => this.unityComponent;

        protected sealed override void OnDestroy()
        {
            this.Destroy();
            this.unityComponent = null;
            base.OnDestroy();
        }

        protected virtual void Destroy()
        {

        }
    }

    public abstract class UIComponent<T> : UComponent<T>, IUIBehaviour where T : UIBehaviour
    {
        UIBehaviour IUIBehaviour.UIBehaviour => this.unityComponent;

        public RectTransformComponent RectTransform => parent?.GetRectTransform();
    }
}
