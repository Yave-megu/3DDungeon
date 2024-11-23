using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class MazeCell
{
    public int type; // 0 = 바닥, 1 = 벽
    public bool has_monster;
    public int monster_strength;
    public bool has_reward;
    public int reward_value;
    public bool is_trap;
    public bool is_start;
    public bool is_end;
    public bool is_wall;
}

[Serializable]
public class Maze
{
    public List<List<MazeCell>> mazeData { get; set; }
}


public class TileLoader : MonoBehaviour
{
    // Inspector에서 JSON 파일을 Drag & Drop하여 할당
    public TextAsset[] mazeJsonFiles; // JSON 파일 (TextAsset)

    public GameObject floorPrefab; // 바닥 프리팹
    public GameObject wallPrefab; // 벽 프리팹
    public GameObject monsterPrefab; // 몬스터 프리팹
    public GameObject rewardPrefab; // 보상 프리팹
    public GameObject trapPrefab; // 함정 프리팹
    public GameObject startPrefab; // 시작점 프리팹

    public GameObject endPrefab; // 도착점 프리팹

    public Grid grid;
    public Tilemap tilemap;
    private Tile EndTile;
    private TileBase endTileBase;
    private Tile floorTile;
    private TileBase floorTileBase;
    private Tile StartTile;

    private TileBase startTileBase;
    private Tile wallTile;
    private TileBase wallTileBase;

    [Button]
    private void Awake()
    {
        floorTile = new Tile();
        wallTile = new Tile();
        StartTile = new Tile();
        EndTile = new Tile();

        wallPrefab.gameObject.transform.localScale = new Vector3(4, 16, 4);
        floorPrefab.gameObject.transform.localScale = new Vector3(4, 4, 4);
        startPrefab.gameObject.transform.localScale = new Vector3(4, 4, 4);
        endPrefab.gameObject.transform.localScale = new Vector3(4, 4, 4);

        floorTile.gameObject = floorPrefab;
        wallTile.gameObject = wallPrefab;
        StartTile.gameObject = startPrefab;
        EndTile.gameObject = endPrefab;

        startTileBase = StartTile;
        endTileBase = EndTile;
        floorTileBase = floorTile;
        wallTileBase = wallTile;

        foreach (var mazeJsonFile in mazeJsonFiles)
        {
            if (mazeJsonFile == null)
            {
                Debug.LogError("JSON file is missing! Please assign it in the inspector.");
                return;
            }

            // JSON 파일을 읽어서 Maze 객체로 디시리얼라이즈

            string name = mazeJsonFile.name;
            grid = new GameObject(name).AddComponent<Grid>();
            grid.transform.position = new Vector3(0, 0, 0);
            grid.GetComponent<Grid>().cellSize = new Vector3(4, 4, 0);
            grid.GetComponent<Grid>().cellLayout = GridLayout.CellLayout.Rectangle;
            grid.GetComponent<Grid>().cellSwizzle = GridLayout.CellSwizzle.XZY;
            tilemap = new GameObject("Tilemap").AddComponent<Tilemap>();
            tilemap.transform.position = new Vector3(0, 0, 0);
            tilemap.transform.parent = grid.transform;
            tilemap.gameObject.AddComponent<TilemapRenderer>();
            tilemap.gameObject.AddComponent<Rigidbody>();
            tilemap.GetComponent<Rigidbody>().isKinematic = true;


            LoadMaze(mazeJsonFile);
        }
    }

    // Start is called before the first frame update


    private void LoadMaze(TextAsset mazeJsonFile)
    {

        string json = mazeJsonFile.text; // TextAsset의 텍스트 읽기
        List<List<MazeCell>> maze = JsonConvert.DeserializeObject<List<List<MazeCell>>>(json);

        // 미로 출력
        GenerateMaze(maze);
    }

    private void GenerateMaze(List<List<MazeCell>> maze)
    {
        int rows = maze.Count;
        int cols = maze[0].Count;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                var position = new Vector3Int(x, y, 0);
                tilemap.SetTile(position, floorTile);
            }
        }

        // 미로의 각 셀을 Unity 오브젝트로 생성
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                var cell = maze[y][x];
                var position = new Vector3Int(x, y, 0); // 유니티에서는 (x, y, z) 좌표 사용
                if (cell.type == 1) // 외벽
                {
                    // tilemap.DeleteCells(position, position);
                    // position.z -= 3;
                    tilemap.SetTile(position, wallTile);
                    // Instantiate(wallPrefab, position, Quaternion.identity);
                }
                else // 바닥
                {

                    // Instantiate(floorPrefab, position, Quaternion.identity);

                    // 바닥에 몬스터, 보상, 트랩 등을 추가
                    // if (cell.has_monster)
                    // {
                    //     var monster = Instantiate(monsterPrefab, position, Quaternion.identity);
                    //     monster.name = $"Monster_Strength_{cell.monster_strength}";
                    // }
                    // if (cell.has_reward)
                    // {
                    //     var reward = Instantiate(rewardPrefab, position, Quaternion.identity);
                    //     reward.name = $"Reward_Value_{cell.reward_value}";
                    // }
                    // if (cell.is_trap)
                    // {
                    //     Instantiate(trapPrefab, position, Quaternion.identity);
                    // }

                    // 시작점과 도착점
                    if (cell.is_start)
                    {
                        tilemap.SetTile(position, StartTile);
                        // Instantiate(startPrefab, position, Quaternion.identity);s
                    }
                    if (cell.is_end)
                    {
                        tilemap.SetTile(position, EndTile);
                        // Instantiate(endPrefab, position, Quaternion.identity);
                    }
                }
            }
        }
    }
}