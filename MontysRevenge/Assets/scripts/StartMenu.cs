using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Button newGame;
    public Button quitGame;
    

    private void Start()
    {
        newGame.onClick.AddListener(StartNewGame);    
        quitGame.onClick.AddListener(Quit);    
    }


    private static void StartNewGame()
    {
        SceneManager.LoadScene("Juntando");
    }

    private static void Quit()
    {
        Application.Quit();
    }
    
    
}
