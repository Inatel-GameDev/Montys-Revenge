using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public enum EstadosMonty{
    Buscando,
    Seguindo,
    Atacando
}

/*
 Estados
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
    public Buraco buracoAlvo;
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
        if (batendo)
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
        if (estadoAtual == EstadosMonty.Seguindo && buracoAlvo != null )
        {
            if (!buracoAlvo.temPlayer)
            {
                buracoAlvo = null;
                estadoAtual = EstadosMonty.Buscando;
                MontyController.instance.MontyDisponivel(this);
            }
            else
            {
                move();
            }
        }

        if (estadoAtual == EstadosMonty.Atacando)
        {
            if (buracoAtual.temPlayer)
            {
                batendo = true;
                StartCoroutine(bater());
                // travar player
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
        Vector3 pos = new Vector3(buracoAlvo.transform.position.x, 0, buracoAlvo.transform.position.z);
        transform.position = Vector3.MoveTowards(
            transform.position,
            pos
            , MontyController.instance.speed * Time.deltaTime);
    }

    // Condição entrada 
    public void recebeAlvo(Buraco buraco)
    {
        buracoAlvo = buraco;
        estadoAtual = EstadosMonty.Seguindo;
    }

    IEnumerator bater()
    {
        Debug.Log("bate");
        yield return new WaitForSeconds(1.5f);
        buracoAtual.temMonty = false;
        buracoAtual.temPlayer = false;
        buracoAtual = null;
        estadoAtual = EstadosMonty.Buscando;
        MontyController.instance.MontyDisponivel(this);
        batendo = false;
    }


    private void OnCollisionStay(Collision obj)
    {
        // Condição de entrada
        if (obj.gameObject.CompareTag("Buraco")) {
            Buraco bu = obj.gameObject.GetComponent<Buraco>();
            if (estadoAtual != EstadosMonty.Atacando && bu == buracoAlvo)
            {
                buracoAlvo = null;
                buracoAtual = obj.gameObject.GetComponent<Buraco>();
                estadoAtual = EstadosMonty.Atacando;
            }
        }
    }

}

