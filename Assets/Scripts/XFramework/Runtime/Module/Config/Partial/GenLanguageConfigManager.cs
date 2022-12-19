using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFramework
{
    public partial class GenLanguageConfigManager
    {
        private Dictionary<string, int> keyDict = new Dictionary<string, int>();

        protected override void AfterEndInit()
        {
            base.AfterEndInit();
            foreach (var config in GetAllValues())
            {
                keyDict.Add(config.Key, config.Id);
            }
        }

        public bool GetConfigByKey(string key, out GenLanguageConfig config)
        {
            config = null;
            if (keyDict.TryGetValue(key, out var id))
            {
                config = Get(id);
            }

            return config != null;
        }
    }
}
