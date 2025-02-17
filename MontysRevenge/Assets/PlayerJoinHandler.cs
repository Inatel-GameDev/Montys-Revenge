using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls; 
using System.Collections.Generic;

public class PlayerJoinHandler : MonoBehaviour
{
    public GameObject[] players; // Lista de Players (até 4)
    private List<Gamepad> assignedGamepads = new List<Gamepad>(); // Lista de controles já atribuídos

    private void Start()
    {
        // Garante que os jogadores comecem desativados
        foreach (var player in players)
        {
            if (player != null)
            {
                player.SetActive(false);
                var playerInput = player.GetComponent<PlayerInput>();
                if (playerInput != null)
                {
                    playerInput.enabled = false; // Desativa o PlayerInput inicialmente
                }
            }
        }
    }

    private void Update()
    {
        // Verifica se algum gamepad foi conectado e se foi pressionado um botão
        foreach (var gamepad in Gamepad.all)
        {
            if (!assignedGamepads.Contains(gamepad) && AnyButtonPressed(gamepad))
            {
                AssignPlayerToGamepad(gamepad); // Atribui um jogador ao controle
            }
        }
    }

    private void AssignPlayerToGamepad(Gamepad gamepad)
    {
        // Verifica se há espaço para ativar um jogador
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].activeSelf) // Verifica se o jogador ainda não foi ativado
            {
                players[i].SetActive(true); // Ativa o jogador
                assignedGamepads.Add(gamepad); // Adiciona o gamepad à lista de controles atribuídos

                var playerInput = players[i].GetComponent<PlayerInput>();
                if (playerInput != null)
                {
                    // Atribui o controle de Gamepad ao PlayerInput
                    playerInput.SwitchCurrentControlScheme("Gamepad", gamepad);
                    playerInput.enabled = true; // Habilita o PlayerInput para o jogador
                }

                Debug.Log($"Jogador {i + 1} ativado com controle: {gamepad.name}");
                return; // Termina a execução assim que o jogador é ativado
            }
        }
    }

    private bool AnyButtonPressed(Gamepad gamepad)
    {
        // Verifica se algum botão foi pressionado no controle
        foreach (var control in gamepad.allControls)
        {
            if (control is ButtonControl button && button.wasPressedThisFrame)
            {
                return true; // Retorna true se algum botão for pressionado
            }
        }
        return false; // Retorna false caso nenhum botão tenha sido pressionado
    }
}