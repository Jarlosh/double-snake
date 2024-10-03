using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace DoubleSnake
{
    public class PlayerInputQueue: ITickable
    {
        private readonly Queue<Vector2Int> queue = new();
        private Vector2Int LastDirection { get; set; } = Vector2Int.up;

        public void Tick()
        {
            if (Input.GetKeyDown("up") && LastDirection != Vector2.down)
            {
                Enqueue(Vector2Int.up);
            }
            else if (Input.GetKeyDown("down") && LastDirection != Vector2.up)
            {
                Enqueue(Vector2Int.down);
            }
            else if (Input.GetKeyDown("left") && LastDirection != Vector2.right)
            {
                Enqueue(Vector2Int.left);
            }
            else if (Input.GetKeyDown("right") && LastDirection != Vector2.left)
            {
                Enqueue(Vector2Int.right);
            }
        }

        public void Enqueue(Vector2Int direction)
        {
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