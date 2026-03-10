using UnityEngine;
using UnityEngine.InputSystem;


public class NewMonoBehaviourScript : MonoBehaviour
{

    public float moveSpeed = 5f;
    private Vector2 moveInput;

    Vector3 normalizedVector;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>(); 
    }
    
  
    void Update()
    {
        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0);

        float sqrMagnitude = direction.x * direction.x + direction.y * direction.z * direction.z;
        float manitude = Mathf.Sqrt(sqrMagnitude);

        // (0으로 나누기 방지) 
        if (manitude > 0)
            normalizedVector = direction / manitude;
        else normalizedVector = Vector3.zero;

            transform.Translate(direction * moveSpeed * Time.deltaTime);
    }
}
