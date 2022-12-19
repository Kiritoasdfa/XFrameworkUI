using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace XFramework
{
    public class XEventTrigger : EventTrigger       
    {
        private Dictionary<EventTriggerType, UnityAction<PointerEventData>> m_delegates = new Dictionary<EventTriggerType, UnityAction<PointerEventData>>();

        public void AddListener(EventTriggerType triggerType, UnityAction<PointerEventData> action)
        {
            if (action == null)
                return;

            if (!m_delegates.TryGetValue(triggerType, out var o))
            {
                o = action;
                m_delegates.Add(triggerType, o);
            }
            else
            {
                o += action;
            }
        }

        public void RemoveListener(EventTriggerType triggerType, UnityAction<PointerEventData> action)
        {
            if (action == null)
                return;

            if (m_delegates.TryGetValue(triggerType, out var o))
            {
                o -= action;
            }
        }

        public void RemoveListener(EventTriggerType triggerType)
        {
            m_delegates.Remove(triggerType);
        }

        public void RemoveAllListeners()
        {
            m_delegates.Clear();
        }

        public override void OnScroll(PointerEventData eventData)
        {
            this.Call(EventTriggerType.Scroll, eventData);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            this.Call(EventTriggerType.BeginDrag, eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            this.Call(EventTriggerType.Drag, eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            this.Call(EventTriggerType.EndDrag, eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            this.Call(EventTriggerType.PointerClick, eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            this.Call(EventTriggerType.PointerUp, eventData);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            this.Call(EventTriggerType.PointerDown, eventData);
        }

        private void Call(EventTriggerType trye, PointerEventData eventData)
        {
            if (m_delegates.TryGetValue(trye, out var action))
                action?.Invoke(eventData);
        }

        private void OnDestroy()
        {
            m_delegates.Clear();
        }
    }
}
