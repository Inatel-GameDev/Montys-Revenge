using System.Collections;
using UnityEngine;

public enum EstadosMonty{
    Buscando,
    Seguindo,
    Atacando
}

/*
 Buscando
    Entrada: @ Start, 
             @ Não ter um alvo para seguir ou bater & nao ter um player disponivel, 
             Jogador que esta seguindo foge para dentro 
     Saída: Receber um alvo válido para seguir ou bater  
    @ Comportamento: aviso para o controller

Seguindo
    Entrada: Estar buscando & ter um player disponivel 
    Saída: Jogador alvo se esconde 
           Chegou no alvo 
    Comportamento: 

Atacando
    Entrada: Estar do lado de um buraco e ter um jogador válido 
    Saída: Fim da animação 
    Comportamento: Verifica se jogador esta ali, segura o jogador, realiza animação 

 */

public class Monty : MonoBehaviour
{
    public EstadosMonty estadoAtual;
    public Transform buracoAlvo;
    public Buraco buracoAtual;
    public bool batendo;

    void Start()
    {
        batendo = false;
        // Condição de entrada
        estadoAtual = EstadosMonty.Buscando;
        // Comportamento
        MontyController.instance.MontyDisponivel(this);
    }
    
    void Update()
    {
        if(batendo)
            return;
        // Condição de entrada
        if (estadoAtual != EstadosMonty.Buscando)
        {
            if (buracoAlvo == null && buracoAtual == null)
            {
                estadoAtual = EstadosMonty.Buscando;
            }
        }

        // Comportamento
        if(estadoAtual == EstadosMonty.Seguindo && buracoAlvo != null)
        {
            move();
        }

        if (estadoAtual == EstadosMonty.Atacando)
        {
            if (buracoAtual.temPlayer)
            {
                batendo = true;
                StartCoroutine(bater());
            }
            else
            {
                estadoAtual = EstadosMonty.Buscando;
                MontyController.instance.MontyDisponivel(this);
            }
            
        }
        
    }

    void move()
    {
        transform.position = Vector3.MoveTowards(
            transform.position
            , buracoAlvo.position
            , MontyController.instance.speed * Time.deltaTime);
    }
    
    // Condição entrada 
    public void recebeAlvo(Transform buraco)
    {
        buracoAlvo = buraco;
        estadoAtual = EstadosMonty.Seguindo;
    }

    IEnumerator bater()
    {
        Debug.Log("bate");
        yield return new WaitForSeconds(1.5f);
        buracoAtual = null;
        estadoAtual = EstadosMonty.Buscando;
        MontyController.instance.MontyDisponivel(this);
        batendo = false;
    }
    
    
    private void OnTriggerStay(Collider obj){
        // Condição de entrada
        if (obj.CompareTag("Buraco") && estadoAtual != EstadosMonty.Atacando)
        {
            buracoAlvo = null;
            buracoAtual = obj.gameObject.GetComponent<Buraco>();
            estadoAtual = EstadosMonty.Atacando;
        }
    }
    
}

