using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NameToSpriteMapEntry
{
  public string name;
  public Sprite sprite;
}

[CreateAssetMenu(fileName = "DialogueAvatarOptions", menuName = "Dialogue Avatar Options", order = 2)]
public class DialogueAvatarOptions : ScriptableObject
{
  public List<NameToSpriteMapEntry> nameToSpriteMap = new List<NameToSpriteMapEntry>();
}
