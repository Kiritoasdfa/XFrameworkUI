using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework
{
    public interface IConfigLoader
    {
        byte[] LoadOne(string name);

        Task<byte[]> LoadOneAsync(string name);

        Task<Dictionary<string, byte[]>> LoadAllAsync();
    }
}
