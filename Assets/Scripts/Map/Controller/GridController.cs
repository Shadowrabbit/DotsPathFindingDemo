// ******************************************************************
//       /\ /|       @file       GridController.cs
//       \ V/        @brief      网格控制器
//       | "")       @author     Shadowrabbit, yingtu0401@gmail.com
//       /  |                    
//      /  \\        @Modified   2021-10-13 03:26:01
//    *(__\_\        @Copyright  Copyright (c) 2021, Shadowrabbit
// ******************************************************************

using System;
using Core;
using UnityEngine;

namespace RabiStar.ECS
{
    public class GridController : BaseMonoSingleton<GridController>
    {
        private Grid _gridModel;
        private GridRender _gridView;
        private bool _needUpdateMesh;
        public event Action<int, int> OnGridNodeChanged;
        public int Width => _gridModel.Width;
        public int Height => _gridModel.Height;

        protected void Awake()
        {
            var mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            _gridView = new GridRender(mesh);
        }

        protected void OnEnable()
        {
            //事件监听
            InputController.Instance.OnRightMouseButtonDown += OnRightMouseButtonDown;
        }

        protected void OnDisable()
        {
            InputController.Instance.OnRightMouseButtonDown -= OnRightMouseButtonDown;
        }

        protected void LateUpdate()
        {
            if (!_needUpdateMesh)
            {
                return;
            }

            _needUpdateMesh = false;
            _gridView.RefreshMesh(_gridModel);
        }

        /// <summary>
        /// 更新网格地图
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void UpdateGrid(int width, int height)
        {
            _gridModel = new Grid(width, height, 1, Vector3.zero);
            _gridView.RefreshMesh(_gridModel);
        }

        /// <summary>
        /// 更变某个节点的可行性
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void ChangeNodeWalkable(int x, int y)
        {
            //数据更变
            var isWalkable = _gridModel.IsGridNodeWalkable(x, y);
            _gridModel.SetGridNodeWalkable(x, y, !isWalkable);
            //可行性更变
            OnGridNodeChanged?.Invoke(x, y);
            _needUpdateMesh = true;
        }

        /// <summary>
        /// 鼠标右键点击回调
        /// </summary>
        /// <param name="worldPos"></param>
        private void OnRightMouseButtonDown(Vector3 worldPos)
        {
            worldPos.z = 0;
            var gridNode = _gridModel.GetGridNode(worldPos);
            if (gridNode == null)
            {
                Debug.LogError($"世界坐标{worldPos.ToString()}处不存在grid node");
                return;
            }

            ChangeNodeWalkable(gridNode.x, gridNode.y);
        }
    }
}