using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework
{
    public static class TaskExtend
    {
        /// <summary>
        /// ��Э���ã���������
        /// </summary>
        /// <param name="self"></param>
        public static async void Coroutine(this Task self)
        {
            await self;
        }
    }
}
