using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static XFramework.UIMain;

namespace XFramework
{
	[UIEvent(UIType.UIMain)]
    internal sealed class UIMainEvent : AUIEvent, IUILayer
    {
        public override string Key => UIPathSet.UIMain;

        public override bool IsFromPool => true;

        public override bool AllowManagement => true;

        public UILayer Layer => UILayer.Low;

        public override UI OnCreate()
        {
            return UI.Create<UIMain>();
        }
    }

    public partial class UIMain : UI, IAwake
	{	
		public void Initialize()
		{
            this.GetButton(KSetting)?.AddClickListener(this.OpenSetting);
            this.GetButton(KQuit)?.AddClickListener(this.GoStartScene);
            this.GetButton(KLeftIcon)?.AddClickListener(this.OpenCharactor);
		}

        private void OpenSetting()
        {
            UIHelper.Create(UIType.UISetting);
        }

        private void OpenCharactor()
        {
            UIHelper.Create(UIType.UICharacter);
        }

        private void GoStartScene()
        {
            var sceneManager = Common.Instance.Get<SceneController>();
            sceneManager.LoadSceneAsync<StartScene>(SceneName.Start);
        }

        protected override void OnClose()
		{
			
		}
	}
}
