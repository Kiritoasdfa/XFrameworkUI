using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XFramework
{
    [DisallowMultipleComponent]
    public class UIReference : Reference
    {
        private static Dictionary<int, XText> textList = new Dictionary<int, XText>();

        [SerializeField, HideInInspector]
        private List<ElementData> _textData = new List<ElementData>();

        private Dictionary<string, XText> _textDict = new Dictionary<string, XText>();

        public static IEnumerable<XText> TextList()
        {
            return textList.Values;
        }

        internal static void AddText(int instanceId, XText text)
        {
            textList.Add(instanceId, text);
        }

        internal static void RemoveText(int instanceId)
        {
            textList.Remove(instanceId);
        }

        public XText GetText(string key)
        {
            _textDict.TryGetValue(key, out XText text);
            return text;
        }

        public IEnumerable<string> GetAllTextKeys()
        {
            return _textDict.Keys;
        }

        public IEnumerable<XText> GetAllText()
        {
            return _textDict.Values;
        }

        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();

            _textDict.Clear();
            foreach (var data in _textData)
            {
                var key = data.Key;
                var text = data.Object as XText;
                if (text != null)
                {
                    if (!_textDict.ContainsKey(key))
                        _textDict.Add(key, text);
                }
            }
        }
    }
}
