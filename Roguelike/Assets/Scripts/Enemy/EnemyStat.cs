using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStat", menuName = "Enemy/EnemyStat")]
public class EnemyStat : ScriptableObject
{
  public int maxHp;
  public int currentHp;
  
  public float targetRadius;
  public float targetRange;
  
}
