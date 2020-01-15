功能：自动添加cs文件头部

打开unity安装目录对应.\Unity\Editor\Data\Resources\ScriptTemplates\81-C# Script-NewBehaviourScript.cs.txt文件

1、增加头部
//=====================================================
// - FileName:      #SCRIPTNAME#.cs
// - Created:       #AuthorName#
// - UserName:  #CreateTime#
// - Email:         #AuthorEmail#
// - Description:   
// -  (C) Copyright 2008 - 2019, skyworth,Inc.
// -  All Rights Reserved.
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class #SCRIPTNAME# : MonoBehaviour {

    // Use this for initialization
    void Start () {
        #NOTRIM#
    }
    
    // Update is called once per frame
    void Update () {
        #NOTRIM#
    }
}

2、将CopyRight.cs文件放于工程中的任一Editor目录下！