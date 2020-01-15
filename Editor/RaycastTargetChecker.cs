//=====================================================
// - FileName:      RaycastTargetChecker.cs
// - Created:       jack.fang
// - UserName:  2019/11/27 17:19:53
// - Email:         fangdexi@skyworth.com
// - Description:   检测组件中的RaycastTarget
// -  (C) Copyright 2008 - 2019, skyworth,Inc.
// -  All Rights Reserved.
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class RaycastTargetChecker : EditorWindow
{

    private MaskableGraphic[] graphics;
    private bool hideUnchecked = false;
    private bool showBorders = true;
    private Color borderColor = Color.blue;
    private Vector2 scrollPosition = Vector2.zero;

    private static RaycastTargetChecker instance = null;

    [MenuItem("Tools/Checker/RaycastTarget Checker")]
    private static void Open()
    {
        instance = instance ?? EditorWindow.GetWindow<RaycastTargetChecker>("RaycastTargets");
        instance.Show();
    }

    void OnEnable()
    {
        instance = this;
    }

    void OnDisable()
    {
        instance = null;
    }

    void OnGUI()
    {
        using (EditorGUILayout.HorizontalScope horizontalScope = new EditorGUILayout.HorizontalScope())
        {
            showBorders = EditorGUILayout.Toggle("Show Gizmos", showBorders, GUILayout.Width(200.0f));
            borderColor = EditorGUILayout.ColorField(borderColor);
        }
        hideUnchecked = EditorGUILayout.Toggle("Hide Unchecked", hideUnchecked);

        GUILayout.Space(12.0f);
        Rect rect = GUILayoutUtility.GetLastRect();
        GUI.color = new Color(0.0f, 0.0f, 0.0f, 0.25f);
        GUI.DrawTexture(new Rect(0.0f, rect.yMin + 6.0f, Screen.width, 4.0f), EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(0.0f, rect.yMin + 6.0f, Screen.width, 1.0f), EditorGUIUtility.whiteTexture);
        GUI.DrawTexture(new Rect(0.0f, rect.yMin + 9.0f, Screen.width, 1.0f), EditorGUIUtility.whiteTexture);
        GUI.color = Color.white;

        graphics = GameObject.FindObjectsOfType<MaskableGraphic>();

        using (GUILayout.ScrollViewScope scrollViewScope = new GUILayout.ScrollViewScope(scrollPosition))
        {
            scrollPosition = scrollViewScope.scrollPosition;
            for (int i = 0; i < graphics.Length; i++)
            {
                MaskableGraphic graphic = graphics[i];
                if (hideUnchecked == false || graphic.raycastTarget == true)
                {
                    DrawElement(graphic);
                }
            }
        }
        foreach (var item in graphics)
        {
            EditorUtility.SetDirty(item);
        }
        Repaint();
    }

    private void DrawElement(MaskableGraphic graphic)
    {
        using (EditorGUILayout.HorizontalScope horizontalScope = new EditorGUILayout.HorizontalScope())
        {
            Undo.RecordObject(graphic, "Modify RaycastTarget");
            graphic.raycastTarget = EditorGUILayout.Toggle(graphic.raycastTarget, GUILayout.Width(20));
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField(graphic, typeof(MaskableGraphic), true);
            EditorGUI.EndDisabledGroup();
        }
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    private static void DrawGizmos(MaskableGraphic source, GizmoType gizmoType)
    {
        if (instance != null && instance.showBorders == true && source.raycastTarget == true)
        {
            Vector3[] corners = new Vector3[4];
            source.rectTransform.GetWorldCorners(corners);
            Gizmos.color = instance.borderColor;
            for (int i = 0; i < 4; i++)
            {
                Gizmos.DrawLine(corners[i], corners[(i + 1) % 4]);
            }
            if (Selection.activeGameObject == source.gameObject)
            {
                Gizmos.DrawLine(corners[0], corners[2]);
                Gizmos.DrawLine(corners[1], corners[3]);
            }
        }
        SceneView.RepaintAll();
    }
}
