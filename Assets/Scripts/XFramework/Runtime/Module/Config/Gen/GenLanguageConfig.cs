using System.Collections.Generic;
using ProtoBuf;
using Newtonsoft.Json;

namespace XFramework
{
    /// <summary>
    /// 多语言/GenLanguage
    /// </summary>
    [ProtoContract]
    public partial class GenLanguageConfigManager : ConfigInstance<GenLanguageConfig>
    { 
        private static GenLanguageConfigManager _instance;

        public static GenLanguageConfigManager Instance => _instance;

        [ProtoMember(1)]
        [JsonProperty]
        private List<GenLanguageConfig> list = new List<GenLanguageConfig>();

        protected override List<GenLanguageConfig> _list => list;

        public GenLanguageConfigManager()
        {
            _instance = this;
        }
    }
	
    /// <summary>
    /// 多语言/GenLanguage
    /// </summary>
	[ProtoContract]
    public partial class GenLanguageConfig : ProtoObject, IConfig
    {
        int IConfig.ConfigId => Id;

		/// <summary>
		/// 
		/// </summary>
		[ProtoMember(1, IsRequired = true)]
		[JsonProperty]
		public int Id { get; private set; }
		/// <summary>
		/// 键值
		/// </summary>
		[ProtoMember(2, IsRequired = true)]
		[JsonProperty]
		public string Key { get; private set; }
		/// <summary>
		/// 简体中文
		/// </summary>
		[ProtoMember(3, IsRequired = true)]
		[JsonProperty]
		public string CN { get; private set; }
		/// <summary>
		/// 繁体中文
		/// </summary>
		[ProtoMember(4, IsRequired = true)]
		[JsonProperty]
		public string TC { get; private set; }
		/// <summary>
		/// 英文
		/// </summary>
		[ProtoMember(5, IsRequired = true)]
		[JsonProperty]
		public string EN { get; private set; }

        public override void EndInit()
        {              

            AfterEndInit();
        }
    }
}