// ******************************************************************
//       /\ /|       @file       PathFindingJob.cs
//       \ V/        @brief      寻路计算
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-14 02:26:17
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
    public struct PathFindingJob : IJob
    {
        public Entity entity; //在计算寻路的实体
        public int2 gridSize; //地图的尺寸
        public NativeArray<PathNode> pathNodeArray; //寻路用节点数组
        public int2 startPos; //寻路起点
        public int2 endPos; //寻路终点

        public void Execute()
        {
            //上下左右偏移
            var neighbourOffsetArray = new NativeArray<int2>(4, Allocator.Temp)
            {
                [0] = new int2(-1, 0), [1] = new int2(+1, 0), [2] = new int2(0, +1), [3] = new int2(0, -1)
            };
            //起始节点id
            var startNodeId = GetNodeId(startPos.x, startPos.y, gridSize.x);
            //起始节点初始化
            var startPathNode = pathNodeArray[startNodeId];
            startPathNode.gCost = 0;
            startPathNode.hCost = GetHCost(startPos, endPos);
            pathNodeArray[startNodeId] = startPathNode;
            //开放节点列表
            var openList = new NativeList<int>(Allocator.Temp) {startNodeId};
            //结束节点id
            var endPathNodeId = GetNodeId(endPos.x, endPos.y, gridSize.x);
            //关闭节点哈希集
            var closedMap = new NativeHashSet<int>(gridSize.x * gridSize.y, Allocator.Temp);
            while (openList.Length > 0)
            {
                //取开放列表内最低消耗节点
                var currentPathNodeId = GetLowestCostPathNodeIndex(openList);
                var currentPathNode = pathNodeArray[currentPathNodeId];
                //当前节点是结束节点 寻路成功 结束
                if (currentPathNodeId == endPathNodeId)
                {
                    break;
                }

                //从开放列表中撤销当前节点
                for (var i = 0; i < openList.Length; i++)
                {
                    if (openList[i] != currentPathNodeId) continue;
                    openList.RemoveAtSwapBack(i);
                    break;
                }

                //当前节点添加到关闭集
                closedMap.Add(currentPathNodeId);
                //对当前节点的邻节点操作
                for (var i = neighbourOffsetArray.Length - 1; i >= 0; i--)
                {
                    var neighbourOffset = neighbourOffsetArray[i];
                    var neighbourPos = new int2(currentPathNode.x + neighbourOffset.x,
                        currentPathNode.y + neighbourOffset.y);
                    //临界点超出地图范围 不处理
                    if (!IsPositionInsideGrid(neighbourPos, gridSize))
                    {
                        continue;
                    }

                    //当前邻节点id
                    var neighbourPathNodeId = GetNodeId(neighbourPos.x, neighbourPos.y, gridSize.x);
                    //当前邻节点已经记录在关闭集内
                    if (closedMap.Contains(neighbourPathNodeId))
                    {
                        continue;
                    }

                    //当前邻节点数据
                    var neighbourPathNode = pathNodeArray[neighbourPathNodeId];
                    //当前邻节点不可行走
                    if (!neighbourPathNode.isWalkable)
                    {
                        continue;
                    }

                    //从当前节点到当前邻节点实际消耗
                    var gCostFromCurrentPathNode = currentPathNode.gCost + 1;
                    //当前节点到当前邻节点并没有之前的路径消耗更低
                    if (gCostFromCurrentPathNode >= neighbourPathNode.gCost)
                    {
                        continue;
                    }

                    //更低的消耗 更新实际消耗
                    neighbourPathNode.gCost = gCostFromCurrentPathNode;
                    //更新父节点来源
                    neighbourPathNode.cameFromNodeId = currentPathNodeId;
                    //计算过的节点 一定在开放列表或者关闭集
                    if (neighbourPathNode.hCost <= 0)
                    {
                        //没计算过的节点 计算预估消耗
                        neighbourPathNode.hCost = GetHCost(neighbourPos, endPos);
                        //加入开放列表
                        openList.Add(neighbourPathNodeId);
                    }

                    pathNodeArray[neighbourPathNodeId] = neighbourPathNode;
                }
            }

            //释放非托管内存
            neighbourOffsetArray.Dispose();
            openList.Dispose();
            closedMap.Dispose();
        }

        /// <summary>
        /// 获取最低启发消耗的节点id
        /// </summary>
        /// <param name="openList"></param>
        /// <returns></returns>
        private int GetLowestCostPathNodeIndex(NativeList<int> openList)
        {
            //默认第一个元素为最低
            var lowestCostPathNode = pathNodeArray[openList[0]];
            for (var i = 1; i < openList.Length; i++)
            {
                var currentPathNode = pathNodeArray[openList[i]];
                //当前节点启发消耗更高 跳过
                if (currentPathNode.FCost > lowestCostPathNode.FCost)
                {
                    continue;
                }

                //当前节点启发消耗更低 选用当前节点
                if (currentPathNode.FCost < lowestCostPathNode.FCost)
                {
                    lowestCostPathNode = currentPathNode;
                    continue;
                }

                //启发消耗相等的情况下 当前节点预估消耗更高 
                if (currentPathNode.gCost >= lowestCostPathNode.gCost)
                {
                    continue;
                }

                //启发消耗相等的情况下 选用预估消耗更低的当前节点 
                lowestCostPathNode = currentPathNode;
            }

            return lowestCostPathNode.id;
        }

        /// <summary>
        /// 当前节点是否在网格尺寸内
        /// </summary>
        /// <param name="gridPosition"></param>
        /// <param name="gridSize"></param>
        /// <returns></returns>
        private static bool IsPositionInsideGrid(int2 gridPosition, int2 gridSize)
        {
            return gridPosition.x >= 0 && gridPosition.y >= 0 && gridPosition.x < gridSize.x &&
                   gridPosition.y < gridSize.y;
        }

        /// <summary>
        /// 获取两点之间的预估消耗
        /// </summary>
        /// <param name="posA"></param>
        /// <param name="posB"></param>
        /// <returns></returns>
        private static int GetHCost(int2 posA, int2 posB)
        {
            return math.abs(posA.x - posB.x) + math.abs(posA.y - posB.y);
        }

        /// <summary>
        /// 获取节点id
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="gridWidth"></param>
        /// <returns></returns>
        private static int GetNodeId(int x, int y, int gridWidth)
        {
            return x + y * gridWidth;
        }
    }
}