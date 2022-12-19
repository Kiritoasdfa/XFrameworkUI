using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XFramework
{
    /// <summary>
    /// 如果需要在加载场景时显示进度条，则继承这个类
    /// <para>进度条逻辑在UILoading.cs</para>
    /// </summary>
    public abstract class LoadingScene : Scene, ILoading, IEvent<EventType.OnResourcesLoaded>
    {
        void IEvent<EventType.OnResourcesLoaded>.HandleEvent(EventType.OnResourcesLoaded args)
        {
            UIHelper.Remove(UIType.UILoading);
            this.OnCompleted();
            this.isCompleted = true;
        }

        public virtual void GetObjects(ICollection<string> objKeys)
        {

        }

        public float SceneProgress()
        {
            return SceneResManager.Progress(this.SceneObject);
        }

        protected sealed override async Task WaitForCompleted()
        {
            UIHelper.Create<ILoading>(UIType.UILoading, this);
            await Task.CompletedTask;
        }
    }
}
