using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
	public float waitTime = 10.0f;

	void Start()
    {
		StartCoroutine(deleteTemporary());
	}
	IEnumerator deleteTemporary() // Eliminar ficheiros temporários
	{
		Debug.Log("Deleting Temporary Tiles...");
		yield return new WaitForSeconds(waitTime);
		Destroy(gameObject);
	}
}
