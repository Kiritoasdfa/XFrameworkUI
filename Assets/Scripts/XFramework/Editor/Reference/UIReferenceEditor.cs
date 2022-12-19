using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Text;

namespace XFramework
{
    [CustomEditor(typeof(UIReference))]
    public class UIReferenceEditor : ReferenceEditor
    {
        private enum ResourceType
        {
            Resources,
            AA,
        }

        private RectTransform rectTransform;

        private UIReferenceConfig config;

        ///// <summary>
        ///// ����ģ���ļ�·��
        ///// </summary>
        //private const string TemplateFilePath = "./Template/UICodeTemplate.txt";

        ///// <summary>
        ///// UI���ϵ�·��
        ///// </summary>
        //private const string SetFilePath = "Assets/Scripts/XFramework/Runtime/Module/UI/UISet.cs";

        ///// <summary>
        ///// UIType���·��
        ///// </summary>
        //private const string UITypeFilePath = "Assets/Scripts/XFramework/Runtime/Module/UI/UIType.cs";

        ///// <summary>
        ///// UI���Ŀ¼
        ///// </summary>
        //private const string ClassExploer = "Assets/Scripts/XFramework/Runtime/World/Game/UI";

        /// <summary>
        /// �����ļ�·��
        /// </summary>
        private const string ConfigPath = "Assets/Scripts/XFramework/Editor/Reference/Config/UIReferenceConfig.asset";

        /// <summary>
        /// UIType��ߵĲ���
        /// </summary>
        private const string UITypePrefix = "public const string (className) =";

        /// <summary>
        /// UIType��ֵ
        /// </summary>
        private const string UITypeValue = "nameof(XFramework.(className));";

        private ResourceType resourceType = ResourceType.AA;

        protected override void OnEnable()
        {
            base.OnEnable();
            config = AssetDatabase.LoadAssetAtPath<UIReferenceConfig>(ConfigPath);

            rectTransform = (target as UIReference).Get<RectTransform>();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (config == null || !rectTransform)
                return;

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("����Key", GUILayout.MinHeight(45) /*"Export Keys"*/))
            {
                ExportKeys();
            }

            if (GUILayout.Button("ˢ��\n�����Զ���", GUILayout.MinHeight(45)))
            {
                RefreshAllXText();
            }

            if (GUILayout.Button("��������\n(����UI)", GUILayout.MinHeight(45) /*"Export All (AllowManagement)"*/))
            {
                ExportAll(true);
            }

            if (GUILayout.Button("��������\n(����UI)", GUILayout.MinHeight(45) /*"Export All (Common)"*/))
            {
                ExportAll(false);
            }

            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }

        /// <summary>
        /// �������е�key
        /// </summary>
        private void ExportKeys()
        {
            string folderPath = GetFolderPath();
            if (folderPath.IsNullOrEmpty())
            {
                Debug.LogError("����Ŀ¼Ϊ�գ�");
                return;
            }

            string className = GetClassName();
            ExportKeys(className, folderPath);

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// ����UI����
        /// </summary>
        private void ExportAll(bool allowManagement)
        {
            if (Application.isPlaying)
                return;

            string folderPath = GetFolderPath();
            if (folderPath.IsNullOrEmpty())
            {
                Debug.LogError("����Ŀ¼Ϊ�գ�");
                return;
            }

            string className = GetClassName();

            ExportCode(className, folderPath, allowManagement);
            ExportKeys(className, folderPath);
            ExportUIType(className);
            ExportUIPathSet(className);

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="className"></param>
        /// <param name="folderPath"></param>
        /// <param name="allowManagement"></param>
        private void ExportCode(string className, string folderPath, bool allowManagement)
        {
            string fileName = $"{className}.cs";
            string classFilePath = $"{folderPath}/{fileName}";

            if (File.Exists(classFilePath))
            {
                Debug.LogError($"{fileName}�ļ��Ѿ�����\n·��Ϊ{classFilePath}");
                return;
            }

            string templateFilePath = config.CodeTemplateFilePath;

            if (!File.Exists(templateFilePath))
            {
                Debug.LogError($"����Code����ʱ�ļ�������, ����·��Ϊ{templateFilePath}");
                return;
            }

            string template = File.ReadAllText(templateFilePath);
            template = template.Replace("(className)", className).Replace("(UITypeName)", className);
            if (allowManagement)
            {
                template = template.Replace("(IUILayer)", ", IUILayer").Replace("(AllowManagement)", allowManagement.ToString().ToLower());
                template = template.Replace("(IUILayerCode)", "public UILayer Layer => UILayer.Low;");
            }
            else
            {
                template = template.Replace("(IUILayer)", string.Empty).Replace("(AllowManagement)", allowManagement.ToString().ToLower());
                template = template.Replace("(IUILayerCode)", "// ��UI�ǲ���UIManager�����");
            }

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            File.WriteAllText(classFilePath, template);

            Debug.Log($"{fileName}���ɳɹ�\n·��Ϊ{classFilePath}");
        }

        /// <summary>
        /// �������е�Key
        /// </summary>
        private void ExportKeys(string className, string folderPath)
        {
            string templateFilePath = config.KeyTemplateFilePath;
            
            if (!File.Exists(templateFilePath))
            {
                Debug.LogError($"����Key����ʱ�ļ�������, ����·��Ϊ{templateFilePath}");
                return;
            }

            string template = File.ReadAllText(templateFilePath);
            string prefix = "K";
            StringBuilder sb = new StringBuilder();
            int index = 0;

            foreach (var key in prefixKeys)
            {
                string content = $"\t\tpublic const string {prefix}{key} = \"{key}\";";
                if (index == 0)
                    sb.Append(content);
                else
                    sb.Append($"\n{content}");

                ++index;
            }
            foreach (var key in dragKeys)
            {
                string content = $"\t\tpublic const string {prefix}{key} = \"{key}\";";
                if (index == 0)
                    sb.Append(content);
                else
                    sb.Append($"\n{content}");

                ++index;
            }

            template = template.Replace("(className)", className).Replace("(content)", sb.ToString());

            folderPath = $"{folderPath}/Keys";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string fileName = $"{className}Keys.cs";
            string classFilePath = $"{folderPath}/{fileName}";
            File.WriteAllText(classFilePath, template);

            Debug.Log($"{fileName}���ɳɹ�\n·��Ϊ{classFilePath}");
        }

        /// <summary>
        /// ������UIType
        /// </summary>
        /// <param name="className"></param>
        private void ExportUIType(string className)
        {
            string filePath = config.UITypeFilePath;

            if (!File.Exists(filePath))
            {
                Debug.LogError($"UIType.cs�����ڣ�����·��Ϊ{filePath}");
                return;
            }

            string flag = "#endregion 1";
            List<string> lines = new List<string>(File.ReadAllLines(filePath));
            string prefix = UITypePrefix.Replace("(className)", className);
            string content = $"\t\t{prefix} {UITypeValue.Replace("(className)", className)}";
            int nameIndex = lines.FindIndex(str => str.Contains(prefix));

            if (nameIndex >= 0)
            {
                lines[nameIndex] = content;
            }
            else
            {
                int flagIndex = lines.FindIndex(str => str.Trim() == flag);
                if (flagIndex >= 0)
                {
                    int inex = flagIndex;
                    lines.Insert(flagIndex, content);
                    lines.Insert(flagIndex + 1, string.Empty);
                }
                else
                {
                    Debug.LogError($"UIType.cs��û���ҵ����{flag}");
                    return;
                }
            }

            File.WriteAllLines(filePath, lines);
            Debug.Log($"{className}����ӵ�UIType.cs��");
        }

        /// <summary>
        /// ������UIPathSet
        /// </summary>
        /// <param name="className"></param>
        private void ExportUIPathSet(string className)
        {
            string filePath = config.UIPathSetFilePath;

            if (!File.Exists(filePath))
            {
                Debug.LogError($"UIPathSet.cs�����ڣ�����·��Ϊ{filePath}");
                return;
            }

            string flag = "#endregion 1";
            string assetPath = GetAssetPath();
            List<string> lines = new List<string>(File.ReadAllLines(filePath));
            string prefix = $"public const string {className} =";
            string content = $"\t\t{prefix} \"{assetPath}\";";
            int nameIndex = lines.FindIndex(str => str.Contains(prefix));

            if (nameIndex >= 0)
            {
                lines[nameIndex] = content;
            }
            else
            {
                int flagIndex = lines.FindIndex(str => str.Trim() == flag);
                if (flagIndex >= 0)
                {
                    int inex = flagIndex;
                    lines.Insert(flagIndex, content);
                    lines.Insert(flagIndex + 1, string.Empty);
                }
                else
                {
                    Debug.LogError($"UIPathSet.cs��û���ҵ����{flag}");
                    return;
                }
            }

            File.WriteAllLines(filePath, lines);
            Debug.Log($"{className}����ӵ�UIPathSet.cs��");
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        private string GetClassName()
        {
            string name = rectTransform.name;
            string className = name;

            if (className.EndsWith("panel", true, null))
                className = className.Remove(className.Length - 5);

            if (className.EndsWith("ui", true, null))
                className = className.Remove(className.Length - 2);

            if (className.StartsWith("ui", true, null))
                className = className.Remove(0, 2);

            className = $"UI{className}";

            return className;
        }

        /// <summary>
        /// Ŀ¼·��
        /// </summary>
        /// <returns></returns>
        private string GetFolderPath()
        {
            string path = config?.ClassExploer;
            if (path.IsNullOrEmpty())
                return null;

            string className = GetClassName();
            path = $"{path}/{className}";
            return path;
        }

        /// <summary>
        /// ��Դ·��
        /// </summary>
        /// <returns></returns>
        private string GetAssetPath()
        {
            GameObject obj = rectTransform?.gameObject;
            if (!obj)
                return string.Empty;

            string path = AssetDatabase.GetAssetPath(obj);
            if (path.IsNullOrEmpty())
                return string.Empty;

            switch (resourceType)
            {
                case ResourceType.Resources:
                    {
                        if (path.StartsWith("Assets/", true, null))
                        {
                            path = path.Substring(7);
                        }

                        if (path.StartsWith("Resources/"))
                        {
                            path = path.Substring(10);
                        }

                        if (path.EndsWith(".prefab", true, null))
                        {
                            path = path.Substring(0, path.Length - 7);
                        }

                        return path;
                    }
                case ResourceType.AA:
                    {
                        path = Path.GetFileNameWithoutExtension(path);
                        return path;
                    }
                default:
                    return path;
            }
        }

        #region Language

        /// <summary>
        /// ˢ�¸ö��������е�XText���������ӵ�UIReference��
        /// </summary>
        private void RefreshAllXText()
        {
            UnityEditor.SerializedProperty dataProperty = serializedObject.FindProperty("_textData");
            dataProperty.ClearArray();

            List<XText> texts = GetTextComponents();
            Dictionary<string, int> keys = new Dictionary<string, int>();
            foreach (XText txt in texts)
            {
                if (txt.IgnoreLanguage)
                    continue;

                string key = txt.name;
                string newKey = key;

                keys.TryGetValue(key, out int count);
                if (count > 0)
                    newKey += count.ToString();

                keys[key] = ++count;

                AddReference(dataProperty, newKey, txt);
            }

            UnityEditor.EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }

        /// <summary>
        /// ��ӵ�������
        /// </summary>
        /// <param name="dataProperty"></param>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        private void AddReference(SerializedProperty dataProperty, string key, Object obj)
        {
            int index = dataProperty.arraySize;
            dataProperty.InsertArrayElementAtIndex(index);
            var element = dataProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("Key").stringValue = key;
            element.FindPropertyRelative("Object").objectReferenceValue = obj;
        }

        /// <summary>
        /// ��ȡ���е�XText���
        /// </summary>
        /// <returns></returns>
        private List<XText> GetTextComponents()
        {
            List<XText> list = new List<XText>();

            rectTransform.GetComponentsInChildren<XText>(true, list);

            return list;
        }

        #endregion
    }
}
#endif