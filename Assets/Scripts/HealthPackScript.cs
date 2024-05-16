using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackScript : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player") //se colidir SO com o jogador da lhe health
		{
            if (TinyPlayerController.playerHealth <= 2) //nao da health se o jogador tiver full
            {
				TinyPlayerController.playerHealth += 1; //da health ao jogador
				Destroy(gameObject); //mata se
			}
		}
	}
}