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
        private bool hasInitialized;
        public event Action<int, int> OnGridNodeChanged;
        public event Action OnGridInitialized;
        public int Width => _gridModel.Width;
        public int Height => _gridModel.Height;
        public float CellSize => _gridModel.CellSize;

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
            if (InputController.Instance == null)
            {
                return;
            }

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
        /// 某个节点是否可行
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsNodeWalkable(int x, int y)
        {
            return _gridModel.IsGridNodeWalkable(x, y);
        }

        /// <summary>
        /// 获取节点id
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetNodeId(int x, int y) => _gridModel.GetNodeId(x, y);

        /// <summary>
        /// 初始化网格地图
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void InitGrid(int width, int height)
        {
            _gridModel = new Grid(width, height, 1, Vector3.zero);
            _gridView.RefreshMesh(_gridModel);
            OnGridInitialized?.Invoke();
            hasInitialized = true;
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
            //todo 可行性更变
            OnGridNodeChanged?.Invoke(x, y);
            _needUpdateMesh = true;
        }

        /// <summary>
        /// 鼠标右键点击回调
        /// </summary>
        /// <param name="worldPos"></param>
        private void OnRightMouseButtonDown(Vector3 worldPos)
        {
            //没有初始化完成 不接收输入命令
            if (!hasInitialized)
            {
                return;
            }

            worldPos.z = 0;
            var gridNode = _gridModel.GetGridNode(worldPos);
            if (gridNode == null)
            {
                Debug.LogError($"世界坐标{worldPos.ToString()}处不存在grid node");
                return;
            }

            //更改点击出的行走性
            ChangeNodeWalkable(gridNode.x, gridNode.y);
        }
    }
}