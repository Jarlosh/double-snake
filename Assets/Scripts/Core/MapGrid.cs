
using UnityEngine;

namespace DoubleSnake.Core
{
    public class MapGrid
    {
        public Vector3 LocalPosToWorld(Vector2Int local)
        {
            return new Vector3(local.x, local.y, 0);
        }
    }   
}
