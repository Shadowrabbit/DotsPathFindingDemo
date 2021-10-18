// ******************************************************************
//       /\ /|       @file       Grid.cs
//       \ V/        @brief      地图网格
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 12:34:05
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace RabiStar.ECS
{
    public class Grid
    {
        private readonly Vector3 _pos; //网格位置
        private readonly GridNode[,] _gridNodeArray; //网格节点组
        public int Width { get; }
        public int Height { get; }
        public float CellSize { get; }

        public Grid(int width, int height, float cellSize, Vector3 pos)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
            _pos = pos;
            _gridNodeArray = new GridNode[width, height];
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    _gridNodeArray[x, y] = new GridNode(x, y);
                }
            }
        }

        /// <summary>
        /// 获取节点Id
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetNodeId(int x, int y)
        {
            return x + y * Width;
        }

        /// <summary>
        /// 获取某个节点的世界坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Vector3 GetWorldPos(int x, int y) => _pos + new Vector3(x, y) * CellSize;

        /// <summary>
        /// 获取某个节点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public GridNode GetGridNode(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                return default;
            }

            return _gridNodeArray[x, y];
        }

        /// <summary>
        /// 获取某个节点 通过世界坐标
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public GridNode GetGridNode(Vector3 worldPos)
        {
            var x = Mathf.FloorToInt((worldPos - _pos).x / CellSize);
            var y = Mathf.FloorToInt((worldPos - _pos).y / CellSize);
            return GetGridNode(x, y);
        }

        /// <summary>
        /// 某个节点是否可行走
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsGridNodeWalkable(int x, int y)
        {
            return _gridNodeArray[x, y].IsWalkable;
        }

        /// <summary>
        /// 设置某个节点的可行走性
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isWalkable"></param>
        public void SetGridNodeWalkable(int x, int y, bool isWalkable)
        {
            _gridNodeArray[x, y].SetWalkable(isWalkable);
        }
    }
}