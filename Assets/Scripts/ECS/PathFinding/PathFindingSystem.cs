// ******************************************************************
//       /\ /|       @file       PathFindingSystem.cs
//       \ V/        @brief      寻路系统
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 11:14:09
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace RabiStar.ECS
{
    public class PathFindingSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem _endSimulationEntityCommandBufferSystem; //命令缓冲系统
        private NativeArray<PathNode> _pathNodeArray; //寻路节点数组

        protected override void OnCreate()
        {
            _endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            //初始化寻路节点
        }

        protected override void OnStartRunning()
        {
            //如果放在OnCreate可能优先在控制系统初始化前引用地图模块
            InitPathNodeArray();
        }

        protected override void OnUpdate()
        {
            // var gridWidth = GridController.Instance.Width;
            // var gridHeight = GridController.Instance.Height;
            // var gridSize = new int2(gridWidth, gridHeight);
            // //计算列表 用于储存计算结果
            // var pathFindingJobList = new List<PathFindingJob>();
            // //工作的句柄列表
            // var jobHandleList = new NativeList<JobHandle>(Allocator.Temp);
            //并行缓冲器 会开多个线程处理job
            var entityCommandBuffer = _endSimulationEntityCommandBufferSystem.CreateCommandBuffer().AsParallelWriter();
            // //_pathNodeArray在堆上 在栈上克隆一份
            // var pathNodeArray = _pathNodeArray;
            // //寻路计算 结果在pathNodeArray的endPathNode上
            // Entities.WithoutBurst().ForEach((Entity entity, int entityInQueryIndex,
            //     in PathFindingComponentData pathFindingComponentData) =>
            // {
            //     //每份寻路的计算结果都需要储存
            //     var tempPathNodeArray = new NativeArray<PathNode>(pathNodeArray, Allocator.TempJob);
            //     var pathFindingJob = new PathFindingJob
            //     {
            //         entity = entity,
            //         gridSize = gridSize,
            //         pathNodeArray = tempPathNodeArray,
            //         startPos = pathFindingComponentData.startPos,
            //         endPos = pathFindingComponentData.endPos
            //     };
            //     pathFindingJobList.Add(pathFindingJob);
            //     jobHandleList.Add(pathFindingJob.Schedule());
            // }).Run();
            // //开始计算
            // JobHandle.CompleteAll(jobHandleList);
            // //将寻路结果从节点转换为路径 路径将保存在pathBuffer中
            // foreach (var pathConvertingJob in pathFindingJobList.Select(pathFindingJob => new PathConvertingJob
            // {
            //     entity = pathFindingJob.entity,
            //     gridSize = pathFindingJob.gridSize,
            //     pathNodeArray = pathFindingJob.pathNodeArray,
            //     pathFindingComponentDataFromEntity = GetComponentDataFromEntity<PathFindingComponentData>(),
            //     pathFollowComponentDataFromEntity = GetComponentDataFromEntity<PathFollowComponentData>(),
            //     pathBufferFromEntity = GetBufferFromEntity<PathBufferData>()
            // }))
            // {
            //     pathConvertingJob.Run();
            // }
            //移除寻路组件
            Entities.ForEach((Entity entity, int entityInQueryIndex,
                in PathFindingComponentData pathFindingComponentData) =>
            {
                //缓存移除命令 当前帧计算完毕后再撤销组件
                entityCommandBuffer.RemoveComponent<PathFindingComponentData>(entityInQueryIndex, entity);
            }).Schedule();
        }

        /// <summary>
        /// 初始化寻路节点数组
        /// </summary>
        private void InitPathNodeArray()
        {
            var gridController = GridController.Instance;
            var width = gridController.Width;
            var height = gridController.Height;
            _pathNodeArray = new NativeArray<PathNode>(width * height, Allocator.TempJob);
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var pathNodeId = gridController.GetNodeId(x, y);
                    var pathNode = new PathNode
                    {
                        x = x,
                        y = y,
                        id = pathNodeId,
                        gCost = int.MaxValue,
                        isWalkable = gridController.IsNodeWalkable(x, y),
                        cameFromNodeId = -1
                    };
                    _pathNodeArray[pathNodeId] = pathNode;
                }
            }
        }
    }
}