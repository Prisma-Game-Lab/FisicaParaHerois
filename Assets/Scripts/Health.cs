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
    private Animator Anim;

	public Boss Boss;
	public float XTeleportLeft;
	public float XTeleportRight;
	public Transform BossSpawnInspector;

    //propriedade implementada da interface
    public float HealthPoints
    {
        get { return currentHP; }
    }

    

    void Awake()
    {
        currentHP = maxHP;
        Anim = this.GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log("Ouch!");

		if (Anim != null) {
			Anim.SetTrigger ("dano");
		}

        if (currentHP <= 0) Die();
		Boss.Left = !Boss.Left;

		switch (Boss.Left) {
		case true:
			transform.position = new Vector3 (XTeleportLeft, transform.position.y, transform.position.z);
			transform.localScale = new Vector3(transform.localScale.x *-1, transform.localScale.y, transform.localScale.z);
			BossSpawnInspector.transform.position = new Vector3 (-26.6f, transform.position.y, transform.position.z);
			break;
		case false:
			transform.position = new Vector3 (XTeleportRight, transform.position.y, transform.position.z);
			transform.localScale = new Vector3(transform.localScale.x *-1, transform.localScale.y, transform.localScale.z);
			BossSpawnInspector.transform.position = new Vector3 (1.9f, transform.position.y, transform.position.z);
			break;
		}
    }

    public void Die()
    {
		if (Boss == null) {
			Destroy (this);
			return;
		} else {
			Boss.OnDeath ();
		}
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