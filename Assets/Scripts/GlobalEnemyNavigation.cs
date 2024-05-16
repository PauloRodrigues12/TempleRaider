using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEnemyNavigation : MonoBehaviour
{
    public static GameObject[] waypoints; //array de todos os waypoints presentes
    public GameObject[] enemies; //array de todos os inimigos presentes
    public static GameObject currentWaypoint; //o waypoint atual a ser seguido
    public bool canChooseWP = false; //verifica se pode escolher um WP novo para nao o fazer todas as frames
    private float minDistance = 0.2f; //distancia minima q um inimigo tem de chegar a um WP para poder escolher outro


    void Start()
    {
        
    }

    void Update()
    {
		waypoints = GameObject.FindGameObjectsWithTag("Waypoint"); //vai buscar os WPs
		enemies = GameObject.FindGameObjectsWithTag("Enemy"); //vai buscar os inimigos
	}

    private void FixedUpdate()
    {
        if (canChooseWP) //se for possivel escolher um WP
        {
            currentWaypoint = SelectWaypoint(); //chama a funcao e escolhe um
            canChooseWP = false; //proibe de escolher outro imediatamente a seguir
        }

        foreach (GameObject enemy in enemies) //faz a verificacao em cada inimigo no array
        {
            float distance = Vector3.Distance(enemy.transform.position, currentWaypoint.transform.position);
			//checka se algum inimigo chegou perto suficiente e se esse inimigo tava em grouped, senao nao conta
			if (distance <= minDistance && enemy.GetComponent<EnemyNavigation>().enemyState == EnemyNavigation.EnemyState.Grouped) 
            {
                canChooseWP = true; //pode escolher outro WP
            }
        }
    }

    //funcao para selecionar WPs
    public static GameObject SelectWaypoint()
    {
        if (waypoints.Length > 0) //se o array nao tiver vazio
        {
            int currentWaypointIndex = Random.Range(0, waypoints.Length); //o id do WP atual e escolhido a sorte da lista de todos os WPs
            GameObject currentWaypoint = waypoints[currentWaypointIndex]; //esse id e passado para um gameobject
            return currentWaypoint; //devolve o gameobject que agora e o waypoint selecionado
        }
        else //se por alguma razao o array tiver vazio devolve um erro
        {
            Debug.Log("EMPTY");
            return null;
        }
    }
}