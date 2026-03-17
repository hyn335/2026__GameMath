using UnityEngine;

public class DotExample : MonoBehaviour
{
    public Transform player;

    public float viewAngle = 60f; // 시야각


    
    void Update()
    {
        Vector3 toplayer = player.position - transform.position; // 플레이어를 보는 방향
        Vector3 forward = transform.forward; //적의 앞방향z+
        
        float dot = Vector3.Dot(forward, toplayer);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg; //내적을 각도로 변환

        if (angle < viewAngle / 2)
        {
            Debug.Log("플레이어가 적의 시야에 있음");
        }


    }
}
