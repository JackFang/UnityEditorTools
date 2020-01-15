//=====================================================
// - FileName:      ScriptOperater.cs
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
using UnityEngine.UI;
/// <summary>
/// Text设置处理
/// </summary>
public class ScriptOperater : EditorWindow
{
    [MenuItem("Tools/Script/组件控制器 %1")]
    public static void Open()
    {
        EditorWindow.GetWindow(typeof(ScriptOperater), true, "组件控制器");
    }

    public System.Type targetType;
    private int type_index = 0;

    Object targetObject;
    Object originObject;
    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        targetObject = EditorGUILayout.ObjectField("选择目标组件", targetObject,typeof(MonoScript),true, GUILayout.MinWidth(100));
        type_index = EditorGUILayout.IntPopup("选择依赖组件",type_index
                                                    , new string[] {"None","Text", "Image", "Button", "InputField" }
                                                    , new int[] { 0, 1, 2, 3,4 }
                                                    , GUILayout.MinWidth(100));
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("添加目标组件到场景"))
        {
            AddComponentToSceneObject();
        }
        if (GUILayout.Button("添加目标组件到预制件"))
        {
            AddComponentToPrefab();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("删除场景目标组件"))
        {
            DelComponentToSceneObject();
        }
        if (GUILayout.Button("删除预制件目标组件"))
        {
            DelComponentToPrefab();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void AddComponentToSceneObject()
    {
        Object[] objects = new Object[0];
        if (targetObject !=null)
        {
            System.Type cls = ((MonoScript)targetObject).GetClass();
            int addCount = 0;
            switch (type_index)
            {
                case 0:
                    break;
                case 1:
                    objects = Selection.GetFiltered(typeof(Text), SelectionMode.Deep);
                    foreach (Object obj in objects)
                    {
                        if (obj != null)
                        {
                            Text TempText = (Text)obj;
                            Undo.RecordObject(TempText, TempText.gameObject.name);
                           
                            if(TempText.gameObject.GetComponent(cls) == null)
                            {
                                TempText.gameObject.AddComponent(((MonoScript)targetObject).GetClass());
                                Debug.Log(obj.name + ":添加" + cls.Name);
                                addCount++;
                            }
                            else
                            {
                                Debug.Log(obj.name + ":已有" + cls.Name + "组件");
                            }
                            EditorUtility.SetDirty(TempText);
                        }
                    }
                    break;
                case 2:
                    objects = Selection.GetFiltered(typeof(Image), SelectionMode.Deep);
                    foreach (Object obj in objects)
                    {
                        if (obj != null)
                        {
                            Image TempImage = (Image)obj;
                            Undo.RecordObject(TempImage, TempImage.gameObject.name);

                            if (TempImage.gameObject.GetComponent(cls) == null)
                            {
                                TempImage.gameObject.AddComponent(((MonoScript)targetObject).GetClass());
                                Debug.Log(obj.name + ":添加" + cls.Name);
                                addCount++;
                            }
                            else
                            {
                                Debug.Log(obj.name + ":已有" + cls.Name + "组件");
                            }
                            EditorUtility.SetDirty(TempImage);
                        }
                    }
                    break;
                case 3:
                    objects = Selection.GetFiltered(typeof(Button), SelectionMode.Deep);
                    foreach (Object obj in objects)
                    {
                        if (obj != null)
                        {
                            Button TempButton = (Button)obj;
                            Undo.RecordObject(TempButton, TempButton.gameObject.name);

                            if (TempButton.gameObject.GetComponent(cls) == null)
                            {
                                TempButton.gameObject.AddComponent(((MonoScript)targetObject).GetClass());
                                Debug.Log(obj.name + ":添加" + cls.Name);
                                addCount++;
                            }
                            else
                            {
                                Debug.Log(obj.name + ":已有" + cls.Name + "组件");
                            }
                            EditorUtility.SetDirty(TempButton);
                        }
                    }
                    break;
                case 4:
                    objects = Selection.GetFiltered(typeof(InputField), SelectionMode.Deep);
                    foreach (Object obj in objects)
                    {
                        if (obj != null)
                        {
                            InputField TempInputField = (InputField)obj;
                            Undo.RecordObject(TempInputField, TempInputField.gameObject.name);

                            if (TempInputField.gameObject.GetComponent(cls) == null)
                            {
                                TempInputField.gameObject.AddComponent(((MonoScript)targetObject).GetClass());
                                Debug.Log(obj.name + ":添加" + cls.Name);
                                addCount++;
                            }
                            else
                            {
                                Debug.Log(obj.name + ":已有" + cls.Name + "组件");
                            }
                            EditorUtility.SetDirty(TempInputField);
                        }
                    }
                    break;
            }
            Debug.Log("<color=green> 当前节点共有: </color>" + objects.Length
                          + "<color=green> 个符合的节点添加:</color>" + cls.Name 
                          + "<color=green>组件</color>" + addCount
                          + "<color=green>个</color>");
        }
        else
        {
            Debug.LogError("Error!----Please Select Operate Script!!!!");
        }
    }

    private void AddComponentToPrefab()
    {
        if (targetObject != null)
        {
            System.Type cls = ((MonoScript)targetObject).GetClass();
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
                        switch (type_index)
                        {
                            case 0://None
                                break;
                            case 1://Text
                                tmpArr = tmpObj.GetComponentsInChildren<Text>(true);
                                if (tmpArr != null && tmpArr.Length > 0)
                                    objectList.Add(tmpArr);
                                break;
                            case 2://Image
                                tmpArr = tmpObj.GetComponentsInChildren<Image>(true);
                                if (tmpArr != null && tmpArr.Length > 0)
                                    objectList.Add(tmpArr);
                                break;
                            case 3://Button
                                tmpArr = tmpObj.GetComponentsInChildren<Button>(true);
                                if (tmpArr != null && tmpArr.Length > 0)
                                    objectList.Add(tmpArr);
                                break;
                            case 4://IputField
                                tmpArr = tmpObj.GetComponentsInChildren<InputField>(true);
                                if (tmpArr != null && tmpArr.Length > 0)
                                    objectList.Add(tmpArr);
                                break;
                        }
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

                    if ((objectList[i][j] as Component).GetComponent(cls) == null)
                    {
                        (objectList[i][j] as Component).gameObject.AddComponent(((MonoScript)targetObject).GetClass());
                        Debug.Log(objectList[i][j].name + ":添加" + cls.Name);
                        realCount++;
                    }
                    else
                    {
                        Debug.Log(objectList[i][j].name + ":已有" + cls.Name + "组件");
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
            Debug.LogError("Error!----Please Select Operate Script!!!!");
        }
    }

    private void DelComponentToSceneObject()
    {
        Object[] objects = new Object[0];
        if (targetObject != null)
        {
            System.Type cls = ((MonoScript)targetObject).GetClass();
            int delCount = 0;
            switch (type_index)
            {
                case 0:
                    objects = Selection.GetFiltered(typeof(GameObject), SelectionMode.Deep);
                    foreach (Object obj in objects)
                    {
                        if (obj != null)
                        {
                            GameObject TempGo = (GameObject)obj;
                            //var serializedObject = new SerializedObject(TempGo);

                            Component cpt = TempGo.GetComponent(cls);
                            if (cpt != null)
                            {
                                //copy之前的组件信息
                                //UnityEditorInternal.ComponentUtility.CopyComponent(cpt);
                                //移除组件信息
                                DestroyImmediate(cpt);
                                //serializedObject.ApplyModifiedProperties();
                                delCount++;
                                Debug.Log(obj.name + ":删除" + cls.Name + "组件");
                            }
                            else
                            {
                                Debug.Log(obj.name + ":不含" + cls.Name + "组件");
                            }
                            EditorUtility.SetDirty(TempGo);
                        }
                    }
                    break;
                case 1:
                    objects = Selection.GetFiltered(typeof(Text), SelectionMode.Deep);
                    foreach (Object obj in objects)
                    {
                        if (obj != null)
                        {
                            Text TempText = (Text)obj;
                            //var serializedObject = new SerializedObject(TempText.gameObject);

                            Component cpt = TempText.gameObject.GetComponent(cls);
                            if (cpt != null)
                            {
                                // copy之前的组件信息
                                //UnityEditorInternal.ComponentUtility.CopyComponent(cpt);
                                //移除组件信息
                                DestroyImmediate(cpt);
                                //serializedObject.ApplyModifiedProperties();
                                Debug.Log(obj.name + ":删除" + cls.Name + "组件");
                                delCount++;
                            }
                            else
                            {
                                Debug.Log(obj.name + ":不含" + cls.Name + "组件");
                            }
                            EditorUtility.SetDirty(TempText);
                        }
                    }
                    break;
                case 2:
                    objects = Selection.GetFiltered(typeof(Image), SelectionMode.Deep);
                    foreach (Object obj in objects)
                    {
                        if (obj != null)
                        {
                            Image TempImage = (Image)obj;
                            Undo.RecordObject(TempImage, TempImage.gameObject.name);

                            Component cpt = TempImage.gameObject.GetComponent(cls);
                            if (cpt != null)
                            {
                                DestroyImmediate(cpt);
                                Debug.Log(obj.name + ":删除" + cls.Name + "组件");
                                delCount++;
                            }
                            else
                            {
                                Debug.Log(obj.name + ":不含" + cls.Name + "组件");
                            }
                        }
                    }
                    break;
                case 3:
                    objects = Selection.GetFiltered(typeof(Button), SelectionMode.Deep);
                    foreach (Object obj in objects)
                    {
                        if (obj != null)
                        {
                            Button TempButton = (Button)obj;
                            Undo.RecordObject(TempButton, TempButton.gameObject.name);

                            Component cpt = TempButton.gameObject.GetComponent(cls);
                            if (cpt != null)
                            {
                                DestroyImmediate(cpt);
                                Debug.Log(obj.name + ":删除" + cls.Name + "组件");
                                delCount++;
                            }
                            else
                            {
                                Debug.Log(obj.name + ":不含" + cls.Name + "组件");
                            }
                            EditorUtility.SetDirty(TempButton);
                        }
                    }
                    break;
                case 4:
                    objects = Selection.GetFiltered(typeof(InputField), SelectionMode.Deep);
                    foreach (Object obj in objects)
                    {
                        if (obj != null)
                        {
                            InputField TempInputField = (InputField)obj;
                            Undo.RecordObject(TempInputField, TempInputField.gameObject.name);

                            Component cpt = TempInputField.gameObject.GetComponent(cls);
                            if (cpt == null)
                            {
                                DestroyImmediate(cpt);
                                Debug.Log(obj.name + ":删除" + cls.Name + "组件");
                                delCount++;
                            }
                            else
                            {
                                Debug.Log(obj.name + ":不含" + cls.Name + "组件");
                            }
                            EditorUtility.SetDirty(TempInputField);
                        }
                    }
                    break;
            }
            Debug.Log("<color=green> 当前节点共有: </color>" + objects.Length
                          + "<color=green> 个符合的节点删除:</color>" + cls.Name 
                          + "<color=green>组件</color>" + delCount 
                          + "<color=green>个</color>");
        }
        else
        {
            Debug.LogError("Error!----Please Select Operate Script!!!!");
            //TODO:这里可以用于删除空脚本（丢失的脚本）
        }
    }

    private void DelComponentToPrefab()
    {
        if (targetObject != null)
        {
            System.Type cls = ((MonoScript)targetObject).GetClass();
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
                        switch (type_index)
                        {
                            case 0://None
                                tmpArr = tmpObj.GetComponentsInChildren<GameObject>(true);
                                if (tmpArr != null && tmpArr.Length > 0)
                                    objectList.Add(tmpArr);
                                break;
                            case 1://Text
                                tmpArr = tmpObj.GetComponentsInChildren<Text>(true);
                                if (tmpArr != null && tmpArr.Length > 0)
                                    objectList.Add(tmpArr);
                                break;
                            case 2://Image
                                tmpArr = tmpObj.GetComponentsInChildren<Image>(true);
                                if (tmpArr != null && tmpArr.Length > 0)
                                    objectList.Add(tmpArr);
                                break;
                            case 3://Button
                                tmpArr = tmpObj.GetComponentsInChildren<Button>(true);
                                if (tmpArr != null && tmpArr.Length > 0)
                                    objectList.Add(tmpArr);
                                break;
                            case 4://IputField
                                tmpArr = tmpObj.GetComponentsInChildren<InputField>(true);
                                if (tmpArr != null && tmpArr.Length > 0)
                                    objectList.Add(tmpArr);
                                break;
                        }
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

                    Component cpt = (objectList[i][j] as Component).GetComponent(cls);
                    if (cpt != null)
                    {
                        DestroyImmediate(cpt,true);
                        Debug.Log(objectList[i][j].name + ":删除" + cls.Name);
                        realCount++;
                    }
                    else
                    {
                        Debug.Log(objectList[i][j].name + ":不含" + cls.Name + "组件");
                    }
                    EditorUtility.SetDirty(objectList[i][j]);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("<color=green> 当前ProJect共有：Prefab </color>" + ids.Length
                              + "<color=green> 个，带有目标组件Prefab </color>" + objectList.Count
                              + "<color=green> 个，需要删除的组件 </color>" + objectCount
                              + "<color=green> 个,  实际删除组件 </color>" + realCount + "<color=green> 个</color>");
        }
        else
        {
            Debug.LogError("Error!----Please Select Operate Script!!!!");
            //TODO:这里可以用于删除空脚本（丢失的脚本）
        }
    }
}
