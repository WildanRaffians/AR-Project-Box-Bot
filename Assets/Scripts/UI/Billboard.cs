using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camTransform;

    void Start()
    {
        camTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        // Membuat Canvas selalu menghadap kamera
        transform.LookAt(transform.position + camTransform.forward);
    }
}