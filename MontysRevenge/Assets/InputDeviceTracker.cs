using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class InputDeviceTracker : MonoBehaviour
{
    public List<int> deviceIds = new List<int>();
    private const int maxDevices = 4;
    public event Action OnConnected;

    private void OnEnable()
    {
        InputSystem.onAnyButtonPress.Call(RegisterDevice);
    }

    private void OnDisable()
    {
        //Descobre como desloga a manete dps
    }

    public void RegisterDevice(InputControl control)
    {
        int deviceId = control.device.deviceId;
        if (!deviceIds.Contains(deviceId) && deviceIds.Count < maxDevices)
        {
            OnConnected?.Invoke();
            deviceIds.Add(deviceId);
            Debug.Log($"Dispositivo registrado: {deviceId}");
        }
    }
}
