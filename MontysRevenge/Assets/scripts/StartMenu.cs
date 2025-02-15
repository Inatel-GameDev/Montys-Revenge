using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Button newGame;
    public Button quitGame;
    public Button howToPlay;
    public Button voltar;
    public Canvas startMenu;
    public Canvas tutorial;

    private void Start()
    {
        newGame.onClick.AddListener(StartNewGame);    
        quitGame.onClick.AddListener(Quit);
        howToPlay.onClick.AddListener(HowToPlay);
        voltar.onClick.AddListener(Voltar);
    }


    private static void StartNewGame()
    {
        SceneManager.LoadScene("Juntando");
    }

    private static void Quit()
    {
        Application.Quit();
    }
    
    private void HowToPlay()
    {
        tutorial.gameObject.SetActive(true);
        startMenu.gameObject.SetActive(false);
    }

    private void Voltar()
    {
        startMenu.gameObject.SetActive(true);
        tutorial.gameObject.SetActive(false);
    }
    
}
