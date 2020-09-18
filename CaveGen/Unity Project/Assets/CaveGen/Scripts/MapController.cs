using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public MapGenerator mapGenerator;

    public GameObject playerPrefab;
    public int numPlayersToSpawn = 4;


    [Header("Debug")]
    public bool showDebugCubes = false;
    public Transform cubeParent;

    private GameObject[,] debugCubes;

    private List<List<MapGenerator.Coord>> activeGreenCubes;


    void Start() {
        this.Initialize();
    }

    public void Initialize() {
        this.debugCubes = new GameObject[mapGenerator.width, mapGenerator.height];
        this.activeGreenCubes = new List<List<MapGenerator.Coord>>();
        this.mapGenerator.GenerateMap();
        this.CreateCubes();
        this.SpawnPlayers();
    }

    // Update is called once per frame
	void Update() {
        // Test World to map coord
		if (Input.GetMouseButton(0)) {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo)) {
                MapGenerator.Coord nearestPoint = mapGenerator.NearestWorldPointToCoord(hitInfo.point);
                MapTileType tile = mapGenerator.GetMapCoord(nearestPoint);

                if (tile != MapTileType.OUT_OF_BOUNDS) {
                    this.debugCubes[nearestPoint.tileX, nearestPoint.tileY].GetComponent<MeshRenderer>().material.color = Color.red;
                }

            }
		}

        // Restart
        if (Input.GetKeyDown(KeyCode.R)) {
            this.Initialize();
        }

        // Spawn random stuff
        if (Input.GetKeyDown(KeyCode.S)) {
            int numCount = 5;
            int radius = 1;
            List<MapGenerator.Coord> loc = mapGenerator.GetRandomOpenCoords(numCount, radius, true);
            foreach (MapGenerator.Coord coord in loc) {

                List<MapGenerator.Coord> cubesForSquare = new List<MapGenerator.Coord>();

                for (int x = -radius; x <= radius; x++) {
			        for (int y = -radius; y <= radius; y++) {
                        int drawX = coord.tileX + x;
                        int drawY = coord.tileY + y;
                        this.debugCubes[drawX, drawY].GetComponent<MeshRenderer>().material.color = Color.green;
                        MapGenerator.Coord newCoord = new MapGenerator.Coord(drawX, drawY);
                        mapGenerator.SetMapTile(newCoord, MapTileType.OTHER);
                        cubesForSquare.Add(newCoord);
                    }
                }

                this.activeGreenCubes.Add(cubesForSquare);
            }
        }

        // Remove a patch of random stuff
        if (Input.GetKeyDown(KeyCode.D) && this.activeGreenCubes.Count > 0) {
            List<MapGenerator.Coord> selectedCube = this.activeGreenCubes[0];
            this.activeGreenCubes.RemoveAt(0);
            foreach(MapGenerator.Coord coord in selectedCube) {
                this.debugCubes[coord.tileX, coord.tileY].GetComponent<MeshRenderer>().material.color = Color.white;
                this.mapGenerator.SetMapTile(coord, MapTileType.EMPTY);
            }
        }
	}

    private void CreateCubes() {
        if (showDebugCubes) {
            for (int x = 0; x < mapGenerator.width; x++) {
                List<GameObject> debugCubeRow = new List<GameObject>();
                for (int y = 0; y < mapGenerator.height; y++) {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.SetParent(this.cubeParent);
                    cube.transform.position = mapGenerator.CoordToWorldPoint(new MapGenerator.Coord(x, y));
                    cube.transform.localScale = Vector3.one * 0.5f;
                    cube.gameObject.name = "Cube: " + x + " " + y;

                    this.debugCubes[x, y] = cube;
                }
            }
        }
    }

    private void SpawnPlayers() {
        List<MapGenerator.Coord> playerLocations = mapGenerator.GetRandomOpenCoords(this.numPlayersToSpawn, 1, false);
        foreach (MapGenerator.Coord coord in playerLocations) {
            Vector3 spawnLocation = mapGenerator.CoordToWorldPoint(coord);
            GameObject player = Instantiate(playerPrefab, spawnLocation, Quaternion.identity) as GameObject;
        }
    }

}