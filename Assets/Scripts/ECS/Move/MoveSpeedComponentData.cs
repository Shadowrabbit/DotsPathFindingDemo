// ******************************************************************
//       /\ /|       @file       MoveSpeedComponentData.cs
//       \ V/        @brief      移动速度
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-14 12:01:15
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Unity.Entities;

namespace RabiStar.ECS
{
    public struct MoveSpeedComponentData : IComponentData
    {
        public int speed;
    }
}