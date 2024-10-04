using System;
using System.Collections.Generic;
using System.Linq;
using DoubleSnake.Core;
using UnityEngine;
using Zenject;

namespace DoubleSnake.Snake
{
    // todo: move cell occupation logic to mind class, leave here only graphic updates   
    public class SnakeSegmentController: IGridEntity
    {
        private readonly LinkedList<SnakeSegmentView> segments = new();
        private SnakeSegmentView.Factory segmentFactory;
        private SnakeSettings snakeSettings;
        private MapGrid grid;

        public int Count => segments.Count;
        
        public Vector2Int HeadPosition
        {
            get => Count > 0
                ? segments.First.Value.Position
                : throw new NullReferenceException("There are no segments");
        }
        
        public Vector2Int TailPosition
        {
            get => Count > 0
                ? segments.Last.Value.Position
                : throw new NullReferenceException("There are no segments");
        }

        [Inject]
        private void Construct(MapGrid mapGrid, SnakeSegmentView.Factory pool, SnakeSettings mySnakeSettings)
        {
            grid = mapGrid;
            segmentFactory = pool;
            snakeSettings = mySnakeSettings;
        }
        
        public void Init(IList<Vector2Int> positions)
        {
            for (int i = positions.Count - 1; i >= 0; i--)
            {
                AddHead(positions[i]);
            }
        }
        
        public void MoveTo(Vector2Int headTarget, bool extend)
        {
            if (extend)
            {
                AddHead(headTarget);
            }
            else
            {
                // optimization heuristic: no need to despawn & spawn, just move last segment to head and update info
                PromoteTailToHead(headTarget);
            }
        }

        public void Clear()
        {
            foreach (var segment in segments)
            {
                segment.Despawn();
            }
            segments.Clear();
        }

        private void AddHead(Vector2Int headTarget)
        {
            var head = segmentFactory.Create(snakeSettings.SegmentColor);
            SetSegmentPosition(head, headTarget);
            segments.AddFirst(head);
        }

        private void PromoteTailToHead(Vector2Int headTarget)
        {
            var last = segments.Last.Value;
            SetSegmentPosition(last, headTarget);
            segments.RemoveLast();
            segments.AddFirst(last);
        }

        private void SetSegmentPosition(SnakeSegmentView segment, Vector2Int gridPosition)
        {
            segment.Position = gridPosition;
            segment.transform.position = grid.LocalPosToWorld(gridPosition);
        }

        public IReadOnlyCollection<Vector2Int> GetUsedPositions()
        {
            return segments.Select(s => s.Position).ToArray();
        }
    }
}