using System;
using UnityEngine;

public class Monty : MonoBehaviour
{
    public EstadosMonty estadoAtual = EstadosMonty.Buscando;
    public Transform buracoAlvo;
    public Buraco buracoAtual;

    void Start()
    {
        MontyController.instance.MontyDisponivel(this);
    }
    
    void Update()
    {
        if(estadoAtual == EstadosMonty.Seguindo && buracoAlvo != null)
        {
            move();
        }

        if (estadoAtual == EstadosMonty.NoBuraco)
        {
            if (buracoAtual.temPlayer)
            {
                bater();    
            }
            else
            {
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
    public void recebeAlvo(Transform buraco){
        buracoAlvo = buraco;
        estadoAtual = EstadosMonty.Seguindo;
    }

    public void bater()
    {
        Debug.Log("bate");
        new WaitForSeconds(1.5f);
    }
    
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("Buraco"))
        {
            Debug.Log("trigger");
            buracoAtual = obj.gameObject.GetComponent<Buraco>();
            estadoAtual = EstadosMonty.NoBuraco;
        }
    }
    
}

public enum EstadosMonty{
    Buscando,
    Seguindo,
    NoBuraco
}