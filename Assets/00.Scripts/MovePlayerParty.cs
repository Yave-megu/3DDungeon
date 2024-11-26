using System.Collections;
using UnityEngine;

public class MovePlayerParty : MonoBehaviour
{
    public float moveDistance = 4f; // 한 번 이동할 거리
    public float moveSpeed = 5f; // 이동 속도
    public float rotationSpeed = 300f; // 회전 속도 (각도/초)
    public float raycastDistance = 2.5f; //탐색 범위

    private PlayerParty _playerParty;
    private bool isMoving; // 현재 움직이는 중인지 확인

    private bool isRotating; // 현재 회전 중인지 확인
    public bool EndTile { get; set; }
    public bool StartTile { get; set; }
    public int Stamina { get; set; }
    public int MaxStamina { get; set; }

    private void Awake() { }

    private void Start()
    {

        _playerParty = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerParty>();
        EndTile = false;
        StartTile = false;
        MaxStamina = _playerParty.MaxStamina;
        Stamina = MaxStamina;
    }

    private void Update()
    {
        // W 키로 이동
        if (Input.GetKeyDown(KeyCode.W) && !isMoving && !isRotating)
        {
            if (CanMoveForward())
            {
                var forward = transform.forward; // 현재 캐릭터의 전방
                StartCoroutine(Move(forward));
                if (CheckOnBlock().collider.CompareTag("TrapTile"))
                {
                    Stamina -= 20;
                }
                else if (CheckOnBlock().collider.CompareTag("MonsterTile"))
                {
                    Stamina -= 30;
                }
                else if (CheckOnBlock().collider.CompareTag("RewardTile"))
                {
                    Stamina += 20;
                    if (Stamina > MaxStamina)
                    {
                        Stamina = MaxStamina;
                    }
                }
                else
                {
                    Stamina--;
                }
                _playerParty.Stamina = Stamina;

            }

        }

        // A 키로 왼쪽으로 회전
        if (Input.GetKeyDown(KeyCode.A) && !isRotating && !isMoving)
        {
            StartCoroutine(Rotate(-90f)); // -90도 회전
        }

        // D 키로 오른쪽으로 회전
        if (Input.GetKeyDown(KeyCode.D) && !isRotating && !isMoving)
        {
            StartCoroutine(Rotate(90f)); // 90도 회전
        }

        // S 키로 뒤로 회전
        if (Input.GetKeyDown(KeyCode.S) && !isRotating && !isMoving)
        {
            StartCoroutine(Rotate(180f)); // 180도 회전
        }
        if (Input.GetKeyDown(KeyCode.F) && !isRotating && !isMoving) //Interact key
        {
            var temp = CheckOnBlock();
            if (temp.collider.CompareTag("EndTile"))
            {
                EndTile = true;
            }
            if (temp.collider.CompareTag("StartTile"))
            {
                StartTile = true;
            }
        }

    }



    // 일정 거리 이동
    private IEnumerator Move(Vector3 direction)
    {
        isMoving = true;

        var startPosition = transform.position; // 시작 위치
        var targetPosition = startPosition + direction.normalized * moveDistance; // 목표 위치

        float distanceMoved = 0f;
        while (distanceMoved < moveDistance)
        {
            float moveStep = moveSpeed * Time.deltaTime; // 한 프레임당 이동 거리
            distanceMoved += moveStep;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveStep);
            yield return null; // 다음 프레임까지 대기
        }
        // Debug.Log("target: "+targetPosition);
        transform.position = targetPosition; // 정확한 위치로 보정
        isMoving = false;
    }

    // 부드럽게 회전
    private IEnumerator Rotate(float angle)
    {
        isRotating = true;

        float rotationDone = 0f;
        while (Mathf.Abs(rotationDone) < Mathf.Abs(angle))
        {
            float rotationStep = rotationSpeed * Time.deltaTime; // 한 프레임당 회전량
            float rotationThisFrame = Mathf.Sign(angle) * Mathf.Min(rotationStep, Mathf.Abs(angle - rotationDone));
            transform.Rotate(Vector3.up, rotationThisFrame);
            rotationDone += rotationThisFrame;
            yield return null; // 다음 프레임까지 대기
        }

        // 정확한 각도로 보정
        float totalRotation = Mathf.Round(transform.eulerAngles.y / 90f) * 90f;
        transform.rotation = Quaternion.Euler(0, totalRotation, 0);

        isRotating = false;
    }

    private bool CanMoveForward()
    {
        RaycastHit hit;
        var rayOrigin = transform.position + Vector3.up * 0.5f; // 캐릭터 높이의 중간에서 Ray 시작
        var rayDirection = transform.forward; // 전방으로 Ray 쏘기

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, raycastDistance))
        {
            // Ray가 충돌했다면, 이동 불가
            Debug.Log($"앞에 장애물 있음: {hit.collider.name}");
            return false;
        }

        // 충돌이 없으면 이동 가능
        return true;
    }
    private RaycastHit CheckOnBlock()
    {
        RaycastHit hit;
        var rayOrigin = transform.position + Vector3.up * 0.5f; // 캐릭터 높이의 중간에서 Ray 시작
        var rayDirection = Vector3.down; // 전방으로 Ray 쏘기

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, raycastDistance))
        {
            // Ray가 충돌했다면, 이동 불가
            Debug.Log($"타일 위에 있음: {hit.collider.name}");
            if (hit.collider.CompareTag("EndTile"))
            {

                Debug.Log("Clear");
            }
            if (hit.collider.CompareTag("StartTile"))
            {

                Debug.Log("Start");
            }
        }
        return hit;

        // 충돌이 없으면 이동 가능
    }
}