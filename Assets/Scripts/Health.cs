using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class Health : MonoBehaviour, IDamageable
{

    public float maxHP;
    public float currentHP;
    public float damageValue;
    public float invulnerability_time;
    public bool imune;
    public float minForce;

    //propriedade implementada da interface
    public float HealthPoints
    {
        get { return currentHP; }
    }

    

    void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log("Ouch!");
        if (currentHP <= 0) Die();
    }

    public void Die()
    {
        Destroy(this);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        //se está imune, nada acontece
        if (imune) return;

        //perde vida se impacto é forte o bastante

        if (collision.relativeVelocity.magnitude * collision.otherRigidbody.mass >= minForce)
        {
            TakeDamage(damageValue);
            imune = true;
            StartCoroutine("InvulnerableTime");
        }

    }

    IEnumerator InvulnerableTime()
    {
        imune = true;
        yield return new WaitForSeconds(invulnerability_time);
        imune = false;
    }

}