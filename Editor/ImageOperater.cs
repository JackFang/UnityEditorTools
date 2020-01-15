//=====================================================
// - FileName:      ImageOperater.cs
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
using UnityEngine.UI;
using UnityEditor;

public class ImageOperater : EditorWindow
{

    [MenuItem("Tools/Script/Image替换 %2")]
    public static void Open()
    {
        EditorWindow.GetWindow(typeof(ImageOperater), true, "图片控制器");
    }

    Object switchSprite;
    Object originSprite;
    int switchCount=0;

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        switchSprite = EditorGUILayout.ObjectField("选择理想的Image", switchSprite, typeof(Sprite), true, GUILayout.MinWidth(100));
        originSprite = EditorGUILayout.ObjectField("选择需要特换的Image", originSprite, typeof(Sprite), true, GUILayout.MinWidth(100));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("替换Sprite到场景"))
        {
            SwitchSpriteToSceneObject();
        }
        if (GUILayout.Button("替换Sprite到预制件"))
        {
            SwitchSpriteToToPrefab();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void SwitchSpriteToSceneObject()
    {
        if (switchSprite != null)
        {
            Object[] objects = Selection.GetFiltered(typeof(Image), SelectionMode.Deep);
            switchCount = 0;
            foreach (Object obj in objects)
            {
                if (obj != null)
                {
                    Image TempImg = (Image)obj;
                    Undo.RecordObject(TempImg, TempImg.gameObject.name);

                    if(TempImg.sprite != null)
                    {
                        string sp_name = TempImg.sprite.name;
                        if(sp_name == originSprite.name)
                        {
                            TempImg.sprite = (Sprite)switchSprite;
                            Debug.Log("图片-->"+obj.name + ":替换" + switchSprite.name);
                            switchCount++;
                        }
                    }
                    else
                    {
                        Debug.Log(obj.name + ":未赋予SourceSprite");
                    }
                    EditorUtility.SetDirty(TempImg);
                }
            }
            Debug.Log("<color=green> 当前节点共有: </color>" + objects.Length
                          + "<color=green>组件,替换目标</color>" + switchCount
                          + "<color=green>个</color>");
        }
        else
        {
            Debug.LogError("Error!----Please Select Operate Sprite!!!!");
        }
    }

    private void SwitchSpriteToToPrefab()
    {
        if (switchSprite != null)
        {
            List<Object[]> objectList = new List<Object[]>();
            //获取Asset文件夹下所有Prefab的GUID
            string[] ids = AssetDatabase.FindAssets("t:Prefab");
            string tmpPath;
            GameObject tmpObj;
            Object[] tmpArr;
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
                        tmpArr = tmpObj.GetComponentsInChildren<Image>(true);
                        if (tmpArr != null && tmpArr.Length > 0)
                            objectList.Add(tmpArr);
                    }
                }
            }

            int objectCount = 0;
            int realCount = 0;
            for (int i = 0; i < objectList.Count; i++)
            {
                for (int j = 0; j < objectList[i].Length; j++)
                {
                    objectCount++;
                    Undo.RecordObject(objectList[i][j], objectList[i][j].name);

                    string sp_name = ((Image)objectList[i][j]).sprite.name;
                    if (sp_name == originSprite.name)
                    {
                        ((Image)objectList[i][j]).sprite = (Sprite)switchSprite;
                        Debug.Log("图片-->" + ((Image)objectList[i][j]).name + ":替换" + switchSprite.name);
                        realCount++;
                    }
                    EditorUtility.SetDirty(objectList[i][j]);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("<color=green> 当前ProJect共有：Prefab </color>" + ids.Length
                              + "<color=green> 个，带有目标组件Prefab </color>" + objectList.Count
                              + "<color=green> 个，需要添加的组件 </color>" + objectCount
                              + "<color=green> 个,  实际添加组件 </color>" + realCount + "<color=green> 个</color>");
        }
        else
        {
            Debug.LogError("Error!----Please Select Operate Image!!!!");
        }
    }
}
