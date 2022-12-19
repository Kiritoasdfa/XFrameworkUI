using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework
{
    public static class LanguageHelper
    {
        private static LanguageManager GetLanguageManager()
        {
            return Common.Instance.Get<LanguageManager>();
        }

        /// <summary>
        /// 多语言的key转换为真实的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            var mgr = GetLanguageManager();
            if (mgr is null)
                return key;

            return mgr.GetValue(key);
        }

        /// <summary>
        /// 多语言转换
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string FormatText(string key, params object[] args)
        {
            var mgr = GetLanguageManager();

            key = mgr.GetValue(key ?? string.Empty);
            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] is string str)
                    {
                        args[i] = mgr.GetValue(str);
                    }
                }
            }

            return string.Format(key, args);
        }
    }
}
