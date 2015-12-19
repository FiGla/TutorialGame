using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
    [Serializable]
    public class Count {
        public int minimum;
        public int maximum;

        public Count(int min, int max) {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count WallCount = new Count(5, 9);
    public Count FoodCount = new Count(1, 5);
    public GameObject Exit;
    public GameObject[] FloorTiles;
    public GameObject[] EnemyTiles;
    public GameObject[] OuterWallTiles;
    public GameObject[] WallTiles;
    public GameObject[] FoodTiles;

    private Transform BoardHolder; // keep the hairalicy clean ; :v
    private List<Vector3> GridPositions = new List<Vector3>();

    void InitializeList() {
        GridPositions.Clear();
        for(int x=1; x<columns-1; ++x){
            for(int y=1; y <rows-1; ++y){
                GridPositions.Add(new Vector3(x, y, 0f));
            }
            
        }
    }


    void BoardSetup() {
        BoardHolder = new GameObject("Board").transform;
        for(int x=-1; x<columns+1; ++x){
            for(int y=-1; y<rows+1; ++y){
                GameObject ToInstantiate = FloorTiles[Random.Range(0, FloorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                    ToInstantiate = OuterWallTiles[Random.Range(0, OuterWallTiles.Length)];

                GameObject Instance = Instantiate(ToInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                Instance.transform.SetParent(BoardHolder);
            }
        }
    }

    Vector3 RandomPosition() {
        int RandomIndex = Random.Range(0, GridPositions.Count);
        Vector3 randomPosition = GridPositions[RandomIndex];
        GridPositions.RemoveAt(RandomIndex);
        return randomPosition;
    }

    void LayoutRandomObject(GameObject[] tileArray, int maxi, int mini)
    {
        int ObjectCount = Random.Range(mini, maxi + 1);
        for (int i = 0; i < ObjectCount; ++i)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject TileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(TileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int Level){
        BoardSetup();
        InitializeList();
        LayoutRandomObject(WallTiles, WallCount.maximum, WallCount.minimum);
        LayoutRandomObject(FoodTiles, FoodCount.maximum, FoodCount.minimum);
        int EnemyCount = (int)Mathf.Log(Level, 2f);
        LayoutRandomObject(EnemyTiles, EnemyCount, EnemyCount);
        Instantiate(Exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }


}
