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
    public class DropdownComponent : UIComponent<Dropdown>
    {
        public int Value => this.Get().value;

        protected override void Destroy()
        {
            base.Destroy();
            RemoveAllClickListeners();
        }

        public void AddOptions(List<string> options)
        {
            this.Get().AddOptions(options);
        }

        public void AddOptions(string[] options)
        {
            using var list = XList<string>.Create();
            list.AddRange(options);
            this.AddOptions(list);
        }

        public void AddOptinos(List<Dropdown.OptionData> options)
        {
            this.Get().AddOptions(options);
        }

        public void AddOptions(List<Sprite> options)
        {
            this.Get().AddOptions(options);
        }

        public void ClearOptions()
        {
            this.Get().ClearOptions();
        }

        public void RefreshShownValue()
        {
            this.Get().RefreshShownValue();
        }

        public void AddClickListener(UnityAction<int> action)
        {
            this.Get().AddClickListener(action);
        }

        public void RemoveClickListener(UnityAction<int> action)
        {
            this.Get().RemoveClickListener(action);
        }

        public void RemoveAllClickListeners()
        {
            this.Get().RemoveAllClickListeners();
        }

        /// <summary>
        /// 直接触发事件，但不会改变value的值
        /// </summary>
        /// <param name="value"></param>
        public void ClickInvoke(int value)
        {
            this.Get().ClickInvoke(value);
        }

        /// <summary>
        /// 设置value但会触发事件
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(int value)
        {
            this.Get().SetValue(value);
        }

        /// <summary>
        /// 设置value但不会触发事件
        /// </summary>
        /// <param name="value"></param>
        public void SetValueWithoutNotify(int value)
        {
            this.Get().SetValueWithoutNotify(value);
        }

        /// <summary>
        /// 设置value，如果没有设置成功则直接触发事件
        /// </summary>
        /// <param name="value"></param>
        public void SetValueOrInvoke(int value)
        {
            this.Get().SetValueOrInvoke(value);
        }
    }

    public static class DropdownExtensions
    {
        public static DropdownComponent GetDropdown(this UI self)
        {
            return self.TakeComponent<DropdownComponent>(true);
        }

        public static DropdownComponent GetDropdown(this UI self, string key)
        {
            var ui = self.GetFromKeyOrPath(key);
            return ui?.GetDropdown();
        }

        public static void AddClickListener(this Dropdown self, UnityAction<int> action)
        {
            if (action == null)
                return;

            self.onValueChanged.AddListener(action);
        }

        public static void RemoveClickListener(this Dropdown self, UnityAction<int> action)
        {
            self.onValueChanged.RemoveListener(action);
        }

        public static void RemoveAllClickListeners(this Dropdown self)
        {
            self.onValueChanged.RemoveAllListeners();
        }

        /// <summary>
        /// 直接触发事件，但不会改变value的值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void ClickInvoke(this Dropdown self, int value)
        {
            self.onValueChanged.Invoke(value);
        }

        public static void SetValue(this Dropdown self, int value)
        {
            self.value = value;
        }

        /// <summary>
        /// 设置value，如果没有设置成功则直接触发事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void SetValueOrInvoke(this Dropdown self, int value)
        {
            if (self.value != value)
            {
                self.SetValue(value);
            }
            else
            {
                self.ClickInvoke(value);
            }
        }
    }
}
