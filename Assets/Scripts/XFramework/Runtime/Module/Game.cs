using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace XFramework
{
    public class Game : Singleton<Game>, IDisposable
    {
        private IEntry entry;

        public void Start()
        {
            Log.ILog = new UnityLogger();

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Log.Error($"{e}");
            };

            SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);

            entry = new XFEntry();  //可以实现其他entry
            entry.Start();
        }

        public void Update()
        {
            try
            {
                ThreadSynchronizationContext.Instance.Update();
                entry?.Update();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public void LateUpdate()
        {
            try
            {
                entry?.LateUpdate();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public void FixedUpdate()
        {
            try
            {
                entry?.FixedUpdate();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public void Dispose()
        {
            entry?.Dispose();
            Instance = null;
        }
    }
}
