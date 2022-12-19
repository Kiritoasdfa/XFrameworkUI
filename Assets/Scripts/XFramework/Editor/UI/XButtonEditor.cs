using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
namespace XFramework
{
    [CustomEditor(typeof(XButton))]
    internal class XButtonEditor : ButtonEditor
    {
        private List<string> normalPropertyName = new List<string>();

        private List<string> groupPropertyName = new List<string>();

        private static bool foldout;

        private const string groupName = "m_ButtonGroup";

        protected override void OnEnable()
        {
            base.OnEnable();

            if (normalPropertyName.Count == 0)
            {
                normalPropertyName.Add("m_LongPressInterval");
                normalPropertyName.Add("m_MaxLongPressCount");
                normalPropertyName.Add("m_OnLongPress");
                normalPropertyName.Add("m_OnLongPressEnd");
                normalPropertyName.Add("m_OnValueChanged");
            }

            if (groupPropertyName.Count == 0)
            {
                groupPropertyName.Add("m_IsOn");
            }
        }

        public override void OnInspectorGUI()
        {
            //serializedObject.Update();

            base.OnInspectorGUI();

            foldout = EditorGUILayout.Foldout(foldout, "Extensions", true);
            if (foldout)
            {
                SerializedProperty groupProperty = GetProperty(groupName);
                if (groupProperty != null)
                {
                    PropertyField(groupName);
                    if (groupProperty.objectReferenceValue != null)
                    {
                        foreach (var name in groupPropertyName)
                        {
                            PropertyField(name);
                        }
                    }
                    PropertyField("m_SelectedState");
                }

                foreach (var name in normalPropertyName)
                {
                    PropertyField(name);
                }
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }

        private void PropertyField(string propertyPath)
        {
            EditorGUILayout.Separator();
            SerializedProperty property = GetProperty(propertyPath);
            if (property != null)
            {
                EditorGUILayout.PropertyField(property);
            }
        }

        private SerializedProperty GetProperty(string propertyPath)
        {
            SerializedProperty property = serializedObject.FindProperty(propertyPath);
            return property;
        }
    }
}
#endif