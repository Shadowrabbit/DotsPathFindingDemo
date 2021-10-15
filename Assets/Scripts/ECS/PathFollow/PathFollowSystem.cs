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
using Unity.Transforms;

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
            var jobHandleFollow = Entities.ForEach((Entity entity, int entityInQueryIndex,
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
            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandleFollow);
            
            //////////////////////////////////////////////////
            var gridWidth = GridController.Instance.Width;
            var gridHeight = GridController.Instance.Height;
            var cellSize = GridController.Instance.CellSize;
            //随机种子
            var random = new Random(_random.NextUInt(1, (uint) (gridWidth * gridHeight)));
            //var entityCommandBuffer = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            var jobHandleRandomWalk = Entities.WithNone<PathFindingComponentData>().ForEach((Entity entity,
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
            }).ScheduleParallel(jobHandleFollow);
            //延迟改变结构
            _endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(jobHandleRandomWalk);
            Dependency = jobHandleRandomWalk;
        }
    }
}