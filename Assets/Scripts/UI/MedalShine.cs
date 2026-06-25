using UnityEngine;
using System.Collections;

public class MedalShine : MonoBehaviour
{
    public RectTransform shineOverlay; // Tarik objek ShineEffect ke sini
    public float intervalGlow = 4f;   // Berapa detik sekali dia mengkilap

    void OnEnable()
    {
        StartCoroutine(ShineRoutine());
    }

    IEnumerator ShineRoutine()
    {
        while (true)
        {
            float timer = 0;
            float durasi = 0.8f;
            
            // Posisi awal di kiri medali
            Vector3 posisiAwal = new Vector3(-150, 0, 0); 
            // Posisi akhir di kanan medali
            Vector3 posisiAkhir = new Vector3(150, 0, 0);

            while (timer < durasi)
            {
                timer += Time.deltaTime;
                shineOverlay.anchoredPosition = Vector3.Lerp(posisiAwal, posisiAkhir, timer / durasi);
                yield return null;
            }

            yield return new WaitForSeconds(intervalGlow);
        }
    }
}