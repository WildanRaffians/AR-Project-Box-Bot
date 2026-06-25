using UnityEngine;
using UnityEngine.UI; // Wajib untuk Image
using TMPro; // Wajib untuk TextMeshPro
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Objects to Control")]
    public GameObject popupCubeFound;   // Objek Popup yang kita buat di Langkah 1
    public Image mascotImage;          // Komponen Image pada objek 'Mascot' kamu
    public TextMeshProUGUI dialogueText; // Komponen TextMeshPro pada objek 'Dialogue'
    public CanvasGroup dialogueCanvasGroup;
    public GameObject KotakMisi;
    public GameObject Misi1;
    public GameObject Misi2;
    public GameObject Misi3;
    public GameObject tapIndicator;


    [Header("Pop Up Ketemu Bangun Ruang")]
    public GameObject iconKubus;
    public GameObject iconPrisma;
    public GameObject iconBalok;
    public TextMeshProUGUI teksPopupFound;
    public GameObject tagBangunRuang;
    public TextMeshProUGUI teksTagBangunRuang;

    [Header("Settings Transisi")]
    public float fadeDuration = 0.1f;
    public float typingSpeed = 0.05f; // Semakin kecil semakin cepat
    private bool sedangMengetik = false;
    private Coroutine typingCoroutine;
    private Coroutine fadeCoroutine;

    [Header("Popup Completion")]
    public GameObject popupPanel;  //PopUp complite Level
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI messageText;

    [Header("Popup Pilih Level")]
    public GameObject popupPilihLevel;  //PopUp complite Level
    public TextMeshProUGUI titleTextPilihLevel;
    public TextMeshProUGUI messageTextPilihLevel;
    public Button level1;  //PopUp complite Level
    public Button level2;  //PopUp complite Level
    public Button level3;  //PopUp complite Level


    [Header("Lainnya")]
    public CanvasGroup popupCanvasGroup; // Gunakan skrip fade kita sebelumnya

    public GameObject tombolNextLevel;
    public Animator arpyAnim; // Tarik objek Arpy ke sini di Inspector Unity
    private Vector3 originalScale1;
    private Vector3 originalScale2;
    private Vector3 originalScale3;
    private Vector3 originalScale4;

    [Header("Fungsi Pengaturan")]
    public GameObject PopupPengaturan;
    public GameObject confirmOut;

    [Header("Bintang & Medali")]
    public StarAnimate[] bintangKuning; // Tarik ke-3 bintang kuning ke array ini
    public MedalShine medalScript;
    public GameObject MedaliOren;
    public GameObject MedaliSilver;
    public GameObject MedaliGold;
    
    [Header("Bintang & Medali Pilih Level")]
    public StarAnimate[] bintangKuningPilihLevel; // Tarik ke-3 bintang kuning ke array ini
    public GameObject MedaliOrenPilihLevel;
    public GameObject MedaliSilverPilihLevel;
    public GameObject MedaliGoldPilihLevel;

    void Start()
    {
        StartCoroutine(SequenceStart());
    }
    void Awake()
    {
        // Mengambil nilai scale dari objek popupPanel di Inspector saat game pertama kali berjalan
        originalScale1 = popupPanel.transform.localScale;
        originalScale2 = popupCubeFound.transform.localScale;
        originalScale3 = popupPilihLevel.transform.localScale;
        originalScale4 = PopupPengaturan.transform.localScale;
    }

    IEnumerator SequenceStart()
    {
        if(popupCubeFound != null) popupCubeFound.SetActive(false);
        popupPanel.SetActive(false);
        popupPilihLevel.SetActive(false);
        if(KotakMisi != null) KotakMisi.SetActive(false);
        if(Misi1 != null) Misi1.SetActive(false);
        if(Misi2 != null) Misi2.SetActive(false);
        if(Misi3 != null) Misi3.SetActive(false);
        if(dialogueText != null) dialogueText.enabled = false;
        
        yield return new WaitForSeconds(1.0f); 
        if(dialogueText != null) dialogueText.enabled = true;
        dialogueText.GetComponent<SimpleFade>().Appear();

    }
    
    public void OnCubeFound(string namaBangun)
    {
        // 1. Ambil kata pertama saja (pecah berdasarkan spasi, ambil urutan 0)
        // 2. Ubah huruf depannya menjadi kapital
        string kataPertama = namaBangun.Split(' ')[0];
        string namaKapital = kataPertama.ToUpper();

        // 1. Munculkan Popup di tengah
        if(popupCubeFound != null)
        {
            tagBangunRuang.SetActive(true);
            teksTagBangunRuang.text = namaKapital;
            teksPopupFound.fontSize = 58;
            teksPopupFound.text = "Mulai Level " + namaKapital + ".";
            if(namaBangun == "kubus")
            {
                iconKubus.SetActive(true);

                iconPrisma.SetActive(false);
                iconBalok.SetActive(false);
            } 
            else if(namaBangun == "balok")
            {
                iconBalok.SetActive(true);

                iconKubus.SetActive(false);
                iconPrisma.SetActive(false);
            }
            else if(namaBangun == "prisma segitiga")
            {
                iconPrisma.SetActive(true);

                iconKubus.SetActive(false);
                iconBalok.SetActive(false);
            }

            StartCoroutine(SequenceOnCubeFound()); // Durasi 0.5 detik
        }

        // 3. Ubah Teks Dialog
        arpyAnim.SetTrigger("doHU");
        SFXManager.Instance.MainkanArpyNoise(1);
        ShowCubeExplanation(namaKapital + " Ditemukan! Ayo mulai bermain.");
    }
    IEnumerator SequenceOnCubeFound()
    {
        popupCubeFound.SetActive(true);
            
        SFXManager.Instance.MainkanTransisiPop(); 
        StartCoroutine(ScalePopup(2, 0.5f)); // Durasi 0.5 detik
        yield return new WaitForSeconds(2f);
        TutupPopup();
        ShowCubeExplanation("Cari permukaan datar seperti meja atau lantai!\nTunggu sampai ada persegi putih di tengah, lalu ketuk layar.");
    }

    public void OnMulaiLevel(string namaBangun, int level)
    {
        SFXManager.Instance.MainkanTransisiPop();
        if(level == 4)
        {
            teksPopupFound.fontSize = 42;
            teksPopupFound.text = "Seluruh level " + namaBangun + " telah selesai!\n\n Kembali ke menu...";
        } else
        {
            teksPopupFound.fontSize = 58;
            teksPopupFound.text = "Memulai level " + level + " " + namaBangun + "...";
        }
        if(namaBangun == "kubus")
        {
            iconKubus.SetActive(true);

            iconBalok.SetActive(false);
            iconPrisma.SetActive(false);
        } 
        else if(namaBangun == "balok")
        {
            iconBalok.SetActive(true);

            iconPrisma.SetActive(false);
            iconKubus.SetActive(false);
        }
        else if(namaBangun == "prisma segitiga")
        {
            iconPrisma.SetActive(true);

            iconBalok.SetActive(false);
            iconKubus.SetActive(false);
        }

        StartCoroutine(SequenceOnMulaiLevel()); // Durasi 0.5 detik
    }
    IEnumerator SequenceOnMulaiLevel()
    {
        popupCubeFound.SetActive(true);
            
        SFXManager.Instance.MainkanTransisiPop(); 
        StartCoroutine(ScalePopup(2, 0.5f)); // Durasi 0.5 detik
        yield return new WaitForSeconds(2f);
        TutupPopup();
    }

    public void TutupPopup()
    {
        SFXManager.Instance.MainkanClick();
        StopAllCoroutines();
        StartCoroutine(HidePopup(2, 0.3f));
        // if(popupCubeFound != null) popupCubeFound.SetActive(false);
        Debug.Log("tutup!");
        
    }

    public void BukaKotakMisi()
    {
        if(KotakMisi != null) KotakMisi.SetActive(true);
        KotakMisi.GetComponent<SimpleFade>().Appear();
    }
    public void BukaMisi(int set)
    {
        if (set == 1)
        {
            if(Misi1 != null) Misi1.SetActive(true);
            Misi1.GetComponent<SimpleFade>().Appear();
        }
        else if (set == 2)
        {
            if(Misi2 != null) Misi2.SetActive(true);
            Misi2.GetComponent<SimpleFade>().Appear();
        }
        else if (set == 3)
        {
            if(Misi3 != null) Misi3.SetActive(true);
            Misi3.GetComponent<SimpleFade>().Appear();
        }
    }
    
    public void TutupMisi()
    {
        if(KotakMisi != null) KotakMisi.SetActive(false);
        if(Misi1 != null) Misi1.SetActive(false);
        if(Misi2 != null) Misi2.SetActive(false);
        if(Misi3 != null) Misi3.SetActive(false);
    }

    public void ShowCubeExplanation(string teks)
    {
        if (dialogueCanvasGroup.alpha == 0)
        {
            dialogueCanvasGroup.alpha = 1;
        }
        if (dialogueText != null && dialogueCanvasGroup != null)
        {
            fadeCoroutine = StartCoroutine(FadeRoutine(teks));
        }
    }

    IEnumerator FadeRoutine(string teksBaru)
    {
        // 1. Fade Out (Menghilang)
        while (dialogueCanvasGroup.alpha > 0)
        {
            dialogueCanvasGroup.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }

        // 2. Ganti Teks saat transparan
        dialogueText.text = teksBaru;

        // 3. Fade In (Muncul kembali)
        while (dialogueCanvasGroup.alpha < 1)
        {
            dialogueCanvasGroup.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
        
        dialogueCanvasGroup.alpha = 1;
    }

    public IEnumerator AnimasiDialog(string fullText)
    {
        sedangMengetik = true;
        dialogueText.text = "";

        // 1. Fade In Gelembung (Jika alpha masih 0)
        while (dialogueCanvasGroup.alpha < 1)
        {
            dialogueCanvasGroup.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }

        // 2. Efek Mengetik
        foreach (char huruf in fullText.ToCharArray())
        {
            dialogueText.text += huruf;
            yield return new WaitForSeconds(typingSpeed);
        }

        sedangMengetik = false;
    }

    public void ShowCompletionPopup(string judul, string pesan, int jumlahBintang)
    {
        SFXManager.Instance.MainkanTransisiPop();
        if (jumlahBintang > 0 && NotificationManager.Instance != null)
        {
            NotificationManager.Instance.TampilkanNotifikasi("Level Disimpan");
        }
        SFXManager.Instance.MainkanSukses();
        titleText.text = judul;
        messageText.text = pesan;

        // 1. Set skala ke 0 sebelum aktif
        popupPanel.transform.localScale = Vector3.zero;
        // Matikan semua bintang kuning dulu
        foreach(var s in bintangKuning) s.gameObject.SetActive(false);

        if(jumlahBintang == 2)
        {
            MedaliOren.SetActive(false);
            MedaliGold.SetActive(false);
            MedaliSilver.SetActive(true);
        }
        else if(jumlahBintang == 3)
        {
            MedaliGold.SetActive(true);
            MedaliOren.SetActive(false);
            MedaliSilver.SetActive(false);
        }
        else
        {
            MedaliOren.SetActive(true);
            MedaliSilver.SetActive(false);
            MedaliGold.SetActive(false);
        }

        // 2. Aktifkan Panel
        popupPanel.SetActive(true);

        // 3. Jalankan Animasi Scale dengan Efek Bounce
        StopAllCoroutines(); // Agar tidak tumpang tindih
        StartCoroutine(ScalePopup(1, 0.5f)); // Durasi 0.5 detik

        // Panggil animasi bintang secara berurutan
        for (int i = 0; i < jumlahBintang; i++)
        {
            // Berikan delay 0.5s (waktu muncul popup) + (0.2s antar bintang)
            bintangKuning[i].PlayAnim(0.5f + (i * 0.2f));
        }
    }
    
    public void ShowPopupPilihLevel(string namaBangun)
    {
        int levelTerakhir = SaveManager.Instance.AmbilLevelTertinggi(namaBangun);
        int jumlahBintangLevel = 0;

        SFXManager.Instance.MainkanTransisiPop();
        SFXManager.Instance.MainkanSukses();


        if(levelTerakhir == 4)
        {
            titleTextPilihLevel.text = "Level telah selesai";
            messageTextPilihLevel.text = "Kamu telah menyelesaikan semua level. Hebat!";

            jumlahBintangLevel = 3;
            MedaliGoldPilihLevel.SetActive(true);
            MedaliSilverPilihLevel.SetActive(false);
            MedaliOrenPilihLevel.SetActive(false);
        }
        else if(levelTerakhir == 3)
        {
            titleTextPilihLevel.text = "Level 3: Luas Permukaan";
            messageTextPilihLevel.text = "Kamu telah berhasil menyelesaikan 2 level. Kamu bisa memulai level 3!";
            jumlahBintangLevel = 2;
            MedaliGoldPilihLevel.SetActive(false);
            MedaliSilverPilihLevel.SetActive(true);
            MedaliOrenPilihLevel.SetActive(false);
        }
        else if(levelTerakhir == 2)
        {
            titleTextPilihLevel.text = "Level 2: Volume";
            messageTextPilihLevel.text = "Kamu telah berhasil menyelesaikan level 1. Kamu bisa memulai level 2!";
            
            jumlahBintangLevel = 1;
            MedaliGoldPilihLevel.SetActive(false);
            MedaliSilverPilihLevel.SetActive(false);
            MedaliOrenPilihLevel.SetActive(true);

            level3.interactable = false;
        }
        else{
            titleTextPilihLevel.text = "Level 1: Kerangka";
            messageTextPilihLevel.text = "Ini pertama kalinya kamu memulai level bangun ruang ini. Pilih level 1!";
            
            jumlahBintangLevel = 0;
            MedaliGoldPilihLevel.SetActive(false);
            MedaliSilverPilihLevel.SetActive(false);
            MedaliOrenPilihLevel.SetActive(false);
            
            level3.interactable = false;
            level2.interactable = false;
        }

        foreach(var s in bintangKuningPilihLevel) s.gameObject.SetActive(false);
        popupPilihLevel.transform.localScale = Vector3.zero;
        // 2. Aktifkan Panel
        popupPilihLevel.SetActive(true);

        // 3. Jalankan Animasi Scale dengan Efek Bounce
        StopAllCoroutines(); // Agar tidak tumpang tindih
        StartCoroutine(ScalePopup(3, 0.5f)); // Durasi 0.5 detik

        // Panggil animasi bintang secara berurutan
        for (int i = 0; i < jumlahBintangLevel; i++)
        {
            // Berikan delay 0.5s (waktu muncul popup) + (0.2s antar bintang)
            bintangKuningPilihLevel[i].PlayAnim(0.5f + (i * 0.2f));
        }
    }
    public void TutupPopupPilihLevel()
    {
        SFXManager.Instance.MainkanClick();
        StopAllCoroutines();
        StartCoroutine(HidePopup(3, 0.3f));        
    }

    public void TutupKomplitPopup(){
        StopAllCoroutines();
        StartCoroutine(HidePopup(1, 0.3f));
    }

    public void PopupTombolPengaturan()
    {
        if(confirmOut.activeSelf == true)
        {
            confirmOut.SetActive(false);
        }
        if (PopupPengaturan.activeSelf == true)
        {
            TutupPopupTombolPengaturan();
           
        }
        else
        {
            SFXManager.Instance.MainkanClick();
            PopupPengaturan.SetActive(true);
            StartCoroutine(ScalePopup(4, 0.5f)); // Durasi 0.5 detik
        }
    }
    public void TutupPopupTombolPengaturan()
    {
        SFXManager.Instance.MainkanClick();
        // Kembali ke Main Menu
        StopAllCoroutines();
        StartCoroutine(HidePopup(4, 0.3f));
    }
    public void TombolKembali()
    {
        SFXManager.Instance.MainkanClick();
        // Kembali ke Main Menu
        confirmOut.SetActive(true);
        TutupPopupTombolPengaturan();
        Debug.Log("KELUARARRARAR");
    }
    
    public void TombolConfirmOut()
    {
        SFXManager.Instance.MainkanClick();
        // Kembali ke Main Menu
        SceneManager.LoadScene("MainMenu");
        Debug.Log("KELUARARRARAR");
    }
    
    public void TutupConfirmOut()
    {
        SFXManager.Instance.MainkanClick();
        // Kembali ke Main Menu
        confirmOut.SetActive(false);
        PopupTombolPengaturan();
        Debug.Log("KELUARARRARAR");
    }

    IEnumerator ScalePopup(int set, float duration)
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

            if(set == 1)
            {
                popupPanel.transform.localScale = originalScale1 * bounceValue;
            } 
            else if(set == 2){
                popupCubeFound.transform.localScale = originalScale2 * bounceValue;
            } 
            else if(set == 3){
                popupPilihLevel.transform.localScale = originalScale3 * bounceValue;
            } 
            else if(set == 4){
                PopupPengaturan.transform.localScale = originalScale4 * bounceValue;
            } 

            yield return null;
        }

        // Pastikan skala akhir tepat
        if(set == 1)
        {   
            popupPanel.transform.localScale = originalScale1;
        } 
        else if(set == 2)
        {   
            popupCubeFound.transform.localScale = originalScale2;
        }
        else if(set == 3)
        {
            popupPilihLevel.transform.localScale = originalScale3;
        }
        else if(set == 3)
        {
            PopupPengaturan.transform.localScale = originalScale4;
        }
    }
    
    IEnumerator HidePopup(int set, float duration)
    {
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            
            // Animasi mengecil biasa (SmoothStep)
            if(set == 1)
            {
                Vector3 startScale1 = originalScale1;

                popupPanel.transform.localScale = Vector3.Lerp(startScale1, Vector3.zero, Mathf.SmoothStep(0, 1, t));
            } 
            else if(set == 2)
            {
                Vector3 startScale2 = originalScale2;
                popupCubeFound.transform.localScale = Vector3.Lerp(startScale2, Vector3.zero, Mathf.SmoothStep(0, 1, t));
            } 
            else if(set == 3)
            {
                Vector3 startScale3 = originalScale3;
                popupPilihLevel.transform.localScale = Vector3.Lerp(startScale3, Vector3.zero, Mathf.SmoothStep(0, 1, t));
            } 
            else if(set == 4)
            {
                Vector3 startScale3 = originalScale4;
                PopupPengaturan.transform.localScale = Vector3.Lerp(startScale3, Vector3.zero, Mathf.SmoothStep(0, 1, t));
            } 
            yield return null;
        }

        if(set == 1)
        {
            popupPanel.SetActive(false);
        } 
        else if( set == 2)
        {
            popupCubeFound.SetActive(false);
        }
        else if( set == 3)
        {
            popupPilihLevel.SetActive(false);
        }
        else if( set == 4)
        {
            PopupPengaturan.SetActive(false);
        }
        
    }

    IEnumerator FadePopup(float start, float end, float duration)
    {
        float elapsed = 0;
        popupCanvasGroup.alpha = start;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            popupCanvasGroup.alpha = Mathf.Lerp(start, end, elapsed / duration);
            yield return null;
        }
        popupCanvasGroup.alpha = end;
    }

    public void SetTapIndicator(bool status)
    {
        if(tapIndicator != null) tapIndicator.SetActive(status);
    }

    public void TutupDialog()
    {
        if (dialogueCanvasGroup.alpha >= 1)
        {
            dialogueCanvasGroup.alpha = 0;
        }
    }
    
    public void SetTombolNextLevel(bool status){
        if(tombolNextLevel != null) tombolNextLevel.SetActive(status);
    }
}