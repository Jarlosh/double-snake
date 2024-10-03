using System.Collections.Generic;
using DoubleSnake.Snake;
using UnityEngine;
using Zenject;

namespace DoubleSnake
{
    public interface ISnake
    {
        void PrepareTurn();
        void Init(Vector2Int startDirection, IList<Vector2Int> positions);
    }
    
    public class PlayerFacade: ISnake
    {
        private SnakeSegmentController SnakeSegments { get; set; }
        private PlayerInputQueue inputs;

        public Vector2Int StepDirection { get; private set; }
        public Vector2Int HeadPosition => SnakeSegments.HeadPosition;

        public void Init(Vector2Int startDirection, IList<Vector2Int> positions)
        {
            SnakeSegments.Init(positions);
            StepDirection = startDirection;
            inputs.Enqueue(startDirection);
        }
        
        public void PrepareTurn()
        {
            StepDirection = inputs.DequeueDirection();
        }
        
        [Inject]
        private void Construct(SnakeSegmentController segments, PlayerInputQueue myInputs)
        {
            SnakeSegments = segments;
            inputs = myInputs;
        }

        public void MoveTo(Vector2Int targetPosition)
        {
            SnakeSegments.MoveTo(targetPosition, false);
        }
    }
}