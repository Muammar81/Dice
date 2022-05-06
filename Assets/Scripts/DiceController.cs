using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceController : MonoBehaviour
{
    private List<int> sides = new List<int>();
    private Texture[] textures;
    private Renderer rend;
    private Material[] mats;
    private const int sidesCount = 6;

    private void OnEnable() => InputManager.OnInput += InputManager_OnInput;
    private void OnDisable() => InputManager.OnInput -= InputManager_OnInput;

    private void InputManager_OnInput(InputKey key)
    {
        if (key == InputKey.Roll)
        {
            Roll();
        }
    }

    private void Start()
    {
        FillSides();

        textures = Resources.LoadAll<Texture>("Textures");
        rend = GetComponent<Renderer>();
        mats = rend.materials;
    }

    private void FillSides()
    {
        for (int i = 0; i < sidesCount; i++)
        {
            sides.Add(i);
        }
    }

    private void Roll()
    {
        sides = sides.Shuffle().ToList();
        for (int i = 0; i < mats.Length; i++)
        {
            var index = sides[i];
            mats[i].mainTexture = textures[index];
        }
    }
}
