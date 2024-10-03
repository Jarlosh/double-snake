using UnityEngine;
using Zenject;

namespace DoubleSnake.Snake
{
    public class SnakeSegmentView: MonoBehaviour, IPoolable<Color, IMemoryPool>
    {
        private IMemoryPool _pool;
        
        [SerializeField] private SpriteRenderer spriteRenderer;

        public Vector2Int Position { get; set; }

        public void Despawn()
        {
            _pool.Despawn(this);
        }
        
        public void OnDespawned()
        {
            _pool = null;
        }

        public void OnSpawned(Color color, IMemoryPool pool)
        {
            _pool = pool;
            spriteRenderer.color = color;
        }
        
        public class Factory: PlaceholderFactory<Color, SnakeSegmentView>
        {
        }
        
        public class Pool : MonoPoolableMemoryPool<Color, IMemoryPool, SnakeSegmentView>
        {
        }
    }
}