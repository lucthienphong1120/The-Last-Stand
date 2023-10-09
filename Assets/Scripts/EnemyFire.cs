using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
	public GameObject bullet;
	private Transform playerPos;
	private EnemyAI enemyAI;
	public float cooldown = 5f;
	public int eachBulletDamage;
	public int numberOfBullets = 1;
	private float delayEach = 0.3f;
	private float fireTime = 0;
	private float bulletForce = 50;
	public bool spread = false;
	private float maxSpread = 0.2f;
	private float shootDistance = 100;
	private bool enoughRange;
	private bool canShoot;
	// Start is called before the first frame update
	void Start()
	{
		playerPos = GameObject.FindGameObjectWithTag("Player").transform;
		enemyAI = GetComponent<EnemyAI>();
	}

	// Update is called once per frame
	void Update()
	{
		// check if it's range enemy and not stop
		canShoot = enemyAI.RangeEnemy && !enemyAI.stop;
		enoughRange = Vector3.Distance(transform.position, playerPos.position) <= shootDistance;
		if (canShoot && enoughRange)
		{
			// count cooldown only when enough range
			if (fireTime <= 0)
			{
				StartCoroutine(FireBullet(playerPos.position - transform.position));
				fireTime = cooldown;
			}
			else if (fireTime > 0)
			{
				fireTime -= Time.deltaTime;
			}
		}
	}

	IEnumerator FireBullet(Vector3 direction)
	{
		for (int i = 0; i < numberOfBullets; i++)
		{
			// create bullet
			GameObject bulletTmp = Instantiate(bullet, transform.position, Quaternion.identity);
			bulletTmp.GetComponent<BulletController>().bulletDamage = eachBulletDamage;
			Destroy(bulletTmp, 1.5f);
			// fire bullet
			Rigidbody2D bulletRb = bulletTmp.GetComponent<Rigidbody2D>();
			if (!spread)
			{
				// straight
				bulletRb.AddForce(direction.normalized * bulletForce, ForceMode2D.Impulse);
				yield return new WaitForSeconds(delayEach);
			}
			else
			{
				// shotgun
				Vector2 speadDir = direction + new Vector3(Random.Range(-maxSpread, maxSpread), Random.Range(-maxSpread, maxSpread));
				bulletRb.AddForce(speadDir.normalized * bulletForce, ForceMode2D.Impulse);
				yield return null;
			}
		}
	}
}
