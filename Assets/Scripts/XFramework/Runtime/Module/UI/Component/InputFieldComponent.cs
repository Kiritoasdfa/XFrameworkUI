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
    public class InputFieldComponent : UIComponent<InputField>
    {
        public string Content => this.Get().text;

        protected override void Destroy()
        {
            this.RemoveAllValueChangedListeners();
            this.RemoveAllEndEditListeners();
            base.Destroy();
        }

        public void SetText(string value)
        {
            this.Get().SetText(value);
        }

        public void SetText(string key, params object[] args)
        {
            this.Get().SetText(key, args);
        }

        public void AddValueChangedListener(UnityAction<string> action)
        {
            this.Get().AddValueChangedListener(action);
        }

        public void RemoveValueChangedListener(UnityAction<string> action)
        {
            this.Get().RemoveValueChangedListener(action);
        }

        public void RemoveAllValueChangedListeners()
        {
            this.Get().RemoveAllValueChangedListeners();
        }

        public void AddEndEditListener(UnityAction<string> action)
        {
            this.Get().AddEndEditListener(action);
        }

        public void RemoveEndEditListener(UnityAction<string> action)
        {
            this.Get().RemoveEndEditListener(action);
        }

        public void RemoveAllEndEditListeners()
        {
            this.Get().RemoveAllEndEditListeners();
        }
    }

    public static class UIInputFieldExtensions
    {
        public static InputFieldComponent GetInputField(this UI self)
        {
            return self.TakeComponent<InputFieldComponent>(true);
        }

        public static InputFieldComponent GetInputField(this UI self, string key)
        {
            UI ui = self.GetFromKeyOrPath(key);
            return ui?.GetInputField();
        }

        public static void AddValueChangedListener(this InputField self, UnityAction<string> action)
        {
            if (action == null)
                return;

            self.onValueChanged.AddListener(action);
        }

        public static void RemoveValueChangedListener(this InputField self, UnityAction<string> action)
        {
            self.onValueChanged.RemoveListener(action);
        }

        public static void RemoveAllValueChangedListeners(this InputField self)
        {
            self.onValueChanged.RemoveAllListeners();
        }

        public static void AddEndEditListener(this InputField self, UnityAction<string> action)
        {
            if (action == null)
                return;

            self.onEndEdit.AddListener(action);
        }

        public static void RemoveEndEditListener(this InputField self, UnityAction<string> action)
        {
            self.onEndEdit.RemoveListener(action);
        }

        public static void RemoveAllEndEditListeners(this InputField self)
        {
            self.onEndEdit.RemoveAllListeners();
        }

        public static void SetText(this InputField self, string value)
        {
            self.text = value;
        }

        public static void SetText(this InputField self, string key, params object[] args)
        {
            self.text = string.Format(key, args);
        }
    }
}
