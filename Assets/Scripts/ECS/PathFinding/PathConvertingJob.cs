// ******************************************************************
//       /\ /|       @file       PathConvertingJob.cs
//       \ V/        @brief      路径转换计算 把寻路终点转换成路径列表
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-14 04:00:51
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace RabiStar.ECS
{
    [BurstCompile]
    public struct PathConvertingJob : IJob
    {
        public Entity entity;

        public int2 gridSize; //网格尺寸

        //运算完成后自动释放标签
        [DeallocateOnJobCompletion] public NativeArray<PathNode> pathNodeArray;
        public ComponentDataFromEntity<PathFindingComponentData> pathFindingComponentDataFromEntity;
        public ComponentDataFromEntity<PathFollowComponentData> pathFollowComponentDataFromEntity;
        public BufferFromEntity<PathBufferData> pathBufferFromEntity;

        public void Execute()
        {
            //todo 测试
            if (entity.Index != 200)
            {
                return;
            }

            // var pathBuffer = pathBufferFromEntity[entity];
            // //清除缓存
            // pathBuffer.Clear();
            // //实体上的寻路数据
            // var pathFindingComponentData = pathFindingComponentDataFromEntity[entity];
            // //寻路结束节点的id
            // var endPathNodeId = pathFindingComponentData.endPos.x + pathFindingComponentData.endPos.y * gridSize.x;
            // //寻路结束节点
            // var endPathNode = pathNodeArray[endPathNodeId];
            // //没有父节点 寻路失败
            // if (endPathNode.cameFromNodeId == -1)
            // {
            //     pathFollowComponentDataFromEntity[entity] = new PathFollowComponentData {currentPathIndex = -1};
            //     return;
            // }
            //
            // SetPathBuffer(endPathNode, pathBuffer);
            // //设置寻路起点为路径跟随目标
            // pathFollowComponentDataFromEntity[entity] = new PathFollowComponentData
            //     {currentPathIndex = pathBuffer.Length - 1};
        }

        /// <summary>
        /// 设置路径数据
        /// </summary>
        /// <param name="endPathNode"></param>
        /// <param name="pathBuffer"></param>
        private void SetPathBuffer(PathNode endPathNode, DynamicBuffer<PathBufferData> pathBuffer)
        {
            //没有路径
            if (endPathNode.cameFromNodeId == -1)
            {
                return;
            }

            //先添加终点
            pathBuffer.Add(new PathBufferData {position = new int2(endPathNode.x, endPathNode.y)});
            var currentPathNode = endPathNode;
            while (currentPathNode.cameFromNodeId != -1)
            {
                //上个节点
                var lastPathNode = pathNodeArray[currentPathNode.cameFromNodeId];
                pathBuffer.Add(new PathBufferData {position = new int2(currentPathNode.x, currentPathNode.y)});
                currentPathNode = lastPathNode;
            }
        }
    }
}