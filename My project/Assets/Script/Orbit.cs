using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target;   // 중심 (태양 or 지구)
    public float distance = 5f;
    public float speed = 1f;

    private float angle = 0f;

    void Update()
    {
        angle += speed * Time.deltaTime;

        float x = Mathf.Cos(angle) * distance;
        float z = Mathf.Sin(angle) * distance;

        transform.position = target.position + new Vector3(x, 0, z);
    }
}