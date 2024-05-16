using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class EnemyNavigation : MonoBehaviour
{
    public GameObject[] waypoints; //array de todos os WPs existentes
    public GameObject[] enemies; //array de todos os inimigos existentes
    public GameObject currentWaypoint; //waypoint atualmente selecionado
    public Rigidbody2D rb; //rigidbody
    private GameObject player; //o jogador
    public float playerSearchRadius = 1f; //o raio em que o inimigo verifica se existem jogadores
    public float speed = 0.03f; //a velocidade a que o inimigo se move
    //este bool serve para prevenir o inimigo de procurar um WP novo a cada frame
    public bool canChooseWP = true; //se estiver ativo ele pode procurar por um WP 
    public float minDistance = 0.5f; //dist minima que um inimigo tem de se aproximar de um WP para contar como visitado
    public float minDistanceFriends = 1f; //dist minima a q um inimigo tem de estar de outro para se agruparem
    private int globalWaypointLimit = 3; //limite de WPs globais que o jogador pode visitar agrupado ate dar reset
    //este timer e basicamente a duracao do estado reset
    private float canSearchTimer = 5f;
    //este bool e ativado quando o jogador visita o limite de WPs globais e ativa o estado reset
    private bool startSearchTimer = false;
    //este timer e necessario pela natureza da verificacao de colisoes a cada frame
    //basicamente se nao tiver um pequeno buffer entre a colicao e o registo ele pode registar multiplas colisoes em vez de so uma
    private float registerWpHitTimer = 1f;
    //este bool comeca o timer anterior
    public bool startWpTimer = false;
    //private float playerChaseRadius = 100f;

    public enum EnemyState //todos os estados do inimigo
    {
        Wandering, //vaguear pelos waypoints
        Grouped, //agrupado
        Resetting, //estado que ocorre dps do inimigo visitar um certo numero de WPs em estado agrupado, previne agrupamento durante um certo tempo
        Chasing //segue o jogador
    }

    public EnemyState enemyState = EnemyState.Wandering; //estado inicial

    void Start()
    {
        
    }

    void Update()
    {
		waypoints = GameObject.FindGameObjectsWithTag("Waypoint"); //vai buscar os WPs
		player = GameObject.FindGameObjectWithTag("Player"); //vai buscar o player
		enemies = GameObject.FindGameObjectsWithTag("Enemy"); //vai buscar os inimigos
	}

    private void FixedUpdate()
    {
        //as coisas que cada caso faz
        //as cores sao so para melhor visibilidade e desnecessarias
        switch (enemyState)
        {
            case EnemyState.Wandering:
                //gameObject.GetComponent<SpriteRenderer>().color = Color.green; // Mudar a cor
                SearchForPlayer(); //procura o jogador
                startSearchTimer = false; //o timer esta desativado
                canSearchTimer = 5f; //o timer esta reset
                globalWaypointLimit = 3; //o limit de waypoints visitados em grupo esta reset
                SearchForFriends(); //procura por amigos para agrupar
                if (canChooseWP) //se poder escolher um WP
                {
                    currentWaypoint = SelectWaypoint(); //escolhe um WP novo
                    canChooseWP = false; //proibe de escolher outro WP imediatamente a seguir
                }
                MoveToWaypoint(currentWaypoint); //move se para o WP escolhido
                float distanceW = Vector3.Distance(gameObject.transform.position, currentWaypoint.transform.position); //dist entre o jogador e o WP, tem W no nome para distinguir os estados
                if (distanceW <= minDistance) //se chegar ao WP escolhido
                {
                    canChooseWP = true; //da reset e escolhe um novo
                }
                break;

            case EnemyState.Grouped:
				//gameObject.GetComponent<SpriteRenderer>().color = Color.yellow; // Mudar a cor
				SearchForPlayer();
                CheckWaypointLimit(); //verifica se o inimigo ja chegou ao limite de WPs visitados em grupo
                MoveToWaypoint(GlobalEnemyNavigation.currentWaypoint); //move se para o WP global
                if (startWpTimer) //se o timer tiver ativado
                {
                    registerWpHitTimer -= Time.deltaTime; //comeca a countdown
                }
                if (globalWaypointLimit <= 0) //se ja visitou o limite de WPs em grupo
                {
                    enemyState = EnemyState.Resetting; //muda para o estado resetting
                }
                break;

            case EnemyState.Resetting:
				//gameObject.GetComponent<SpriteRenderer>().color = Color.red; // Mudar a cor
				SearchForPlayer();
                if (canChooseWP)
                {
                    currentWaypoint = SelectWaypoint();
                    canChooseWP = false;
                }
                MoveToWaypoint(currentWaypoint);
                float distanceR = Vector3.Distance(gameObject.transform.position, currentWaypoint.transform.position);
                if (distanceR <= minDistance)
                {
                    canChooseWP = true;
                }

                startSearchTimer = true; //ativa o timer

                if (startSearchTimer) //se o timer estiver ativo
                {
                    canSearchTimer -= Time.deltaTime; //comeca a countdown
                }

                if (canSearchTimer <= 0) //se o tempo limite do estado reset tiver acabado
                {
                    enemyState = EnemyState.Wandering; //volta para o estado wandering
                }
                break;

            case EnemyState.Chasing:
				//gameObject.GetComponent<SpriteRenderer>().color = Color.black; // Mudar a cor
				startSearchTimer = false; //reset
                canSearchTimer = 5f; //reset
                globalWaypointLimit = 3; //reset
                ChasePlayer(); //segue o player
                break;
        }
    }

    //funcao de selecionar um WP
    private GameObject SelectWaypoint() //esta funcao devolve um gameobject que vai ser o nosso waypoint
    {
        if (waypoints.Length > 0) //se o array de WPs nao estiver vazio
        {
			int currentWaypointIndex = Random.Range(0, waypoints.Length); //o id do WP atual e escolhido a sorte da lista de todos os WPs
            GameObject currentWaypoint = waypoints[currentWaypointIndex]; //esse id e passado para um gameobject
            return currentWaypoint; //devolve o gameobject que agora e o waypoint selecionado
        }
        else //se por alguma razao o array estiver vazio da um erro
        {
            Debug.Log("EMPTY");
            return null;
        }
    }

    //funcao de mover para o WP
    private void MoveToWaypoint(GameObject waypoint)
    {
        //move o inimigo em direcao ao waypoint
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, waypoint.transform.position, speed);
    }

    //procura por outros inimigos perto dele
    private void SearchForFriends()
    {
        //for loop duplo para verificar todos os inimigos no array
        for (int i = 0; i < enemies.Length - 1; i++)
        {
            for (int j = i + 1; j < enemies.Length; j++)
            {
                //a distancia entre dois inimigos
                float distance = Vector3.Distance(enemies[i].transform.position, enemies[j].transform.position);

                if (distance < minDistanceFriends //se a dist entre 2 inimigos for menor que a dist minima
                    && enemies[i].GetComponent<EnemyNavigation>().enemyState != EnemyState.Resetting //e o inimigo1 nao estiver em reset
                    && enemies[j].GetComponent<EnemyNavigation>().enemyState != EnemyState.Resetting //e o inimigo2 nao estiver em reset
                    && enemies[i].GetComponent<EnemyNavigation>().enemyState != EnemyState.Chasing //e o inimigo1 nao estiver em chase
                    && enemies[j].GetComponent<EnemyNavigation>().enemyState != EnemyState.Chasing) //e o inimigo2 nao estiver em chase
                {
                    enemies[i].GetComponent<EnemyNavigation>().enemyState = EnemyState.Grouped; //o inimigo1 e agrupado
                    enemies[j].GetComponent<EnemyNavigation>().enemyState = EnemyState.Grouped; //o inimigo2 e agrupado
                }
                //neste caso verifica se o inimigo1 esta em chase e junta ambos em chase
                else if (distance < minDistanceFriends && enemies[i].GetComponent<EnemyNavigation>().enemyState == EnemyState.Chasing)
                {
                    enemies[i].GetComponent<EnemyNavigation>().enemyState = EnemyState.Chasing;
                    enemies[j].GetComponent<EnemyNavigation>().enemyState = EnemyState.Chasing;
                }
                //e neste verifica se o inimigo2 esta em chase e faz o mesmo
                else if (distance < minDistanceFriends && enemies[j].GetComponent<EnemyNavigation>().enemyState == EnemyState.Chasing)
                {
                    enemies[i].GetComponent<EnemyNavigation>().enemyState = EnemyState.Chasing;
                    enemies[j].GetComponent<EnemyNavigation>().enemyState = EnemyState.Chasing;
                }
            }
        }
    }

    //funcao q verifica se o limite de WPs em grupo foi visitado
    private void CheckWaypointLimit()
    {
        //dist entre o inimigo e o WP global
        float distance = Vector3.Distance(gameObject.transform.position, GlobalEnemyNavigation.currentWaypoint.transform.position);
        if (distance <= minDistance && enemyState == EnemyState.Grouped) //se a dist foi menor q a dist minima E o inimigo tiver em estado agrupado
        {
            startWpTimer = true; //ativa o timer
        }
        if (registerWpHitTimer <= 0) //se o timer acabar
        {
            globalWaypointLimit -= 1; //remove 1 WP ao numero de WPs globais q o inimigo pode visitar
            registerWpHitTimer = 1f; //da reset ao timer
            startWpTimer = false; //desativa o timer
        }
    }

    //funcao de procurar pelo jogador
    private void SearchForPlayer()
    {
        //dist entre o inimigo e o jogador
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (distance < playerSearchRadius) //se a dist for menor q o raio de procura de inimigo
        {
            enemyState = EnemyState.Chasing; //o inimigo passa a chase mode
        }
    }

    //funcao que da chase ao player
    private void ChasePlayer()
    {
        //move o inimigo na direcao do player
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, player.transform.position, speed);
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position); //dist entre o player e o inimigo
        if (distance > playerSearchRadius)
        {
            enemyState = EnemyState.Resetting; //se o player tiver longe da reset
        }
    }
}