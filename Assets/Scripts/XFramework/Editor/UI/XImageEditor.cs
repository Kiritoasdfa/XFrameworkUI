using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using System.Reflection;

namespace XFramework
{
    [CustomEditor(typeof(Image), true)]
    internal class NewImageEditor : ImageEditor
    {
        protected Sprite m_OverrideSprite;

        protected GUIContent m_OverrideSpriteContent;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_OverrideSpriteContent = EditorGUIUtility.TrTextContent("Override Image");
            var overrideSpriteField = typeof(Image).GetField("m_OverrideSprite", BindingFlags.Instance | BindingFlags.NonPublic);
            m_OverrideSprite = (Sprite)overrideSpriteField.GetValue(target);
        }

        public override void OnInspectorGUI()
        {
            base.serializedObject.Update();
            if (Application.isPlaying)
            {
                GUI.enabled = false;
                EditorGUILayout.ObjectField(m_OverrideSpriteContent, m_OverrideSprite, typeof(Sprite), target);
                EditorGUILayout.Space();
                GUI.enabled = true;
            }
            base.OnInspectorGUI();
        }
    }

    //internal class XImageEditor  : ImageEditor
    //{
    //}
}
