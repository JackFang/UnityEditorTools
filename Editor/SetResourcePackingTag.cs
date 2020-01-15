//=====================================================
// - FileName:      Tools.cs
// - Created:       2020年1月15日
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
using System.Linq;
using System.IO;

public class Tools
{
    [MenuItem("Tools/SpritePacker/PackingTag %#1")]
    static public void SetResourcePackingTag()
    {
        EditorWindow.GetWindow<SetResourcePackingTag>("修改资源的PackingTag").Show();
    }
}

/// <summary>
/// 如果项目中有自动修改PackingTag的脚本 , 可以使用SaveAndReimport方法重新导入.
/// </summary>
public class SetResourcePackingTag : EditorWindow
{
    private List<string> textureList = new List<string>();
    Vector2 scrollPos = Vector2.zero;
    private void StartCheck(string asset_path)
    {
        List<string> withoutExtensions = new List<string>() { ".png", ".jpg" };
        string serchPath = Application.dataPath + "/" + asset_path;
        Debug.Log("Serch Path=" + serchPath);
        string[] files = Directory.GetFiles(serchPath, "*.*", SearchOption.AllDirectories)
            .Where(s => withoutExtensions.Contains(Path.GetExtension(s).ToLower())).ToArray();

        foreach (string file in files)
        {
            string strFile = file.Substring(file.IndexOf("Assets")).Replace('\\', '/');
            textureList.Add(strFile);
        }
    }

    private string GameRestPath = "GameRes/UI";
    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("UI资源路径：", GUILayout.Width(80));
        GameRestPath = EditorGUILayout.TextField(GameRestPath);
        Debug.Log("当前检测路径=" + GameRestPath);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("搜索出所有UI", GUILayout.Width(200), GUILayout.Height(50)))
        {
            textureList.Clear();
            StartCheck(GameRestPath);
        }

        if (GUILayout.Button("修改所有PackingTag", GUILayout.Width(200), GUILayout.Height(50)))
        {
            float count = 0;
            foreach (string t in textureList)
            {
                count++;
                EditorUtility.DisplayProgressBar("Processing...", "替换中... (" + count + " / " + textureList.Count + ")", count / textureList.Count);
                TextureImporter ti = AssetImporter.GetAtPath(t) as TextureImporter;

                //可以重新导入
                ti.SaveAndReimport();
                string dirName = Path.GetDirectoryName(t);
                string folderStr = Path.GetFileName(dirName);
                ti.spritePackingTag = folderStr;
            }
            EditorUtility.ClearProgressBar();
        }
        EditorGUI.EndChangeCheck();
        EditorGUILayout.EndHorizontal();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        foreach (string t in textureList)
        {
            EditorGUILayout.BeginHorizontal();
            string path = t;//t.Substring(t.IndexOf("UI"));
            EditorGUILayout.LabelField("文件路径 : " + path, GUILayout.Width(400));

            string dirName = Path.GetDirectoryName(path);
            string folderStr = Path.GetFileName(dirName);

            Texture2D texture = (Texture2D)AssetDatabase.LoadAssetAtPath(t, typeof(Texture2D));
            EditorGUILayout.ObjectField(texture, typeof(Texture2D), false);

            TextureImporter ti = AssetImporter.GetAtPath(t) as TextureImporter;
            EditorGUILayout.LabelField("当前PackingTag类型 :   [" + ti.spritePackingTag + "]");

            EditorGUILayout.Space();
            if (ti.spritePackingTag != folderStr)
            {
                EditorGUILayout.LabelField("错误 !!!  文件夹名字 : [" + folderStr + "]");
            }
            else
            {
                EditorGUILayout.LabelField("");
            }

            if (GUILayout.Button("修改PackingTag", GUILayout.Width(150)))
            {
                //可以重新导入
                ti.SaveAndReimport();
                ti.spritePackingTag = folderStr;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }
        EditorGUILayout.EndScrollView();
    }
}
