using UnityEngine;
using System.Collections;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [Header("Referensi UI")]
    public RectTransform panelNotifikasi; // Seret objek PanelNotifikasi ke sini
    public TextMeshProUGUI teksNotifikasi; // Referensi teks jika teksnya dinamis

    [Header("Pengaturan Posisi Y")]
    public float yTersembunyi = 150f;  // Posisi Y saat berada di luar atas layar
    public float yMuncul = -50f;       // Posisi Y saat muncul di layar (sesuai gambarmu)

    [Header("Pengaturan Durasi")]
    public float durasiSlide = 0.3f;   // Kecepatan meluncur (detik)
    public float durasiTampil = 2.0f;  // Berapa lama notifikasi diam di layar

    private Vector2 posAwal;
    private bool sedangAnimasi = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Set posisi awal di luar layar saat game mulai
        panelNotifikasi.anchoredPosition = new Vector2(panelNotifikasi.anchoredPosition.x, yTersembunyi);
    }

    // Fungsi utama yang bisa dipanggil dari script lain
    public void TampilkanNotifikasi(string pesan)
    {
        if (teksNotifikasi != null) teksNotifikasi.text = pesan;

        // Jika sedang berjalan, hentikan dulu lalu ulangi dari awal
        if (sedangAnimasi) StopAllCoroutines();
        
        StartCoroutine(SequenceSlideNotifikasi());
    }

    IEnumerator SequenceSlideNotifikasi()
    {
        sedangAnimasi = true;
        float waktuTerpakai = 0f;

        Vector2 posisiLuar = new Vector2(panelNotifikasi.anchoredPosition.x, yTersembunyi);
        Vector2 posisiTarget = new Vector2(panelNotifikasi.anchoredPosition.x, yMuncul);

        SFXManager.Instance.MainkanTransisiPop();

        // 1. ANIMASI SLIDE KE BAWAH (MUNCUL)
        while (waktuTerpakai < durasiSlide)
        {
            waktuTerpakai += Time.deltaTime;
            float persentase = waktuTerpakai / durasiSlide;
            
            // Menggunakan Lerp dengan rumus Sinus agar gerakannya halus (Smooth) di akhir
            float t = Mathf.Sin(persentase * Mathf.PI * 0.5f); 
            panelNotifikasi.anchoredPosition = Vector2.Lerp(posisiLuar, posisiTarget, t);
            
            yield return null;
        }
        panelNotifikasi.anchoredPosition = posisiTarget;

        // 2. DIAM DI LAYAR
        yield return new WaitForSeconds(durasiTampil);

        // 3. ANIMASI SLIDE KE ATAS (SEMBUNYI KEMBALI)
        waktuTerpakai = 0f;
        while (waktuTerpakai < durasiSlide)
        {
            waktuTerpakai += Time.deltaTime;
            float persentase = waktuTerpakai / durasiSlide;
            
            // Gerakan berbalik ke atas
            float t = 1f - Mathf.Cos(persentase * Mathf.PI * 0.5f);
            panelNotifikasi.anchoredPosition = Vector2.Lerp(posisiTarget, posisiLuar, t);
            
            yield return null;
        }
        panelNotifikasi.anchoredPosition = posisiLuar;
        sedangAnimasi = false;
    }
}