using UnityEngine;
using Zenject;

namespace DoubleSnake.Core
{
    public class MapCameraFitter: MonoBehaviour
    {
        [Inject] private MapGrid grid;

#if UNITY_EDITOR
        private void Update() => FitMap();
#else 
        private void Awake() => FitMap();
#endif
        private void Start()
        {
            var gridSize = grid.Size;
            transform.localScale = new Vector3(gridSize.x, gridSize.y, 1);
        }
        
        // todo: safe area
        private void FitMap()
        {
            var cam = Camera.main;
            var gridSize = grid.Size;
            float width = gridSize.x;
            float height = gridSize.y;
            cam!.orthographicSize = 0.5f * ((width > height * cam.aspect)
                ? width / cam.pixelWidth * cam.pixelHeight
                : height);
        }
    }
}