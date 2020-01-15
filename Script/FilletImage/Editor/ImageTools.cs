//=====================================================
// - FileName:     ImageTools.cs
// - Created:       jack.fang
// - UserName:  2019/11/27 16:41:32
// - Email:         fangdexi@skyworth.com
// - Description:   ���Image������˵��б��У����Զ�ȡ��RatcastTarget
// -  (C) Copyright 2008 - 2019, skyworth,Inc.
// -  All Rights Reserved.
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ImageTools
{
    /// <summary>
    /// ����Image���Զ�ȡ��RatcastTarget
    /// </summary>
    [MenuItem("GameObject/UI/Image")]
    static void CreatImage()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("Image", typeof(Image));
                go.GetComponent<Image>().raycastTarget = false;
                go.transform.SetParent(Selection.activeTransform);
            }
        }
    }
    /// <summary>
    /// ����RawImage���Զ�ȡ��RatcastTarget
    /// </summary>
    [MenuItem("GameObject/UI/Raw Image")]
    static void CreatRawImage()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("RawImage", typeof(RawImage));
                go.GetComponent<RawImage>().raycastTarget = false;
                go.transform.SetParent(Selection.activeTransform);
            }
        }
    }

    /// <summary>
    /// ����FilletImage���Զ�ȡ��RatcastTarget
    /// </summary>
    [MenuItem("GameObject/UI/Fillet Image")]
    static void CreatFilletImage()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("FilletImage", typeof(FilletImage));
                go.GetComponent<FilletImage>().raycastTarget = false;
                go.transform.SetParent(Selection.activeTransform);
            }
        }
    }
    /// <summary>
    /// ����FilletRawImage���Զ�ȡ��RatcastTarget
    /// </summary>
    [MenuItem("GameObject/UI/Fillet RawImage")]
    static void CreatFilletRawImage()
    {
        if (Selection.activeTransform)
        {
            if (Selection.activeTransform.GetComponentInParent<Canvas>())
            {
                GameObject go = new GameObject("FilletRawImage", typeof(FilletRawImage));
                go.GetComponent<FilletRawImage>().raycastTarget = false;
                go.transform.SetParent(Selection.activeTransform);
            }
        }
    }
}
