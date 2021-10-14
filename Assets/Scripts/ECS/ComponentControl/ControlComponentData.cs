// ******************************************************************
//       /\ /|       @file       ControlComponentData.cs
//       \ V/        @brief      控制组件数据
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-14 09:55:04
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Unity.Entities;

namespace ECS.ComponentControl
{
    [GenerateAuthoringComponent]
    public struct ControlComponentData : IComponentData
    {
        public bool isOtherSystemEnabled;
    }
}