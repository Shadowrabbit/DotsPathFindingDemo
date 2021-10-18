// ******************************************************************
//       /\ /|       @file       UnitSpawnerSystem.cs
//       \ V/        @brief      单位生成系统(这个系统没使用Dots)
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 11:07:45
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace RabiStar.ECS
{
    public class UnitSpawnerSystem : SystemBase
    {
        private Random _random; //随机数生成器
        private bool _needSpawnUnits; //当前帧是否需要生成单位

        protected override void OnCreate()
        {
            _random = new Random(1);
        }

        protected override void OnStartRunning()
        {
            InputController.Instance.OnKeyDownSpace += OnKeyDownSpace;
        }

        protected override void OnStopRunning()
        {
            if (InputController.Instance == null)
            {
                return;
            }

            InputController.Instance.OnKeyDownSpace -= OnKeyDownSpace;
        }

        protected override void OnUpdate()
        {
            if (!_needSpawnUnits)
            {
                return;
            }

            SpawnUnits();
            _needSpawnUnits = false;
        }

        /// <summary>
        /// 生成单位
        /// </summary>
        private void SpawnUnits()
        {
            var unitSpawnerComponentData = GetSingleton<UnitSpawnerComponentData>();
            for (var i = 0; i < unitSpawnerComponentData.spawnCount; i++)
            {
                var x = _random.NextInt(0, GridController.Instance.Width);
                var y = _random.NextInt(0, GridController.Instance.Height);
                var gridNode = GridController.Instance[x, y];
                var entityToSpawn = EntityManager.Instantiate(unitSpawnerComponentData.prefabToSpawn);
                EntityManager.SetComponentData(entityToSpawn,
                    new Translation {Value = new float3(gridNode.centerX, gridNode.centerY, 0)});
            }
        }

        /// <summary>
        /// 空格键按下回调
        /// </summary>
        private void OnKeyDownSpace()
        {
            _needSpawnUnits = true;
        }
    }
}