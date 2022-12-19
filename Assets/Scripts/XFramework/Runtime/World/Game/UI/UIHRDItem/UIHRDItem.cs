using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XFramework
{
	[UIEvent(UIType.UIHRDItem)]
    internal sealed class UIHRDItemEvent : AUIEvent
    {
	    public override string Key => UIPathSet.UIHRDItem;

        public override bool IsFromPool => true;

		public override bool AllowManagement => false;

		public override UI OnCreate()
        {
            return UI.Create<UIHRDItem>(true);
        }
    }

    public partial class UIHRDItem : UIChild, IAwake<int>
	{
		private int configId;

		private HRDConfig config;

		private Vector3 offest;

		private Vector2 direction;

		private Vector3 startWorldPosition;

		private MiniTween tween;

		public void Initialize(int configId)
		{
			this.SetId(configId);
			this.configId = configId;
			this.config = HRDConfigManager.Instance.Get(configId);

			this.InitView();
		}

		private void InitView()
        {
			var transform = this.GetRectTransform();
			var size = UIHRD.Size;

			this.GetImage().SetSprite(config.Res, false);
			transform.SetSize(size * config.W, size * config.H);
			transform.SetPivot(Vector2.zero);
			transform.SetAnchorMin(Vector2.zero);
			transform.SetAnchorMax(Vector2.zero);
			transform.SetAnchoredPosition(size * config.X, size * config.Y);

			var eventTrigger = this.GetXEventTrigger();
			eventTrigger.AddListener(EventTriggerType.BeginDrag, this.OnBeginDrag);
			eventTrigger.AddListener(EventTriggerType.Drag, this.OnDrag);
			eventTrigger.AddListener(EventTriggerType.EndDrag, this.OnEndDrag);
		}

		private void OnBeginDrag(PointerEventData eventData)
        {
			//Log.Error($"{eventData.delta.normalized}");
			var dir = eventData.delta;
			var x = Mathf.Abs(dir.x);
			var y = Mathf.Abs(dir.y);

			if (x >= y)
				this.direction = Vector2.right;
			else
				this.direction = Vector2.up;

			var transform = this.GetRectTransform();
			var worldPosition = eventData.pointerCurrentRaycast.worldPosition;
			var selfPosition = transform.Position();
			this.offest = selfPosition - worldPosition;
			this.startWorldPosition = worldPosition;
            this.RootUI<UIHRD>().BeginMove(this.configId, worldPosition);
        }

		private void OnDrag(PointerEventData eventData)
        {
			var transform = this.GetRectTransform();
			var dir = this.direction;
			var worldPosition = eventData.pointerCurrentRaycast.worldPosition;
			var diff = worldPosition - this.startWorldPosition;

			worldPosition = this.startWorldPosition + new Vector3(diff.x * dir.x, diff.y * dir.y, 0);
			var target = this.offest + worldPosition;
			//transform.SetPosition(target);

			if (this.RootUI<UIHRD>().TryMoveGrid(this.configId, worldPosition, out var position))
            {
				this.tween?.Cancel(transform);
				this.tween = transform.DoMove(position, 0.1f);
			}
		}

		private void OnEndDrag(PointerEventData eventData)
		{
			this.RootUI<UIHRD>().EndMove();
        }
		 
		protected override void OnClose()
		{
			this.tween = null;
			this.configId = 0;
			this.config = null;
			base.OnClose();
		}
	}
}
