using UnityEngine;

public class Sounds : MonoBehaviour
{
    public static Sounds instance;
    [Header("Ambiente")]
    public AudioClip musica;
    public AudioClip start;
    public AudioClip finish;
    public AudioClip winner;
    public AudioClip vitoria;
    
    [Header("Monty")]
    public AudioClip porrada;
    public AudioClip andar;

    [Header("Monty")]
    public AudioClip sair;
    public AudioClip entrar;
    public AudioClip atingido;


    void Awake()
    {
        instance = this;
    }
}
