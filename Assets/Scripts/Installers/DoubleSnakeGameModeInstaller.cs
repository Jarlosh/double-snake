using DoubleSnake.Core;
using DoubleSnake.Snake;
using UnityEngine;
using Zenject;

namespace DoubleSnake.Installers
{
    public class DoubleSnakeGameModeInstaller: MonoInstaller
    {
        [SerializeField] private DoubleSnakeGameMode.Settings settings;
        [SerializeField] private MapGrid.Settings gridSettings;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<MapGrid>()
                .AsSingle()
                .WithArguments(gridSettings);

            Container
                .BindInstance(settings);
            
            Container
                .BindInterfacesAndSelfTo<TickController>()
                .AsSingle()
                .WithArguments(settings.TickSettings);

            Container
                .BindInterfacesTo<PauseController>()
                .AsSingle()
                .NonLazy(); 
            
            Container
                .BindInterfacesTo<DoubleSnakeGameMode>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<PlayerFacade>()
                .FromSubContainerResolve()
                .ByMethod(InstallPlayer)
                .WithKernel()
                .AsSingle();
        }

        private void InstallPlayer(DiContainer subcontainer)
        {
            subcontainer
                .Bind<PlayerFacade>()
                .AsSingle();
            
            subcontainer
                .Bind<SnakeSegmentController>()
                .AsSingle()
                .WithArguments(settings.PlayerSnakeSettings);

            subcontainer
                .BindInterfacesAndSelfTo<PlayerInputQueue>()
                .AsSingle();
        }
    }
}