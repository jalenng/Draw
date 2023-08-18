using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorConfig : ScriptableObject
{
    [SerializeField] public List<CursorEntry> cursors = new List<CursorEntry>();
}


[System.Serializable]
public class CursorEntry
{
    [SerializeField] public string name;
    [SerializeField] public Vector2 hotspot;
    [SerializeField] public Texture2D sprite;
}
