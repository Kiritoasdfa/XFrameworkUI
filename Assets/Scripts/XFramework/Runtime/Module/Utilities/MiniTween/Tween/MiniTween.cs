using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFramework
{
    public enum MiniLoopType
    {
        /// <summary>
        /// ��ѭ��
        /// </summary>
        None,
        /// <summary>
        /// ���¿�ʼ
        /// </summary>
        Restart,
        /// <summary>
        /// A��B��B��A
        /// </summary>
        Yoyo,
    }

    public abstract class MiniTween : XObject
    {
        protected XObject parent;

        protected long tagId;

        /// <summary>
        /// �Ѿ����/ȡ��
        /// </summary>
        public bool IsCancel { get; protected set; }

        public Task<bool> Task { get; protected set; }

        /// <summary>
        /// �ѹ�ʱ��
        /// </summary>
        protected float elapsedTime;

        /// <summary>
        /// ����ʱ��
        /// </summary>
        protected float duration;

        /// <summary>
        /// ִ�д���
        /// </summary>
        protected int executeCount;

        /// <summary>
        /// ѭ������
        /// </summary>
        protected MiniLoopType loopType;

        /// <summary>
        /// ��ɺ�Ļص�
        /// </summary>
        protected Action completed_Action { get; set; }

        protected XCancellationToken cancellationToken { get; set; }

        /// <summary>
        /// ��ǰ����
        /// </summary>
        public float Progress => this.duration > 0 ? Mathf.Clamp01(this.elapsedTime / this.duration) : -1f;

        /// <summary>
        /// �Ƿ������
        /// </summary>
        public bool IsCompelted => (this.Progress >= 1f && this.executeCount == 0) || this.IsCancel;

        protected override void OnStart()
        {
            base.OnStart();
            this.IsCancel = false;
            this.executeCount = 1;
            this.loopType = MiniLoopType.None;
        }

        /// <summary>
        /// ȡ��
        /// </summary>
        /// <param name="completed">�������</param>
        public abstract void Cancel(XObject parent, bool completed);

        /// <summary>
        /// ȡ��
        /// </summary>
        /// <param name="parent">��ΪTweenʹ�õ������أ�Ϊ�˱����Լ��洢�󲻼�ʱ�ͷţ�����Ҫ����parent�Ƿ����ڲ���parentһ��</param>
        public virtual void Cancel(XObject parent)
        {
            if (!this.CheckParentValid(parent))
                return;

            this.Cancel();
        }

        /// <summary>
        /// ȡ��
        /// </summary>
        /// <param name="parent">��ΪTweenʹ�õ������أ�Ϊ�˱����Լ��洢�󲻼�ʱ�ͷţ�����Ҫ����parent�Ƿ����ڲ���parentһ��</param>
        protected void Cancel()
        {
            if (this.IsCancel)
                return;

            this.IsCancel = true;
            if (this.Progress >= 0f && this.Progress < 1f)
                this.cancellationToken?.Cancel();
            this.cancellationToken = null;
            this.Task = null;
            this.executeCount = 0;
            Common.Instance.Get<MiniTweenManager>()?.Remove(this);
        }

        /// <summary>
        /// ���鴫������parent�Ƿ��tween��parent��ȫһ��
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected bool CheckParentValid(XObject parent)
        {
            return this.parent == parent && this.tagId == parent.TagId;
        }

        internal abstract Type GetArgType();

        /// <summary>
        /// �����ѹ�ʱ��֮��
        /// </summary>
        protected abstract void AddElapsedTimeAfter();

        /// <summary>
        /// ����
        /// </summary>
        protected abstract void Reset();

        /// <summary>
        /// �����ѹ�ʱ��
        /// </summary>
        /// <param name="deltaTime"></param>
        internal void AddElapsedTime(float deltaTime)
        {
            if (!this.CheckIsValid())
                return;

            this.elapsedTime += deltaTime;
            this.AddElapsedTimeAfter();
            this.CheckCompleted();
        }

        /// <summary>
        /// �����Ƿ���Ч
        /// </summary>
        /// <returns></returns>
        private bool CheckIsValid()
        {
            if (parent != null)
            {
                if (parent.IsDisposed || parent.TagId != tagId)
                {
                    this.Cancel();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// �����Ƿ����
        /// </summary>
        /// <returns>�Ƿ����</returns>
        protected void CheckCompleted()
        {
            if (this.Progress >= 1f)
            {
                if (this.executeCount > 0) 
                    --this.executeCount;
                if (this.IsCompelted)
                {
                    this.completed_Action?.Invoke();
                    this.Cancel();
                }
                else
                {
                    this.Reset();
                }
            }
        }

        /// <summary>
        /// �����ɺ�ļ���
        /// </summary>
        /// <param name="action"></param>
        public void AddOnCompleted(Action action)
        {
            this.completed_Action += action;
        }

        /// <summary>
        /// �Ƴ���ɺ�ļ���
        /// </summary>
        /// <param name="action"></param>
        public void RemoveOnCompleted(Action action)
        {
            this.completed_Action -= action;
        }

        /// <summary>
        /// �Ƴ�������ɺ�ļ���
        /// </summary>
        public void RemoveAllOnCompleted()
        {
            this.completed_Action = null;
        }

        /// <summary>
        /// ����ѭ��ִ��
        /// </summary>
        /// <param name="loopType"></param>
        /// <param name="count">ִ�д�����С��0ʱΪ����ѭ����0��Ч</param>
        public void SetLoop(MiniLoopType loopType, int count = 1)
        {
            if (count == 0)
            {
                Log.Error("MiniTween SetLoop error, count == 0");
                return;
            }

            this.loopType = loopType;
            switch (loopType)
            {
                case MiniLoopType.None:
                    this.executeCount = this.executeCount != 0 ? 1 : this.executeCount;
                    break;
                case MiniLoopType.Restart:
                    this.executeCount = count;
                    break;
                case MiniLoopType.Yoyo:
                    this.executeCount = count > 0 ? count * 2 : -1;
                    break;
                default:
                    break;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.Cancel();
            this.elapsedTime = 0;
            this.duration = 0;
            this.parent = null;
            this.tagId = 0;
            this.RemoveAllOnCompleted();
        }
    }

    public class MiniTween<T> : MiniTween, IAwake<XObject, T, T, float>
    {
        /// <summary>
        /// ��̬�仯ʱ�ļ���
        /// </summary>
        protected Action<T> setValue_Action;

        /// <summary>
        /// ��ʼֵ
        /// </summary>
        protected T startValue;

        /// <summary>
        /// Ŀ��ֵ
        /// </summary>
        protected T endValue;

        internal override Type GetArgType()
        {
            return typeof(T);
        }

        protected override void AddElapsedTimeAfter()
        {
            throw new NotImplementedException();
        }

        protected override void Reset()
        {
            base.elapsedTime = 0;
            switch (base.loopType)
            {
                case MiniLoopType.None:
                    break;
                case MiniLoopType.Restart:
                    break;
                case MiniLoopType.Yoyo:
                    (this.startValue, this.endValue) = (this.endValue, this.startValue);
                    break;
                default:
                    break;
            }
        }

        public override void Cancel(XObject parent)
        {
            this.Cancel(parent, false);
        }

        public override void Cancel(XObject parent, bool completed)
        {
            if (!base.CheckParentValid(parent))
                return;

            if (base.IsCancel)
                return;

            if (completed)
            {
                base.IsCancel = true;
                base.elapsedTime = base.duration;
                this.setValue_Action?.Invoke(this.endValue);
                this.completed_Action?.Invoke();
            }

            base.IsCancel = false;
            base.Cancel();
        }

        /// <summary>
        /// ��Ӷ�̬������ÿ�仯һ�ε���һ�μ���
        /// </summary>
        /// <param name="action"></param>
        public void AddListener(Action<T> action)
        {
            this.setValue_Action += action;
        }

        /// <summary>
        /// �Ƴ���̬����
        /// </summary>
        /// <param name="action"></param>
        public void RemoveListener(Action<T> action)
        {
            this.setValue_Action -= action;
        }

        /// <summary>
        /// �Ƴ����еĶ�̬����
        /// </summary>
        public void RemoveAllListeners()
        {
            this.setValue_Action = null;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.RemoveAllListeners();
        }

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <param name="duration"></param>
        public virtual void Initialize(XObject parent, T startValue, T endValue, float duration)
        {
            base.parent = parent;
            base.tagId = parent.TagId;
            this.startValue = startValue;
            this.endValue = endValue;
            base.duration = duration;
        }

        internal virtual void SetTask(Task<bool> task, XCancellationToken cancellationToken)
        {
            base.Task = task;
            base.cancellationToken = cancellationToken;
        }
    }
}
