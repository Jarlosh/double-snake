using System;
using UnityEngine;
using Zenject;

namespace DoubleSnake
{
    public class TickController: ITickable
    {
        private Settings settings;
        private float lastTickTime;
        private bool isPlaying = false;

        private float tickDeltaTime => Time.time - lastTickTime;
        
        public float TimeSinceStart { get; set; }

        public bool IsPlaying
        {
            get => isPlaying;
            set
            {
                isPlaying = value;
                if (isPlaying)
                {
                    lastTickTime = Time.time;
                }
                else
                {
                    TimeSinceStart += tickDeltaTime;
                }
            }
        }

        public event Action OnTickEvent;

        [Inject]
        private void Construct(Settings mySettings)
        {
            settings = mySettings;
        }
        

        public void Tick()
        {
            if (!IsPlaying)
            {
                return;
            }
            
            var delay = 1f / settings.TicksPerSecond;
            var ticksCount = tickDeltaTime / delay;

            while (ticksCount > 0)
            {
                lastTickTime += delay;
                TimeSinceStart += delay;
                ticksCount--;
                OnTickEvent?.Invoke();
            }
        }

        [Serializable]
        public class Settings
        {
            [field: SerializeField] public float TicksPerSecond { get; private set; } = 2;
        }
    }
}