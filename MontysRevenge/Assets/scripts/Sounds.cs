using UnityEngine;

public class Sounds : MonoBehaviour
{
    public static Sounds instance;

    [Header("Ambiente")]
    public AudioClip musicaJogo;
    public AudioClip musicaSelecao;
    public AudioClip musicaVitoria;
    public AudioClip start;
    public AudioClip finish;
    public AudioClip winner;
    public AudioClip vitoria;
    
    [Header("Monty")]
    public AudioClip porrada;
    public AudioClip andar;

    [Header("Jogadores")]
    public AudioClip mario;
    public AudioClip luigi;
    public AudioClip wario;
    public AudioClip waluigi;

    void Awake()
    {
        instance = this;
    }
}
