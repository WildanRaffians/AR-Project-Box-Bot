using UnityEngine;
using System.Collections;

public class CubeAnimation : MonoBehaviour
{
    [Header("Settings Durasi")]
    public float durasiIn = 0.6f;       // Kecepatan muncul
    public float durasiOut = 0.5f;      // Kecepatan mengecil/hilang
    
    [Header("Settings Posisi & Skala")]
    public float offsetBawah = 0.3f;    // Jarak muncul dari arah bawah
    public Vector3 targetScale = Vector3.one; // Ukuran akhir kubus

    private Vector3 posisiAkhir;
    private bool sudahInisialisasi = false;

    void Awake()
    {
        // Jika objek sudah ada di Hierarchy sejak awal game, 
        // kita langsung kunci posisinya di sini.
        if (!sudahInisialisasi)
        {
            InisialisasiPosisi();
        }
    }

    // FUNGSI BARU: Untuk mengunci posisi target objek secara dinamis
    public void InisialisasiPosisi()
    {
        posisiAkhir = transform.localPosition;
        sudahInisialisasi = true;
    }

    // Fungsi untuk memicu animasi MUNCUL
    public void MunculkanKubus()
    {
        if (!sudahInisialisasi) InisialisasiPosisi();
        StopAllCoroutines(); 
        StartCoroutine(AnimateShow());
    }

    // Fungsi untuk memicu animasi MENGECIL & MENGHILANG
    public void HilangkanKubus(System.Action onComplete = null)
    {
        if (!sudahInisialisasi) InisialisasiPosisi();
        StopAllCoroutines();
        StartCoroutine(AnimateHide(onComplete));
    }

    IEnumerator AnimateShow()
    {
        // Mulai dari posisi agak ke bawah dan skala nol
        // transform.localPosition = posisiAkhir + new Vector3(0, -offsetBawah, 0);
        transform.localScale = Vector3.zero;

        float timer = 0;
        while (timer < durasiIn)
        {
            timer += Time.deltaTime;
            float progress = Mathf.SmoothStep(0, 1, timer / durasiIn);

            // Interpolasi posisi dan skala secara bersamaan
            // transform.localPosition = Vector3.Lerp(posisiAkhir + new Vector3(0, -offsetBawah, 0), posisiAkhir, progress);
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, progress);
            yield return null; 
        }

        // Pastikan posisi akhir presisi
        // transform.localPosition = posisiAkhir;
        transform.localScale = targetScale;
    }

    IEnumerator AnimateHide(System.Action onComplete)
    {
        float timer = 0;
        Vector3 skalaAwalSaatIni = transform.localScale;

        while (timer < durasiOut)
        {
            timer += Time.deltaTime;
            float progress = Mathf.SmoothStep(0, 1, timer / durasiOut);

            // Mengecilkan skala menuju nol
            transform.localScale = Vector3.Lerp(skalaAwalSaatIni, Vector3.zero, progress);
            yield return null;
        }

        transform.localScale = Vector3.zero;
        
        // Nonaktifkan objek setelah animasi selesai
        gameObject.SetActive(false);

        // Kembalikan skala internal ke target asli agar saat di-SetActive(true) lagi tidak bernilai 0
        transform.localScale = targetScale;

        // Jalankan logika lanjutan dari LevelManager jika ada
        if (onComplete != null) onComplete();
    }
}