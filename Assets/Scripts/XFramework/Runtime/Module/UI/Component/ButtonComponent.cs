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
    public class ButtonComponent : UIComponent<Button>
    {
        private XButtonGroup defaultGroup;

        protected override void EndInitialize()
        {
            base.EndInitialize();
            if (this.Get() is XButton btn)
            {
                defaultGroup = btn.ButtonGroup;
            }
        }

        protected override void Destroy()
        {
            this.RemoveAllClickListeners();
            if (this.Get() is XButton btn)
            {
                btn.RemoveAllLongPressListeners();
                btn.RemoveAllLongPressEndListeners();
                btn.RemoveAllValueChangedListeners();
                btn.ButtonGroup = this.defaultGroup;
            }
            base.Destroy();
        }

        /// <summary>
        /// 检查是否可以转为XButton，不能转就报错
        /// </summary>
        /// <param name="btn"></param>
        /// <returns></returns>
        protected bool CheckIsXButton(out XButton btn)
        {
            btn = null;
            if (this.Get() is XButton xbtn)
            {
                btn = xbtn;
            }
            else
            {
                Log.Error($"RootUI = {this.parent.RootUIType()}, Name = {this.parent.Name}, Button不能转为XButton");
            }

            return btn != null;
        }

        public XButton AsXButton => this.Get() as XButton;

        /// <summary>
        /// 设置是否可以交互
        /// </summary>
        /// <param name="value"></param>
        public void SetInteractable(bool value)
        {
            this.Get().interactable = value;
        }

        public void AddClickListener(UnityAction action)
        {
            this.Get().AddClickListener(action);
        }

        public void RemoveClickListener(UnityAction action)
        {
            this.Get().RemoveClickListener(action);
        }

        public void RemoveAllClickListeners()
        {
            this.Get().RemoveAllClickListeners();
        }

        /// <summary>
        /// 直接触发点击事件
        /// </summary>
        public void ClickInvoke()
        {
            this.Get().ClickInvoke();
        }

        public void SetGroup(XButtonGroup group)
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.ButtonGroup = group;
            }
        }

        public void SetSelectState(GameObject state)
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.SelectedState = state;
            }
        }

        /// <summary>
        /// 添加长按时的事件监听
        /// </summary>
        /// <param name="action"></param>
        public void AddLongPressListener(UnityAction<int> action)
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.AddLongPressListener(action);
            }
        }

        /// <summary>
        /// 移除长按时的事件监听
        /// </summary>
        /// <param name="action"></param>
        public void RemoveLongPressListener(UnityAction<int> action)
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.RemoveLongPressListener(action);
            }
        }

        /// <summary>
        /// 移除所有长按时的事件监听
        /// </summary>
        public void RemoveAllLongPressListeners()
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.RemoveAllLongPressListeners();
            }
        }

        /// <summary>
        /// 直接触发长按时事件
        /// </summary>
        public void LongPressInvoke(int value)
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.LongPressInvoke(value);
            }
        }

        /// <summary>
        /// 添加长按结束时的事件监听
        /// </summary>
        /// <param name="action"></param>
        public void AddLongPressEndListener(UnityAction<int> action)
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.AddLongPressEndListener(action);
            }
        }

        /// <summary>
        /// 移除长按结束时的事件监听
        /// </summary>
        /// <param name="action"></param>
        public void RemoveLongPressEndListener(UnityAction<int> action)
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.RemoveLongPressEndListener(action);
            }
        }

        /// <summary>
        /// 移除所有长按结束时的事件监听
        /// </summary>
        public void RemoveAllLongPressEndListeners()
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.RemoveAllLongPressEndListeners();
            }
        }

        /// <summary>
        /// 直接出发长按结束的事件监听
        /// </summary>
        /// <param name="value"></param>
        public void LongPressEndInvoke(int value)
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.LongPressEndInvoke(value);
            }
        }

        public void AddValueChangedListener(UnityAction<bool> action)
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.AddValueChangedListener(action);
            }
        }

        public void RemoveValueChangedListener(UnityAction<bool> action)
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.RemoveValueChangedListener(action);
            }
        }

        public void RemoveAllValueChangedListeners()
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.RemoveAllValueChangedListeners();
            }
        }

        public void ValueChangedInvoke(bool value)
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.ValueChangedInvoke(value);
            }
        }

        public void SetIsOn(bool value)
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.SetIsOn(value);
            }
        }

        public void SetIsOnWithoutNotify(bool value)
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.SetIsOnWithoutNotify(value);
            }
        }

        /// <summary>
        /// 设置IsOn，如果没有设置成功则直接触发事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public void SetIsOnOrInvoke(bool value)
        {
            if (this.CheckIsXButton(out var btn))
            {
                btn.SetIsOnOrInvoke(value);
            }
        }
    }

    public static class UIButtonExtensions
    {
        public static ButtonComponent GetButton(this UI self)
        {
            return self.TakeComponent<ButtonComponent>(true);
        }

        public static ButtonComponent GetButton(this UI self, string key)
        {
            UI ui = self.GetFromKeyOrPath(key);
            return ui?.GetButton();
        }

        public static void AddClickListener(this Button self, UnityAction action)
        {
            if (action is null)
                return;

            self.onClick.AddListener(action);
        }

        public static void RemoveClickListener(this Button self, UnityAction action)
        {
            if (action is null)
                return;

            self.onClick.RemoveListener(action);
        }

        public static void RemoveAllClickListeners(this Button self)
        {
            self.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// 直接触发点击事件
        /// </summary>
        /// <param name="self"></param>
        public static void ClickInvoke(this Button self)
        {
            self.onClick.Invoke();
        }

        /// <summary>
        /// 添加长按时的事件监听
        /// </summary>
        /// <param name="self"></param>
        /// <param name="action"></param>
        public static void AddLongPressListener(this XButton self, UnityAction<int> action)
        {
            if (action is null)
                return;

            self.onLongPress.AddListener(action);
        }

        /// <summary>
        /// 移除长按时的事件监听
        /// </summary>
        /// <param name="self"></param>
        /// <param name="action"></param>
        public static void RemoveLongPressListener(this XButton self, UnityAction<int> action)
        {
            if (action is null)
                return;

            self.onLongPress.RemoveListener(action);
        }

        /// <summary>
        /// 移除所有长按时的事件监听
        /// </summary>
        /// <param name="self"></param>
        public static void RemoveAllLongPressListeners(this XButton self)
        {
            self.onLongPress.RemoveAllListeners();
        }

        /// <summary>
        /// 直接触发长按时事件
        /// </summary>
        /// <param name="self"></param>
        public static void LongPressInvoke(this XButton self, int value)
        {
            self.onLongPress.Invoke(value);
        }

        /// <summary>
        /// 添加长按结束时的事件监听
        /// </summary>
        /// <param name="self"></param>
        /// <param name="action"></param>
        public static void AddLongPressEndListener(this XButton self, UnityAction<int> action)
        {
            if (action is null)
                return;

            self.onLongPressEnd.AddListener(action);
        }

        /// <summary>
        /// 移除长按结束时的事件监听
        /// </summary>
        /// <param name="self"></param>
        /// <param name="action"></param>
        public static void RemoveLongPressEndListener(this XButton self, UnityAction<int> action)
        {
            if (action is null)
                return;

            self.onLongPressEnd.RemoveListener(action);
        }

        /// <summary>
        /// 移除所有长按结束时的事件监听
        /// </summary>
        /// <param name="self"></param>
        public static void RemoveAllLongPressEndListeners(this XButton self)
        {
            self.onLongPressEnd.RemoveAllListeners();
        }

        /// <summary>
        /// 直接出发长按结束的事件监听
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void LongPressEndInvoke(this XButton self, int value)
        {
            self.onLongPressEnd.Invoke(value);
        }

        public static void AddValueChangedListener(this XButton self, UnityAction<bool> action)
        {
            if (action is null)
                return;

            self.onValueChanged.AddListener(action);
        }

        public static void RemoveValueChangedListener(this XButton self, UnityAction<bool> action)
        {
            if (action is null)
                return;

            self.onValueChanged.RemoveListener(action);
        }

        public static void RemoveAllValueChangedListeners(this XButton self)
        {
            self.onValueChanged.RemoveAllListeners();
        }

        public static void ValueChangedInvoke(this XButton self, bool value)
        {
            self.onValueChanged.Invoke(value);
        }

        public static void SetIsOn(this XButton self, bool value)
        {
            self.IsOn = value;
        }

        public static void SetIsOnWithoutNotify(this XButton self, bool value)
        {
            self.SetIsOnWithoutNotify(value);
        }

        /// <summary>
        /// 设置IsOn，如果没有设置成功则直接触发事件
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void SetIsOnOrInvoke(this XButton self, bool value)
        {
            if (self.IsOn != value)
            {
                self.SetIsOn(value);
            }
            else
            {
                self.ValueChangedInvoke(value);
            }
        }
    }
}
