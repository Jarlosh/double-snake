using System;
using UnityEngine;

namespace DoubleSnake.Snake
{
    [Serializable]
    public class SnakeSettings
    {
        [field: SerializeField] public Color SegmentColor { get; private set; }
    }
}