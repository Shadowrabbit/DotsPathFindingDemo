// ******************************************************************
//       /\ /|       @file       PathNode.cs
//       \ V/        @brief      寻路用节点数据
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-14 01:48:34
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

namespace RabiStar.ECS
{
    public struct PathNode
    {
        public int id; //节点的索引
        public int x;
        public int y;
        public int gCost; //实际消耗值
        public int hCost; //预估消耗值
        public bool isWalkable; //是否可行走
        public int cameFromNodeId; //路径的上个节点的索引
        public int FCost => gCost + hCost; //启发消耗值
    }
}