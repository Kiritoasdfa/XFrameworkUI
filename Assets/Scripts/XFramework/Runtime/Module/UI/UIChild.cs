using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework
{
    public class UIChild : UI, IUID
    {
        /// <summary>
        /// 这个id只能赋值一次，因为提供给IUID接口使用，这个id一旦赋值会提供给UIList使用;
        /// 如果中途改变了这个id会造成UIList引用出问题
        /// </summary>
        private long m_Id = 0;

        public long Id => m_Id;

        protected void SetId(long id)
        {
            if (id == 0)
            {
                Log.Error("UIChild不能设置id为0");
                return;
            }

            if (this.m_Id != 0)
            {
                Log.Error("UIChild的id已有值，不能继续赋值");
                return;
            }

            this.m_Id = id;
        }

        protected override void OnClose()
        {
            this.m_Id = 0;
            base.OnClose();
        }
    }
}
