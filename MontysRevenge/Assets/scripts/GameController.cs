using System;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    
    public TMP_Text timerText;
    public float Timer = 30;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        Timer -=  Time.fixedDeltaTime;
        timerText.text = Timer <= 0 ? "0" : ((int) Timer).ToString();
    }

    // Intro da fase

    // Fim de Jogo 

    // Pausar Jogo 
}
