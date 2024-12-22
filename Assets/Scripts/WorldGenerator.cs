using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    // Other scripts
    PaintTiles paintTiles;

    // Shared Variables
    public bool paintMode = false;

    // Variaveis
    public List<GameObject> tile;

    // Numero de tiles gerados
    public int[] seed;
    public int seedSize = 10;

    // Seed String
    public string seedString = "Seed: ";

    // Rules
    private bool overlap = false;

    void Start()
    {
        GenerateSeed();
        BuildMap();
    }

    void GenerateSeed()
    {
        seed = new int[seedSize]; // Tamanho da Seed

        for (int i = 0; i < seed.Length; i++) // Criacao da Seed
        {
            seed[i] = UnityEngine.Random.Range(1, 5); // Gerar numero aleatorio para a seed
            seedString += seed[i].ToString(); // Adicionar valor a seedString
        }
        Debug.Log(seedString); // Debug da seedString
    }

    void BuildMap()
    {
        for (int i = 0; i < seed.Length; i++) // Construcao do mapa
        {

            switch (seed[i]) // 1 - UP | 2 - RIGHT | 3 - DOWN | 4 - LEFT
            {
                case 1:
                    transform.position += new Vector3(0, 12, 0); // Movimento do WorldGenerator

                    // Funcao para impedir overlaping
                    OverlapUP();

                    Instantiate(tile[0], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
                    break;
                case 2:
                    transform.position += new Vector3(12, 0, 0); // Movimento do WorldGenerator

                    // Funcao para impedir overlaping
                    OverlapRIGHT();

                    Instantiate(tile[1], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
                    break;
                case 3:
                    transform.position += new Vector3(0, -12, 0); // Movimento do WorldGenerator

                    // Funcao para impedir overlaping
                    OverlapDOWN();

                    Instantiate(tile[2], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
                    break;
                case 4:
                    transform.position += new Vector3(-12, 0, 0); // Movimento do WorldGenerator

                    // Funcao para impedir overlaping
                    OverlapLEFT();

                    Instantiate(tile[3], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
                    break;
            }
        }
        paintMode = true;
    }

    bool DetectObjects()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("TemporaryTile");

        foreach (GameObject go in allObjects)
        {
            float dist = Vector3.Distance(transform.position, go.transform.position);
            if (dist <= 1f || dist == 0)
            {
                Debug.Log("Overlap Found!");
                return true;
            }
        }

        return false;
    }

    void OverlapUP()
    {
        do
        {
            overlap = DetectObjects(); // Detetar outros objetos
            if (overlap) transform.position += new Vector3(0, 12, 0); // Movimento do WorldGenerator
        }
        while (overlap == true);
    }
    void OverlapRIGHT()
    {
        do
        {
            overlap = DetectObjects(); // Detetar outros objetos
            if (overlap) transform.position += new Vector3(12, 0, 0); // Movimento do WorldGenerator
        }
        while (overlap == true);
    }
    void OverlapDOWN()
    {
        do
        {
            overlap = DetectObjects(); // Detetar outros objetos
            if (overlap) transform.position += new Vector3(0, -12, 0); // Movimento do WorldGenerator
        }
        while (overlap == true);
    }
    void OverlapLEFT()
    {
        do
        {
            overlap = DetectObjects(); // Detetar outros objetos
            if (overlap) transform.position += new Vector3(-12, 0, 0); // Movimento do WorldGenerator
        }
        while (overlap == true);
    }
}