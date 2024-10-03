using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace DoubleSnake.Core
{
    public class PauseController: ITickable
    {
        [Inject] private IGameMode gameMode;
        
        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameMode.StartGame();
            }
            
            // todo: implement without scene reload
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}