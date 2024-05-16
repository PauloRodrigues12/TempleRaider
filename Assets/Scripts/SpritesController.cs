using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpritesController : MonoBehaviour
{
	[Header("False = Portal | True = Star")]
	public bool _mode = false;

	// Objects
	public GameObject _portal;

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player" && _mode == true)
		{
			_portal.SetActive(true);
			Destroy(gameObject);
		}

		if (collider.gameObject.tag == "Player" && _mode == false)
		{
			SceneManager.LoadScene("Win");
		}
	}
}
