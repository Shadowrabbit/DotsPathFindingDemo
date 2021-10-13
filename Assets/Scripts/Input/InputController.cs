// ******************************************************************
//       /\ /|       @file       InputController.cs
//       \ V/        @brief      输入控制器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 04:49:36
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System;
using Core;
using UnityEngine;

namespace RabiStar.ECS
{
    public class InputController : BaseMonoSingleton<InputController>
    {
        public event Action<Vector3> OnLeftMouseButtonDown; //鼠标左键点击事件
        public event Action<Vector3> OnRightMouseButtonDown; //鼠标右键点击事件
        public event Action OnKeyDownSpace; //键盘按下空格事件

        protected void Update()
        {
            var mousePosition = Input.mousePosition; //鼠标位置
            if (Camera.main == null)
            {
                Debug.LogError("找不到主相机");
                return;
            }

            var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftMouseButtonDown?.Invoke(worldPosition);
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                OnRightMouseButtonDown?.Invoke(worldPosition);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnKeyDownSpace?.Invoke();
            }
        }
    }
}