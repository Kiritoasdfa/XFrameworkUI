using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework
{
	[UIEvent(UIType.UIHRD)]
    internal sealed class UIHRDEvent : AUIEvent, IUILayer
	{
	    public override string Key => UIPathSet.UIHRD;

        public override bool IsFromPool => true;

		public override bool AllowManagement => true;

		public UILayer Layer => UILayer.Mid;

        public override UI OnCreate()
        {
            return UI.Create<UIHRD>();
        }
    }

    public partial class UIHRD : UI, IAwake
	{
		public class Grid
		{
			public static float Width;

			public static float Height;

			public Vector2Int Position { get; private set; }

			public Vector2 WorldPosition { get; private set; }

			public int Id { get; private set; }

			public Grid(Vector2 leftLower, int x, int y)
			{
				this.SetPosition(leftLower, new Vector2Int(x, y));
			}

			private void SetPosition(Vector2 leftLower, Vector2Int position)
			{
				this.Position = position;
				var x = leftLower.x + position.x * Width;
				var y = leftLower.y + position.y * Height;
				this.WorldPosition = new Vector2(x, y);
			}

			public void SetId(int id)
            {
				this.Id = id;
            }
		}

		public const int Size = 120;

		public const int Row = 5;

		public const int Col = 4;

		public Vector2 leftLower = Vector2.zero;

		public Vector2 leftUpper = Vector2.zero;

		public Vector2 rightUpper = Vector2.zero;

		public Vector2 rightLower = Vector2.zero;

        private Grid[,] grids = new Grid[Row, Col];

		private UnOrderMapList<int, Grid> gridsDict = new UnOrderMapList<int, Grid>();

		private int gridIndex = -1;

        public void Initialize()
		{
			this.GetButton(KClose)?.AddClickListener(this.Close);

			this.InitGrid();
			this.InitView();
		}

		private void InitGrid()
        {
			Vector3[] corners = new Vector3[4];
			var transform = this.GetRectTransform(KBg);
			transform.GetWorldCorners(corners);
			Vector2 leftLower = corners[0];
			Vector2 leftUpper = corners[1];
			Vector2 rightUpper = corners[2];
			Vector2 rightLower = corners[3];

			float width = Mathf.Abs(leftLower.x - rightUpper.x) / Col;
			float height = Mathf.Abs(leftLower.y - leftUpper.y) / Row;
			Grid.Width = width;
			Grid.Height = height;
			this.leftLower = leftLower;
			this.leftUpper = leftUpper;
			this.rightUpper = rightUpper;
			this.rightLower = rightLower;

            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
					var grid = new Grid(leftLower, j, i);
					this.grids[i, j] = grid;
                }
            }
		}

		private void InitView()
        {
			var configs = HRDConfigManager.Instance.GetAllValues();
			var list = this.GetList(KBg);
            foreach (var config in configs)
            {
				int id = config.Id;
                int row = config.X + config.W;
                int col = config.Y + config.H;
                for (int i = config.X; i < row; i++)
                {
                    for (int j = config.Y; j < col; j++)
                    {
                        var grid = this.grids[j, i];
						grid.SetId(id);
                        this.gridsDict.Add(id, grid);
                    }
                }

                list.CreateWithUIType(UIType.UIHRDItem, id);
			}
		}

		/// <summary>
		/// 坐标是否在范围内
		/// </summary>
		/// <param name="position"></param>
		/// <param name="isLeftLower"></param>
		/// <returns></returns>
		private bool Contain(Vector2 position, bool isLeftLower)
		{
			if (isLeftLower)
            {
				position.x += Grid.Width * 0.5f;
				position.y += Grid.Height * 0.5f;
            }

			if (position.x < this.leftLower.x)
				return false;

			if (position.x > this.rightLower.x)
				return false;

			if (position.y < this.leftLower.y)
				return false;

			if (position.y > this.rightUpper.y)
				return false;

			return true;
		}

		/// <summary>
		/// 将世界坐标转换为索引
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		private Vector2Int GetIndex(Vector3 position)
        {
			var distanceX = Mathf.Abs(position.x - this.leftLower.x);
			var distanceY = Mathf.Abs(position.y - this.leftLower.y);
			int row = (int)(distanceY / Grid.Height);
			int col = (int)(distanceX / Grid.Width);

			return new Vector2Int(row, col);
		}

		public void BeginMove(int configId, Vector3 position)
		{
			if (!this.Contain(position, false))
				return;

			var vector = this.GetIndex(position);
			var worldPosition = this.grids[vector.x, vector.y].WorldPosition;

			if (this.gridsDict.TryGetValue(configId, out var gridList))
            {
				this.gridIndex = gridList.FindIndex(grid => grid.WorldPosition == worldPosition);
			}
		}

		public void EndMove()
        {
			this.gridIndex = -1;
        }

		public bool TryMoveGrid(int configId, Vector3 position, out Vector3 result)
        {
			result = default;
			if (this.gridIndex < 0)
            {
				//Log.Error("格子不存在");
				return false;
            }

			if (!this.Contain(position, false))
            {
				//Log.Error("超出范围");
				return false;
            }

			if (!this.gridsDict.TryGetValue(configId, out var curGridList))
            {
				//Log.Error("没有配置的格子");
				return false;
            }

			var vector = this.GetIndex(position);
			var row = vector.x;
			var col = vector.y;
			var selectGrid = this.grids[row, col];
			var worldPosition = curGridList[gridIndex].WorldPosition;
			var newWorldPosition = selectGrid.WorldPosition;
			var direction = newWorldPosition - worldPosition;
			if (direction == Vector2.zero)
            {
				//Log.Error("相同位置不移动");
				return false;
            }

			if (selectGrid.Id > 0 && selectGrid.Id != configId)
            {
				//Log.Error($"该位置有id{selectGrid.Id}");
				return false;
            }

            for (int i = 0; i < curGridList.Count; i++)
            {
				if (i == this.gridIndex)
					continue;

				var world = curGridList[i].WorldPosition + direction;
				if (!this.Contain(world, true))
					return false;

				var index = this.GetIndex(world);
				var targetGrid = this.grids[index.x, index.y];
				if (targetGrid.Id > 0 && targetGrid.Id != configId)
                {
					//Log.Error($"该位置有id{selectGrid.Id}");
					return false;
                }
            }

            for (int i = 0; i < curGridList.Count; i++)
            {
				curGridList[i].SetId(0);
			}

			for (int i = 0; i < curGridList.Count; i++)
			{
				var world = curGridList[i].WorldPosition + direction;
				var index = this.GetIndex(world);
				var targetGrid = this.grids[index.x, index.y];

				curGridList[i] = targetGrid;
				targetGrid.SetId(configId);

				if (i == 0)
				{
					result = world;
					result.z = position.z;
				}
			}

			return true;
        }
		
		protected override void OnClose()
		{
			//this.grids.Clear();
			base.OnClose();
		}
	}
}
