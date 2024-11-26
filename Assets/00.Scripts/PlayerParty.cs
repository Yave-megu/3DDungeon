using UnityEngine;

public class PlayerParty : MonoBehaviour
{
    public int Stamina;
    public int MaxStamina;
    public GameObject[] partyMemberPrefabs;
    public GameObject[] partyMembersInstance;
    private int _partyIndex;
    private GameObject instacne;
    private int partyCount;
    public static GameObject Instance { get; set; }


    private void Awake()
    {
        Stamina = MaxStamina;
    }

    private void Start()
    {

        var pos = GameObject.FindGameObjectWithTag("StartTile").transform.position;
        pos.y = 3.5f;
        transform.position = pos;

        pos.y -= 1;
        partyMembersInstance = new GameObject[partyMemberPrefabs.Length];
        for (int i = 0; i < partyMemberPrefabs.Length; i++)
        {
            partyMembersInstance[i] = Instantiate(partyMemberPrefabs[i], pos, Quaternion.identity);
            partyMembersInstance[i].transform.parent = transform;
            partyMembersInstance[i].SetActive(false);
        }
        partyMembersInstance[_partyIndex].SetActive(true);
        MaxStamina = 150;
        Stamina = MaxStamina;
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
        else if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Open Inventory");
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("Open Belt");
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Open Character");
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Open Map");
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Open Menu");
        }
    }
}