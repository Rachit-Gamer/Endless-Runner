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

            GameObject selectedTurnTile = SelectRandomGameObjectFromList(turnTiles);
            if (selectedTurnTile != null)
            {
                SpawnTile(selectedTurnTile.GetComponent<Tile>(), false);
            }
            else
            {
                Debug.LogError("No turn tile selected!");
            }
        }

        private void SpawnTile(Tile tile, bool spawnObstacle)
        {
            Quaternion newTileRotation = tile.gameObject.transform.rotation * Quaternion.LookRotation(currentTileDirection, Vector3.up);
            prevTile = GameObject.Instantiate(tile.gameObject, currentTileLocation, newTileRotation);
            currentTiles.Add(prevTile);
            currentTileLocation += Vector3.Scale(prevTile.GetComponent<Renderer>().bounds.size, currentTileDirection); //Multiplies one vector with another.
        }
        private GameObject SelectRandomGameObjectFromList(List<GameObject> list)
        {
            if (list.Count == 0) return null;
            return list[Random.Range(0,list.Count)];

        }
    }
}
