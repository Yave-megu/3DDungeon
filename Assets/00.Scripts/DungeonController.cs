using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DungeonController : MonoBehaviour
{

    public GameObject[] dungeons;

    [FormerlySerializedAs("PlayerPArty")]
    [FormerlySerializedAs("player")]
    public GameObject PlayerParty;

    public TMP_Text textMesh;

    public GameObject resultPopup;
    // public GameObject PlayerParty;

    private MovePlayerParty _movePlayerParty;
    private PlayerParty _playerParty;

    private int currentStage;

    private int postStage;
    private GameObject[] thisDungeons;

    public GameObject Instance { get; set; }

    public void Awake()
    {
        Instance = gameObject;
        PlayerParty = Instantiate(PlayerParty);

        thisDungeons = new GameObject[dungeons.Length];
        for (int i = 0; i < dungeons.Length; i++)
        {
            thisDungeons[i] = Instantiate(dungeons[i]);
            thisDungeons[i].SetActive(false);
            if (i == 0)
            {
                thisDungeons[i].SetActive(true);
                currentStage = i;
            }
        }

        resultPopup.SetActive(false);


    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _movePlayerParty = PlayerParty.GetComponent<MovePlayerParty>();
        _playerParty = PlayerParty.GetComponent<PlayerParty>();
        textMesh.text = "Total Step: " + _movePlayerParty.Stamina;
        PlayerPositionInitialize();

    }

    // Update is called once per frame
    private void Update()
    {
        textMesh.text = $"Stamina {_movePlayerParty.Stamina} / {_movePlayerParty.MaxStamina}";
        if (_movePlayerParty.EndTile)
        {
            NextDungeon();
        }
        if (_movePlayerParty.StartTile)
        {
            PostDungeon();
        }
    }
    private void PlayerPositionInitialize()
    {
        if (_movePlayerParty.StartTile is false && _movePlayerParty.EndTile is false)
        {
            var inintPos = GameObject.FindWithTag("StartTile").transform.position;
            inintPos.y += 2;
            PlayerParty.transform.position = inintPos;
        }


        if (_movePlayerParty.EndTile)
        {
            var upInitPos = GameObject.FindWithTag("StartTile").transform.position;
            upInitPos.y += 2;
            PlayerParty.transform.position = upInitPos;
        }
        if (_movePlayerParty.StartTile)
        {
            var downInitPos = GameObject.FindWithTag("EndTile").transform.position;
            downInitPos.y += 2;
            PlayerParty.transform.position = downInitPos;
        }
        _movePlayerParty.EndTile = false;
        _movePlayerParty.StartTile = false;
    } // ReSharper disable Unity.PerformanceAnalysis
    [Button]
    private void NextDungeon()
    {
        currentStage++;
        if (currentStage >= thisDungeons.Length)
        {
            currentStage = thisDungeons.Length - 1;
            Debug.Log("마지막 층입니다");
            _movePlayerParty.EndTile = false;
            Time.timeScale = 0;
            resultPopup.gameObject.SetActive(true);
            resultPopup.gameObject.GetComponentInChildren<TMP_Text>().text = "Total : " + _movePlayerParty.Stamina;
            return;
        }
        thisDungeons[currentStage - 1].SetActive(false);
        thisDungeons[currentStage].SetActive(true);
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
            _movePlayerParty.StartTile = false;
            return;
        }
        thisDungeons[currentStage + 1].SetActive(false);
        thisDungeons[currentStage].SetActive(true);
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