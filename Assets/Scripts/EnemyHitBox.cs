using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class EnemyHitBox : Collidable
{
    // Damage
    public int damage = 2;
    public float pushForce = 5;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            Damage dmg = new Damage
            {
                // Create a new dmg object, before sending it to the player.
                damageAmount = damage,
                origin = transform.position,
                pushForce = pushForce
            };

            coll.SendMessage("ReceiveDamage", dmg);
        }
    }
}
