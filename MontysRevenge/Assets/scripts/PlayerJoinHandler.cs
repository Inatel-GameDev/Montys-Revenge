using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls; 
using System.Collections.Generic;

public class PlayerJoinHandler : MonoBehaviour
{
    public InputDeviceTracker input;
    public InputAction attackAction;

    private void OnEnable()
    {
        // Enable the input action and subscribe to the performed event
        attackAction.Enable();
        attackAction.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event to avoid memory leaks
        attackAction.performed -= OnAttackPerformed;
        attackAction.Disable();
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        // Access the device that triggered the action
        var device = context.control.device;

        // Example: Check if the input came from a specific device
        int specificDeviceId = 1; // Replace with the specific device ID you're interested in

        if (device.deviceId == specificDeviceId)
        {
            Debug.Log("Attack triggered from the specific device!");
            OnAttack();
        }
        else
        {
            Debug.Log("Attack triggered from a different device.");
        }
    }

    private void OnAttack()
    {
        // This is your custom function that gets called when the attack happens
        Debug.Log("Attack performed!");
    }
}