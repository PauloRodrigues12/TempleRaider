using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2;

    private Transform playerPos;
    private Transform spawnPoint;

    private void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        spawnPoint = GameObject.FindGameObjectWithTag("Respawn").transform;

        playerPos.position = spawnPoint.position;
    }

    private void Update()
    {
        // Obtém as entradas do teclado
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calcula a direção de movimento
        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput);

        // Move o jogador
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Correr
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 4;
        }
        else moveSpeed = 2;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cliff"))
        {
            SceneManager.LoadScene("Defeat");
        }
    }

	public void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			SceneManager.LoadScene("Defeat");
		}
	}
}
