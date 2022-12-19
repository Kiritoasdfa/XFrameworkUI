using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFramework
{
    public class MainScene : LoadingScene
    {
        protected override void OnCompleted()
        {
            UIHelper.Create(UIType.UIMain);
        }
    }
}
