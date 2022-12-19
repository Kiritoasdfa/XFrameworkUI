using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace XFramework
{
    public static class UIExtensions
    {
        /// <summary>
        /// 获取一个UIComponent，如果没有则创建一个(前提是GameObject存在所需的Component)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inherit">继承查找</param>
        /// <returns></returns>
        public static T TakeComponent<T>(this UI self, bool inherit = true) where T : UI.Component, new()
        {
            var comp = self.GetUIComponent<T>(inherit);
            if (comp != null)
                return comp;

            return CreateUIComponent(self, typeof(T)) as T;
        }

        /// <summary>
        /// 获取一个UIComponent，如果没有则创建一个(前提是GameObject存在所需的Component)
        /// </summary>
        /// <param name="self"></param>
        /// <param name="uiCompType"></param>
        /// <returns></returns>
        public static UI.Component TakeComponent(this UI self, Type uiCompType)
        {
            var comp = self.GetUIComponent(uiCompType);
            if (comp != null)
                return comp;

            return CreateUIComponent(self, uiCompType);
        }

        private static UI.Component CreateUIComponent(UI self, Type uiCompType)
        {
            if (!TypesManager.Instance.TryGetUnityCompType(uiCompType, out var unityCompType))
                return null;

            var unityComponent = self.GetComponent(unityCompType);
            if (!unityComponent)
                return null;

            var comp = ObjectFactory.Create<UnityEngine.Component>(uiCompType, unityComponent, true) as UI.Component;
            self.AddUIComponent(comp);

            return comp;
        }
    }
}
