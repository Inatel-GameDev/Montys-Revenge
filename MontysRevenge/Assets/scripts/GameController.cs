using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public TMP_Text timerText;
    public float Timer ;
    public Camera _camera;
    public GameObject InitialScreen;
    public Image Blackout;
    public Image Finish_text;
    public Image Wins_text;
    public TMP_Text winnerName;
    public GameObject WinningScreen;
    public Image Whiteout;
    public GameObject Begin;
    public bool PlayMode = false;
    InputDeviceTracker controles;
    public SelectorController[] jogadores;
    public GameObject[] podio;
    public Image[] PlayerProfile;
    public Color selectedColor;
    public GameObject[] ReadyText;
    private int playerCount = 0;

    public MontyController montyController;
    //
    public Vector3 endingPosition;
    private Vector3 initialScale;
    public  GameObject winnerPosition;
    public GameObject losers;

    public Canvas telaPause;

    
    private void Awake()
    {
        instance = this;
    }
    
    private void Start()
    {
        controles = GetComponentInChildren<InputDeviceTracker>();
        controles.OnConnected += ShowPlayers;
        InitialScreen.SetActive(true);
        Begin.SetActive(false);
        initialScale = Finish_text.transform.localScale * 1.1f;
        Finish_text.transform.localScale = initialScale;
    }

    private void FixedUpdate()
    {
        if(PlayMode){
            Timer -= Time.fixedDeltaTime;
            timerText.text = Timer <= 0 ? "0" : ((int)Timer).ToString();
            
            if(Timer <= 0){
                PlayMode = false;
                Timer = 0;
                StartCoroutine(StartEndingSequence());
            }
        }
    }

    public void StartGame(){
        Debug.Log("Start");
        if(controles.deviceIds.Count >= 1 && !PlayMode && Timer > 0){
            StartCoroutine(StartIntroSequence());
        }
    }


    private IEnumerator StartIntroSequence()
    {
        montyController.PausaMontys();
        

        yield return StartCoroutine(BlackoutTransition(true));
        // Ativar o blackout por 1 segundo
        
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
        
        yield return new WaitForSeconds(1f);
        Begin.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Begin.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        
        PlayMode = true;
        AtivarPlayers();
        
        montyController.LiberaMontys();
    }


    private IEnumerator StartEndingSequence(){
        montyController.PausaMontys();
        PlayMode = false;
        yield return StartCoroutine(FadeImages(Finish_text));
        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(BlackoutTransition(false));
        CheckWinner();
        WinningScreen.SetActive(true);
        yield return StartCoroutine(FadeImages(Wins_text));
        StartCoroutine(Move(losers, new(1.8f, 0.15f, -0.15f), new(-1.5f, 0.15f, -0.15f), 1f));
        yield return new WaitForSeconds(2f);
        telaPause.gameObject.SetActive(true);
      
    }

    private IEnumerator BlackoutTransition(bool isStart){
        float duration = 2f;
        float halfDuration = duration / 2f;
        // Acender gradualmente
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            SetAlpha(t / halfDuration);
            yield return null;
        }
        SetAlpha(1); // Garante que fique totalmente

        if(isStart){
            InitialScreen.SetActive(false);
        }else{
            _camera.transform.position = endingPosition;
            _camera.transform.rotation = Quaternion.Euler(25, _camera.transform.rotation.eulerAngles.y, _camera.transform.rotation.eulerAngles.z);;
        }

        yield return new WaitForSeconds(0.5f);
        // Apagar gradualmente
        for (float t = 0; t < halfDuration; t += Time.deltaTime)
        {
            SetAlpha(1 - (t / halfDuration));
            yield return null;
        }
        SetAlpha(0); // Garante que fique totalmente invisível
    }

    private IEnumerator FadeImages(Image UI_Image)
    {
        Vector3 finalScale = initialScale * 0.909f;
        float elapsedTime = 0f;
        Color parentInitialColor = UI_Image.color;
        Color childInitialColor = Whiteout.color;

        while (elapsedTime < 0.15f)
        {
            float t = elapsedTime / 0.15f;
            
            // Ajustar o alpha das imagens
            UI_Image.color = new Color(parentInitialColor.r, parentInitialColor.g, parentInitialColor.b, Mathf.Lerp(0f, 1f, t));
            Whiteout.color = new Color(childInitialColor.r, childInitialColor.g, childInitialColor.b, Mathf.Lerp(1f, 0f, t));
            UI_Image.transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Garantir que os valores finais são aplicados corretamente
        UI_Image.color = new Color(parentInitialColor.r, parentInitialColor.g, parentInitialColor.b, 1f);
        Whiteout.color = new Color(childInitialColor.r, childInitialColor.g, childInitialColor.b, 0f);

        yield return new WaitForSeconds(0.5f);

        elapsedTime = 0f;
        while (elapsedTime < 0.2f)
        {
            float t = elapsedTime / 0.2f;
            
            // Mover para cima e reduzir a opacidade
            UI_Image.transform.localPosition = Vector3.Lerp(Vector3.zero, Vector3.zero + new Vector3(0, 15, 0), t);
            UI_Image.color = new Color(parentInitialColor.r, parentInitialColor.g, parentInitialColor.b, Mathf.Lerp(1f, 0f, t));
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Garantir que os valores finais são aplicados corretamente
        UI_Image.transform.localPosition = Vector3.zero + new Vector3(0, 15, 0);
        UI_Image.color = new Color(parentInitialColor.r, parentInitialColor.g, parentInitialColor.b, 0f);
    }

    private IEnumerator Move(GameObject obj, Vector3 startPos, Vector3 endPos, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            obj.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.position = endPos;
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

    public void CheckWinner(){
        if (jogadores == null || jogadores.Length == 0) return;

        // Encontrar o player com mais pontos
        SelectorController winner = jogadores.OrderByDescending(sc => sc.player.pontos).FirstOrDefault();
        winnerName.text = winner.player.Nome;
        
        // Posicionar o vencedor na posição desejada
        if (winner != null)
        {
            winner.transform.position = winnerPosition.transform.position;
        }
        
        // Definir os outros players como filhos de groupParent
        foreach (var sc in jogadores)
        {
            if (sc != winner)
            {
                sc.transform.SetParent(losers.transform);
            }
        }
    }
}
