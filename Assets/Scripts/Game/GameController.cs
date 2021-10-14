// ******************************************************************
//       /\ /|       @file       GameController.cs
//       \ V/        @brief      游戏总控制器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 07:06:56
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using Core;

namespace RabiStar.ECS
{
    public class GameController : BaseMonoSingleton<GameController>
    {
        public int mapWidth;
        public int mapHeight;

        private void Start()
        {
            GridController.Instance.UpdateGrid(mapWidth, mapHeight);
        }
    }
}