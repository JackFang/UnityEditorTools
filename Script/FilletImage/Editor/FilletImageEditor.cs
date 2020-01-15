//=====================================================
// - FileName:      FilletImageEditor.cs
// - Created:       jack.fang
// - UserName:  2019/11/22 18:27:52
// - Email:         fangdexi@skyworth.com
// - Description:   FilletImage组件属性面板
// -  (C) Copyright 2008 - 2019, skyworth,Inc.
// -  All Rights Reserved.
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FilletImage), true)]
public class FilletImageEditor : Editor {

    SerializedProperty m_Sprite;
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
        //DrawDefaultInspector();
        m_Sprite = serializedObject.FindProperty("m_Sprite");
        m_Radius = serializedObject.FindProperty("FilletRadius");
        m_TriangleNum = serializedObject.FindProperty("FilletTriangleNum");
        m_Color = serializedObject.FindProperty("m_Color");
        m_Material = serializedObject.FindProperty("m_Material");
        m_RaycastTarget = serializedObject.FindProperty("m_RaycastTarget");
       // m_ImageType = (Types)serializedObject.FindProperty("m_ImageType");

        serializedObject.Update();
        EditorGUILayout.PropertyField(m_Sprite);
        EditorGUILayout.PropertyField(m_Color);
        EditorGUILayout.PropertyField(m_Material);
        EditorGUILayout.PropertyField(m_RaycastTarget);

        EditorGUILayout.PropertyField(m_Radius);
        EditorGUILayout.PropertyField(m_TriangleNum);
        serializedObject.ApplyModifiedProperties();
    }

}
