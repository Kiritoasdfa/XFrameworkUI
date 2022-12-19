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
    public class SliderComponent : UIComponent<Slider>
    {
        public float Value => this.Get().value;

        protected override void Destroy()
        {
            this.RemoveAllValueListeners();
            base.Destroy();
        }

        public void AddValueListener(UnityAction<float> action)
        {
            this.Get().AddValueListener(action);
        }

        public void RemoveValueListener(UnityAction<float> action)
        {
            this.Get().RemoveValueListener(action);
        }

        public void RemoveAllValueListeners()
        {
            this.Get().RemoveAllValueListeners();
        }

        /// <summary>
        /// 直接触发事件，但不会改变value的值
        /// </summary>
        /// <param name="value"></param>
        public void ValueInvoke(float value)
        {
            this.Get().ValueInvoke(value);
        }

        /// <summary>
        /// 设置value，同时会触发onValueChanged事件
        /// </summary>
        /// <param name="value"></param>
        public void SetValue(float value)
        {
            this.Get().SetValue(value);
        }

        /// <summary>
        /// 设置value，并不会触发onValueChanged事件
        /// </summary>
        /// <param name="value"></param>
        public void SetValueWithoutNotify(float value)
        {
            this.Get().SetValueWithoutNotify(value);
        }
    }

    public static class UISliderExtensions
    {
        public static SliderComponent GetSlider(this UI self)
        {
            return self.TakeComponent<SliderComponent>(true);
        }

        public static SliderComponent GetSlider(this UI self, string key)
        {
            UI ui = self.GetFromKeyOrPath(key);
            return ui?.GetSlider();
        }

        public static void AddValueListener(this Slider self, UnityAction<float> action)
        {
            if (action == null)
                return;

            self.onValueChanged.AddListener(action);
        }

        public static void RemoveValueListener(this Slider self, UnityAction<float> action)
        {
            self.onValueChanged.RemoveListener(action);
        }

        public static void RemoveAllValueListeners(this Slider self)
        {
            self.onValueChanged.RemoveAllListeners();
        }

        /// <summary>
        /// 直接触发事件，但不会改变value的值
        /// </summary>
        /// <param name="self"></param>
        /// <param name="value"></param>
        public static void ValueInvoke(this Slider self, float value)
        {
            self.onValueChanged.Invoke(value);
        }

        public static void SetValue(this Slider self, float value)
        {
            self.value = value;
        }
    }
}
