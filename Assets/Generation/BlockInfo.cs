using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Test/Block information")]

public class BlockInfo : ScriptableObject
{
    public BlockType Type;
    public Vector2 PixelOffset;

    public AudioClip StepSound;
}
