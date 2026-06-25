using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [Header("Referensi UI")]
    public CanvasGroup fadeCanvasGroup; // Pasang Canvas Group dari panel hitam
    public float durasiFade = 0.5f;

    void Awake()
    {
        // Membuat objek ini menjadi Singleton (Abadi antar Scene)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Fungsi utama yang dipanggil untuk pindah scene dengan animasi mulus
    public void PindahScene(string namaSceneTujuan)
    {
        StartCoroutine(MulaiTransisi(namaSceneTujuan));
    }

    IEnumerator MulaiTransisi(string namaSceneTujuan)
    {
        // 1. FADE IN (Layar perlahan menutup jadi hitam)
        float timer = 0;
        fadeCanvasGroup.blocksRaycasts = true; // Mencegah user klik tombol lain saat transisi

        while (timer < durasiFade)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0, 1, timer / durasiFade);
            yield return null;
        }
        fadeCanvasGroup.alpha = 1;

        // 2. PINDAH SCENE DI BELAKANG LAYAR
        AsyncOperation operasiSelesai = SceneManager.LoadSceneAsync(namaSceneTujuan);
        while (!operasiSelesai.isDone)
        {
            yield return null; // Tunggu sampai scene tujuan selesai dimuat
        }

        // 3. FADE OUT (Layar perlahan membuka dari hitam ke scene baru)
        timer = 0;
        while (timer < durasiFade)
        {
            timer += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1, 0, timer / durasiFade);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0;
        fadeCanvasGroup.blocksRaycasts = false; // Izinkan user klik lagi di scene baru
    }
}