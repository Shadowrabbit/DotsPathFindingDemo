// ******************************************************************
//       /\ /|       @file       MoveComponentData.cs
//       \ V/        @brief      移动组件数据
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 11:40:54
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Unity.Entities;
using Unity.Mathematics;

namespace RabiStar.ECS
{
    public struct MoveComponentData : IComponentData
    {
        public float3 targetPos; //目标点
    }
}
