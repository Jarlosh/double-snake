using System;
using System.Collections.Generic;
using DoubleSnake.Snake;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DoubleSnake.Core
{
    public class DoubleSnakeGameMode: IGameMode, IInitializable, IDisposable
    {
        private TickController tickController;
        private PlayerFacade player;
        private MapGrid grid;
        private Settings settings;

        [Inject]
        private void Construct(
            Settings mySettings, 
            TickController myTickController, 
            PlayerFacade myPlayerFacade,
            MapGrid myMapGrid)
        {
            settings = mySettings;
            tickController = myTickController;
            player = myPlayerFacade;
            grid = myMapGrid;
        }
        
        public void Initialize()
        {
            tickController.OnTickEvent += OnTick;
            InitGame();
        }

        public void Dispose()
        {
            tickController.OnTickEvent -= OnTick;
        }

        private void InitGame()
        {
            // todo: move to settings
            const int distance = 3; 
            
            var headDirection = RandomHelpers.CoinFlip()  
                ? RandomHelpers.CoinFlip() ? Vector2Int.left : Vector2Int.right
                : RandomHelpers.CoinFlip() ? Vector2Int.up : Vector2Int.down;
            var offsetSign = RandomHelpers.CoinFlip() ? +1 : -1;
            
            InitSnakeSegments(player, headDirection, distance, offsetSign, settings.StartLength);
        }

        private void InitSnakeSegments(
            ISnake snake, 
            Vector2Int direction, 
            int distance, 
            int offsetSign, 
            int lenght)
        {
            var ortogonal = direction.x == 0
                ? Vector2Int.up
                : Vector2Int.right;
            var offset = distance * offsetSign * ortogonal;
            
            using (UnityEngine.Pool.ListPool<Vector2Int>.Get(out var positions))
            {
                positions.Clear();
                GenerateSnakePositions(Vector2Int.zero + offset, direction, lenght, positions);
                snake.Init(direction, positions);
            }
        }

        private void GenerateSnakePositions(
            Vector2Int headPosition, 
            Vector2Int headDirection, 
            int length, 
            ICollection<Vector2Int> results)
        {
            for (int i = 0; i < length; i++)
            {
                results.Add(grid.Clamp(headPosition - headDirection * i));
            }
        }

        public void OnPlayPressed()
        {
            if (player.IsAlive)
            {
                tickController.IsPlaying = !tickController.IsPlaying;
            }
            else
            {
                Reset();
            }
        }

        private void Reset()
        {
            // debug temp 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        private void OnTick()
        {
            PrepareTurn();
            MoveSnakes();

            void PrepareTurn()
            {
                player.PrepareTurn();
            }

            void MoveSnakes()
            {
                var next = player.NextPosition;
                switch (grid[next])
                {
                    case ISnake snake:
                        player.Die();
                        tickController.IsPlaying = false;
                        break; 
                }
                if(player.IsAlive)
                {
                    player.Move();
                }
            }
        }
        
        [Serializable]
        public class Settings
        {
            [field: SerializeField] public SnakeSettings PlayerSnakeSettings { get; private set; }
            [field: SerializeField] public TickController.Settings TickSettings { get; private set; }
            [field: SerializeField] public int StartLength { get; private set; }
        }
    }
}