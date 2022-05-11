using System;
using UnityEngine;

public class DiceRotation : MonoBehaviour
{
    [SerializeField] [Range(1f,50f)] private float rotationSpeed=15f;
    float initialRotationSpeed;
    int direction = 1;
    private bool paused;
    
    private void OnEnable() => InputManager.OnInput += InputManager_OnInput;
    private void OnDisable() => InputManager.OnInput -= InputManager_OnInput;

    private void Start()
    {
        initialRotationSpeed = rotationSpeed;
    }
    private void InputManager_OnInput(InputKey key)
    {
        if (key == InputKey.Rotate)
        {
            direction *= -1;
        }

        if (key == InputKey.Pause)
        {
            paused = !paused;
            rotationSpeed = paused ? 0 : initialRotationSpeed;
        }
    }

    private void Update() => transform.Rotate(Vector3.up * direction * rotationSpeed * Time.deltaTime,Space.World);
}
