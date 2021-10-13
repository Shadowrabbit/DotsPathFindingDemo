// ******************************************************************
//       /\ /|       @file       PathFollowComponentData.cs
//       \ V/        @brief      路径跟随组件数据
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 11:43:15
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Unity.Entities;

namespace RabiStar.ECS
{
    public struct PathFollowComponentData : IComponentData
    {
        public int currentPathIndex; //当前寻路中的路径的节点索引(索引0是终点 最大索引是起点 倒序)
    }
}
