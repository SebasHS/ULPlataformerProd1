using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

	public int health = 100;

	//public GameObject deathEffect;

	public void TakeDamage(int damage)
	{
		health -= damage;

		//StartCoroutine(DamageAnimation());

		if (health <= 0)
		{
            Debug.Log("Te moriste");
			//Die();
		}
	}
}