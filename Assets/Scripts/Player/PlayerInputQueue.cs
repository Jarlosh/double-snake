using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DoubleSnake
{
    // todo: implement mobile inputs 
    public class PlayerInputQueue: ITickable
    {
        private readonly Queue<Vector2Int> queue = new();
        private Vector2Int LastDirection { get; set; } = Vector2Int.up;

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Enqueue(Vector2Int.up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Enqueue(Vector2Int.down);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Enqueue(Vector2Int.left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Enqueue(Vector2Int.right);
            }
        }

        public void Enqueue(Vector2Int direction)
        {
            if (LastDirection == -direction)
            {
                return;
            }
            queue.Enqueue(direction);
            LastDirection = direction;
        }

        public Vector2Int PeekDirection()
        {
            return queue.TryPeek(out var direction) 
                ? direction 
                : LastDirection;
        }
        
        public Vector2Int DequeueDirection()
        {
            return queue.TryDequeue(out var direction) 
                ? direction 
                : LastDirection;
        }
    }
}