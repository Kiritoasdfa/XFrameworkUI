using System.Collections.Generic;
using ProtoBuf;
using Newtonsoft.Json;

namespace XFramework
{
    /// <summary>
    /// 测试配置/TestConfig
    /// </summary>
    [ProtoContract]
    public partial class TestConfigManager : ConfigInstance<TestConfig>
    { 
        private static TestConfigManager _instance;

        public static TestConfigManager Instance => _instance;

        [ProtoMember(1)]
        [JsonProperty]
        private List<TestConfig> list = new List<TestConfig>();

        protected override List<TestConfig> _list => list;

        public TestConfigManager()
        {
            _instance = this;
        }
    }
	
    /// <summary>
    /// 测试配置/TestConfig
    /// </summary>
	[ProtoContract]
    public partial class TestConfig : ProtoObject, IConfig
    {
        int IConfig.ConfigId => Id;

		/// <summary>
		/// ID
		/// </summary>
		[ProtoMember(1, IsRequired = true)]
		[JsonProperty]
		public int Id { get; private set; }
		/// <summary>
		/// 姓名
		/// </summary>
		[ProtoMember(2, IsRequired = true)]
		[JsonProperty]
		public string Name { get; private set; }
		/// <summary>
		/// 城市
		/// </summary>
		[ProtoMember(3, IsRequired = true)]
		[JsonProperty]
		public string[] City { get; private set; }
		/// <summary>
		/// 身高
		/// </summary>
		[ProtoMember(4, IsRequired = true)]
		[JsonProperty]
		public float[] Height { get; private set; }

        public override void EndInit()
        {              
			City ??= System.Array.Empty<string>();
			Height ??= System.Array.Empty<float>();

            AfterEndInit();
        }
    }
}