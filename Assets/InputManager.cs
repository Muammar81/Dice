using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event Action<InputKey> OnInput = delegate { };
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnInput?.Invoke(InputKey.Roll);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            OnInput?.Invoke(InputKey.Rotate);
        }
    }
}
    public enum InputKey { Roll, Rotate }
