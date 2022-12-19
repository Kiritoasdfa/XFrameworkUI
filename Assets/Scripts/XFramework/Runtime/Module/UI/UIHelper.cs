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
        /// 创建一个不受UIManager管理的UI，通常是作为某UI的子UI
        /// </summary>
        /// <param name="uiType"></param>
        /// <param name="parentObj"></param>
        /// <param name="awake">立即初始化</param>
        /// <returns></returns>
        public static UI Create(string uiType, Transform parentObj, bool awake = true)
        {
            UI ui = Common.Instance.Get<UIManager>()?.Create(uiType, parentObj, awake);
            return ui;
        }

        /// <summary>
        /// 创建一个不受UIManager管理的UI，通常是作为某UI的子UI，带一个初始化参数，如果需要更多的参数，可以自行扩展或者使用struct
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
        /// 创建一个受UIManager管理的UI，通常是作为独立UI
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
		/// 创建一个受UIManager管理的UI，通常是作为独立UI，带一个初始化参数，如果需要更多的参数，可以自行扩展或者使用struct
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
        /// 创建一个受UIManager管理的UI，通常是作为独立UI
        /// <para>层级为默认配置层级</para>
        /// </summary>
        /// <param name="uiType"></param>
        /// <returns></returns>
        public static UI Create(string uiType)
        {
            UI ui = Common.Instance.Get<UIManager>()?.Create(uiType);
            return ui;
        }

        /// <summary>
        /// 创建一个受UIManager管理的UI，通常是作为独立UI，带一个初始化参数，如果需要更多的参数，可以自行扩展或者使用struct
        /// <para>层级为默认配置层级</para>
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
