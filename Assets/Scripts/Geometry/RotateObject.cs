using UnityEngine;
using UnityEngine.InputSystem;

public class RotateObject : MonoBehaviour
{
    [Header("Settings")]
    public float rotationSpeed = 0.15f;
    
    // Variabel ini bisa kamu matikan/hidupkan dari LevelManager
    public bool canRotate = false; 

    private bool isDragging = false;

    void Update()
    {
        // 1. Cek apakah fitur rotasi sedang diizinkan (misal hanya di Misi Sisi)
        if (!canRotate || Pointer.current == null) return;

        // 2. Deteksi Mulai Sentuh
        if (Pointer.current.press.wasPressedThisFrame)
        {
            isDragging = true;
        }

        // 3. Deteksi Lepas Sentuh
        if (Pointer.current.press.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        // 4. Logika Rotasi saat Digeser
        if (isDragging)
        {
            Vector2 delta = Pointer.current.delta.ReadValue();

            // Rotasi horizontal (mengelilingi sumbu Y dunia)
            transform.Rotate(Vector3.up, -delta.x * rotationSpeed, Space.World);
            
            // Rotasi vertikal (mengelilingi sumbu kanan kamera agar lebih intuitif)
            transform.Rotate(Camera.main.transform.right, delta.y * rotationSpeed, Space.World);
        }
    }
}