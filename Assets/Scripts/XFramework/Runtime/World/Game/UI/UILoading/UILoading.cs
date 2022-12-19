using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework
{
	[UIEvent(UIType.UILoading)]
    internal sealed class UILoadingEvent : AUIEvent, IUILayer
    {
        public override string Key => UIPathSet.UILoading;

        public override bool IsFromPool => true;

        public override bool AllowManagement => true;

        public UILayer Layer => UILayer.High;

        public override UI OnCreate()
        {
            return UI.Create<UILoading>(true);
        }
    }

    public partial class UILoading : UI, IAwake<ILoading>
	{
        private ILoading m_loading;

        /// <summary>
        /// �������ص�������
        /// </summary>
        private const int SceneMaxProgress = 10;

        /// <summary>
        /// �������ص�ǰ����
        /// </summary>
        private float sceneProgress;

        /// <summary>
        /// ��ǰ���ص���Դ��
        /// </summary>
        private int curCount;

        /// <summary>
        /// ��ǰ���ص��ܸ���
        /// </summary>
        private int totalCount;

        /// <summary>
        /// ��һ�θ��µĽ���
        /// </summary>
        private float beforeProgress;

        /// <summary>
        /// Ҫʵ������Ԥ�Ƶ�key
        /// </summary>
        private List<string> objKeys = new List<string>();

        private long timerId;

        private MiniTween tween;

		public void Initialize(ILoading loadArg)
		{
            this.m_loading = loadArg;
            loadArg.GetObjects(objKeys);
            this.totalCount = objKeys.Count + SceneMaxProgress;

            this.GetImage(KFill).SetFillAmount(0);
            this.GetText(KProgress).SetText(string.Empty);

            //����һ��ÿִ֡�е������൱��Update
            var timerMgr = Common.Instance.Get<TimerManager>();
            timerId = timerMgr.RepeatedFrameTimer(this.Update);

            //��ʼ������Դ
            this.LoadAssets().Coroutine();
		} 
        
        private void Update()
        {
            float progress = this.m_loading.SceneProgress();
            this.sceneProgress = progress * SceneMaxProgress;
            this.DoFillAmount().Coroutine();

            if (this.sceneProgress >= SceneMaxProgress)
            {
                this.RemoveTimer();
            }
        }

        /// <summary>
        /// ����������Դ
        /// </summary>
        /// <returns></returns>
        private async Task LoadAssets()
        {
            using var tasks = XList<Task>.Create();
            var timerMgr = Common.Instance.Get<TimerManager>();
            var tagId = this.TagId;

            Transform parent = Common.Instance.Get<Global>().GameRoot;
            foreach (var key in this.objKeys)
            {
                tasks.Add(this.LoadObjectAsync(key, parent));
            }

            //�ȴ�������Դ�������
            await Task.WhenAll(tasks);
            //��Ϊ���첽�������������첽ʱ����౻�ͷ��ˣ�����Ҫ��tagId�ж�һ��
            //�����������Զ���أ���ôtagIdÿ��ȡ��������仯
            //������tagId�ж�������Ƿ���Ч�����׵ķ�ʽ
            if (tagId != this.TagId)    
                return;

            //�ȴ������Ľ�����
            while (this.sceneProgress < SceneMaxProgress)
            {
                await timerMgr.WaitFrameAsync();
                if (tagId != this.TagId)
                    return;
            }

            if (this.tween != null)
            {
                //�ȴ��������������
                if (!this.tween.IsCompelted)
                {
                    await this.tween.Task;
                    if (tagId != this.TagId)
                        return;
                }
            }

            //�ӳ�100����
            await timerMgr.WaitAsync(100);
            if (tagId != this.TagId)
                return;

            //��Դ������ϣ�������Ϣ֪ͨ�������࣬���յ������Ϣ���ദ��ʣ�µ�����
            base.Publish(new EventType.OnResourcesLoaded());
        }

        /// <summary>
        /// ʵ����GameObject
        /// </summary>
        /// <param name="key"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private async Task LoadObjectAsync(string key, Transform parent)
        {
            var tagId = this.TagId;
            GameObject obj = await ResourcesManager.InstantiateAsync(this, key, parent, true);
            ResourcesManager.ReleaseInstance(obj);
            if (tagId != this.TagId)
                return;

            ++curCount;
            this.DoFillAmount().Coroutine();
        }

        /// <summary>
        /// ˿���仯������
        /// </summary>
        /// <returns></returns>
        private async Task DoFillAmount()
        {
            float count = this.curCount + this.sceneProgress;
            if (count == this.beforeProgress)
                return;

            this.beforeProgress = count;
            float t = this.beforeProgress / this.totalCount;

            this.tween?.Cancel(this);
            var fill = this.GetImage(KFill);
            var txt = this.GetText(KProgress);
            var tweenMgr = Common.Instance.Get<MiniTweenManager>();
            var miniTween = tweenMgr.To(this, fill.GetFillAmount(), t, 0.2f);
            miniTween.AddListener(v =>
            {
                if (fill is null || txt is null)
                {
                    miniTween.Cancel(this);
                    return;
                }

                fill.SetFillAmount(v);
                txt.SetTextWithKey("{0:F0}%", v * 100);
            });
            this.tween = miniTween;
            await this.tween.Task;
        }

        /// <summary>
        /// �Ƴ���ʱ��
        /// </summary>
        private void RemoveTimer()
        {
            var timerMgr = Common.Instance.Get<TimerManager>();
            timerMgr?.RemoveTimerId(ref this.timerId);
            this.timerId = 0;
        }
		
		protected override void OnClose()
		{
            this.m_loading = null;
            this.RemoveTimer();
            this.curCount = 0;
            this.totalCount = 0;
            this.beforeProgress = 0;
            this.sceneProgress = 0;
            this.objKeys.Clear();
            this.tween = null;
        }
    }
}
