using DoubleSnake.Core;
using DoubleSnake.Snake;
using UnityEngine;
using Zenject;

namespace DoubleSnake.Installers
{
    public class DoubleSnakeGameModeInstaller: MonoInstaller
    {
        [SerializeField] private DoubleSnakeGameMode.Settings settings;

        public override void InstallBindings()
        {
            Container
                .Bind<MapGrid>()
                .AsSingle();

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