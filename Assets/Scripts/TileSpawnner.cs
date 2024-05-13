using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TempleRun
{
    public class TileSpawnner : MonoBehaviour
    {
        [SerializeField] private int tileStartCount = 10;
        [SerializeField] private int minimumStraightTiles = 3;
        [SerializeField] private int maximumStraightTiles = 15;
        [SerializeField] private GameObject startingTile;
        [SerializeField] private List<GameObject> turnTiles;
        [SerializeField] private List<GameObject> obstacles;

        private Vector3 currentTileLocation = Vector3.zero;
        private Vector3 currentTileDirection = Vector3.forward;
        private GameObject prevTile;

        private List<GameObject> currentTiles;
        private List<GameObject> currentObstacles;

        private void Start()
        {
            currentTiles = new List<GameObject>();
            currentObstacles = new List<GameObject>();

            Random.InitState(System.DateTime.Now.Millisecond); // Consider initializing elsewhere.

            if (startingTile != null)
            {
                for (int i = 0; i < tileStartCount; i++)
                {
                    SpawnTile(startingTile.GetComponent<Tile>(), false);
                }
            }
            else
            {
                Debug.LogError("Starting tile is not assigned!");
            }

            SpawnTile(turnTiles[0].GetComponent<Tile>(), true);
            AddNewDirection(Vector3.left);
        }

        private void SpawnTile(Tile tile, bool spawnObstacle)
        {
            Quaternion newTileRotation = tile.gameObject.transform.rotation * Quaternion.LookRotation(currentTileDirection, Vector3.up);
            prevTile = GameObject.Instantiate(tile.gameObject, currentTileLocation, newTileRotation);
            currentTiles.Add(prevTile);

            if(spawnObstacle) 
            {
                SpawnObstacle();
            }

            if (tile.type== TileType.STRAIGHT)
            {
                currentTileLocation += Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size, currentTileDirection); //Multiplies one vector with another.
            }
            
        }

        private void DeletePreviousTiles()
        {
            for (int i = currentTiles.Count - 1; i >= 1; i--)
            {
                GameObject tile = currentTiles[i];
                currentTiles.RemoveAt(i);
                Destroy(tile);
            }

            for (int i = currentObstacles.Count - 1; i >= 1; i--)
            {
                GameObject obstacle = currentObstacles[i];
                currentObstacles.RemoveAt(i);
                Destroy(obstacle);
            }
        }
        private void AddNewDirection(Vector3 direction)
        {
            currentTileDirection = direction;
            DeletePreviousTiles();

            Vector3 tilePlacementScale;
            if(prevTile.GetComponent<Tile>().type != TileType.SIDEWAYS) 
            {
                tilePlacementScale = Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size/2 + (Vector3.one * startingTile.GetComponent<BoxCollider>().size.z/2),currentTileDirection);
            }
            else
            {
                tilePlacementScale = Vector3.Scale((prevTile.GetComponent<Renderer>().bounds.size-(Vector3.one * 2)) + (Vector3.one * startingTile.GetComponent<BoxCollider>().size.z / 2), currentTileDirection);
            }

            currentTileLocation += tilePlacementScale;

            int currentpathLength = Random.Range(minimumStraightTiles, maximumStraightTiles);
            for(int i = 0; i< currentpathLength; i++) 
            {
                SpawnTile(startingTile.GetComponent<Tile>(), (i==0) ? false : true);
            }

            SpawnTile(SelectRandomGameObjectFromList(turnTiles).GetComponent<Tile>(),true);
        }

        private void SpawnObstacle()
        {
            if (Random.value > 0.2f) return;

            GameObject obstaclePrefab = SelectRandomGameObjectFromList(obstacles);
            Quaternion newObjectRotation = obstaclePrefab.gameObject.transform.rotation * Quaternion.LookRotation(currentTileDirection, Vector3.up);
            GameObject obstacle = Instantiate(obstaclePrefab, currentTileLocation, newObjectRotation);
            currentObstacles.Add(obstacle);
        }
        private GameObject SelectRandomGameObjectFromList(List<GameObject> list)
        {
            if (list.Count == 0) return null;
            return list[Random.Range(0,list.Count)];

        }
    }
}
