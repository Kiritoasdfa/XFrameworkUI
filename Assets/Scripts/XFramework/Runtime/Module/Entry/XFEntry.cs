using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework
{
    public class XFEntry : IEntry
    {
        public virtual void Dispose()
        {
            EventManager.Instance.Dispose();
            TypesManager.Instance.Dispose();
            Common.Instance.Dispose();
            ObjectPool.Instance.Dispose();
            GameObjectPool.Instance.Dispose();
            TimeInfo.Instance.Dispose();
            ResourcesManager.Instance.Dispose();
        }

        public virtual void Update()
        {
            Common.Instance.Update();
        }

        public virtual void LateUpdate()
        {
            Common.Instance.LateUpdate();
        }

        public virtual void FixedUpdate()
        {
            Common.Instance.FixedUpdate();
        }

        public virtual void Start()
        {
            this.LoadAsync().Coroutine();
        }

        private async Task LoadAsync()
        {
            ResourcesManager.Instance.SetLoader(new AAResourcesLoader());
            SceneResManager.Instance.SetLoader(new AASceneLoader());
            TimeInfo.Instance.Init();
            TypesManager.Instance.Init();
            ObjectPool.Instance.Init();
            GameObjectPool.Instance.Init();

            var global = ObjectFactory.Create<Global>();
            global.Cover?.SetViewActive(true);

            //加载资源前要先创建这两个
            ObjectFactory.Create<ResourcesRefDetection>();
            ObjectFactory.Create<TaskManager>();

            //加载配置表，配置表是资源，所以放在这里
            var configMgr = ObjectFactory.Create<ConfigManager>();
            configMgr.SetLoader(new AAConfigLoader());
            await configMgr.LoadAllConfigsAsync();     //等待配置表加载完毕才能执行下面的内容

            ObjectFactory.Create<TimerManager>();
            ObjectFactory.Create<AudioManager>();
            ObjectFactory.Create<MiniTweenManager>();
            ObjectFactory.Create<LanguageManager>();
            ObjectFactory.Create<UIEventManager>();
            ObjectFactory.Create<UIManager>();

            var userMgr = ObjectFactory.Create<UserDataManager>();
            await userMgr.LoadAsync();  //加载存档，不一定要写在这，写在注册之后即可

            var sceneController = ObjectFactory.Create<SceneController>();
            var sceneObj = sceneController.LoadSceneAsync<StartScene>(SceneName.Start);
            await SceneResManager.WaitForCompleted(sceneObj);
            global.Cover?.SetViewActive(false);
        }
    }
}
