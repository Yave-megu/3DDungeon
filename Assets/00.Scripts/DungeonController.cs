using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class DungeonController : MonoBehaviour
{

    public GameObject[] dungeons;
    public GameObject player;
    public TMP_Text textMesh;
    public GameObject resultPopup;
    private int currentStage;

    private PlayerMove playerMove;

    private int postStage;
    // private GameObject[] thisDungeons;

    public GameObject Instance { get; set; }

    public void Awake()
    {
        Instance = gameObject;
        for (int i = 0; i < dungeons.Length; i++)
        {
            dungeons[i].SetActive(false);
            if (i == 0)
            {
                dungeons[i].SetActive(true);
                currentStage = i;
            }
        }
        // textMesh = new TextMeshPro();
        // resultText = new TextMeshPro();
        textMesh.text = "Total Step: " + "0";
        resultPopup.SetActive(false);
        // thisDungeons = new GameObject[dungeons.Length];
        // thisDungeons[0] = Instantiate(dungeons[0]);
        // thisDungeons[1] = Instantiate(dungeons[1]);
        // thisDungeons[2] = Instantiate(dungeons[2]);
        // thisDungeons[3] = Instantiate(dungeons[3]);
        // thisDungeons[4] = Instantiate(dungeons[4]);


    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        playerMove = player.GetComponent<PlayerMove>();
        PlayerPositionInitialize();
        // thisDungeons[0].SetActive(true);
        // thisDungeons[1].SetActive(false);
        // thisDungeons[2].SetActive(false);
        // thisDungeons[3].SetActive(false);
        // thisDungeons[4].SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        textMesh.text = "Total Step: " + playerMove.StepCount;
        if (playerMove.EndTile)
        {
            NextDungeon();
        }
        if (playerMove.StartTile)
        {
            PostDungeon();
        }
    }
    private void PlayerPositionInitialize()
    {
        if (playerMove.StartTile is false && playerMove.EndTile is false)
        {
            var inintPos = GameObject.FindWithTag("StartTile").transform.position;
            inintPos.y += 2;
            player.transform.position = inintPos;
        }


        if (playerMove.EndTile)
        {
            var upInitPos = GameObject.FindWithTag("StartTile").transform.position;
            upInitPos.y += 2;
            player.transform.position = upInitPos;
        }
        if (playerMove.StartTile)
        {
            var downInitPos = GameObject.FindWithTag("EndTile").transform.position;
            downInitPos.y += 2;
            player.transform.position = downInitPos;
        }
        playerMove.EndTile = false;
        playerMove.StartTile = false;
    } // ReSharper disable Unity.PerformanceAnalysis
    [Button]
    private void NextDungeon()
    {
        currentStage++;
        if (currentStage >= dungeons.Length)
        {
            currentStage = dungeons.Length - 1;
            Debug.Log("마지막 층입니다");
            playerMove.EndTile = false;
            Time.timeScale = 0;
            resultPopup.gameObject.SetActive(true);
            resultPopup.gameObject.GetComponentInChildren<TMP_Text>().text = "Total : " + playerMove.StepCount;
            return;
        }
        dungeons[currentStage - 1].SetActive(false);
        dungeons[currentStage].SetActive(true);
        PlayerPositionInitialize();
        // postStage = currentStage;
        // currentStage = nextStage;
        // nextStage = currentStage + 1;
        // if (currentStage > dungeons.Length)
        // {
        //     Debug.Log("마지막 층입니다");
        //     return;
        // }
        //
        // thisDungeons[postStage].gameObject.SetActive(false);
        // thisDungeons[currentStage].gameObject.SetActive(true);
    }

    [Button]
    private void PostDungeon()
    {
        currentStage--;
        if (currentStage < 0)
        {
            currentStage = 0;
            Debug.Log("마을로 돌아갑니다");
            playerMove.StartTile = false;
            return;
        }
        dungeons[currentStage + 1].SetActive(false);
        dungeons[currentStage].SetActive(true);
        PlayerPositionInitialize();
        // nextStage = currentStage;
        // currentStage = postStage;
        // postStage = currentStage - 1;
        // if (postStage < 0)
        // {
        //     Debug.Log("마을로 돌아갑니다");
        //     return;
        // }
        //
        //
        // thisDungeons[nextStage].gameObject.SetActive(false);
        // thisDungeons[currentStage].gameObject.SetActive(true);
    }
}