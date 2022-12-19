using System.Collections.Generic;
using ProtoBuf;
using Newtonsoft.Json;

namespace XFramework
{
    /// <summary>
    /// 华容道/HRD
    /// </summary>
    [ProtoContract]
    public partial class HRDConfigManager : ConfigInstance<HRDConfig>
    { 
        private static HRDConfigManager _instance;

        public static HRDConfigManager Instance => _instance;

        [ProtoMember(1)]
        [JsonProperty]
        private List<HRDConfig> list = new List<HRDConfig>();

        protected override List<HRDConfig> _list => list;

        public HRDConfigManager()
        {
            _instance = this;
        }
    }
	
    /// <summary>
    /// 华容道/HRD
    /// </summary>
	[ProtoContract]
    public partial class HRDConfig : ProtoObject, IConfig
    {
        int IConfig.ConfigId => Id;

		/// <summary>
		/// 
		/// </summary>
		[ProtoMember(1, IsRequired = true)]
		[JsonProperty]
		public int Id { get; private set; }
		/// <summary>
		/// X坐标
		/// </summary>
		[ProtoMember(2, IsRequired = true)]
		[JsonProperty]
		public int X { get; private set; }
		/// <summary>
		/// Y坐标
		/// </summary>
		[ProtoMember(3, IsRequired = true)]
		[JsonProperty]
		public int Y { get; private set; }
		/// <summary>
		/// 高度
		/// </summary>
		[ProtoMember(4, IsRequired = true)]
		[JsonProperty]
		public int H { get; private set; }
		/// <summary>
		/// 宽度
		/// </summary>
		[ProtoMember(5, IsRequired = true)]
		[JsonProperty]
		public int W { get; private set; }
		/// <summary>
		/// 资源路径
		/// </summary>
		[ProtoMember(6, IsRequired = true)]
		[JsonProperty]
		public string Res { get; private set; }

        public override void EndInit()
        {              

            AfterEndInit();
        }
    }
}