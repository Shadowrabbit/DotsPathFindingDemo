// ******************************************************************
//       /\ /|       @file       BaseMonoSingleton.cs
//       \ V/        @brief      Mono单例基类
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 05:03:58
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using UnityEngine;

namespace Core
{
    public class BaseMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                //实例不存在
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }

                return _instance;
            }
        }
    }
}