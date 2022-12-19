using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using XFramework.UIEventType;

namespace XFramework
{
	[UIEvent(UIType.UIStart)]
    internal sealed class UIStartEvent : AUIEvent, IUILayer
    {
        public override string Key => UIPathSet.UIStart;

        public override bool IsFromPool => true;

        public override bool AllowManagement => true;

        public UILayer Layer => UILayer.Low;

        public override UI OnCreate()
        {
            return UI.Create<UIStart>();
        }
    }

    public partial class UIStart : UI, IAwake, IEvent<UIEventType.OnOpenSetting>
	{	
		public void Initialize()
		{
            this.GetButton(KSetting)?.AddClickListener(OpenSetting);
            this.GetButton(KPlay)?.AddClickListener(GoMainScene);
            this.GetButton(KGame)?.AddClickListener(OpenHRD);
		}

        private void OpenSetting()
        {
            AudioManager.Instance.PlaySFXAudio("Click");    //建议写一个统一的const string字段代替这个Click
            UIHelper.Create(UIType.UISetting);
        }

        private void OpenHRD()
        {
            UIHelper.Create(UIType.UIHRD);
        }

        private void GoMainScene()
        {
            var sceneManager = Common.Instance.Get<SceneController>();
            sceneManager.LoadSceneAsync<MainScene>(SceneName.Main);
        }

		protected override void OnClose()
		{
			
		}

        void IEvent<OnOpenSetting>.HandleEvent(OnOpenSetting args)
        {
            Log.Info("打开了设置界面");
        }
    }
}
