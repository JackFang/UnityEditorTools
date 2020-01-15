//=====================================================
// - FileName:      FilletImage.cs
// - Created:       jack.fang
// - UserName:  2019/11/22
// - Email:         fangdexi@skyworth.com
// - Description:   圆角Image组件
// -  (C) Copyright 2008 - 2019, skyworth,Inc.
// -  All Rights Reserved.
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;

public class FilletImage : Image {

    [Header("使用多少个三角面填充每个圆角")]
    [Range(1, 20)] public int FilletTriangleNum=8;//10个左右可得到不错的圆角效果
    [Header("每个圆角半径")]
    [Range(0,360)]public float FilletRadius=10;

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        if (!this.isActiveAndEnabled) return;
        Vector4 val = GetDrawingDimensions(false);
        Vector4 val_uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;

        Color32 val_color32 = color;
        toFill.Clear();
        //限制角度FilletRadius
        float radius = FilletRadius;
        if (radius > (val.z - val.x) / 2) radius = (val.z - val.x) / 2;
        if (radius > (val.w - val.y) / 2) radius = (val.w - val.y) / 2;
        if (radius < 0) radius = 0;

        //计算val_uv中对应的半径值坐标轴的半径
        float uv_RadiusX = radius / (val.z-val.x);
        float uv_RadiusY = radius / (val.w-val.y);

        //0，1
        toFill.AddVert(new Vector3(val.x, val.w - radius), val_color32, new Vector2(val_uv.x, val_uv.w - uv_RadiusY));
        toFill.AddVert(new Vector3(val.x, val.y + radius), val_color32, new Vector2(val_uv.x, val_uv.y + uv_RadiusY));

        //2，3，4，5
        toFill.AddVert(new Vector3(val.x + radius, val.w), val_color32, new Vector2(val_uv.x + uv_RadiusX, val_uv.w));
        toFill.AddVert(new Vector3(val.x + radius, val.w - radius), val_color32, new Vector2(val_uv.x + uv_RadiusX, val_uv.w - uv_RadiusY));
        toFill.AddVert(new Vector3(val.x + radius, val.y + radius), val_color32, new Vector2(val_uv.x + uv_RadiusX, val_uv.y + uv_RadiusY));
        toFill.AddVert(new Vector3(val.x + radius, val.y), val_color32, new Vector2(val_uv.x + uv_RadiusX, val_uv.y));

        //6，7，8，9
        toFill.AddVert(new Vector3(val.z - radius, val.w), val_color32, new Vector2(val_uv.z - uv_RadiusX, val_uv.w));
        toFill.AddVert(new Vector3(val.z - radius, val.w - radius), val_color32, new Vector2(val_uv.z - uv_RadiusX, val_uv.w - uv_RadiusY));
        toFill.AddVert(new Vector3(val.z - radius, val.y + radius), val_color32, new Vector2(val_uv.z - uv_RadiusX, val_uv.y + uv_RadiusY));
        toFill.AddVert(new Vector3(val.z - radius, val.y), val_color32, new Vector2(val_uv.z - uv_RadiusX, val_uv.y));

        //10，11
        toFill.AddVert(new Vector3(val.z, val.w - radius), val_color32, new Vector2(val_uv.z, val_uv.w - uv_RadiusY));
        toFill.AddVert(new Vector3(val.z, val.y + radius), val_color32, new Vector2(val_uv.z, val_uv.y + uv_RadiusY));

        //左边的矩形
        toFill.AddTriangle(1, 0, 3);
        toFill.AddTriangle(1, 3, 4);
        //中间的矩形
        toFill.AddTriangle(5, 2, 6);
        toFill.AddTriangle(5, 6, 9);
        //右边的矩形
        toFill.AddTriangle(8, 7, 10);
        toFill.AddTriangle(8, 10, 11);

        //开始构造四个角
        List<Vector2> vCenterList = new List<Vector2>();
        List<Vector2> uvCenterList = new List<Vector2>();
        List<int> vCenterVertList = new List<int>();

        //右上角的圆心
        vCenterList.Add(new Vector2(val.z - radius, val.w - radius));
        uvCenterList.Add(new Vector2(val_uv.z - uv_RadiusX, val_uv.w - uv_RadiusY));
        vCenterVertList.Add(7);

        //左上角的圆心
        vCenterList.Add(new Vector2(val.x + radius, val.w - radius));
        uvCenterList.Add(new Vector2(val_uv.x + uv_RadiusX, val_uv.w - uv_RadiusY));
        vCenterVertList.Add(3);

        //左下角的圆心
        vCenterList.Add(new Vector2(val.x + radius, val.y + radius));
        uvCenterList.Add(new Vector2(val_uv.x + uv_RadiusX, val_uv.y + uv_RadiusY));
        vCenterVertList.Add(4);

        //右下角的圆心
        vCenterList.Add(new Vector2(val.z - radius, val.y + radius));
        uvCenterList.Add(new Vector2(val_uv.z - uv_RadiusX, val_uv.y + uv_RadiusY));
        vCenterVertList.Add(8);

        //每个三角形的顶角
        float degreeDelta = (float)(Mathf.PI / 2 / FilletTriangleNum);

        float curDegree = 0;

        for (int i = 0; i < vCenterVertList.Count; i++)
        {
            int preVertNum = toFill.currentVertCount;
            for (int j = 0; j <= FilletTriangleNum; j++)
            {
                float cosA = Mathf.Cos(curDegree);
                float sinA = Mathf.Sin(curDegree);
                Vector3 vPosition = new Vector3(vCenterList[i].x + cosA * radius, vCenterList[i].y + sinA * radius);
                Vector3 uvPosition = new Vector2(uvCenterList[i].x + cosA * uv_RadiusX, uvCenterList[i].y + sinA * uv_RadiusY);
                toFill.AddVert(vPosition, val_color32, uvPosition);
                curDegree += degreeDelta;
            }
            curDegree -= degreeDelta;
            for (int j = 0; j <= FilletTriangleNum - 1; j++)
            {
                toFill.AddTriangle(vCenterVertList[i], preVertNum + j + 1, preVertNum + j);
            }
        }
    }

    private Vector4 GetDrawingDimensions(bool preserveAspect_bool=false)
    {
        Vector4 padding = overrideSprite == null ? Vector4.zero : DataUtility.GetPadding(overrideSprite);
        Rect i_rect = GetPixelAdjustedRect();
        Vector2 i_size = overrideSprite == null ? new Vector2(i_rect.width, i_rect.height) : new Vector2(overrideSprite.rect.width, overrideSprite.rect.height);

        Vector2Int sprite_wh = new Vector2Int(Mathf.RoundToInt(i_size.x), Mathf.RoundToInt(i_size.y));

        if(preserveAspect_bool && i_size.sqrMagnitude >0f)
        {
            float sprite_ratio = i_size.x / i_size.y;
            float rect_ratio = i_rect.width / i_rect.height;

            if(sprite_ratio > rect_ratio)
            {
                float old_height = i_rect.height;
                i_rect.height = i_rect.width * (1f / sprite_ratio);
                i_rect.y += (old_height - i_rect.height) * rectTransform.pivot.y;
            }
            else
            {
                float old_width = i_rect.width;
                i_rect.width = i_rect.height * sprite_ratio;
                i_rect.x += (old_width - i_rect.width) * rectTransform.pivot.x;
            }
        }

        Vector4 val = new Vector4(
            padding.x/ sprite_wh.x,
            padding.y/sprite_wh.y,
            (sprite_wh.x-padding.z)/sprite_wh.x,
            (sprite_wh.y-padding.w)/sprite_wh.y
            );

        val = new Vector4(
            i_rect.x + i_rect.width * val.x,
            i_rect.y + i_rect.height * val.y,
            i_rect.x + i_rect.width * val.z,
            i_rect.y + i_rect.height * val.w
            );

        return val;
    }
}
