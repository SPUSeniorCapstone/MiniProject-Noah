using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Melee : Weapon
{
    private void OnTriggerEnter(Collider collider)
    {
        if (attacking)
        {
            var normal = transform.position - FindObjectOfType<Player>().transform.position;
            normal.y = 0f;
            normal.Normalize();

            var entity = collider.transform.GetComponent<Entity>();
            if (entity != null)
            {
                var p = entity as Player;
                if (p == null)
                {
                    if (playerWeapon)
                    {
                        entity.DamageEntity(damage, knockback * normal);
                    }
                }else if (!playerWeapon)
                {
                    entity.DamageEntity(damage, knockback * normal);
                }

            }
        }
    }
}

