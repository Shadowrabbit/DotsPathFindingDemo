// ******************************************************************
//       /\ /|       @file       GridNode.cs
//       \ V/        @brief      地图网格节点
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 12:28:01
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

namespace RabiStar.ECS
{
    public class GridNode
    {
        public readonly int x; //地图网格x坐标
        public readonly int y; //地图网格y坐标
        public bool IsWalkable { get; private set; }

        public GridNode(int x, int y)
        {
            this.x = x;
            this.y = y;
            IsWalkable = true;
        }

        /// <summary>
        /// 设置可行走性
        /// </summary>
        /// <param name="value"></param>
        public void SetWalkable(bool value)
        {
            IsWalkable = value;
        }
    }
}