using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework
{
	[UIEvent(UIType.UISetting)]
    internal sealed class UISettingEvent : AUIEvent, IUILayer
    {
        public override string Key => UIPathSet.UISetting;

        public override bool IsFromPool => true;

        public override bool AllowManagement => true;

        public UILayer Layer => UILayer.High;

        public override UI OnCreate()
        {
            return UI.Create<UISetting>();
        }
    }

    public partial class UISetting : UI, IAwake
	{
        private readonly static string[] options = { "简体中文", "繁体中文", "英文" };

		public void Initialize()
		{
            this.Publish(new UIEventType.OnOpenSetting());
            this.GetButton(KExit)?.AddClickListener(this.Close);
            this.InitSlider();
            this.InitLanguage();
		}

        private void InitSlider()
        {
            var bgmSlider = this.GetSlider(KBGMSlider);
            var sfxSlider = this.GetSlider(KSFXSlider);
            var audioMgr = AudioManager.Instance;

            bgmSlider.SetValueWithoutNotify(audioMgr.GetBgmVolume());
            sfxSlider.SetValueWithoutNotify(audioMgr.GetSFXVolume());

            bgmSlider.AddValueListener(BgmValueChanged);
            sfxSlider.AddValueListener(SFXValueChanged);
        }

        private void InitLanguage()
        {
            var languageMgr = Common.Instance.Get<LanguageManager>();
            var type = languageMgr.Language_Type;
            var dropDown = this.GetDropdown(KDropdown);
            dropDown.ClearOptions();
            dropDown.AddOptions(options);
            dropDown.SetValue(type - 1);

            dropDown.AddClickListener(value =>
            {
                languageMgr.SetLanguageType(value + 1);
            });
        }

        private void BgmValueChanged(float value)
        {
            AudioManager.Instance.SetBgmVolume(value);
        }

        private void SFXValueChanged(float value)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }

        public override void OnCancel()
        {
            this.Close();
        }

        protected override void OnClose()
		{
			
		}
	}
}
