using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinyPlayerController : MonoBehaviour
{
    public Rigidbody2D rb; //rb do jogador
    public static int playerHealth = 3; //health (obvio)
    public bool canTakeDamage = true; //checka se o jogador pode levar dano. é uma mini ajuda ao jogador para poder fugir
    public float damageProtectionTimer = 3f; //passado 3 segundos o jogador volta a poder levar dano

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		MovePlayer(); //move o jogador
        PlayerDeath(); //checka se ele ta morto

        if (damageProtectionTimer <= 0) //se o timer tiver acabado
        {
            canTakeDamage = true; //o jogador volta a poder levar dano
            damageProtectionTimer = 3f; //o timer da reset para a proxima vez q levar dano
        }
	}

	private void FixedUpdate()
	{
		if (!canTakeDamage) //se o jogador nao poder levar dano pq levou antes 
		{
			damageProtectionTimer -= Time.deltaTime; //comeca a countdown do timer
		}
	}

    //move o jogador
    //isto mexe o gajo com o rato foi so para testar
    //se nao tiver em erro ja temos character controller
	private void MovePlayer()
    {
		Vector3 mousePosition = Input.mousePosition;

		mousePosition.z = 10;

		Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

		transform.position = new Vector3(worldMousePosition.x, worldMousePosition.y, transform.position.z);
	}

    //faz dano ao jogador
    public void LoseHealth()
    {
        playerHealth -= 1;
    }

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Enemy") //se colidir com um inimigo
        {
            if (canTakeDamage) //e for possivel levar dano
            {
                LoseHealth(); //ele leva dano
                canTakeDamage = false; //e ativa a mini ajuda
            }
        }
	}

    //funcao de morrer
    //neste momento so faz um debug mas e facil mudar quando quisermos
	private void PlayerDeath()
    {
        if (playerHealth <= 0)
        {
            Debug.Log("YOU ARE DEAD");
        }
    }
}