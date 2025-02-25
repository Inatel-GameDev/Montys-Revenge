using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{

    public Button newGame;
    public Button quitGame;
    public Button menu;
    
    private void Start()
    {
        newGame.onClick.AddListener(StartNewGame);    
        quitGame.onClick.AddListener(Quit);
        menu.onClick.AddListener(Menu);
    }

    private static void Menu()
    {
        SceneManager.LoadScene("MenuStart");
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


