using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MontyController : MonoBehaviour
{
    public static MontyController instance; 
    [SerializeField] private Queue<Monty> montys = new Queue<Monty>();
    [SerializeField] private Monty[] montysArray;
    [SerializeField] private Buraco[] posicaoBuracos;
    [Header("Ajustes para Dificuldade")] 
    public float speed = 1f;
    public float tempoDeBatidas = 1.2f;
    public float velocidadeDeGiro = 5f;
    
     private void Awake()
     {
         instance = this;
     }
     
    void Update()
    {
        verificaJogadores();
    }

    void verificaJogadores()
    {
        foreach (var buraco in posicaoBuracos)
        {
            if (!buraco.temPlayer || buraco.temMonty) continue;
            if (montys.Any())
            {
                buraco.temMonty = true;
                ordenaMonty(montys.Dequeue(), buraco);
                break;
            }
        }
    }

    public void ordenaMonty(Monty monty, Buraco buraco)
    {
        monty.recebeAlvo(buraco);
    }

    public void MontyDisponivel(Monty monty)
    {
        montys.Enqueue(monty);    
    }

    public void PausaMontys()
    {
        foreach (Monty monty in montysArray)
        {
            monty.estadoAtual = EstadosMonty.Espera;
        }
    }

    public void StartMontys()
    {
        foreach (Monty monty in montysArray)
        {
            monty.Comeca();
            
        }
    }
    
}
