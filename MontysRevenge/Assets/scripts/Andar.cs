using System.Collections;
using UnityEngine;

public class Andar : MonoBehaviour
{
    public float altura;
    public float tempo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Vai());
    }


    IEnumerator Vai(){

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + altura, transform.position.z), tempo);

        yield return new WaitForSeconds(tempo);
        
            StartCoroutine(Volta());
        
    }

    IEnumerator Volta(){

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y - altura, transform.position.z), tempo);
        yield return new WaitForSeconds(tempo);
        
            StartCoroutine(Vai());
        
    }
}
