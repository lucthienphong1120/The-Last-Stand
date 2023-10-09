using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	public int bulletDamage;
	//public bool isEnemyBullet = false;

	// remove bullet when hit anywhere (player/enemy/wall)
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Obstacles"))
		{
			Destroy(gameObject);
		}
		// manual delete at other script
		if (collision.CompareTag("Enemy"))
		{
            Destroy(gameObject);
        }
	}
}
