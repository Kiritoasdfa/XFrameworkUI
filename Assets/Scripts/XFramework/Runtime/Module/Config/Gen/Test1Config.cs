using System.Collections.Generic;
using ProtoBuf;
using Newtonsoft.Json;

namespace XFramework
{
    /// <summary>
    /// 测试配置/Test1Config
    /// </summary>
    [ProtoContract]
    public partial class Test1ConfigManager : ConfigInstance<Test1Config>
    { 
        private static Test1ConfigManager _instance;

        public static Test1ConfigManager Instance => _instance;

        [ProtoMember(1)]
        [JsonProperty]
        private List<Test1Config> list = new List<Test1Config>();

        protected override List<Test1Config> _list => list;

        public Test1ConfigManager()
        {
            _instance = this;
        }
    }
	
    /// <summary>
    /// 测试配置/Test1Config
    /// </summary>
	[ProtoContract]
    public partial class Test1Config : ProtoObject, IConfig
    {
        int IConfig.ConfigId => Id;

		/// <summary>
		/// ID
		/// </summary>
		[ProtoMember(1, IsRequired = true)]
		[JsonProperty]
		public int Id { get; private set; }
		/// <summary>
		/// 名称
		/// </summary>
		[ProtoMember(2, IsRequired = true)]
		[JsonProperty]
		public string Name { get; private set; }
		/// <summary>
		/// 速度
		/// </summary>
		[ProtoMember(3, IsRequired = true)]
		[JsonProperty]
		public float Speed { get; private set; }

        public override void EndInit()
        {              

            AfterEndInit();
        }
    }
}