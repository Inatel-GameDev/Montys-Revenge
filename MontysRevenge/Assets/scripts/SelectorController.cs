using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public enum InputDirection
{
    None,
    Left,
    Right,
    Top,
    TopLeft ,
    TopRight,
    Bottom,
    BottomLeft,
    BottomRight
}

public class SelectorController : MonoBehaviour
{
    public GameObject[] buracos;
    private Buraco buracoAtual;
    public GameObject buracoInicial;
    public GameObject Placar;
    public PlayerController player;
    public bool canMove;
    InputDirection dir;

    void Start()
    {
        canMove = true;
        transform.SetParent(buracoInicial.transform);
        player.CoroutineFinished += () => canMove = true; // Olha ele usando expressão lambda todo foda todo csharp
        buracoAtual = buracoInicial.GetComponent<Buraco>();
        transform.localPosition = new Vector3(0,0.71f,0);
        Placar.SetActive(true);
    }

    // Método chamado pelo Player Input
    public void OnMove(InputValue value)
    {
        dir = GetInputDirection(value.Get<Vector2>());
    }

    public InputDirection GetInputDirection(Vector2 input)
    {
        if (input.magnitude < 0.2f) // Ignora zonas mortas
            return InputDirection.None;

        input = input.normalized;

        // Vetores para comparar direções
        Vector2 left = Vector2.left;
        Vector2 right = Vector2.right;
        Vector2 top = Vector2.up;
        Vector2 topLeft = new(-0.707f, 0.707f);
        Vector2 topRight = new(0.707f, 0.707f);
        Vector2 bottom = Vector2.down;
        Vector2 bottomLeft = new(-0.707f, -0.707f);
        Vector2 bottomRight = new(0.707f, -0.707f);

        // Comparações com Dot Product
        if (Vector2.Dot(input, left) > 0.7f) return InputDirection.Left;
        if (Vector2.Dot(input, right) > 0.7f) return InputDirection.Right;
        if (Vector2.Dot(input, top) > 0.7f) return InputDirection.Top;
        if (Vector2.Dot(input, topLeft) > 0.7f) return InputDirection.TopLeft;
        if (Vector2.Dot(input, topRight) > 0.7f) return InputDirection.TopRight;
        if (Vector2.Dot(input, bottom) > 0.7f) return InputDirection.Bottom;
        if (Vector2.Dot(input, bottomLeft) > 0.7f) return InputDirection.BottomLeft;
        if (Vector2.Dot(input, bottomRight) > 0.7f) return InputDirection.BottomRight;

        return InputDirection.None;
    }

    public void OnAttack()
    {
        if(!player.isHit){
            if (player.isActiveAndEnabled){
                StartCoroutine(player.Move(1));
            }
            else{
                player.gameObject.SetActive(true);
                StartCoroutine(player.Move(-1));
            }
        }
    }

    // Update is called once per frame
    //Fazer um tempo de offhand dps de mudar pra um buraco novo
    private void Update()
    {
        if (canMove)
        {
            Select();
        }

        if (player.isOut)
        {
            canMove = false;
        }

        // flag para o monty começar a seguir 
        if (player.isOut)
        {
            buracoAtual.temPlayer = true;
        }
    }

    public void StunaPlayer()
    {
        Debug.Log("Atingindo");
        StartCoroutine(Stun());
    }

private IEnumerator Stun()
    {
        canMove = false;
        yield return new WaitForSeconds(1);
        StartCoroutine(player.Move(1));
        canMove = true;
    }
    
    public void FlagBuraco()
    {
        buracoAtual.temPlayer = false;
        buracoAtual.temMonty = false;
    }
    

    private void Select(){
        switch(buracoAtual.pos){
            case 0:
            if(dir == InputDirection.BottomLeft || dir == InputDirection.Bottom || dir == InputDirection.Left){
                GoTo(2);
            }
            if(dir == InputDirection.BottomRight || dir == InputDirection.Right ){GoTo(3);}
            break;

            case 1:
            if(dir == InputDirection.Right || dir == InputDirection.TopRight){
                GoTo(2);
            }
            if(dir == InputDirection.Bottom || dir == InputDirection.BottomRight){GoTo(5);}
            break;

            case 2:
            if(dir == InputDirection.Top || dir == InputDirection.TopRight){
                GoTo(0);
            }
            if(dir == InputDirection.Left){
                GoTo(1);
            }
            if(dir == InputDirection.BottomLeft || dir == InputDirection.Bottom){
                GoTo(5);
            }
            if(dir == InputDirection.BottomRight){
                GoTo(6);
            }
            if(dir == InputDirection.Right){
                GoTo(3);
            }
            break;

            case 3:
            if(dir == InputDirection.Top || dir == InputDirection.TopLeft){
                GoTo(0);
            }
            if(dir == InputDirection.Right){
                GoTo(4);
            }
            if(dir == InputDirection.BottomRight || dir == InputDirection.Bottom){
                GoTo(7);
            }
            if(dir == InputDirection.BottomLeft){
                GoTo(6);
            }
            if(dir == InputDirection.Left){
                GoTo(2);
            }
            break;

            case 4:
            if(dir == InputDirection.Left || dir == InputDirection.TopLeft){
                GoTo(3);
            }
            if(dir == InputDirection.Bottom || dir == InputDirection.BottomLeft){GoTo(7);}
            break;

            case 5:
            if(dir == InputDirection.Top || dir == InputDirection.TopLeft){
                GoTo(1);
            }
            if(dir == InputDirection.Right){
                GoTo(6);
            }
            if(dir == InputDirection.BottomRight){
                GoTo(9);
            }
            if(dir == InputDirection.TopRight){
                GoTo(2);
            }
            if(dir == InputDirection.Bottom){
                GoTo(8);
            }
            break;

            case 6:
            if(dir == InputDirection.Top || dir == InputDirection.TopLeft){
                GoTo(2);
            }
            if(dir == InputDirection.TopRight){
                GoTo(3);
            }
            if(dir == InputDirection.BottomRight || dir == InputDirection.Right){
                GoTo(7);
            }
            if(dir == InputDirection.BottomLeft || dir == InputDirection.Left){
                GoTo(5);
            }
            if(dir == InputDirection.Bottom){
                GoTo(9);
            }
            break;

            case 7:
            if(dir == InputDirection.Top || dir == InputDirection.TopRight){
                GoTo(4);
            }
            if(dir == InputDirection.Left){
                GoTo(6);
            }
            if(dir == InputDirection.BottomLeft){
                GoTo(9);
            }
            if(dir == InputDirection.TopLeft){
                GoTo(3);
            }
            if(dir == InputDirection.Bottom){
                GoTo(10);
            }
            break;

            case 8:
            if(dir == InputDirection.Top || dir == InputDirection.TopLeft){
                GoTo(5);
            }
            if(dir == InputDirection.Right || dir == InputDirection.TopRight){GoTo(9);}
            break;

            case 9:
            if(dir == InputDirection.Top){
                GoTo(6);
            }
            if(dir == InputDirection.TopLeft){
                GoTo(5);
            }
            if(dir == InputDirection.TopRight){
                GoTo(7);
            }
            if(dir == InputDirection.Left){
                GoTo(8);
            }
            if(dir == InputDirection.Right){
                GoTo(10);
            }
            break;

            case 10:
            if(dir == InputDirection.Top || dir == InputDirection.TopRight){
                GoTo(7);
            }
            if(dir == InputDirection.Left || dir == InputDirection.TopLeft){GoTo(9);}
            break;

        }
    }

    private void GoTo(int indexBuraco){
        transform.SetParent(buracos[indexBuraco].transform);
        transform.localPosition = new Vector3(0,0.71f,0);
        buracoAtual = GetComponentInParent<Buraco>();
        transform.localScale = new(1f+(buracoAtual.transform.childCount/10f), 0.02f, 1f+(buracoAtual.transform.childCount/10f));
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown(){
        canMove = false;
        yield return new WaitForSeconds(0.15f);
        canMove = true;
    }
}
