using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class MontyController : MonoBehaviour
{
    public static MontyController instance; 
    [SerializeField] private Queue<Monty> montys = new Queue<Monty>();
    [SerializeField] private Buraco[] posicaoBuracos;
     public float speed = 5.0f;
     
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
                Debug.Log("ordena");
                buraco.temMonty = true;
                ordenaMonty(montys.Dequeue(), buraco.transform);
                break;
            }
        }
    }

    public void ordenaMonty(Monty monty, Transform buraco)
    {
        monty.recebeAlvo(buraco);
    }

    public void MontyDisponivel(Monty monty)
    {
        montys.Enqueue(monty);    
    }
    
}
