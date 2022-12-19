using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework
{
    public static class UIHelper
    {
        /// <summary>
        /// ����һ������UIManager�����UI��ͨ������ΪĳUI����UI
        /// </summary>
        /// <param name="uiType"></param>
        /// <param name="parentObj"></param>
        /// <param name="awake">������ʼ��</param>
        /// <returns></returns>
        public static UI Create(string uiType, Transform parentObj, bool awake = true)
        {
            UI ui = Common.Instance.Get<UIManager>()?.Create(uiType, parentObj, awake);
            return ui;
        }

        /// <summary>
        /// ����һ������UIManager�����UI��ͨ������ΪĳUI����UI����һ����ʼ�������������Ҫ����Ĳ���������������չ����ʹ��struct
        /// </summary>
        /// <typeparam name="P1"></typeparam>
        /// <param name="uiType"></param>
        /// <param name="p1"></param>
        /// <param name="parentObj"></param>
        /// <returns></returns>
        public static UI Create<P1>(string uiType, P1 p1, Transform parentObj)
        {
            UI ui = Common.Instance.Get<UIManager>()?.Create(uiType, p1, parentObj);
            return ui;
        }

        /// <summary>
        /// ����һ����UIManager�����UI��ͨ������Ϊ����UI
        /// </summary>
        /// <param name="uiType"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static UI Create(string uiType, UILayer layer)
        {
            UI ui = Common.Instance.Get<UIManager>()?.Create(uiType, layer);
            return ui;
        }

        /// <summary>
		/// ����һ����UIManager�����UI��ͨ������Ϊ����UI����һ����ʼ�������������Ҫ����Ĳ���������������չ����ʹ��struct
        /// </summary>
        /// <typeparam name="P1"></typeparam>
        /// <param name="uiType"></param>
        /// <param name="p1"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static UI Create<P1>(string uiType, P1 p1, UILayer layer)
        {
            UI ui = Common.Instance.Get<UIManager>()?.Create(uiType, p1, layer);
            return ui;
        }

        /// <summary>
        /// ����һ����UIManager�����UI��ͨ������Ϊ����UI
        /// <para>�㼶ΪĬ�����ò㼶</para>
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        public static UI Create(string uiType)
        {
            UI ui = Common.Instance.Get<UIManager>()?.Create(uiType);
            return ui;
        }

        /// <summary>
        /// ����һ����UIManager�����UI��ͨ������Ϊ����UI����һ����ʼ�������������Ҫ����Ĳ���������������չ����ʹ��struct
        /// <para>�㼶ΪĬ�����ò㼶</para>
        /// </summary>
        /// <typeparam name="P1"></typeparam>
        /// <param name="uiType"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public static UI Create<P1>(string uiType, P1 p1)
        {
            UI ui = Common.Instance.Get<UIManager>()?.Create(uiType, p1);
            return ui;
        }

        public static void Remove(string uiType)
        {
            Common.Instance.Get<UIManager>()?.Remove(uiType);
        }

        public static void Clear()
        {
            Common.Instance.Get<UIManager>()?.Clear();
        }

        #region UIComponent

        public static void SetEnabled(this IUIBehaviour self, bool enabled)
        {
            self.UIBehaviour.enabled = enabled;
        }

        public static bool GetEnabled(this IUIBehaviour self)
        {
            return self.UIBehaviour.enabled;
        }

        public static bool IsActiveAndEnabled(this IUIBehaviour self)
        {
            return self.UIBehaviour.isActiveAndEnabled;
        }

        #endregion
    }
}
