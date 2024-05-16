using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PaintTiles : MonoBehaviour
{
	// Other scripts
    WorldGenerator wg;

	// This tile info
    public string _tileType = "";
    public bool _painted = false;

	// All the tile lists
	public List<GameObject> U_tiles;
	public List<GameObject> R_tiles;
	public List<GameObject> D_tiles;
	public List<GameObject> L_tiles;
	public List<GameObject> UD_tiles;
	public List<GameObject> RL_tiles;
	public List<GameObject> UR_tiles;
	public List<GameObject> RD_tiles;
	public List<GameObject> DL_tiles;
	public List<GameObject> UL_tiles;
	public List<GameObject> URD_tiles;
	public List<GameObject> UDL_tiles;
	public List<GameObject> URL_tiles;
	public List<GameObject> RDL_tiles;
	public List<GameObject> URDL_tiles;

	// Rules
	private bool overlap = false;

	// Checker
	private bool _analysed = false;

	void Start()
    {
		if (_painted == false)
		{
			// Detect other tiles for creation
			DetectUP();
			DetectRIGHT();
			DetectDOWN();
			DetectLEFT();

			// Creation
			//CreateTile();

			_analysed= true;
		}
	}

	void Update()
	{
        if (_analysed == true && _painted == false)
        {
			// Creation
			CreateTile();

			_painted = true;
		}
	}

	void CreateTile()
	{
		if (_tileType == "U") Instantiate(U_tiles[Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
		if (_tileType == "R") Instantiate(R_tiles[Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
		if (_tileType == "D") Instantiate(D_tiles[Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
		if (_tileType == "L") Instantiate(L_tiles[Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
		if (_tileType == "UD") Instantiate(UD_tiles[Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
		if (_tileType == "RL") Instantiate(RL_tiles[Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
		if (_tileType == "UR") Instantiate(UR_tiles[Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
		if (_tileType == "RD") Instantiate(RD_tiles[Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
		if (_tileType == "DL") Instantiate(DL_tiles[Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
		if (_tileType == "UL") Instantiate(UL_tiles[Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
		if (_tileType == "URD") Instantiate(URD_tiles[Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
		if (_tileType == "UDL") Instantiate(UDL_tiles[Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
		if (_tileType == "URL") Instantiate(URL_tiles[Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
		if (_tileType == "RDL") Instantiate(RDL_tiles[Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
		if (_tileType == "URDL") Instantiate(URDL_tiles[Random.Range(0, 2)], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity); // Pintar Tile no tile atual
	}

	bool DetectOtherTiles()
	{
		GameObject[] allObjects = GameObject.FindGameObjectsWithTag("TemporaryTile");

		foreach (GameObject go in allObjects)
		{
			float dist = Vector3.Distance(transform.position, go.transform.position);
			if (dist <= 1f || dist == 0)
			{
				Debug.Log("Detected Object!");
				return true;
			}
		}

		return false;
	}

	void DetectUP()
	{
		transform.position += new Vector3(0, 12, 0); // Mover o objeto para detetar
		overlap = DetectOtherTiles(); // Detetar outros objetos
		if (overlap) _tileType += "U";
		transform.position += new Vector3(0, -12, 0); // Mover o objeto de volta
	}
	void DetectRIGHT()
	{
		transform.position += new Vector3(12, 0, 0); // Mover o objeto para detetar
		overlap = DetectOtherTiles(); // Detetar outros objetos
		if (overlap) _tileType += "R";
		transform.position += new Vector3(-12, 0, 0); // Mover o objeto de volta
	}
	void DetectDOWN()
	{
		transform.position += new Vector3(0, -12, 0); // Mover o objeto para detetar
		overlap = DetectOtherTiles(); // Detetar outros objetos
		if (overlap) _tileType += "D";
		transform.position += new Vector3(0, 12, 0); // Mover o objeto de volta
	}
	void DetectLEFT()
	{
		transform.position += new Vector3(-12, 0, 0); // Mover o objeto para detetar
		overlap = DetectOtherTiles(); // Detetar outros objetos
		if (overlap) _tileType += "L";
		transform.position += new Vector3(12, 0, 0); // Mover o objeto de volta
	}
}
