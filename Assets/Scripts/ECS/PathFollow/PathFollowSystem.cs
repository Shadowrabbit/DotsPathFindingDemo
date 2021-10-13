// ******************************************************************
//       /\ /|       @file       PathFollowSystem.cs
//       \ V/        @brief      路径跟随系统 如果实体存在寻路数据 则找到当前路径当前点的数据 移交MoveSystem处理
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 11:12:23
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Unity.Entities;

namespace RabiStar.ECS
{
    public class PathFollowSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            //throw new System.NotImplementedException();
        }
    }
}
