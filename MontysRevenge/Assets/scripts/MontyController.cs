using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MontyController : MonoBehaviour
{
    public static MontyController instance; 
    [SerializeField] private Queue<Monty> montys;
    [SerializeField] private Buraco[] posicaoBuracos;
     public float speed = 5.0f;
     
    
    void Update()
    {
        verificaJogadores();
    }

    void verificaJogadores()
    {
        for (int i = 0; i < posicaoBuracos.Length ; i++) {
            if (posicaoBuracos[i].temPlayer) {
                ordenaMonty(montys.Dequeue(), posicaoBuracos[i].transform);
            }
        }
    }

    public void ordenaMonty(Monty monty, Transform buraco)
    {
        monty.buracoAlvo = buraco;
        monty.estadoAtual = EstadosMonty.Seguindo;
    }

    public void MontyDisponivel(Monty monty)
    {
        montys.Enqueue(monty);    
    }

    
    
    
}
