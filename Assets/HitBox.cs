using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Health health;
    public void OnHit(float damage)
    {
        if (health.currentHealth > 0) health.TakeDamage(damage);
    }
}
