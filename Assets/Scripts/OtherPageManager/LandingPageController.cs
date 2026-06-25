using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LandingPageController : MonoBehaviour
{
    [Header("Referensi")]
    public Animator arpyAnimator;       // Tarik objek Arpy yang punya komponen Animator ke sini
   
    [Header("Pengaturan Waktu")]
    [Tooltip("Sesuaikan dengan panjang durasi clip animasi doYeay kamu (dalam detik)")]
    public float durasiAnimasi = 2.5f; 

    void Start()
    {
        if (arpyAnimator != null)
        {
            StartCoroutine(AlurLandingPage());
        }
        else
        {
            Debug.LogWarning("Animator Arpy belum dipasang! Langsung pindah scene.");
            PindahKeMainMenu();
        }
    }

    IEnumerator AlurLandingPage()
    {
        // 1. Jalankan animasi doYeay
        arpyAnimator.SetTrigger("doHU");

        // 2. Tunggu sampai animasi selesai berjalan
        yield return new WaitForSeconds(durasiAnimasi);

        // 3. Pindah ke Scene Main Menu
        PindahKeMainMenu();
    }

    void PindahKeMainMenu()
    {
        if (SaveManager.Instance.AmbilNama() != "")
        {
            // Jika sudah ada nama, langsung lompat ke Main Menu tanpa basa-basi
            SceneTransition.Instance.PindahScene("MainMenu");
            return;
        }
        else
        {
            SceneTransition.Instance.PindahScene("LoginPage");
        }
    }
}