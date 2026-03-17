using UnityEngine;
using TMPro;

public class SimpleAngleLauncher : MonoBehaviour
{
    public TMP_InputField angleInputField;
    public GameObject spherePrefab;
    public Transform firePoint;
    public float force = 15f;

    public void Lanuch()
    {
        float angle = float.Parse(angleInputField.text);
        float rad = angle * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));

        GameObject Sphere = Instantiate(spherePrefab, firePoint.position, Quaternion.identity);
        Rigidbody rb = Sphere.GetComponent<Rigidbody>();

        rb.AddForce((dir + Vector3.up * 0.3f).normalized * force, ForceMode.Impulse);
    }
}
