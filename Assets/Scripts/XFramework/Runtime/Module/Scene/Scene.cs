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
        /// 场景名称
        /// </summary>
        public string Name { get; private set; }

        protected SceneObject SceneObject { get; private set; }

        /// <summary>
        /// 场景是否加载完成
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
        /// 等待场景加载完毕
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
        /// 当场景加载完成时执行
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
