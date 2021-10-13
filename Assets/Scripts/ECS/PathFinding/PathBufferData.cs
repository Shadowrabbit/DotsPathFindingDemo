// ******************************************************************
//       /\ /|       @file       PathBufferData.cs
//       \ V/        @brief      寻路系统 路径数据
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 12:26:18
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Unity.Entities;

namespace RabiStar.ECS
{
    //大概会使用的内存长度
    [InternalBufferCapacity(50)]
    public struct PathBufferData : IBufferElementData
    {
    }
}