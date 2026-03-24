using UnityEngine;

// Unity 스크립트 | 참조 0개
public class ForwardVisualizer : MonoBehaviour
{
    public float rayLength = 2.0f;
    public Color gizmoColor = Color.blue;

    // Unity 메시지 | 참조 0개
    private void OnDrawGizmos()
    {
        DrawForwardRay();
    }

    // 참조 1개
    private void DrawForwardRay()
    {
        Vector3 startPos = transform.position;
        Vector3 forwardDir = transform.forward * rayLength;
        Vector3 endPos = startPos + forwardDir;

        Gizmos.color = gizmoColor;
        Gizmos.DrawRay(startPos, forwardDir);
    }
}
