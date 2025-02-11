using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public String Nome;
    public float pontos;
    Vector3 initialPosition;
    public bool isOut;
    public bool isHit;
    public event System.Action CoroutineFinished;
    public SelectorController selector;

    public TMP_Text nome_txt;
    public TMP_Text pontos_txt;
    private void Start() {
        nome_txt.text = Nome;
        pontos = 0;
    }
    

    private void OnEnable() {
        transform.localPosition = Vector3.zero;
        initialPosition = transform.localPosition;
        isOut = true;
    }
    void FixedUpdate()
    {
        if(!isHit){
            pontos += Time.fixedDeltaTime;
        }
        pontos_txt.text = pontos.ToString("N" + 2);
    }

    public IEnumerator Move(int dir){ //indo ou vindo
        float t = 0f;
        float duration = 0.15f;
        float moveAmount = 10f;
        
        while (t < duration)
        {
            // Movimento no eixo Z
            transform.localPosition = new Vector3(initialPosition.x, initialPosition.y - Mathf.Lerp(0, dir*moveAmount, t / duration), initialPosition.z);
            
            t += Time.deltaTime;
            yield return null;
        }
        if(dir == 1){
            selector.FlagBuraco();
            isOut = false;
            CoroutineFinished?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
