using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : ScriptableObject
{  
     [Header("META")]
     public string weaponNAME;
     public int damage;
     public float fireRate;
     public int range;
     public MeshRenderer mesh;    
}
