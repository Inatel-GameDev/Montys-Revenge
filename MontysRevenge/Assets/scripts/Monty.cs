using UnityEngine;

public class Monty : MonoBehaviour
{
    public EstadosMonty estadoAtual = EstadosMonty.Buscando;
    public  Transform buracoAlvo;
    
    void Update()
    {
        if(estadoAtual == EstadosMonty.Seguindo)
        {
            move();
        }
        // se não
        // idle
    }

    void move()
    {
        transform.position = Vector3.MoveTowards(
            transform.position
            , buracoAlvo.position
            , MontyController.instance.speed * Time.deltaTime);
    }

    void chegouNoBuraco()
    {
        estadoAtual = EstadosMonty.Buscando; 
        // verifica player ainda ali 
        // bate 
        MontyController.instance.MontyDisponivel(this);
    }
    
    
    private void OnTriggerEnter(Collider obj)
    {
        if (obj.CompareTag("Buraco"))
        {
            chegouNoBuraco();
        }
    }
}

public enum EstadosMonty{
    Buscando,
    Seguindo
}