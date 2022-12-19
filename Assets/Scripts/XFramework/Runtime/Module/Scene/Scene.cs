using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework
{
    public class Scene : XObject, IUpdate, ILateUpdate, IFixedUpdate
    {
        /// <summary>
        /// ��������
        /// </summary>
        public string Name { get; private set; }

        protected SceneObject SceneObject { get; private set; }

        /// <summary>
        /// �����Ƿ�������
        /// </summary>
        protected bool isCompleted;

        public void Init(string name, SceneObject sceneObject)
        {
            this.Name = name;
            this.SceneObject = sceneObject;
            this.isCompleted = false;
            this.WaitForCompleted().Coroutine();
        }

        protected sealed override void OnStart()
        {
            base.OnStart();
        }

        public void Update()
        {
            if (!this.isCompleted)
                return;

            this.OnUpdate();
        }

        public void LateUpdate()
        {
            if (!this.isCompleted)
                return;

            this.OnLateUpdate();
        }

        public void FixedUpdate()
        {
            if (!this.isCompleted)
                return;

            this.OnFixedUpdate();
        }

        protected virtual void OnUpdate()
        {

        }

        protected virtual void OnLateUpdate()
        {

        }

        protected virtual void OnFixedUpdate()
        {

        }

        /// <summary>
        /// �ȴ������������
        /// </summary>
        /// <returns></returns>
        protected virtual async Task WaitForCompleted()
        {
            var tagId = this.TagId;
            await SceneResManager.WaitForCompleted(this.SceneObject);
            if (tagId != this.TagId)
                return;

            this.OnCompleted();
            this.isCompleted = true;
        }

        /// <summary>
        /// �������������ʱִ��
        /// </summary>
        protected virtual void OnCompleted()
        {

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            this.SceneObject = null;
            this.isCompleted = false;
            UIHelper.Clear();
            ResourcesManager.UnloadUnusedAssets();
        }
    }
}
