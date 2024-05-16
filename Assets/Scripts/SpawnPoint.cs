using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public float waitTime = 3f;

	[Header("False = Player & Portal | True = Star")]
	public bool _mode = false;

    void Start()
    {
        StartCoroutine(waitingForMap());
    }

    IEnumerator waitingForMap()
    {
        yield return new WaitForSeconds(waitTime);

        if (_mode)
        {
			GameObject[] starSpawner = GameObject.FindGameObjectsWithTag("StarSpawn");
			int randomNumber = Random.Range(0, starSpawner.Length);

			gameObject.transform.position = starSpawner[randomNumber].transform.position;
		}
        else
        {
			GameObject[] playerSpawner = GameObject.FindGameObjectsWithTag("PlayerSpawn");
			int randomNumber = Random.Range(0, playerSpawner.Length);

			gameObject.transform.position = playerSpawner[randomNumber].transform.position;
		}
    }
}
