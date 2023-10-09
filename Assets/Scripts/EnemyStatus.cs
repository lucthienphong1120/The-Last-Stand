using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
	private SpriteRenderer enemySprite;
	private Animator anim;
	public int enemyDamage;
	public int reward;
	private int currentHP;
	public int maxHP;
	// Start is called before the first frame update
	void Start()
	{
		enemySprite = GetComponentInChildren<SpriteRenderer>();
		anim = GetComponentInChildren<Animator>();
		currentHP = maxHP;
	}

	private void OnTriggerEnter2D(Collider2D collision) // enemy only get damage 1 time from weapon
	{
		if (collision.CompareTag("Weapon"))
		{
			GetHit(collision.GetComponent<BulletController>().bulletDamage);
			//Destroy(collision);
		}
	}

	public void GetHit(int damage)
	{
		// maybe get multi coroutine
		StartCoroutine(DamageEffect());
		currentHP -= damage;
		if (currentHP <= 0)
		{
			currentHP = 0;
			Death();
		}
	}

	void Death()
	{
		Destroy(gameObject, 1f);
		GetComponent<EnemyAI>().stop = true;
		GetComponent<Collider2D>().enabled = false;
		anim.SetTrigger("Death");
		GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().GetExp(reward);
    }

	IEnumerator DamageEffect()
	{
		enemySprite.color = Color.red;
		yield return new WaitForSeconds(0.1f);
		enemySprite.color = Color.white;
	}
}
