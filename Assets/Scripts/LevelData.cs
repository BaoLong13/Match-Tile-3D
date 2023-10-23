using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level")]
public class LevelData : ScriptableObject
{
   public int levelID;
   public int tileAmount;
   public float timeAmount;
}
