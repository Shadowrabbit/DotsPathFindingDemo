// ******************************************************************
//       /\ /|       @file       RandomWalkSystem.cs
//       \ V/        @brief      随机漫步系统 给予没有目标的单位一个随机点寻路
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 11:28:03
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace RabiStar.ECS
{
    //当前帧完成路径跟随后再计算 否则会优先于玩家操作指令
    [UpdateAfter(typeof(PathFollowSystem))]
    public class RandomWalkSystem : SystemBase
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
            var gridWidth = GridController.Instance.Width;
            var gridHeight = GridController.Instance.Height;
            var cellSize = GridController.Instance.CellSize;
            //随机种子
            var random = new Random(_random.NextUInt(1, (uint) (gridWidth * gridHeight)));
            var entityCommandBuffer = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            var jobHandle = Entities.WithNone<PathFindingComponentData>().ForEach((Entity entity,
                int entityInQueryIndex, in PathFollowComponentData pathFollowComponentData,
                in Translation translation) =>
            {
                //当前实体存在路径跟随数据
                if (pathFollowComponentData.currentPathIndex != -1)
                {
                    return;
                }

                //当前的位置四舍五入来计算当前的cell位置
                var worldPos = translation.Value + new float3(.5f * cellSize, .5f * cellSize, 0);
                //寻路起始点
                var startX = (int) math.floor(worldPos.x / cellSize);
                var startY = (int) math.floor(worldPos.y / cellSize);
                //容错
                startX = math.clamp(startX, 0, gridWidth - 1);
                startY = math.clamp(startY, 0, gridHeight - 1);
                //寻路终点
                var endX = random.NextInt(0, gridWidth);
                var endY = random.NextInt(0, gridHeight);
                //设置寻路数据
                entityCommandBuffer.AddComponent(entityInQueryIndex, entity, new PathFindingComponentData()
                {
                    startPos = new int2(startX, startY), endPos = new int2(endX, endY)
                });
            }).Schedule(Dependency);
            //延迟改变结构
            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandle);
            //挂起等job完成
            jobHandle.Complete();
        }
    }
}