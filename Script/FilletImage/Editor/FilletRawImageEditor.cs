//=====================================================
// - FileName:      FilletImageEditor.cs
// - Created:       jack.fang
// - UserName:  2019/11/22 18:27:52
// - Email:         fangdexi@skyworth.com
// - Description:   FilletRawImage组件属性面板
// -  (C) Copyright 2008 - 2019, skyworth,Inc.
// -  All Rights Reserved.
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FilletRawImage), true)]
public class FilletRawImageEditor : Editor
{
    SerializedProperty m_Texture;
    SerializedProperty m_Radius;
    SerializedProperty m_TriangleNum;
    SerializedProperty m_Color;
    SerializedProperty m_Material;
    SerializedProperty m_RaycastTarget;

    protected void OnEnable()
    {

    }
    public override void OnInspectorGUI()
    {
        m_Texture = serializedObject.FindProperty("m_Texture");
        m_Radius = serializedObject.FindProperty("FilletRadius");
        m_TriangleNum = serializedObject.FindProperty("FilletTriangleNum");
        m_Color = serializedObject.FindProperty("m_Color");
        m_Material = serializedObject.FindProperty("m_Material");
        m_RaycastTarget = serializedObject.FindProperty("m_RaycastTarget");

        serializedObject.Update();
        EditorGUILayout.PropertyField(m_Texture);
        EditorGUILayout.PropertyField(m_Color);
        EditorGUILayout.PropertyField(m_Material);
        EditorGUILayout.PropertyField(m_RaycastTarget);

        EditorGUILayout.PropertyField(m_Radius);
        EditorGUILayout.PropertyField(m_TriangleNum);
        serializedObject.ApplyModifiedProperties();
    }
}
