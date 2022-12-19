using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFramework
{
    public sealed class Global : CommonObject
    {
        public Transform GameRoot { get; private set; }

        public Transform UI { get; private set; }

        /// <summary>
        /// ∑‚√ÊÕº
        /// </summary>
        public GameObject Cover { get; private set; }

        public Camera UICamera { get; private set; }

        protected override void Init()
        {
            GameRoot = GameObject.Find("/GameRoot").transform;
            UI = GameRoot.Find("UI");
            Cover = UI.Find("Low/Cover")?.gameObject;
            UICamera = UI.Find("UICamera")?.GetComponent<Camera>();
        }

        protected override void Destroy()
        {
            GameRoot = null;
            UI = null;
            Cover = null;
        }
    }
}
