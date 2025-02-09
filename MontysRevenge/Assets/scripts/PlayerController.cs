using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 initialPosition;
    public bool isOut;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        
    }

    public IEnumerator Move(int dir){ //indo ou vindo
        float t = 0f;
        float duration = 0.1f;
        float moveAmount = 0.1f;
        
        while (t < duration)
        {
            // Movimento no eixo Z
            transform.position = new Vector3(initialPosition.x, initialPosition.y - Mathf.Lerp(0, dir*moveAmount, t / duration), initialPosition.z);
            
            t += Time.deltaTime;
            yield return null;
        }
        if(dir == 1)
            gameObject.SetActive(false);
    }
}
