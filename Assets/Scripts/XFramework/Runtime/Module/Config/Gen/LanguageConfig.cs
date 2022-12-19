using System.Collections.Generic;
using ProtoBuf;
using Newtonsoft.Json;

namespace XFramework
{
    /// <summary>
    /// 多语言/Language
    /// </summary>
    [ProtoContract]
    public partial class LanguageConfigManager : ConfigInstance<LanguageConfig>
    { 
        private static LanguageConfigManager _instance;

        public static LanguageConfigManager Instance => _instance;

        [ProtoMember(1)]
        [JsonProperty]
        private List<LanguageConfig> list = new List<LanguageConfig>();

        protected override List<LanguageConfig> _list => list;

        public LanguageConfigManager()
        {
            _instance = this;
        }
    }
	
    /// <summary>
    /// 多语言/Language
    /// </summary>
	[ProtoContract]
    public partial class LanguageConfig : ProtoObject, IConfig
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