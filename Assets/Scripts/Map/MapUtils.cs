// ******************************************************************
//       /\ /|       @file       MapUtils.cs
//       \ V/        @brief      地图相关工具
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 02:21:16
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

namespace RabiStar.ECS
{
    public static class MapUtils
    {
        /// <summary>
        /// 设置mesh数据
        /// </summary>
        /// <param name="vertices">顶点数组</param>
        /// <param name="uvs">纹理坐标数组</param>
        /// <param name="triangles">三角数组</param>
        /// <param name="index">当前设置的quad的索引</param>
        /// <param name="quadSize">方形切片的尺寸</param>
        /// <param name="quadWorldPos">当前切片所在的世界坐标</param>
        /// <param name="uv00">左下角坐标</param>
        /// <param name="uv11">右上角坐标</param>
        public static void SetMeshArrayData(Vector3[] vertices, Vector2[] uvs, int[] triangles, int index,
            Vector3 quadSize, Vector3 quadWorldPos, Vector2 uv00, Vector2 uv11)
        {
            //顶点
            var vIndex0 = index * 4;
            var vIndex1 = vIndex0 + 1;
            var vIndex2 = vIndex0 + 2;
            var vIndex3 = vIndex0 + 3;
            //尺寸的一半 算顶点坐标用
            quadSize *= .5f;
            vertices[vIndex0] = quadWorldPos + Quaternion.Euler(0, 0, 0) * quadSize;
            vertices[vIndex1] = quadWorldPos + Quaternion.Euler(0, 0, 90) * quadSize;
            vertices[vIndex2] = quadWorldPos + Quaternion.Euler(0, 0, 180) * quadSize;
            vertices[vIndex3] = quadWorldPos + Quaternion.Euler(0, 0, 270) * quadSize;
            //纹理坐标
            uvs[vIndex0] = new Vector2(uv00.x, uv11.y);
            uvs[vIndex1] = new Vector2(uv00.x, uv00.y);
            uvs[vIndex2] = new Vector2(uv11.x, uv00.y);
            uvs[vIndex3] = new Vector2(uv11.x, uv11.y);
            //三角
            var tIndex0 = index * 6;
            var tIndex1 = tIndex0 + 1;
            var tIndex2 = tIndex0 + 2;
            var tIndex3 = tIndex0 + 3;
            var tIndex4 = tIndex0 + 4;
            var tIndex5 = tIndex0 + 5;
            triangles[tIndex0] = vIndex0;
            triangles[tIndex1] = vIndex3;
            triangles[tIndex2] = vIndex1;
            triangles[tIndex3] = vIndex1;
            triangles[tIndex4] = vIndex3;
            triangles[tIndex5] = vIndex2;
        }
    }
}