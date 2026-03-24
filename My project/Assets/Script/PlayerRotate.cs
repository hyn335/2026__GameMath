using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotate : MonoBehaviour
{
    public float rotationSpeed = 100f;
    private Vector2 moveInput;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

    }


    
    void Update()
    {
        Quaternion rotation = Quaternion.Euler(0f,moveInput.x * rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = rotation * transform.rotation;
    }
}
