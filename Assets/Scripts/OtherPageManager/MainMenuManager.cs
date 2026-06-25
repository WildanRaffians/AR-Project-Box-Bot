using UnityEngine;
using UnityEngine.SceneManagement; // Penting untuk pindah scene
using System.Collections;
using TMPro; // Gunakan ini jika Input Field kamu memakai TextMeshPro

public class MainMenuManager : MonoBehaviour
{
    // Nama scene level AR kamu (cek di Build Settings)
    public Animator anim;
    public GameObject tombolMenu;
    public GameObject PopupConfirmOut;
    [SerializeField] private float intervalAnimasi = 15f;
    private Vector3 originalScale1;
    public TextMeshProUGUI textArpy;
    public float typingSpeed = 0.05f; // Semakin kecil semakin cepat
    private bool sedangMengetik = false;

    void Start()
    {        
        // Memulai perulangan animasi
        anim.SetTrigger("doHai");
        SFXManager.Instance.MainkanArpyNoise(3);
        if (anim != null)
        {
            StartCoroutine(RoutineHallo());
        }

        StartCoroutine(SequenceStart());

    }
    IEnumerator SequenceStart()
    {
        // Mengambil nama dari memori HP
        string nama = SaveManager.Instance.pemainAktif.nama_siswa;
        
        yield return StartCoroutine(AnimasiDialog("Halo, " + nama + "!"));
        yield return new WaitForSeconds(3.0f);
        anim.SetTrigger("doHu");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(AnimasiDialog("Ayo bermain!"));
    }
    
    void Awake()
    {
        // Mengambil nilai scale dari objek popupPanel di Inspector saat game pertama kali berjalan
        originalScale1 = PopupConfirmOut.transform.localScale;
    }

    IEnumerator RoutineHallo()
    {
        // Loop ini akan berjalan selama objek Arpy aktif
        while (true)
        {
            // Menunggu selama waktu yang ditentukan
            yield return new WaitForSeconds(intervalAnimasi);

            // Memicu animasi Hallo
            anim.SetTrigger("doHai");
            SFXManager.Instance.MainkanArpyNoise(2);
        }
    }
    public void TombolMulai()
    {
        SFXManager.Instance.MainkanClick();
        if (anim != null) 
        {
            anim.SetTrigger("doHU");
        }
        // Memastikan scene yang dimuat adalah scene yang sudah ada di Build List
        SceneTransition.Instance.PindahScene("DeteksiBangunRuang");
    }

    public void TombolPengaturan()
    {
        SFXManager.Instance.MainkanClick();
        Debug.Log("Membuka Menu Pengaturan...");
        SceneTransition.Instance.PindahScene("Pengaturan");

    }
    
    public void TombolPencapaian()
    {
        SFXManager.Instance.MainkanClick();
        SceneTransition.Instance.PindahScene("Pencapaian");
    }
    

    public void TombolKeluar()
    {
        SFXManager.Instance.MainkanClick();
        Debug.Log("Aplikasi Keluar");
        tombolMenu.SetActive(false);
        PopupConfirmOut.SetActive(true);
        StartCoroutine(ScalePopup(0.5f)); // Durasi 0.5 detik
    }
    
    public void ConfirmOut()
    {
        SFXManager.Instance.MainkanClick();
        Debug.Log("Aplikasi Keluar");
        Application.Quit();
    }
    public void KembaliKeLogin()
    {
        SFXManager.Instance.MainkanClick();
        SceneTransition.Instance.PindahScene("LoginPage");

    }
    
    public void TutupConfirmOut()
    {
        SFXManager.Instance.MainkanClick();
        Debug.Log("Aplikasi Keluar");
        tombolMenu.SetActive(true);
        PopupConfirmOut.SetActive(false);
        
    }
    IEnumerator ScalePopup(float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            // RUMUS BOUNCE (Back Out Easing)
            // Objek akan membesar sampai ~1.1 lalu kembali ke 1.0 secara smooth
            float s = 1.70158f; // Kekuatan pantulan (semakin besar semakin membal)
            t = t - 1;
            float bounceValue = t * t * ((s + 1) * t + s) + 1;

                PopupConfirmOut.transform.localScale = originalScale1 * bounceValue;
            

            yield return null;
        }

        // Pastikan skala akhir tepat
            PopupConfirmOut.transform.localScale = originalScale1;
        
    }

    IEnumerator HidePopup(float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            Vector3 startScale1 = originalScale1;

            PopupConfirmOut.transform.localScale = Vector3.Lerp(startScale1, Vector3.zero, Mathf.SmoothStep(0, 1, t));
            
            yield return null;
        }

        PopupConfirmOut.SetActive(false);
        
    }

    public IEnumerator AnimasiDialog(string fullText)
    {
        sedangMengetik = true;
        textArpy.text = "";

        // 2. Efek Mengetik
        foreach (char huruf in fullText.ToCharArray())
        {
            textArpy.text += huruf;
            yield return new WaitForSeconds(typingSpeed);
        }

        sedangMengetik = false;
    }
}