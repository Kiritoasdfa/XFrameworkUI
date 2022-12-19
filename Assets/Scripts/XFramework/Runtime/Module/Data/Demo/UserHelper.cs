using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// demo
namespace XFramework
{
    public static class UserHelper
    {
        /// <summary>
        /// 获取物品数据集合，如没有，则创建
        /// <para>有一种情况是，可能这个是后加的类，存档里没有，所以要写没有则创建</para>
        /// </summary>
        /// <returns></returns>
        public static ThingData GetThingData()
        {
            var thingData = User.Instance.GetData<ThingData>();
            if (thingData is null)
            {
                thingData = UserData.Create<ThingData>();
                User.Instance.AddData(thingData);
            }

            return thingData;
        }
    }
}
