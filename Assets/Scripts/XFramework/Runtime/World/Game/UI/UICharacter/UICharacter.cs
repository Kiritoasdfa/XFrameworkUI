using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework
{
	[UIEvent(UIType.UICharacter)]
    internal sealed class UICharacterEvent : AUIEvent, IUILayer
    {
	    public override string Key => UIPathSet.UICharacter;

        public override bool IsFromPool => true;

        public override bool AllowManagement => true;

        public UILayer Layer => UILayer.Mid;

        public override UI OnCreate()
        {
            return UI.Create<UICharacter>();
        }
    }

    public partial class UICharacter : UI, IAwake
	{
        private readonly Type[] optionTypes = { typeof(UICharacterThingList), typeof(UICharacterEquipList) };

		public void Initialize()
		{
			this.GetButton(KBtnExit)?.AddClickListener(this.Close);
			this.InitView();
		}

		private void InitView()
        {
            var option = this.GetFromReference(KOptions);
            var optionList = option.GetList();

            for (int i = 0; i < 2; i++)
            {
                int index = i;
                Transform child = optionList.Get().GetChild(i);
                var ui = optionList.Create(child.gameObject, true);

                ui.GetToggle().AddClickListener(isOn =>
                {
                    if (isOn)
                    {
                        this.RemoveChild(KItemView);
                        Type optionType = this.optionTypes[index];
                        this.AddChild(optionType, KItemView, true);
                    }
                });
            }

            optionList.GetChildAt(0).GetToggle().SetIsOnOrInvoke(true);
        }
		
		protected override void OnClose()
		{
			base.OnClose();
		}
    }
}
