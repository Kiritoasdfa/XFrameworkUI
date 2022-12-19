using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.UI;
using UnityEditor;

namespace XFramework
{
    [CustomEditor(typeof(XText))]
    public class XTextEditor : TextEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty ignoreProperty = serializedObject.FindProperty("m_IgnoreLanguage");
            if (ignoreProperty != null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(ignoreProperty);

                if (!ignoreProperty.boolValue)
                {
                    SerializedProperty keyProperty = serializedObject.FindProperty("m_Key");
                    if (keyProperty != null)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.PropertyField(keyProperty);
                        EditorGUILayout.Space();
                    }
                }
                else
                {
                    EditorGUILayout.Space();
                }
            }
            serializedObject.ApplyModifiedProperties();

            base.OnInspectorGUI();
        }
    }
}
