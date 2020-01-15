//=====================================================
// - FileName:      FontSetter.cs
// - Created:       2020年1月15日16:55:53
// - UserName:  jack.fang
// - Email:         fangdexi@skyworh.com
// - Description:   
// -  (C) Copyright 2008 - 2019, skyworth,Inc.
// -  All Rights Reserved.
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
/// <summary>
/// Text设置处理
/// </summary>
public class FontSetter : EditorWindow
{
    [MenuItem("Tools/Text/换字体 &1")]
    public static void Open()
    {
        EditorWindow.GetWindow(typeof(FontSetter), true, "字体设置");
    }

    //public Font toChange = new Font("Arial");
    public Font toChange;
    static Font toChangeFont;

    public int fontStyleIndex = 0;
    void OnGUI()
    {
        //toChange = (Font)EditorGUILayout.ObjectField("选择需要更换的字体", toChange, typeof(Font), true, GUILayout.MinWidth(100));
        //toChangeFont = toChange;
        //if (GUILayout.Button("更换字体"))
        //{
        //    Change();
        //}

        fontStyleIndex = EditorGUILayout.IntPopup(fontStyleIndex
                                                    , new string[] { FontStyle.Normal.ToString(), FontStyle.Bold.ToString(), FontStyle.Italic.ToString(), FontStyle.BoldAndItalic.ToString() }
                                                    , new int[] { (int)FontStyle.Normal, (int)FontStyle.Bold, (int)FontStyle.Italic, (int)FontStyle.BoldAndItalic }
                                                    );
        if (GUILayout.Button("更换场景字体样式"))
        {
            ChangeSceneFontStyle();
        }

        if (GUILayout.Button("更换预置物字体样式"))
        {
            ChangePrefabFontStyle();
        }
    }

    public static void Change()
    {
        Object[] Texts = Selection.GetFiltered(typeof(Text), SelectionMode.Deep);
        foreach (Object text in Texts)
        {
            if (text)
            {
                //如果是UGUI讲UILabel换成Text就可以  
                Text TempText = (Text)text;
                Undo.RecordObject(TempText, TempText.gameObject.name);
                TempText.font = toChangeFont;
                Debug.Log(text.name + ":" + TempText.text);
                EditorUtility.SetDirty(TempText);
            }
        }
    }

    public void ChangeSceneFontStyle()
    {
        Object[] Texts = Selection.GetFiltered(typeof(Text), SelectionMode.Deep);
        foreach (Object text in Texts)
        {
            if (text)
            {
                //如果是UGUI将Text换成UILabel就可以  
                Text TempText = (Text)text;
                Undo.RecordObject(TempText, TempText.gameObject.name);
                TempText.fontStyle = (FontStyle)fontStyleIndex;
                Debug.Log(text.name + ":" + TempText.fontStyle + ":" + TempText.text );
                EditorUtility.SetDirty(TempText);
            }
        }
        Debug.Log("<color=green> 当前节点共有：Text </color>" + Texts.Length
                          + "<color=green> 个</color>");
    }
    public void ChangePrefabFontStyle()
    {
        List<Text[]> textList = new List<Text[]>();
        //获取Asset文件夹下所有Prefab的GUID
        string[] ids = AssetDatabase.FindAssets("t:Prefab");
        string tmpPath;
        GameObject tmpObj;
        Text[] tmpArr;
        for (int i = 0; i < ids.Length; i++)
        {
            tmpObj = null;
            tmpArr = null;
            //根据GUID获取路径
            tmpPath = AssetDatabase.GUIDToAssetPath(ids[i]);
            if (!string.IsNullOrEmpty(tmpPath))
            {
                //根据路径获取Prefab(GameObject)
                tmpObj = AssetDatabase.LoadAssetAtPath(tmpPath, typeof(GameObject)) as GameObject;
                if (tmpObj != null)
                {
                    //获取Prefab及其子物体孙物体···的所有Text组件
                    tmpArr = tmpObj.GetComponentsInChildren<Text>(true);
                    if (tmpArr != null && tmpArr.Length > 0)
                        textList.Add(tmpArr);
                }
            }
        }
        //替换所有Text组件的字体
        int textCount = 0;
        for (int i = 0; i < textList.Count; i++)
        {
            for (int j = 0; j < textList[i].Length; j++)
            {
                textCount++;
                Undo.RecordObject(textList[i][j], textList[i][j].gameObject.name);
                //textList[i][j].font = targetFont;
                textList[i][j].fontStyle = (FontStyle)fontStyleIndex;
                Debug.Log(textList[i][j].name + ":" + textList[i][j].fontStyle + ":" + textList[i][j].text);
                EditorUtility.SetDirty(textList[i][j]);
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("<color=green> 当前ProJect共有：Prefab </color>" + ids.Length 
                          + "<color=green> 个，带有Text组件Prefab </color>" + textList.Count 
                          + "<color=green> 个，Text组件 </color>" + textCount + "<color=green> 个 </color>");
    }
}
