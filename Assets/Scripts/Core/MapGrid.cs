
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DoubleSnake.Core
{
    public interface IGridEntity
    {
    }
    
    public class MapGrid
    {
        private IGridEntity[,] map;
        private Settings settings;

        public Vector2Int Size => settings.Size;

        public IGridEntity this[int x, int y]
        {
            get => map[x, y];
            set => map[x, y] = value;
        }

        public IGridEntity this[Vector2Int next]
        {
            get => this[next.x, next.y];
            set => this[next.x, next.y] = value;
        }

        public void SetPositions(IEnumerable<Vector2Int> positions, IGridEntity value)
        {
            foreach (var position in positions)
            {
                this[position] = value;
            }
        }
        
        [Inject]
        private void Construct(Settings mySettings)
        {
            settings = mySettings;
            map = new IGridEntity[settings.Size.x, settings.Size.y];
        }

        public Vector2Int Clamp(Vector2Int local)
        {
            var size = Size;
            local.x = (local.x + size.x) % size.x;
            local.y = (local.y + size.y) % size.y;
            return local;
        }
        
        public Vector3 LocalPosToWorld(Vector2Int local)
        {
            var clamped = Clamp(local);
            return new Vector3(clamped.x - Size.x / 2f, clamped.y - Size.y / 2f, 0);
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public Vector2Int Size { get; private set; }
        }
    }
}
