using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

	public float health = 100;
	[SerializeField] public GameObject pause;
	//public GameObject deathEffect;

	public void TakeDamage(float damage)
	{
		health -= damage;
		HealthSystem.Instance.TakeDamage(damage);

		//StartCoroutine(DamageAnimation());

		if (health <= 0)
		{
			Debug.Log("Te moriste");
			pause.SetActive(true);
			//Die();
		}
	}
}