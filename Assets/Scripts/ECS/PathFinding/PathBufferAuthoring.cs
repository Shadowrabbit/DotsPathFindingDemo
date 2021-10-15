// ******************************************************************
//       /\ /|       @file       PathBufferAuthoring.cs
//       \ V/        @brief      路径缓冲生成器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-14 10:49:27
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Unity.Entities;
using UnityEngine;

namespace RabiStar.ECS
{
    public class PathBufferAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        /// <summary>
        /// obj转换为entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dstManager"></param>
        /// <param name="conversionSystem"></param>
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            //转换时添加个pathBuffer
            dstManager.AddBuffer<PathBufferData>(entity);
        }
    }
}