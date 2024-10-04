using System.Collections.Generic;
using DoubleSnake.Core;
using DoubleSnake.Snake;
using UnityEngine;
using Zenject;

namespace DoubleSnake
{
    public class PlayerFacade: SnakeBase
    {
        private PlayerInputQueue inputs;
        private Vector2Int StepDirection { get; set; }
        public override Vector2Int NextPosition => grid.Clamp(SnakeSegments.HeadPosition + StepDirection);

        [Inject]
        private void Construct(SnakeSegmentController segments, PlayerInputQueue myInputs, MapGrid myGrid)
        {
            SnakeSegments = segments;
            inputs = myInputs;
            grid = myGrid;
        }

        public override void Init(Vector2Int startDirection, IList<Vector2Int> positions)
        {
            base.Init(startDirection, positions);
            StepDirection = startDirection;
            inputs.Enqueue(startDirection);
        }

        public override void PrepareTurn()
        {
            StepDirection = inputs.DequeueDirection();
        }
    }
}