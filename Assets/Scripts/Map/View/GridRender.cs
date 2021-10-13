// ******************************************************************
//       /\ /|       @file       GridRender.cs
//       \ V/        @brief      地图渲染
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 01:57:05
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace RabiStar.ECS
{
    public class GridRender
    {
        private readonly Mesh _gridMesh; //地图网格

        public GridRender(Mesh mesh)
        {
            _gridMesh = mesh;
        }

        /// <summary>
        /// 刷新网格
        /// </summary>
        public void RefreshMesh(Grid grid)
        {
            var quadCount = grid.Width * grid.Height; //方格数量
            var vertices = new Vector3[4 * quadCount]; //顶点
            var uvs = new Vector2[4 * quadCount]; //纹理坐标
            var triangles = new int[6 * quadCount]; //三角
            for (var x = 0; x < grid.Width; x++)
            {
                for (var y = 0; y < grid.Height; y++)
                {
                    var index = grid.GetNodeId(x, y);
                    var quadSize = new Vector3(grid.CellSize, grid.CellSize);
                    var gridNode = grid.GetGridNode(x, y);
                    //可行走取左半 不可行走取右半
                    var uv00 = gridNode.IsWalkable ? new Vector2(0, 0) : new Vector2(.5f, .5f);
                    var uv11 = gridNode.IsWalkable ? new Vector2(.5f, .5f) : new Vector2(1f, 1f);
                    //世界坐标
                    var worldPos = grid.GetWorldPos(x, y);
                    //设置整个地图Mesh的数据
                    MapUtils.SetMeshArrayData(vertices, uvs, triangles, index, quadSize, worldPos, uv00, uv11);
                }
            }

            _gridMesh.vertices = vertices;
            _gridMesh.uv = uvs;
            _gridMesh.triangles = triangles;
        }
    }
}