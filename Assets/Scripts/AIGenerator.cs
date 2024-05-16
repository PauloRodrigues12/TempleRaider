using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGenerator : MonoBehaviour
{
	public WorldGenerator worldGenerator;
	public GlobalEnemyNavigation globalEnemyNavigation;

    public GameObject _ghost;
	public GameObject _orc;
	private int enemiesAmount = 0;

	public float waitTime = 1f; // Time to wait for a ghost spawn

	[Header("False = Orc | True = Ghost")]
	public bool _mode = false;

	void Start()
	{
		StartCoroutine(waitingForMap());
	}

	void Update()
	{
		enemiesAmount = worldGenerator.seedSize / 5;
	}

	IEnumerator waitingForMap()
	{
		yield return new WaitForSeconds(waitTime);

		int posX = 0;
		GameObject[] orcObjects = GameObject.FindGameObjectsWithTag("OrcSpawn");
		if (_mode)
		{
			for (int i = 0; i < enemiesAmount; i++)
			{
				Instantiate(_ghost, new Vector3(transform.position.x + posX, transform.position.y, transform.position.z), Quaternion.identity); // Spawn ghost
				posX += 20;
			}

		} else {

			for (int i = 0; i < enemiesAmount; i++)
			{
				int randomSpawn = Random.Range(0, orcObjects.Length);
				Instantiate(_orc, new Vector3(orcObjects[randomSpawn].transform.position.x, orcObjects[randomSpawn].transform.position.y, orcObjects[randomSpawn].transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
			}
		}
		globalEnemyNavigation.canChooseWP = true;
	}
}