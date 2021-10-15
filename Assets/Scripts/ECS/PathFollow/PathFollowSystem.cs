// ******************************************************************
//       /\ /|       @file       PathFollowSystem.cs
//       \ V/        @brief      路径跟随系统 如果实体存在寻路数据 则找到当前路径当前点的数据 移交MoveSystem处理
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 11:12:23
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Unity.Entities;
using Unity.Mathematics;

namespace RabiStar.ECS
{
    public class PathFollowSystem : SystemBase
    {
        private Random _random;
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem; //命令缓冲系统

        protected override void OnCreate()
        {
            _random = new Random(1);
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            //并行缓冲器 会开多个线程处理job
            var entityCommandBuffer = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            //有路径跟随组件 有路径
            var jobHandle = Entities.ForEach((Entity entity, int entityInQueryIndex,
                DynamicBuffer<PathBufferData> pathBuffer, ref PathFollowComponentData pathFollowComponentData) =>
            {
                //当前没有路径要跟随
                if (pathFollowComponentData.currentPathIndex < 0)
                {
                    return;
                }

                //存在路径
                var pathBufferData = pathBuffer[pathFollowComponentData.currentPathIndex];
                //目标点
                var targetPos = new float3(pathBufferData.position.x, pathBufferData.position.y, 0);
                //添加移动组件数据
                var moveComponentData = new MoveComponentData {targetPos = targetPos};
                entityCommandBuffer.AddComponent(entityInQueryIndex, entity, moveComponentData);
                //下个点
                pathFollowComponentData.currentPathIndex--;
            }).ScheduleParallel(Dependency);
            //添加移动组件数据破坏了archetype结构 所以使用缓冲区延迟执行
            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);
            jobHandle.Complete();
        }
    }
}