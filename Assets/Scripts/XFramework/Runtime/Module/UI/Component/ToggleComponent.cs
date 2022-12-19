using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework
{
    public class ToggleComponent : UIComponent<Toggle>
    {
        public bool IsOn => this.Get().isOn;

        protected override void Destroy()
        {
            this.RemoveAllClickListeners();
            base.Destroy();
        }

        public void AddClickListener(UnityAction<bool> action)
        {
            this.Get().AddClickListener(action);
        }

        public void RemoveClickListener(UnityAction<bool> action)
        {
            this.Get().RemoveClickListener(action);
        }

        public void RemoveAllClickListeners()
        {
            this.Get().RemoveAllClickListeners();
        }

        /// <summary>
        /// 直接触发事件，但不会改变isOn的值
        /// </summary>
        /// <param name="value"></param>
        public void ClickInvoke(bool value)
        {
            this.Get().ClickInvoke(value);
        }

        /// <summary>
        /// 设置IsOn，如果设置失败则直接触发事件
        /// </summary>
        /// <param name="value"></param>
        public void SetIsOnOrInvoke(bool value)
        {
            this.Get().SetIsOnOrInvoke(value);
        }

        /// <summary>
        /// 设置IsOn
        /// <para>如果设置成功并且你添加了onValueChanged事件则也会触发</para>
        /// </summary>
        /// <param name="value"></param>
        public void SetIsOn(bool value)
        {
            this.Get().SetIsOn(value);
        }

        /// <summary>
        /// 设置IsOn但不会触发事件
        /// </summary>
        /// <param name="value"></param>
        public void SetIsOnWithoutNotify(bool value)
        {
            this.Get().SetIsOnWithoutNotify(value);
        }
    }

    public static class UIToggleExtensions
    {
        public static ToggleComponent GetToggle(this UI self)
        {
            return self.TakeComponent<ToggleComponent>(true);
        }

        public static ToggleComponent GetToggle(this UI self, string key)
        {
            UI ui = self.GetFromKeyOrPath(key);
            return ui?.GetToggle();
        }

        public static void AddClickListener(this Toggle self, UnityAction<bool> action)
        {
            if (action == null)
                return;

            self.onValueChanged.AddListener(action);
        }

        public static void RemoveClickListener(this Toggle self, UnityAction<bool> action)
        {
            self.onValueChanged.RemoveListener(action);
        }

        public static void RemoveAllClickListeners(this Toggle self)
        {
            self.onValueChanged.RemoveAllListeners();
        }

        /// <summary>
        /// 直接触发事件，但不会改变isOn的值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void ClickInvoke(this Toggle self, bool value)
        {
            self.onValueChanged.Invoke(value);
        }

        public static void SetIsOn(this Toggle self, bool value)
        {
            self.isOn = value;
        }

        /// <summary>
        /// 设置isOn，如果没有设置成功则直接触发事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void SetIsOnOrInvoke(this Toggle self, bool value)
        {
            if (self.isOn != value)
            {
                self.SetIsOn(value);
            }
            else
            {
                self.ClickInvoke(value);
            }
        }
    }
}
