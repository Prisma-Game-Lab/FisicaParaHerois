using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable : IKillable
{
    float HealthPoints { get; }

    void TakeDamage(float damage);
}