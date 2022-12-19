using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework
{
    public interface IEntry : IDisposable, IUpdate, ILateUpdate, IFixedUpdate
    {
        void Start();
    }
}
