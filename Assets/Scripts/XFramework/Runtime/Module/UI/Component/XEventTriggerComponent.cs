using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace XFramework
{
    public class XEventTriggerComponent : UComponent<XEventTrigger>
    {
        protected override void Destroy()
        {
            this.RemoveAllListeners();
            base.Destroy();
        }

        public void AddListener(EventTriggerType triggerType, UnityAction<PointerEventData> action)
        {
            this.Get().AddListener(triggerType, action);
        }

        public void RemoveListener(EventTriggerType triggerType, UnityAction<PointerEventData> action)
        {
            this.Get().RemoveListener(triggerType, action);
        }

        public void RemoveListener(EventTriggerType triggerType)
        {
            this.Get().RemoveListener(triggerType);
        }

        public void RemoveAllListeners()
        {
            this.Get().RemoveAllListeners();
        }
    }

    public static class EventTriggerExtensions
    {
        public static XEventTriggerComponent GetXEventTrigger(this UI self)
        {
            return self.TakeComponent<XEventTriggerComponent>(true);
        }

        public static XEventTriggerComponent GetXEventTrigger(this UI self, string key)
        {
            UI ui = self.GetFromKeyOrPath(key);
            return ui?.GetXEventTrigger();
        }
    }
}
