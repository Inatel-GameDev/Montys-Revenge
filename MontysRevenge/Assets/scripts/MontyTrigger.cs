using System;
using UnityEngine;

public class MontyTrigger : MonoBehaviour
{
    [SerializeField] private Monty monty;
    
    private void OnTriggerStay(Collider obj)
    {
        if (obj.CompareTag("Buraco"))
        {
            monty.verificaBater(obj.gameObject.GetComponent<Buraco>());
        }    
    }
}
