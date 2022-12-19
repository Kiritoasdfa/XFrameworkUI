using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework
{
	[UIEvent(UIType.UIEquipItem)]
    internal sealed class UIEquipItemEvent : AUIEvent
    {
	    public override string Key => UIPathSet.UIEquipItem;

        public override bool IsFromPool => true;

		public override bool AllowManagement => false;

		public override UI OnCreate()
        {
            return UI.Create<UIEquipItem>(true);
        }
    }

    public partial class UIEquipItem : UIChild, IAwake<int>
	{
		private int equipId;

		public void Initialize(int thingId)
		{
			this.SetId(thingId);
			this.equipId = thingId;
			this.InitView();
		}

		private void InitView()
		{
			string icon = this.equipId % 2 == 0 ? "Layer 207" : "Layer 208";
			this.GetImage(KIcon).SetSprite(icon, true);
		}

		protected override void OnClose()
		{
			this.equipId = 0;
			base.OnClose();
		}
	}
}
