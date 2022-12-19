using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework
{
	[UIEvent(UIType.UIThingItem)]
    internal sealed class UIThingItemEvent : AUIEvent
    {
	    public override string Key => UIPathSet.UIThingItem;

        public override bool IsFromPool => true;

		public override bool AllowManagement => false;

		public override UI OnCreate()
        {
            return UI.Create<UIThingItem>(true);
        }
    }

    public partial class UIThingItem : UIChild, IAwake<int>
	{
		private int thingId;

		public void Initialize(int thingId)
		{
			this.SetId(thingId);
			this.thingId = thingId;
			this.InitView();
		}

		private void InitView()
        {
			string icon = this.thingId % 2 == 0 ? "item copy" : "item";
			this.GetImage(KIcon).SetSprite(icon, true);
		}
		
		protected override void OnClose()
		{
			this.thingId = 0;
			base.OnClose();
		}
	}
}
