using System;
using UnityEngine;

public class DiceRotation : MonoBehaviour
{
    [SerializeField] [Range(1f,50f)] private float speed=15f;
    int direction = 1;

    private void OnEnable() => InputManager.OnInput += InputManager_OnInput;

    private void InputManager_OnInput(InputKey key)
    {
        if (key == InputKey.Rotate)
        {
            direction *= -1;
        }
    }

    private void OnDisable() => InputManager.OnInput -= InputManager_OnInput;
    private void Update() => transform.Rotate(Vector3.up * direction * speed * Time.deltaTime);
}
