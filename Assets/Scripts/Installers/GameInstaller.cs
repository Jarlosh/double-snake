using System;
using DoubleSnake.Snake;
using UnityEngine;
using Zenject;

namespace DoubleSnake.Installers
{
    public class GameInstaller: MonoInstaller
    {
        [Serializable]
        public class Settings
        {
            [field: SerializeField] public GameObject segmentPrefab { get; private set; }
        }

        [SerializeField] private Settings settings;
        
        public override void InstallBindings()
        {
            Container
                .BindFactory<Color, SnakeSegmentView, SnakeSegmentView.Factory>()
                .FromPoolableMemoryPool<Color, SnakeSegmentView, SnakeSegmentView.Pool>(x => x
                    .WithInitialSize(5)
                    .FromComponentInNewPrefab(settings.segmentPrefab)
                    .UnderTransformGroup("Segments"));
        }
    }
}