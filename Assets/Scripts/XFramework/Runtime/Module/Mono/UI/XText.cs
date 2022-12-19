using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework
{
    public class XText : Text
    {
        [Tooltip("���Զ�����")]
        [SerializeField]
        private bool m_IgnoreLanguage;

        [Tooltip("�����Ե�Key")]
        [SerializeField]
        private string m_Key = string.Empty;

        private List<object> _objs = new List<object>();

        private object[] _args;

        private int _instanceId;

        /// <summary>
        /// ������key
        /// </summary>
        public string Key => m_Key.Trim();

        /// <summary>
        /// �����Բ���
        /// </summary>
        public object[] Args =>  _args ?? _objs.ToArray();

        /// <summary>
        /// ���Զ�����
        /// </summary>
        public bool IgnoreLanguage => m_IgnoreLanguage;

        protected override void Awake()
        {
            base.Awake();
            _instanceId = gameObject.GetInstanceID();
            m_Key = m_Key.Trim();
            
            if (!m_IgnoreLanguage)
                UIReference.AddText(_instanceId, this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (!m_IgnoreLanguage)
                UIReference.RemoveText(_instanceId);
        }

        public void SetKey(string key, params object[] args)
        {
            if (m_IgnoreLanguage)
                return;

            m_Key = key?.Trim();
            _args = null;

            if (args != null && args.Length == 0)
                _args = System.Array.Empty<object>();

            _objs.Clear();
            _objs.AddRange(args);
        }
    }
}
