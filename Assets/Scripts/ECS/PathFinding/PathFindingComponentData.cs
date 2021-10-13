// ******************************************************************
//       /\ /|       @file       PathFindingComponentData.cs
//       \ V/        @brief      寻路系统数据组件
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 11:47:01
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Unity.Entities;
using Unity.Mathematics;

namespace RabiStar.ECS
{
    public struct PathFindingComponentData : IComponentData
    {
        public int2 startPos; //寻路起始点
        public int2 endPos; //寻路目标点
    }
}
