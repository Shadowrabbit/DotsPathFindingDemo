// ******************************************************************
//       /\ /|       @file       RandomWalkSystem.cs
//       \ V/        @brief      随机漫步系统 给予没有目标的单位一个随机点寻路
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 11:28:03
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Unity.Entities;

namespace RabiStar.ECS
{
    //当前帧完成路径跟随后再计算 否则会优先于玩家操作指令
    [UpdateAfter(typeof(PathFollowSystem))]
    public class RandomWalkSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            
        }
    }
}
