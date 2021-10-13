// ******************************************************************
//       /\ /|       @file       UnitSpawnerComponentData.cs
//       \ V/        @brief      
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 06:31:25
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Unity.Entities;

namespace RabiStar.ECS
{
    [GenerateAuthoringComponent]
    public struct UnitSpawnerComponentData : IComponentData
    {
        public Entity prefabToSpawn; //待生成的预制体
        public int spawnCount; //每次生成的数量
    }
}