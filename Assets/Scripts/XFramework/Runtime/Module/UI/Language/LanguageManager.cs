using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework
{
    /// <summary>
    /// 多语言管理
    /// </summary>
    public sealed class LanguageManager : CommonObject
    {
        private enum LanguageType
        {
            /// <summary>
            /// 简体
            /// </summary>
            CN = 1,
            /// <summary>
            /// 繁体
            /// </summary>
            TC = 2,
            /// <summary>
            /// 英文
            /// </summary>
            EN = 3,
        }

        private const string LanguageKey = "LANGUAGE";

        private LanguageType _languageType;

        public int Language_Type => (int)_languageType;

        protected override void Init()
        {
            base.Init();
            int type = PlayerPrefsHelper.GetInt(LanguageKey, (int)LanguageType.CN);
            _languageType = (LanguageType)type;
        }

        /// <summary>
        /// 获取多语言值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            if (LanguageConfigManager.Instance is null)
                return key;

            if (LanguageConfigManager.Instance.GetConfigByKey(key, out var config))
            {
                switch (_languageType)
                {
                    case LanguageType.CN:
                        return config.CN;
                    case LanguageType.TC:
                        return config.TC;
                    case LanguageType.EN:
                        return config.EN;
                    default:
                        return key;
                }
            }
            else if (GenLanguageConfigManager.Instance.GetConfigByKey(key, out var config1))
            {
                switch (_languageType)
                {
                    case LanguageType.CN:
                        return config1.CN;
                    case LanguageType.TC:
                        return config1.TC;
                    case LanguageType.EN:
                        return config1.EN;
                    default:
                        return key;
                }
            }

            return key;
        }

        /// <summary>
        /// 设置多语言类型
        /// </summary>
        /// <param name="type"></param>
        public void SetLanguageType(int type)
        {
            if ((int)_languageType == type)
                return;

            _languageType = (LanguageType)type;
            PlayerPrefsHelper.SetInt(LanguageKey, type, true);

            foreach (var xt in UIReference.TextList())
            {
                if (xt.Key.IsNullOrEmpty())
                    continue;

                xt.RefreshText();
            }
        }
    }
}
