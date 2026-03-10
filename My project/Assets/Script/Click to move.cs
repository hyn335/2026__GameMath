using UnityEngine;
using UnityEngine.InputSystem;

public class Clicktomove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;

    private Vector2 mouseScreenPosition;
    private Vector3 targetPosition;

    private bool isMoving = false;
    private bool isSprinting = false;

    public void OnPoint(InputValue value)
    {
        mouseScreenPosition = value.Get<Vector2>(); // 마우스 위치 업데이트
    }

    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            RaycastHit[] hits = Physics.RaycastAll(ray); // 레이저 경로에 있는 모든 물체를 탐색

            foreach (RaycastHit hit in hits) // 모든 물체에 한해 반복
            {
                if (hit.collider.gameObject != gameObject) // 부딪힌 물체가 나 자신이 아닐 때만
                {

                    targetPosition = hit.point; // Plane에 부딧힌 지점을 타겟
                    targetPosition.y = transform.position.y;
                    isMoving = true;

                    break; // 탐색 했으니 foreach 반복 중단

                }
            }
        }
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed; // 버튼을 누르고 있으면 true, 떼면 false
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Vector3 dir = targetPosition - transform.position;

            // 벡터 길이 직접 계산
            float length = Mathf.Sqrt(dir.x * dir.x + dir.z * dir.z);

            // 속도 계산
            float speed = moveSpeed;
            if (isSprinting)
            {
                speed *= sprintMultiplier;
            }

            // 정규화 후 이동
            transform.position += new Vector3(
                dir.x / length,
                0,
                dir.z / length
            ) * speed * Time.deltaTime;

            if (length < 0.1f)
            {
                isMoving = false;
            }
        }
    }
}
