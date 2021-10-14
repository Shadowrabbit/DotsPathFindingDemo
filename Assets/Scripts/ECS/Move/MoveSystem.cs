// ******************************************************************
//       /\ /|       @file       MoveSystem.cs
//       \ V/        @brief      移动系统 存在目标点数据 则向目标移动
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 11:33:24
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace RabiStar.ECS
{
    public class MoveSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem; //命令缓冲系统

        protected override void OnCreate()
        {
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var entityCommandBuffer = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            var jobHandle = Entities
                .ForEach((Entity entity, int entityInQueryIndex, ref Translation translation,
                    in MoveComponentData moveComponentData, in MoveSpeedComponentData moveSpeedComponentData) =>
                {
                    //方向
                    var dir = math.normalizesafe(moveComponentData.targetPos - translation.Value);
                    //速度
                    var speed = moveSpeedComponentData.speed;
                    translation.Value += speed * deltaTime * dir;
                    //移动完成 撤销移动组件
                    entityCommandBuffer.RemoveComponent(entityInQueryIndex, entity, typeof(MoveComponentData));
                }).ScheduleParallel(Dependency);
            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);
        }
    }
}