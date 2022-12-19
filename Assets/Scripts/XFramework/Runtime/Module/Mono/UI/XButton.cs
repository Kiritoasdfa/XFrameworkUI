using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework
{
    public class XButton : Button
    {
        [System.Serializable]
        public class ButtonLongPressEvent : UnityEvent<int>
        {
            
        }

        [System.Serializable]
        public class ButtonValueEvent : UnityEvent<bool>
        {

        }

        /// <summary>
        /// ��ǰ���۵ļ��ʱ��
        /// </summary>
        private float m_CurLongPressInterval = 0;

        /// <summary>
        /// ��������
        /// </summary>
        private int m_LongPressCount = 0;

        /// <summary>
        /// ��󳤰�����
        /// </summary>
        [SerializeField]
        private int m_MaxLongPressCount = 0;

        /// <summary>
        /// �������ʱ��
        /// </summary>
        [SerializeField, Min(0.01f)]
        private float m_LongPressInterval = 0.3f;

        /// <summary>
        /// ����
        /// </summary>
        private bool m_IsPointerDown = false;

        /// <summary>
        /// �����������¼��Ƿ�ִ�й�
        /// </summary>
        private bool m_IsLongPressEndExecuted = false;

        /// <summary>
        /// �Ƿ񼤻���º�û���뿪������û��̧��
        /// </summary>
        private bool m_IsPointerActive = false;

        #region Group

        /// <summary>
        /// ��ѡ/��ѡʱʹ��
        /// </summary>
        [SerializeField]
        private bool m_IsOn = false;

        /// <summary>
        /// ��ť��
        /// </summary>
        [SerializeField]
        private XButtonGroup m_ButtonGroup = null;

        /// <summary>
        /// ѡ��״̬
        /// </summary>
        [SerializeField]
        private GameObject m_SelectedState = null;

        /// <summary>
        /// ѡ��״̬������
        /// </summary>
        private CanvasGroup m_SelectedCanvasGroup = null;

        #endregion

        /// <summary>
        /// ��ť����״̬��������֧�ֳ���
        /// </summary>
        public bool IsPointerActive
        {
            get
            {
                return m_IsPointerActive && m_IsPointerDown && (m_MaxLongPressCount >= 0 ? m_LongPressCount < m_MaxLongPressCount : true);
            }
            set
            {
                if (m_IsPointerActive != value)
                {
                    m_IsPointerActive = value;
                    if (value)
                    {
                        m_CurLongPressInterval = 0;
                        m_LongPressCount = 0;
                        m_IsLongPressEndExecuted = false;
                    }
                }
            }
        }

        /// <summary>
        /// �Ƿ��ڳ���
        /// </summary>
        public bool IsLongPress => m_LongPressCount > 0;

        /// <summary>
        /// ��������
        /// </summary>
        public int LongPresCount => m_LongPressCount;

        /// <summary>
        /// ��󳤰�����, С��0Ϊ���޴�
        /// </summary>
        public int MaxLongPressCount
        {
            get => m_MaxLongPressCount;
            set => m_MaxLongPressCount = value;
        }

        /// <summary>
        /// �������
        /// </summary>
        public float LongPressInterval
        {
            get => m_LongPressInterval;
            set
            {
                m_LongPressInterval = System.Math.Max(value, 0.01f);
            }
        }

        #region Group

        /// <summary>
        /// ��ť��
        /// </summary>
        public XButtonGroup ButtonGroup
        {
            get => m_ButtonGroup;
            set
            {
                if (m_ButtonGroup != value)
                {
                    if (m_ButtonGroup != null)
                        m_ButtonGroup.UnregisterButton(this);

                    m_ButtonGroup = value;
                    if (m_ButtonGroup == null)
                        return;

                    m_ButtonGroup.RegisterButton(this);
                    if (m_ButtonGroup.IsActive() && m_IsOn)
                        m_ButtonGroup.NotifyButton(m_IsOn, this, true);
                }
            }
        }

        /// <summary>
        /// ��ѡ/��ѡ״̬ʱ��ѡ��״̬
        /// </summary>
        public bool IsOn
        {
            get => m_IsOn;
            set
            {
                ValueChanged(value, true);
            }
        }

        /// <summary>
        /// ѡ��״̬
        /// </summary>
        public GameObject SelectedState
        {
            get => m_SelectedState;
            set
            {
                if (value != m_SelectedState)
                {
                    m_SelectedState = value;
                }
            }
        }

        protected CanvasGroup SelectedCanvasGroup
        {
            get
            {
                if (m_SelectedCanvasGroup == null)
                {
                    if (m_SelectedState != null)
                    {
                        if (!m_SelectedState.TryGetComponent(out m_SelectedCanvasGroup))
                            m_SelectedCanvasGroup = m_SelectedState.gameObject.AddComponent<CanvasGroup>();
                    }
                }

                return m_SelectedCanvasGroup;
            }
        }

        #endregion

        [UnityEngine.Serialization.FormerlySerializedAs("onLongPress")]
        [SerializeField]
        private ButtonLongPressEvent m_OnLongPress = new ButtonLongPressEvent();

        [UnityEngine.Serialization.FormerlySerializedAs("onLongPressEnd")]
        [SerializeField]
        private ButtonLongPressEvent m_OnLongPressEnd = new ButtonLongPressEvent();

        [UnityEngine.Serialization.FormerlySerializedAs("onValueChanged")]
        [SerializeField]
        private ButtonValueEvent m_OnValueChanged = new ButtonValueEvent();

        /// <summary>
        /// ����ʱִ�е��¼�
        /// �����ĳ����Ĵ���
        /// </summary>
        public ButtonLongPressEvent onLongPress
        {
            get => m_OnLongPress;
            set => m_OnLongPress = value;
        }

        /// <summary>
        /// ����������ִ�е��¼�
        /// �������ܳ�������
        /// </summary>
        public ButtonLongPressEvent onLongPressEnd
        {
            get => m_OnLongPressEnd;
            set => m_OnLongPressEnd = value;
        }

        /// <summary>
        /// ��ѡ/��ѡ����¼�����
        /// </summary>
        public ButtonValueEvent onValueChanged
        {
            get => m_OnValueChanged;
            set => m_OnValueChanged = value;
        }

        protected override void Awake()
        {
            base.Awake();

            m_IsOn = false;
            SelectedStateChanged();
            if (m_ButtonGroup)
            {
                m_ButtonGroup.RegisterButton(this);
            }
        }

        protected virtual void Update()
        {
            if (!IsPointerActive)
                return;

            float interval = Time.unscaledDeltaTime;
            m_CurLongPressInterval += interval;
            if (m_CurLongPressInterval >= LongPressInterval)
            {
                m_CurLongPressInterval = 0;
                ++m_LongPressCount;
                LongPress();
            }
        }

        protected override void OnDestroy()
        {
            if (m_ButtonGroup)
                m_ButtonGroup.UnregisterButton(this);

            base.OnDestroy();
        }

        #region Group

        /// <summary>
        /// ����ѡ��״̬�������������¼�
        /// </summary>
        /// <param name="value"></param>
        public void SetIsOnWithoutNotify(bool value)
        {
            ValueChanged(value, false);
        }

        #endregion

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (m_LongPressCount > 0)
            {
                LongPressEnd();
                return;
            }

            ValueChanged();
            base.OnPointerClick(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            IsPointerActive = false;
            if (m_LongPressCount > 0)
            {
                LongPressEnd();
            }
            base.OnPointerExit(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            IsPointerActive = false;
            m_IsPointerDown = false;
            base.OnPointerUp(eventData);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            IsPointerActive = true;
            m_IsPointerDown = true;
            base.OnPointerDown(eventData);
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        private void LongPress()
        {
            if (IsActive() && IsInteractable())
            {
                m_OnLongPress.Invoke(m_LongPressCount);
            }
        }

        /// <summary>
        /// ���������¼�
        /// </summary>
        private void LongPressEnd()
        {
            m_LongPressCount = 0;
            if (!m_IsLongPressEndExecuted && IsActive() && IsInteractable())
            {
                m_OnLongPressEnd.Invoke(m_LongPressCount);
                m_IsLongPressEndExecuted = true;
            }
        }

        #region Group

        private void ValueChanged()
        {
            if (IsActive() && IsInteractable())
            {
                if (m_ButtonGroup != null && m_ButtonGroup.IsActive())
                {
                    if (m_ButtonGroup.ButtonType == ButtonType.Single)
                    {
                        IsOn = true;
                    }
                    else
                    {
                        IsOn = !IsOn;
                    }
                }

            }
        }

        /// <summary>
        /// ѡ��״̬�л�
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sendCallback"></param>
        private void ValueChanged(bool value, bool sendCallback)
        {
            if (!IsActive() || !IsInteractable())
                return;

            if (m_IsOn == value)
                return;

            m_IsOn = value;
            if (m_ButtonGroup != null && m_ButtonGroup.IsActive())
            {
                m_ButtonGroup.NotifyButton(value, this, sendCallback);
            }

            SelectedStateChanged();
            if (sendCallback)
                onValueChanged.Invoke(m_IsOn);
        }

        /// <summary>
        /// ѡ��/δѡ��״̬�л�
        /// </summary>
        protected virtual void SelectedStateChanged()
        {
            var canvasGroup = SelectedCanvasGroup;
            if (!canvasGroup)
                return;

            canvasGroup.SetViewActive(m_IsOn);
        }

        #endregion
    }
}
