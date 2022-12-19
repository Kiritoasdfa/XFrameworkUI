using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace XFramework
{
    internal class BuiltinSceneInstance
    {
        public string Key { get; private set; }

        public AsyncOperation Operation { get; private set; }

        public BuiltinSceneInstance(string key, AsyncOperation asyncOperation)
        {
            Key = key;
            Operation = asyncOperation;
        }
    }

    public class BuiltinSceneLoader : SceneLoader
    {
        public override object LoadScene(string key, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(key, loadSceneMode);
            BuiltinSceneInstance scene = new BuiltinSceneInstance(key, null);
            return scene;
        }

        public override object LoadSceneAsync(string key, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(key, loadSceneMode);
            BuiltinSceneInstance scene = new BuiltinSceneInstance(key, operation);
            return scene;
        }

        public override bool IsDone(SceneObject sceneObjet)
        {
            BuiltinSceneInstance scene = (BuiltinSceneInstance)sceneObjet.SceneHandle;
            if (scene.Operation is null)
                return true;

            return scene.Operation.isDone;
        }

        public override float Progress(SceneObject sceneObjet)
        {
            BuiltinSceneInstance scene = (BuiltinSceneInstance)sceneObjet.SceneHandle;
            if (scene.Operation is null)
                return 1f;

            if (scene.Operation.isDone)
                return 1f;

            return scene.Operation.progress;
        }

        public async override Task UnloadSceneAsync(object handle)
        {
            BuiltinSceneInstance scene = (BuiltinSceneInstance)handle;
            string key = scene.Key;
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(key);

            var taskMgr = Common.Instance.Get<TaskManager>();
            await taskMgr.WaitForCompleted(asyncOperation);
        }

        public async override Task WaitForCompleted(SceneObject sceneObjet)
        {
            BuiltinSceneInstance scene = (BuiltinSceneInstance)sceneObjet.SceneHandle;
            if (scene.Operation is null)
                return;

            var taskMgr = Common.Instance.Get<TaskManager>();
            await taskMgr.WaitForCompleted(scene.Operation);
        }
    }
}
