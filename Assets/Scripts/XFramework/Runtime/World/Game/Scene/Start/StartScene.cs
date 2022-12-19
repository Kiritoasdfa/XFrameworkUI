using System.Collections;   
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework
{
    public class StartScene : LoadingScene, IEvent<int>, IEvent<float>
    {
        public override void GetObjects(ICollection<string> objKeys)
        {
            objKeys.Add(UIPathSet.UIStart);
            objKeys.Add(UIPathSet.UISetting);
        }

        protected override void OnCompleted()
        {
            Log.Debug($"进入Start场景");
            UIHelper.Create(UIType.UIStart);
            EventManager.Instance.Publish(1);

            TestConfig testConfig = TestConfigManager.Instance?.Get(1);
            if (testConfig != null)
                Log.Info(JsonHelper.ToJson(testConfig));
            else
                Log.Error("请检查配置是否正确导出和加载");

            AudioManager.Instance.PlayBgmAudio("BGM", true);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            //Log.Debug("StartScene Update");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug("离开Start场景");
        }

        public void HandleEvent(int args)
        {
            Log.Debug($"TestEvent {args}");
        }

        public void HandleEvent(float args)
        {
            throw new System.NotImplementedException();
        }
    }
}
