using System.Collections.Generic;
using DoubleSnake.Core;
using DoubleSnake.Snake;
using UnityEngine;
using Zenject;

namespace DoubleSnake
{
    public interface ISnake: IGridEntity
    {
        bool IsAlive { get; }
        void PrepareTurn();
        void Move();
        void Die();
        void Init(Vector2Int startDirection, IList<Vector2Int> positions);
    }
    
    // todo: extract turn choose logic to new class, this one would work better if we leave here only snake facade 
    public abstract class SnakeBase: ISnake
    {
        protected SnakeSegmentController SnakeSegments { get; set; }
        protected MapGrid grid;

        public bool IsAlive { get; private set; }
        public bool IsExtended { get; set; }

        public abstract Vector2Int NextPosition { get; }
        
        [Inject]
        private void Construct(SnakeSegmentController segments, MapGrid myGrid)
        {
            SnakeSegments = segments;
            grid = myGrid;
        }
        
        public virtual void Init(Vector2Int startDirection, IList<Vector2Int> positions)
        {
            IsAlive = true;
            IsExtended = false;
            SnakeSegments.Init(positions);
            grid.SetPositions(SnakeSegments.GetUsedPositions(), this);
        }

        public virtual void Die()
        {
            IsAlive = false;
        }

        public void Move()
        {
            grid.SetPositions(SnakeSegments.GetUsedPositions(), null);
            SnakeSegments.MoveTo(NextPosition, IsExtended);
            grid.SetPositions(SnakeSegments.GetUsedPositions(), this);
            IsExtended = false;
        }

        public abstract void PrepareTurn();
    }
}