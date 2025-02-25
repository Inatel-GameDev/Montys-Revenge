using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TMP_Text timerText;
    public float Timer = 30;
    public Camera _camera;
    public GameObject InitialScreen;
    public Image Blackout;
    public GameObject Begin;
    public bool PlayMode = false;
    InputDeviceTracker controles;
    public SelectorController[] jogadores;
    public Image[] PlayerProfile;
    public Color selectedColor;
    public GameObject[] ReadyText;
    private int playerCount = 0;

    private void Start()
    {
        controles = GetComponentInChildren<InputDeviceTracker>();
        controles.OnConnected += ShowPlayers;
        InitialScreen.SetActive(true);
        Begin.SetActive(false);
    }

    public void StartGame(){
        if(!PlayMode)
            StartCoroutine(StartIntroSequence());
    }

    private void FixedUpdate()
    {
        Timer -= Time.fixedDeltaTime;
        timerText.text = Timer <= 0 ? "0" : ((int)Timer).ToString();
        if(Timer <= 0){
            StartEndingSequence();
        }
        if(controles.deviceIds.Count >= 2){
            StartGame();
        }
    }

    private IEnumerator StartIntroSequence()
    {
        PlayMode = true;
        // Desativar a tela inicial
        //InitialScreen.SetActive(false);
        yield return StartCoroutine(BlackoutTransition());
        // Ativar o blackout por 1 segundo
        AtivarPlayers();
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
        _camera.transform.GetPositionAndRotation(out Vector3 startPosition, out Quaternion startRotation);
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

    }

    private IEnumerator StartEndingSequence(){
        PlayMode = false;
        yield return new WaitForSeconds(1f);
        StartCoroutine(BlackoutTransition());
    }

    private IEnumerator BlackoutTransition(){
        float duration = 2f;
        float halfDuration = duration / 2f;
        // Acender gradualmente
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            SetAlpha(t / halfDuration);
            yield return null;
        }
        SetAlpha(1); // Garante que fique totalmente visível
        InitialScreen.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        // Apagar gradualmente
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            SetAlpha(1 - (t / halfDuration));
            yield return null;
        }
        SetAlpha(0); // Garante que fique totalmente invisível
    }

    private void SetAlpha(float alpha)
    {
        if (Blackout != null)
        {
            Color color = Blackout.color;
            color.a = alpha;
            Blackout.color = color;
        }
    }

    public void ShowPlayers(){
        PlayerProfile[playerCount].color = selectedColor;
        ReadyText[playerCount].SetActive(true);
        playerCount++;
    }

    public void AtivarPlayers(){
        for (int i = 0; i < controles.deviceIds.Count; i++)
        {
            jogadores[i].gameObject.SetActive(true);
            jogadores[i].Id = controles.deviceIds[i];
            Debug.Log($"Jogador {jogadores[i].Id} vinculado aos id {controles.deviceIds[i]}");
        }
    }
}
