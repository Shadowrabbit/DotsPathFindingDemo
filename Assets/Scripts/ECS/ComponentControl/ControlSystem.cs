// ******************************************************************
//       /\ /|       @file       ControlSystem.cs
//       \ V/        @brief      组件控制系统
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-14 09:35:50
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using RabiStar.ECS;
using Unity.Entities;

namespace ECS.ComponentControl
{
    public class ControlSystem : SystemBase
    {
        protected override void OnCreate()
        {
            //禁用其他系统 等待地图加载
            SetSystemEnabled(false);
            GridController.Instance.OnGridInitialized += OnGridInitialized;
        }

        protected override void OnDestroy()
        {
            if (GridController.Instance == null)
            {
                return;
            }

            GridController.Instance.OnGridInitialized -= OnGridInitialized;
        }

        protected override void OnUpdate()
        {
        }

        /// <summary>
        /// 地图初始化完毕
        /// </summary>
        private void OnGridInitialized()
        {
            SetSystemEnabled(true);
        }

        /// <summary>
        /// 设置其他系统启用性
        /// </summary>
        /// <param name="enabled"></param>
        private void SetSystemEnabled(bool enabled)
        {
            World.GetOrCreateSystem<MoveSystem>().Enabled = enabled;
            World.GetOrCreateSystem<PathFindingSystem>().Enabled = enabled;
            World.GetOrCreateSystem<PathFollowSystem>().Enabled = enabled;
            //.GetOrCreateSystem<RandomWalkSystem>().Enabled = enabled;
            World.GetOrCreateSystem<UnitSpawnerSystem>().Enabled = enabled;
        }
    }
}