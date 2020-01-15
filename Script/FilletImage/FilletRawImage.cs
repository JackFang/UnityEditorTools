//=====================================================
// - FileName:      FilletRawImage.cs
// - Created:       jack.fang
// - UserName:  2019/11/22 18:27:52
// - Email:         fangdexi@skyworth.com
// - Description:   圆角RawImage组件
// -  (C) Copyright 2008 - 2019, skyworth,Inc.
// -  All Rights Reserved.
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;

public class FilletRawImage : RawImage
{
    [Header("使用多少个三角面填充每个圆角")]
    [Range(1, 20)] public int FilletTriangleNum=8;//10个左右可得到不错的圆角效果
    [Header("每个圆角半径")]
    [Range(0, 512)] public float FilletRadius=10;//图片的一半可成圆

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        if (!this.isActiveAndEnabled) return;
        vh.Clear();
        float tw = rectTransform.rect.width;//图片的宽
        float th = rectTransform.rect.height;//图片的高
        float twr = tw / 2;
        float thr = th / 2;

        if (FilletRadius < 0)
            FilletRadius = 0;
        float radius = FilletRadius;//半径这里需要动态计算确保不会被拉伸
        if (radius > twr)
            radius = twr;
        if (radius < 0)
            radius = 0;
        if (FilletTriangleNum <= 0)
            FilletTriangleNum = 1;

        UIVertex vert = UIVertex.simpleVert;
        vert.color = color;

        //左边矩形
        AddVert(new Vector2(-twr, -thr + radius), tw, th, vh, vert);
        AddVert(new Vector2(-twr, thr - radius), tw, th, vh, vert);
        AddVert(new Vector2(-twr + radius, thr - radius), tw, th, vh, vert);
        AddVert(new Vector2(-twr + radius, -thr + radius), tw, th, vh, vert);
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(0, 2, 3);

        //中间矩形
        AddVert(new Vector2(-twr + radius, -thr), tw, th, vh, vert);
        AddVert(new Vector2(-twr + radius, thr), tw, th, vh, vert);
        AddVert(new Vector2(twr - radius, thr), tw, th, vh, vert);
        AddVert(new Vector2(twr - radius, -thr), tw, th, vh, vert);
        vh.AddTriangle(4, 5, 6);
        vh.AddTriangle(4, 6, 7);

        //右边矩形
        AddVert(new Vector2(twr - radius, -thr + radius), tw, th, vh, vert);
        AddVert(new Vector2(twr - radius, thr - radius), tw, th, vh, vert);
        AddVert(new Vector2(twr, thr - radius), tw, th, vh, vert);
        AddVert(new Vector2(twr, -thr + radius), tw, th, vh, vert);
        vh.AddTriangle(8, 9, 10);
        vh.AddTriangle(8, 10, 11);

        List<Vector2> CirclePoint = new List<Vector2>();

        Vector2 pos0 = new Vector2(-twr + radius, -thr + radius);
        Vector2 pos1 = new Vector2(-twr, -thr + radius);
        CirclePoint.Add(pos0);
        CirclePoint.Add(pos1);

        pos0 = new Vector2(-twr + radius, thr - radius);
        pos1 = new Vector2(-twr + radius, thr);
        CirclePoint.Add(pos0);
        CirclePoint.Add(pos1);

        pos0 = new Vector2(twr - radius, thr - radius);
        pos1 = new Vector2(twr, thr - radius);
        CirclePoint.Add(pos0);
        CirclePoint.Add(pos1);

        pos0 = new Vector2(twr - radius, -thr + radius);
        pos1 = new Vector2(twr - radius, -thr);
        CirclePoint.Add(pos0);
        CirclePoint.Add(pos1);

        float degreeDelta = (float)(Mathf.PI / 2 / FilletTriangleNum);
        List<float> degreeDeltaList = new List<float>() { Mathf.PI, Mathf.PI / 2, 0, (float)3 / 2 * Mathf.PI };

        Vector2 pos2;
        for (int j = 0; j < CirclePoint.Count; j += 2)
        {
            float curDegree = degreeDeltaList[j / 2];
            AddVert(CirclePoint[j], tw, th, vh, vert);
            int thrdIndex = vh.currentVertCount;
            int TriangleVertIndex = vh.currentVertCount - 1;
            List<Vector2> pos2List = new List<Vector2>();
            for (int i = 0; i < FilletTriangleNum; i++)
            {
                curDegree += degreeDelta;
                if (pos2List.Count == 0)
                {
                    AddVert(CirclePoint[j + 1], tw, th, vh, vert);
                }
                else
                {
                    vert.position = pos2List[i - 1];
                    vert.uv0 = new Vector2(pos2List[i - 1].x + 0.5f, pos2List[i - 1].y + 0.5f);
                }
                pos2 = new Vector2(CirclePoint[j].x + radius * Mathf.Cos(curDegree), CirclePoint[j].y + radius * Mathf.Sin(curDegree));
                AddVert(pos2, tw, th, vh, vert);
                vh.AddTriangle(TriangleVertIndex, thrdIndex, thrdIndex + 1);
                thrdIndex++;
                pos2List.Add(vert.position);
            }
        }
    }

    protected Vector2[] GetTextureUVS(Vector2[] vhs, float tw, float th)
    {
        int count = vhs.Length;
        Vector2[] uvs = new Vector2[count];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vhs[i].x / tw + 0.5f, vhs[i].y / th + 0.5f);//矩形的uv坐标  因为uv坐标原点在左下角，vh坐标原点在中心 所以这里加0.5（uv取值范围0~1）
        }
        return uvs;
    }

    protected void AddVert(Vector2 pos0, float tw, float th, VertexHelper vh, UIVertex vert)
    {
        vert.position = pos0;
        vert.uv0 = GetTextureUVS(new[] { new Vector2(pos0.x, pos0.y) }, tw, th)[0];
        vh.AddVert(vert);
    }
}
