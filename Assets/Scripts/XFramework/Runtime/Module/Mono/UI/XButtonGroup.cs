using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework
{
    public enum ButtonType
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 单选
        /// </summary>
        Single,
        /// <summary>
        /// 多选
        /// </summary>
        Multiple
    }

    public class XButtonGroup : UIBehaviour
    {
        private abstract class Choice : IDisposable
        {
            public abstract void NotifyButton(bool value, XButton btn, bool sendCallback);

            public virtual void RegisterButton(XButton btn) { }

            public virtual void UnregisterButton(XButtonGroup group, XButton btn) { }

            public virtual void Dispose() { }
        }

        private class SingleChoice : Choice
        {
            private XButton selectButton;

            public override void NotifyButton(bool value, XButton btn, bool sendCallback)
            {
                if (!value)
                    return;

                if (!btn)
                    return;

                if (btn == selectButton)
                    return;

                if (selectButton)
                {
                    if (sendCallback)
                        selectButton.IsOn = false;
                    else
                        selectButton.SetIsOnWithoutNotify(false);
                }

                selectButton = btn;
            }

            public override void RegisterButton(XButton btn)
            {
                if (btn == selectButton)
                    return;

                if (selectButton)
                    return;

                if (btn.transform.GetSiblingIndex() == 0)
                {
                    btn.IsOn = true;
                }
            }

            public override void UnregisterButton(XButtonGroup group, XButton btn)
            {
                if (selectButton == btn)
                {
                    selectButton = null;
                    group.Refresh();
                }
            }

            public bool Check(XButton btn)
            {
                return btn != selectButton;
            }

            public override void Dispose()
            {
                selectButton = null;
            }
        }

        private class MultipleChoice : Choice
        {
            private HashSet<XButton> group = new HashSet<XButton>();

            public override void NotifyButton(bool value, XButton btn, bool sendCallback)
            {

            }

            public override void RegisterButton(XButton btn)
            {
                group.Add(btn);
            }

            public override void UnregisterButton(XButtonGroup btnGroup, XButton btn)
            {
                group.Remove(btn);
            }

            public void SelectAll(bool value, bool sendCallback)
            {
                foreach (var btn in group)
                {
                    if (sendCallback)
                        btn.IsOn = value;
                    else
                        btn.SetIsOnWithoutNotify(value);
                }
            }

            public override void Dispose()
            {
                this.group.Clear();
            }
        }

        [SerializeField]
        private ButtonType m_ButtonType = ButtonType.None;

        private Choice m_Choice;

        private List<XButton> btnList = new List<XButton>();

        /// <summary>
        /// 按钮的类型
        /// </summary>
        public ButtonType ButtonType
        {
            get => this.m_ButtonType;
        }

        protected override void Start()
        {
            base.Start();
            this.Refresh();
        }

        protected override void OnDestroy()
        {
            this.m_ButtonType = ButtonType.None;
            this.m_Choice = null;
            this.btnList.Clear();
            base.OnDestroy();
        }

        private void Refresh()
        {
            bool isFirst = false;
            bool single = this.ButtonType == ButtonType.Single;

            foreach (var btn in this.btnList)
            {
                if (!isFirst && single)
                {
                    if (btn.IsActive() && btn.IsInteractable())
                    {
                        btn.IsOn = true;
                        isFirst = true;
                    }
                }

                if (!isFirst)
                {
                    btn.IsOn = false;
                }
            }
        }

        private Choice GetChoice()
        {
            if (this.m_Choice == null)
            {
                switch (this.m_ButtonType)
                {
                    case ButtonType.None:
                        break;
                    case ButtonType.Single:
                        this.m_Choice = new SingleChoice();
                        break;
                    case ButtonType.Multiple:
                        this.m_Choice = new MultipleChoice();
                        break;
                    default:
                        break;
                }
            }

            return this.m_Choice;
        }

        public void NotifyButton(bool value, XButton btn, bool sendCallback)
        {
            this.GetChoice()?.NotifyButton(value, btn, sendCallback);
        }

        public void RegisterButton(XButton btn)
        {
            if (!this.btnList.Contains(btn))
                this.btnList.Add(btn);

            this.GetChoice()?.RegisterButton(btn);
        }

        public void UnregisterButton(XButton btn)
        {
            this.btnList.Remove(btn);
            this.GetChoice()?.UnregisterButton(this, btn);
        }

        internal bool CheckCanSelect(XButton btn)
        {
            if (this.ButtonType != ButtonType.Single)
                return true;

            var choice = this.GetChoice();
            if (choice == null)
                return true;

            return (choice as SingleChoice).Check(btn);
        }

        /// <summary>
        /// 全选，会触发监听事件
        /// </summary>
        public void SelectAll()
        {
            this.SelectAll(true, true);
        }

        /// <summary>
        /// 全选，不会触发监听事件
        /// </summary>
        public void SelectAllWithoutNotify()
        {
            this.SelectAll(true, false);
        }

        /// <summary>
        /// 全不选，会触发监听事件
        /// </summary>
        public void UnselectAll()
        {
            this.SelectAll(false, true);
        }

        /// <summary>
        /// 全部取消选中，不会触发监听事件
        /// </summary>
        public void UnselectAllWithoutNotify()
        {
            this.SelectAll(false, false);
        }

        private void SelectAll(bool value, bool sendCallback)
        {
            if (this.m_ButtonType != ButtonType.Multiple)
            {
                Log.Error("XButtonGroup不是多选状态，禁止操作多个对象");
                return;
            }

            (this.GetChoice() as MultipleChoice)?.SelectAll(value, sendCallback);
        }
    }
}
