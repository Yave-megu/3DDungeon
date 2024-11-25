using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    public GameObject[] partyMembersInstance;
    private int _partyIndex;
    private GameObject instacne;
    private int partyCount;
    public static int Stamina { get; set; }
    public static int MaxStamina { get; set; }
    public static GameObject Instance { get; set; }


    private void Awake()
    {
        Instance = gameObject;
        MaxStamina = 150;
        Stamina = MaxStamina;
    }
    private void Start()
    {

        var pos = GameObject.FindGameObjectWithTag("StartTile").transform.position;
        pos.y = 3.5f;
        transform.position = pos;
        partyCount = transform.childCount - 1;
        partyMembersInstance = new GameObject[partyCount];
        for (int i = 0; i < partyCount; i++)
        {
            partyMembersInstance[i] = transform.GetChild(i).gameObject;
            partyMembersInstance[i].SetActive(false);
        }
        partyMembersInstance[_partyIndex].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            partyMembersInstance[_partyIndex].SetActive(false);
            _partyIndex = _partyIndex - 1 < 0 ? partyMembersInstance.Length - 1 : _partyIndex - 1;
            partyMembersInstance[_partyIndex].SetActive(true);
            Debug.Log($"Change {partyMembersInstance[_partyIndex].name}");
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            partyMembersInstance[_partyIndex].SetActive(false);
            _partyIndex = (_partyIndex + 1) % partyMembersInstance.Length;
            partyMembersInstance[_partyIndex].SetActive(true);
            Debug.Log($"Change {partyMembersInstance[_partyIndex].name}");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"Use {partyMembersInstance[_partyIndex].name} skill");
        }
    }
}