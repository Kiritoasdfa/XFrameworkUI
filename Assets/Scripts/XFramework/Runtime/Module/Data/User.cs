using Newtonsoft.Json;

namespace XFramework
{
    public sealed class User : UserData, IAwake
    {
        private static User _instance;

        public static User Instance => _instance;

        private long userId;

        [JsonIgnore]
        public long UserId => userId;

        public void Initialize()
        {
            InitUser();
        }

        protected override void OnDeserialize()
        {
            base.OnDeserialize();
            InitUser();
        }

        private void InitUser()
        {
            _instance = this;
            userId = RandomHelper.GenerateId();
        }

        protected override void Destroy()
        {
            _instance = null;
            base.Destroy();
        }
    }
}
