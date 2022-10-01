using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool playerWeapon = true;
    public bool attacking = false;
    public float damage = 1;
    public float critMod = 1.5f;
    public float knockback = 1;
}
