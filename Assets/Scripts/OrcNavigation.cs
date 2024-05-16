using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrcNavigation : MonoBehaviour
{
	// Player
	private GameObject _player;

	// Chase
	[Header("Chase Variables")]
	public float chaseSpeed = 3f;
	public float distanceDetection = 5f;

	// Wander
	[Header("Wander Variables")]
	public float wanderSpeed = 1f;
	private float directionTimer = 0.5f;
	public Vector2 wanderDirection;
	public float xrange = 5f;
	public float yrange = 5f;

	void Start()
    {
		Vector2 wanderDirection = new Vector2(Random.Range(0 - xrange, 0 + xrange), Random.Range(0 - yrange, 0 + yrange));
	}

    // Update is called once per frame
    void Update()
    {
		_player = GameObject.FindGameObjectWithTag("Player"); //vai buscar o player

		Vector2 playerPosition = _player.transform.position;
		if (Vector2.Distance(transform.position, playerPosition) > distanceDetection)
		{
			directionTimer -= Time.deltaTime;
			if (directionTimer < 0)
			{
				wanderDirection = new Vector2(Random.Range(0 - xrange, 0 + xrange), Random.Range(0 - yrange, 0 + yrange));
				directionTimer = 0.5f;
			}
			transform.position = Vector2.MoveTowards(transform.position, wanderDirection, wanderSpeed * Time.deltaTime);
		}
		else if (Vector2.Distance(transform.position, playerPosition) <= distanceDetection)
		{
			transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, chaseSpeed * Time.deltaTime);
		}
	}
}
