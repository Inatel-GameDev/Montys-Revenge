using System.Collections;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public TMP_Text timerText;
    public float Timer = 30;
    public Camera _camera;
    public GameObject InitialScreen;
    public GameObject Blackout;
    public GameObject Begin;
    public GameObject Wins;
    public bool PlayMode = false;
    public PlayerJoinHandler PlayerJoin;
    public bool ended = false;

    private void Start()
    {
       InitialScreen.SetActive(true);
       Begin.SetActive(false);
    }

    public void StartGame(){
        if(!PlayMode)
            StartCoroutine(StartIntroSequence());
    }

    private void FixedUpdate()
    {
        if (PlayMode)
        {
            Timer -= Time.fixedDeltaTime;
            timerText.text = Timer <= 0 ? "0" : ((int)Timer).ToString();
        }

        if (Timer <= 0 && !ended)
        {
            StartCoroutine(EndingSequence());
            ended = true;
        }
    }

    private IEnumerator EndingSequence()
    {
        MontyController.instance.PausaMontys();
        Blackout.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        
        // Mover camera 
        _camera.transform.position = new Vector3(0, 1.2f, -6.5f); 
        _camera.transform.rotation = Quaternion.Euler(6.7f, 0, 0);
        
        PlayerController pAux = PlayerJoin.players[0].GetComponent<PlayerController>();

        if (PlayerJoin.players.Length > 1)
        {
            for (int i = 1; i < PlayerJoin.players.Length; i++)
            {
                PlayerController p = PlayerJoin.players[i].GetComponent<PlayerController>();
                if (p.pontos > pAux.pontos)
                {
                    pAux = p;
                }
            }
        }
        
        // pAux.enabled = true;
        // pAux.transform.position = new Vector3(0,28.2f,-5.3f);
        
        Blackout.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Wins.SetActive(true);
    }

    private IEnumerator StartIntroSequence()
    {
        // Desativar a tela inicial
        InitialScreen.SetActive(false);
        
        // Ativar o blackout por 1 segundo
        Blackout.SetActive(true);
        yield return new WaitForSeconds(1f);
        Blackout.SetActive(false);
        yield return new WaitForSeconds(1f);
        Begin.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Begin.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        
        // Aproximar a câmera 4 unidades no eixo Y e rotacionar 30 graus no eixo X
        Vector3 targetPosition = _camera.transform.position + new Vector3(0, 3.1f, -1.7f);
        Quaternion targetRotation = Quaternion.Euler(65, _camera.transform.rotation.eulerAngles.y, _camera.transform.rotation.eulerAngles.z);

        float duration = 1f;
        float elapsedTime = 0f;
        Vector3 startPosition = _camera.transform.position;
        Quaternion startRotation = _camera.transform.rotation;

        while (elapsedTime < duration)
        {
            _camera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            _camera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Garantir que a posição final seja precisa
        _camera.transform.position = targetPosition;
        _camera.transform.rotation = targetRotation;

        // Ativar PlayMode
        MontyController.instance.StartMontys();
        PlayMode = true;
    }
}
