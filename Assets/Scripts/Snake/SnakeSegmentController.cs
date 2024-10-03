using System;
using System.Collections.Generic;
using DoubleSnake.Core;
using UnityEngine;
using Zenject;

namespace DoubleSnake.Snake
{
    public class SnakeSegmentController
    {
        private readonly LinkedList<SnakeSegmentView> segments = new();
        private SnakeSegmentView.Factory segmentPool;
        private MapGrid grid;
        private SnakeSettings snakeSettings;

        public int Count => segments.Count;
        
        public Vector2Int HeadPosition
        {
            get => Count > 0
                ? segments.First.Value.Position
                : throw new NullReferenceException("There are no segments");
        }

        [Inject]
        private void Construct(MapGrid mapGrid, SnakeSegmentView.Factory pool, SnakeSettings mySnakeSettings)
        {
            grid = mapGrid;
            segmentPool = pool;
            snakeSettings = mySnakeSettings;
        }
        
        /// <param name="positions">From head to tail</param>
        /// <param name="startDirection">Initial move direction</param>
        public void Init(IList<Vector2Int> positions)
        {
            for (int i = positions.Count - 1; i >= 0; i--)
            {
                AddHead(positions[i]);
            }
        }
        
        public void CollectPositionsNoAlloc(List<Vector2Int> results)
        {
            foreach (var segment in segments)
            {
                results.Add(segment.Position);
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
        }

        private void AddHead(Vector2Int headTarget)
        {
            var head = segmentPool.Create(snakeSettings.SegmentColor);
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
    }
}