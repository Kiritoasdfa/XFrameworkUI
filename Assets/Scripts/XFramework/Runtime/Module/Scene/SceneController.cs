using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UScene = UnityEngine.SceneManagement.Scene;

namespace XFramework
{
    public sealed class SceneController : CommonObject, IUpdate, ILateUpdate, IFixedUpdate
    {
        private SceneObject sceneObject;

        public Scene Scene => sceneObject?.Scene;

        /// <summary>
        /// 卸载当前场景
        /// </summary>
        private void UnLoadCurrentScene()
        {
            this.sceneObject?.Dispose();
            this.sceneObject = null;
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private SceneObject InnerLoadSceneAsync(string key)
        {
            this.UnLoadCurrentScene();
            var sceneObject = SceneResManager.LoadSceneAsync(key, LoadSceneMode.Additive);
            return sceneObject;
        }

        /// <summary>
        /// 创建场景类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="isFromPool"></param>
        /// <returns></returns>
        private Scene InnerCreateScene<T>(string key, bool isFromPool) where T : Scene, new()
        {
            this.sceneObject = this.InnerLoadSceneAsync(key);
            if (this.sceneObject is null)
                return null;

            T scene = ObjectFactory.CreateNoInit<T>(isFromPool);
            this.sceneObject.SetScene(scene);
            scene.Init(key, this.sceneObject);
            return scene;
        }

        public SceneObject LoadSceneAsync<T>(string key, bool isFromPool = false) where T : Scene, new()
        {
            var scene = this.InnerCreateScene<T>(key, isFromPool);
            if (scene != null)
                ObjectHelper.Awake(scene);
            return this.sceneObject;
        }

        public SceneObject LoadSceneAsync<T, P1>(string key, P1 p1, bool isFromPool = false) where T : Scene, IAwake<P1>, new()
        {
            var scene = this.InnerCreateScene<T>(key, isFromPool);
            if (scene != null)
                ObjectHelper.Awake(scene, p1);
            return this.sceneObject;
        }

        public SceneObject LoadSceneAsync<T, P1, P2>(string key, P1 p1, P2 p2, bool isFromPool = false) where T : Scene, IAwake<P1, P2>, new()
        {
            var scene = this.InnerCreateScene<T>(key, isFromPool);
            if (scene != null)
                ObjectHelper.Awake(scene, p1, p2);
            return this.sceneObject;
        }

        public SceneObject LoadSceneAsync<T, P1, P2, P3>(string key, P1 p1, P2 p2, P3 p3, bool isFromPool = false) where T : Scene, IAwake<P1, P2, P3>, new()
        {
            var scene = this.InnerCreateScene<T>(key, isFromPool);
            if (scene != null)
                ObjectHelper.Awake(scene, p1, p2, p3);
            return this.sceneObject;
        }

        public SceneObject LoadSceneAsync<T, P1, P2, P3, P4>(string key, P1 p1, P2 p2, P3 p3, P4 p4, bool isFromPool = false) where T : Scene, IAwake<P1, P2, P3, P4>, new()
        {
            var scene = this.InnerCreateScene<T>(key, isFromPool);
            if (scene != null)
                ObjectHelper.Awake(scene, p1, p2, p3, p4);
            return this.sceneObject;
        }

        public SceneObject LoadSceneAsync<T, P1, P2, P3, P4, P5>(string key, P1 p1, P2 p2, P3 p3, P4 p4, P5 p5, bool isFromPool = false) where T : Scene, IAwake<P1, P2, P3, P4, P5>, new()
        {
            var scene = this.InnerCreateScene<T>(key, isFromPool);
            if (scene != null)
                ObjectHelper.Awake(scene, p1, p2, p3, p4, p5);
            return this.sceneObject;
        }

        public void Update()
        {
            Scene?.Update();
        }

        public void LateUpdate()
        {
            Scene?.LateUpdate();
        }

        public void FixedUpdate()
        {
            Scene?.FixedUpdate();
        }

        protected override void Destroy()
        {
            this.sceneObject?.Dispose();
            this.sceneObject = null;
            base.Destroy();
        }
    }
}
