using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public enum EstadosMonty{
    Buscando,
    Seguindo,
    Atacando,
    Espera
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
    public SelectorController selector;

    void Start()
    {
        batendo = false;
        estadoAtual = EstadosMonty.Espera;
    }

    public void Comeca()
    {
        // Condição de entrada
        estadoAtual = EstadosMonty.Buscando;
        // Comportamento
        MontyController.instance.MontyDisponivel(this);
    }
    
    void Update()
    {
        if(estadoAtual == EstadosMonty.Espera)
            return;
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
        // 0.3 altura para nao entrar no chao 
        Vector3 pos = new Vector3(buracoAlvo.transform.position.x, 0.3f, buracoAlvo.transform.position.z);
        
        Quaternion targetRotation = Quaternion.LookRotation(pos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, MontyController.instance.velocidadeDeGiro * Time.deltaTime);
        //transform.rotation = targetRotation;
        
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
        selector.player.isHit = true;
        Debug.Log("bate");
        selector.StunaPlayer();
        yield return new WaitForSeconds(MontyController.instance.tempoDeBatidas);
        selector.player.isHit = false;
        buracoAtual.temMonty = false;
        buracoAtual.temPlayer = false;
        buracoAtual = null;
        estadoAtual = EstadosMonty.Buscando;
        MontyController.instance.MontyDisponivel(this);
        batendo = false;
    }

    public void verificaBater(Buraco bu)
    {
        if (estadoAtual != EstadosMonty.Atacando && bu == buracoAlvo)
        {
            Debug.Log("b");
            buracoAlvo = null;
            buracoAtual = bu;
            estadoAtual = EstadosMonty.Atacando;
        }
    }
    
}

