using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
	public Seeker seeker;
	private Path path;
	private Coroutine moveCoroutine;
	public Transform character;
	private Vector3 targetPos;
	private float moveSpeed = 15;
	private float minRandomArea = 50;
	public bool RangeEnemy;
	private float minShootDistance = 25;
	public bool stop = false;
	//private float nextDistance = 5;
	// Start is called before the first frame update
	void Start()
	{
		// disable astar logs
		AstarPath.active.logPathResults = PathLog.None;
		// re-calculate path each 0.5s
		InvokeRepeating("CalculatePath", 0, 0.5f);
	}

	void CalculatePath()
	{
		targetPos = FindTarget();
		if (seeker.IsDone())
		{
			seeker.StartPath(transform.position, targetPos, OnPathCallback);
		}
	}

	Vector3 FindTarget()
	{
		Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
		if (playerPos == null)
		{
			return Vector3.zero;
		}
		if (RangeEnemy)
		{
			// walk random around player
			return playerPos + (minRandomArea * new Vector3(Random.Range(-1, 1), Random.Range(-1, 1)).normalized);
		}
		else
		{
			// move directly to player
			return playerPos;
		}
	}

	void OnPathCallback(Path p)
	{
		if (!p.error)
		{
			path = p;
			if (moveCoroutine != null)
			{
				StopCoroutine(moveCoroutine);
			}
			moveCoroutine = StartCoroutine(MoveToTarget());
		}
	}

	IEnumerator MoveToTarget()
	{
		int currentWayPoint = 0;
		while (currentWayPoint < path.vectorPath.Count)
		{
			// move from current pos to next wave path
			Vector3 direction = (path.vectorPath[currentWayPoint] - transform.position).normalized;
			transform.position += (direction * moveSpeed * Time.deltaTime);
			// if collided or enought distance to fire, stop move
			if (stop || (RangeEnemy && Vector3.Distance(transform.position, targetPos) < minShootDistance))
			{
				break;
			}
			// check next hop
			float distance = Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]);
			if (distance <= 0.5f)
			{
				currentWayPoint++;
			}
			// check flip object
			if (direction.x > 0)
			{
				character.localScale = new Vector3(1, 1, 1);
			}
			else if (direction.x < 0)
			{
				character.localScale = new Vector3(-1, 1, 1);
			}
			yield return null;
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			stop = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			stop = false;
		}
	}

}
