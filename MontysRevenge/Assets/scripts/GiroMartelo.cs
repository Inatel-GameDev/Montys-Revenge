using UnityEngine;
using System.Collections;

public class GiroMartelo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Vai());
    }

    IEnumerator Vai(){

        transform.rotation = Quaternion.Lerp(
                Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z),
                Quaternion.Euler(transform.rotation.eulerAngles.x + 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)
                , 0.2f);

        yield return new WaitForSeconds(0.3f);
        
            StartCoroutine(Volta());
        
    }

    IEnumerator Volta(){

        transform.rotation = Quaternion.Lerp(
            Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z),
            Quaternion.Euler(transform.rotation.eulerAngles.x - 90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z)
            , 0.2f);

        yield return new WaitForSeconds(0.3f);
        
            StartCoroutine(Vai());
        
    }

    

}
