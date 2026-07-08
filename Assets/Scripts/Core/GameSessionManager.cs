using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameSessionManager : MonoBehaviour
{
    // --- SINGLETON (BANK DATA GLOBAL) ---
    public static GameSessionManager Instance { get; private set; }

    [Header("Referensi Manajer Level")]
    public MonoBehaviour level1Manager; // Nanti kita ganti tipe datanya setelah scriptnya dibuat
    public MonoBehaviour level2Manager;
    public MonoBehaviour level3Manager;

    [Header("UI & Animasi Global")]
    public UIManager uiManager;
    public Animator arpyAnim;
    public TextMeshProUGUI TagLevel;

    [Header("Pengaturan Input")]
    public float tapThreshold = 5.0f;
    [HideInInspector] public Vector2 startTapPosition;

    [Header("Data Bangun Ruang Aktif")]
    public TipeBangun bangunSekarang;
    public enum TipeBangun { Kubus, PrismaSegitiga, Balok, LimasPersegi, Tabung, Kerucut, Bola }
    
    // Variabel ini bisa diakses dari mana saja karena bersifat public
    public string namaBangun;
    public string dialogKerangka;
    public string teksPopupSelesaiSisi;
    public int targetSisi; 
    public int targetSudut; 
    public int targetRusuk; 
    public int targetLapis;
    public int targetBuka;

    void Awake()
    {
        // Matikan semua level terlebih dahulu
        if(level1Manager != null) level1Manager.enabled = false;
        if(level2Manager != null) level2Manager.enabled = false;
        if(level3Manager != null) level3Manager.enabled = false;

        // Setup Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // =========================================================
    // FUNGSI SETUP & NAVIGASI LEVEL (Pindahan dari kodemu)
    // =========================================================

    public void SetupDataBangun(TipeBangun bangunYangDiscan)
    {
        bangunSekarang = bangunYangDiscan;

        switch (bangunSekarang)
        {
            case TipeBangun.Kubus:
                namaBangun = "kubus";
                targetSisi = 6; targetSudut = 8; targetRusuk = 12;
                targetLapis = 27; targetBuka = 5;
                dialogKerangka = "Kubus memiliki 6 sisi, 8 titik sudut dan 12 rusuk.";
                teksPopupSelesaiSisi = "Luar biasa! Kini kamu tahu kubus punya 6 sisi persegi.";
                break;
                
            case TipeBangun.PrismaSegitiga:
                namaBangun = "prisma segitiga";
                targetSisi = 5; targetSudut = 6; targetRusuk = 9;
                targetLapis = 6; targetBuka = 4;
                dialogKerangka = "Prisma segitiga memiliki 5 sisi, 6 titik sudut dan 9 rusuk.";
                teksPopupSelesaiSisi = "Luar biasa! Kini kamu tahu prisma segitiga punya 5 sisi.";
                break;
                
            case TipeBangun.Balok:
                namaBangun = "balok";
                targetSisi = 6; targetSudut = 8; targetRusuk = 12;
                targetLapis = 24; targetBuka = 5;
                dialogKerangka = "Balok memiliki 6 sisi, 8 titik sudut dan 12 rusuk.";
                teksPopupSelesaiSisi = "Luar biasa! Kini kamu tahu balok punya 6 sisi.";
                break;
        }

        if (uiManager != null) uiManager.OnCubeFound(namaBangun);
        Debug.Log("Sistem mendeteksi: " + namaBangun + ". Siap memulai level!");
    }

    public void MulaiLevel(int level)
    {
        // Ambil referensi agar bisa manggil fungsi di dalamnya
        Level1Manager lvl1 = level1Manager as Level1Manager;
        Level2Manager lvl2 = level2Manager as Level2Manager;
        Level3Manager lvl3 = level3Manager as Level3Manager;

        // Nyalakan hanya level yang diminta
        if (level == 1)
        {
            if(lvl1 != null) {
                lvl1.enabled = true;
                lvl1.StartLevelSisi();
            }
        }
        else if (level == 2)
        {
            if(lvl2 != null) {
                lvl2.enabled = true;
                lvl2.StartLevelVolume();
            }
        }
        else if (level >= 3)
        {
            if(lvl3 != null) {
                lvl3.enabled = true;
                lvl3.StartLevel3();
            }
        }
    }

    // =========================================================
    // FUNGSI HELPER GLOBAL
    // =========================================================

    public IEnumerator TungguInputUser()
    {
        uiManager.SetTapIndicator(true);
        yield return new WaitForSeconds(0.2f);

        while (!Pointer.current.press.wasPressedThisFrame)
        {
            yield return null; 
        }

        uiManager.SetTapIndicator(false);
        SFXManager.Instance.MainkanClick();
        yield return new WaitForSeconds(0.1f);
    }

    public void HandleNextButton()
    {
        SFXManager.Instance.MainkanClick();
        uiManager.TutupKomplitPopup();
        uiManager.SetTombolNextLevel(true);
    }

public void HandleNextLevelButton()
    {
        SFXManager.Instance.MainkanClick();
        StopAllCoroutines();
        uiManager.SetTombolNextLevel(false);

        // Ambil referensi ke masing-masing manager
        Level1Manager lvl1 = level1Manager as Level1Manager;
        Level2Manager lvl2 = level2Manager as Level2Manager;
        Level3Manager lvl3 = level3Manager as Level3Manager;
        
        if (lvl1.jumlahVertexDitemukan == 0 && lvl1.jumlahSisiDitemukan >= targetSisi) {
            lvl1.StartLevelSudut();
            return;
        } 
        else if (lvl1.jumlahRusukDitemukan == 0 && lvl1.jumlahVertexDitemukan >= targetSudut) {
            lvl1.StartLevelRusuk();
            return;
        }
        else if (lvl1.jumlahRusukDitemukan >= targetRusuk && lvl2.currentVolume == 0) {
            if(level1Manager != null) level1Manager.enabled = false;
            // KERANGKA SELESAI! Lompat ke Level 2
            MulaiLevel(2);
            // if(lvl2 != null) lvl2.StartLevelVolume();
            return;
        }
        else if (lvl2.currentVolume >= targetLapis && lvl3.openStep == 0) {
            if(level1Manager != null) level1Manager.enabled = false;
            if(level2Manager != null) level2Manager.enabled = false;
            // VOLUME SELESAI! Lompat ke Level 3
            MulaiLevel(3);
            // Nanti kita panggil lvl3.StartLevel3() di sini
            return;
        }

        // 3. Jika semuanya sudah selesai
        StartCoroutine(SequenceKembaliKeMenu());
    }

    IEnumerator SequenceKembaliKeMenu()
    {
        uiManager.OnMulaiLevel(namaBangun, 4);
        yield return new WaitForSeconds(1.5f);
        SceneTransition.Instance.PindahScene("MainMenu");
    }
}