using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class SimpleTargetingLine : MonoBehaviour
{
    [Header("Line Settings")]
    public Transform startPos; // 선이 시작될 지점 (예: 플레이어 위치)
    [Range(1f, 10f)] public float extend = 2.0f; // 적을 얼마나 지나쳐 뻗을지 (LerpUnclamped의 t값)

    private LineRenderer lr;
    private Transform targetEnemy; // 현재 타겟팅된 적
    private bool isTargeting = false;

    void Awake()
    {
        // LineRenderer 기본 설정
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.widthMultiplier = 0.05f;
        lr.material = new Material(Shader.Find("Unlit/Color")) { color = Color.red };

        lr.enabled = false; // 시작할 때는 끈 상태
    }

    public void OnRightClick(InputValue value)
    {
        if (!value.isPressed) return;

        // 1. 이미 타겟팅 중이면 해제 (초기화)
        if (isTargeting)
        {
            ResetTargeting();
            return;
        }

        // 2. 우클릭으로 적 탐지
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                targetEnemy = hit.transform;
                isTargeting = true;
                lr.enabled = true; // 조준선 켜기
            }
        }
    }

    void Update()
    {
        // 타겟팅 중이 아니거나 적이 사라지면 선을 안 그림
        if (!isTargeting || targetEnemy == null)
        {
            if (lr.enabled) lr.enabled = false;
            return;
        }

        // --- 핵심: LerpUnclamped를 이용한 선 그리기 ---
        Vector3 a = startPos.position;
        Vector3 b = targetEnemy.position;

        // a에서 b 방향으로 가되, extend 비율만큼 뻗어나감 (1.0이면 적 위치까지, 2.0이면 두배 거리까지)
        Vector3 extendedPoint = Vector3.LerpUnclamped(a, b, extend);

        lr.SetPosition(0, a);
        lr.SetPosition(1, extendedPoint);
    }

    private void ResetTargeting()
    {
        isTargeting = false;
        targetEnemy = null;
        lr.enabled = false;
    }
}