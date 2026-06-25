using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public UIManager uiManager;
    public enum TipeBangun { Kubus, PrismaSegitiga, Balok }

    [Header("Pengaturan Bangun Ruang")]
    public TipeBangun bangunSekarang;
    private string namaBangun;
    private string dialogKerangka;
    private string teksPopupSelesaiSisi;
    public int targetSisi; 
    public int targetSudut; 
    public int targetRusuk; 
    
    [Header("Input Settings")]
    public float tapThreshold = 5.0f;
    private Vector2 startTapPosition;
    public TextMeshProUGUI TagLevel;

    [Header("Level 1")]
    public GameObject KubusLevel1; 
    public GameObject prismaLevel1; 
    public GameObject balokLevel1; 
    
    [Header("Misi Sisi")]
    public GameObject faceGroupKubus;
    public GameObject faceGroupPrisma;
    public GameObject faceGroupBalok;
    private int jumlahSisiDitemukan = 0;
    public TextMeshProUGUI namaMisi2; 
    public TextMeshProUGUI counterSisi; 

    [Header("Misi Sudut")]
    public GameObject vertexGroup; 
    public GameObject vertexGroupPrisma; 
    public GameObject vertexGroupBalok; 
    private int jumlahVertexDitemukan = 0;
    public TextMeshProUGUI namaMisi1; 
    public TextMeshProUGUI counterText; 

    [Header("Misi Rusuk")]
    public GameObject edgeGroup;
    public GameObject edgeGroupPrisma;
    public GameObject edgeGroupBalok;
    private int jumlahRusukDitemukan = 0;
    public TextMeshProUGUI namaMisi3; 
    public TextMeshProUGUI counterRusuk; 


    [Header("Level 2 Kubus")]
    public GameObject KubusLevel2;
    public GameObject KubusKecil;
    public GameObject unitCubePrefab;
    public Transform unitContainer;
    private int currentVolume = 0;
    private bool isLevel2Active = false;
    public GameObject objekRumus; 
    public TMPro.TextMeshProUGUI teksRumus; 
    public GameObject penggarisX; 
    public GameObject penggarisY; 
    public GameObject penggarisZ; 
    
    [Header("Level 2 Balok")]
    public GameObject BalokLevel2;
    public GameObject BalokKecil;
    public GameObject unitCubePrefabBalok;
    public Transform unitContainerBalok;
    public GameObject objekRumusBalok; 
    public TMPro.TextMeshProUGUI teksRumusBalok; 
    public GameObject penggarisXBalok; 
    public GameObject penggarisYBalok; 
    public GameObject penggarisZBalok; 

    [Header("Level 2 Volume Prisma")]
    public GameObject PrismaLevel2;     
    public GameObject unitPrismaPrefab; 
    public int targetLapis = 5;        
    public float posisiAwalZ = 0.01f;
    public float tambahSkalaZPerTap = 0.5f; 
    public float pengaliPosisi = 0.5f;
    private GameObject isianVolumeAktif;
    public GameObject objekRumusPrisma; 
    public TMPro.TextMeshProUGUI teksRumusPrisma; 
    public GameObject penggarisXPrisma; 
    public GameObject penggarisYPrisma; 
    public GameObject penggarisZPrisma; 

    [Header("Level 3")]
    public FaseLevel3 faseSekarang = FaseLevel3.Pengenalan;
    public enum FaseLevel3 { Pengenalan, KupasUtama, TampilVariasi, HitungLuas, Selesai }

    [Header("Level 3 Kubus")]
    public GameObject KubusLevel3;
    public Animator jaringAnimator;
    private bool isLevel3Active = false;
    private int countSisiTerbuka = 0;
    private int varianTerbuka = 0;
    private int openStep = 0; // Counter untuk fase membuka
    public int targetBuka = 0;
    // private bool isFullyOpen = false; // Flag penanda fase mewarnai
    public GameObject penggarisXPermukaan;
    public GameObject penggarisYPermukaan;
    public GameObject papanRumusLP; 
    [Header("Variasi Jaring-Jaring kubus")]
    public GameObject[] prefabVariasi; // Masukkan 3 prefab varian di sini lewat Inspector
    private List<GameObject> spawnedVariations = new List<GameObject>(); // Untuk melacak yang sudah muncul

    
    [Header("Level 3 Prisma")]
    public GameObject PrismaLevel3;
    public Animator jaringAnimatorPrisma;
    public GameObject papanRumusLPPrisma; 
    [Header("Variasi Jaring-Jaring Prisma")]
    public GameObject[] prefabVariasiPrisma; 
    
    
    [Header("Level 3 Balok")]
    public GameObject BalokLevel3;
    public Animator jaringAnimatorBalok;
    public GameObject papanRumusLPBalok; 
    [Header("Variasi Jaring-Jaring Balok")]
    public GameObject[] prefabVariasiBalok; 
    

    [Header("Animasi")]
    public Animator arpyAnim; 

    [Header("Referensi Unsur Kubus (Glow)")]
    public ObjectGlow efekSisi; 
    public ObjectGlow efekRusuk; 
    public ObjectGlow efekSudut;
    public ObjectGlow efekVolume;
    public ObjectGlow efekJaring;
    
    [Header("Referensi Unsur Prisma (Glow)")]
    public ObjectGlow efekSisiPrisma; 
    public ObjectGlow efekRusukPrisma; 
    public ObjectGlow efekSudutPrisma;
    public ObjectGlow efekVolumePrisma;
    public ObjectGlow efekJaringPrisma;

    [Header("Referensi Unsur Balok (Glow)")]
    public ObjectGlow efekSisiBalok; 
    public ObjectGlow efekRusukBalok; 
    public ObjectGlow efekSudutBalok;
    public ObjectGlow efekVolumeBalok;
    public ObjectGlow efekJaringBalok;

    // SETUP ------------------------------------------------------------------------------
    // SETUP ------------------------------------------------------------------------------
    // SETUP ------------------------------------------------------------------------------
    public void SetupDataBangun(TipeBangun bangunYangDiscan)
    {
        bangunSekarang = bangunYangDiscan; // Simpan jenis bangunnya

        switch (bangunSekarang)
        {
            case TipeBangun.Kubus:
                namaBangun = "kubus";
                targetSisi = 6;
                targetSudut = 8;
                targetRusuk = 12;
                targetLapis = 27;
                targetBuka = 5;
                dialogKerangka = "Kubus memiliki 6 sisi, 8 titik sudut dan 12 rusuk.";
                teksPopupSelesaiSisi = "Luar biasa! Kini kamu tahu kubus punya 6 sisi persegi.";
                break;
                
            case TipeBangun.PrismaSegitiga:
                namaBangun = "prisma segitiga";
                targetSisi = 5;
                targetSudut = 6;
                targetRusuk = 9;
                targetLapis = 6;
                targetBuka = 4;
                dialogKerangka = "Prisma segitiga memiliki 5 sisi, 6 titik sudut dan 9 rusuk.";
                teksPopupSelesaiSisi = "Luar biasa! Kini kamu tahu prisma segitiga punya 5 sisi.";
                break;
            case TipeBangun.Balok:
                namaBangun = "balok";
                targetSisi = 6;
                targetSudut = 8;
                targetRusuk = 12;
                targetLapis = 24;
                targetBuka = 5;
                dialogKerangka = "Balok memiliki 6 sisi, 8 titik sudut dan 12 rusuk.";
                teksPopupSelesaiSisi = "Luar biasa! Kini kamu tahu balok punya 6 sisi.";
                break;
        }
        if (uiManager != null)
        {
            uiManager.OnCubeFound(namaBangun);
        }
        
        Debug.Log("Sistem mendeteksi: " + namaBangun + ". Siap memulai level!");
    }
    
    public void MulaiLevel(int level)
    {
        // 1. Tanya SaveManager: "Berapa level tertinggi pemain ini di bangun ruang ini?"
        int levelTerakhir = SaveManager.Instance.AmbilLevelTertinggi(namaBangun);

        // 2. Langsung lompat ke level yang sesuai
        if (level == 1)
        {
            Debug.Log("Pemain baru. Memulai Level 1.");
            StartLevelSisi();
        }
        else if (level == 2)
        {
            Debug.Log("Melanjutkan ke Level 2.");
            StartLevelVolume();
        }
        else if (level >= 3)
        {
            Debug.Log("Melanjutkan ke Level 3.");
            StartLevel3();
        }
    }

    // SISI ------------------------------------------------------------------------------
    // SISI ------------------------------------------------------------------------------
    // SISI ------------------------------------------------------------------------------
    public void StartLevelSisi()
    {
        // if (isSequenceActive) return;
        // Jalankan urutan kejadian di bawah
        StartCoroutine(SequenceStartLevelSisi());
    }
    IEnumerator SequenceStartLevelSisi()
    {
        uiManager.OnMulaiLevel(namaBangun, 1);
        yield return new WaitForSeconds(1.5f);

        Collider[] semuaCollider = faceGroupKubus.GetComponentsInChildren<Collider>();
        
        // Jalankan animasi dialog
        vertexGroup.SetActive(false); 
        TagLevel.text = "Level 1";
        uiManager.BukaKotakMisi();

        yield return StartCoroutine(uiManager.AnimasiDialog("Mari kita mengenal " + namaBangun + "!"));
        yield return StartCoroutine(TungguInputUser());
        
        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(2);

        yield return StartCoroutine(uiManager.AnimasiDialog("Seperti yang kamu lihat inilah "+ namaBangun + "!"));
        yield return StartCoroutine(TungguInputUser());
        
        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(uiManager.AnimasiDialog("Pertama, mari kita amati kerangka "+ namaBangun +"!"));
        yield return StartCoroutine(TungguInputUser());

        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(uiManager.AnimasiDialog(dialogKerangka));
        yield return StartCoroutine(TungguInputUser());
        
        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(2);

        if(namaBangun == "kubus")
        {
            // semuaCollider = faceGroupKubus.GetComponentsInChildren<Collider>();
            faceGroupKubus.SetActive(true);
            
            // 2. Matikan satu per satu menggunakan perulangan
            foreach (Collider col in semuaCollider)
            {
                if (col != null) col.enabled = false; // Matikan
                
            }
            if (efekSisi != null) efekSisi.SetGlow(true); // Mulai bersinar Merah
        } 
        else if(namaBangun == "prisma segitiga")
        {
            if (efekSisiPrisma != null) efekSisiPrisma.SetGlow(true); // Mulai bersinar Merah
        }
        else if(namaBangun == "balok")
        {
            if (efekSisiBalok != null) efekSisiBalok.SetGlow(true); // Mulai bersinar Merah
        }


        yield return StartCoroutine(uiManager.AnimasiDialog("Sisi merupakan permukaan atau “kulit luar” pada bangun ruang."));
        yield return StartCoroutine(TungguInputUser());
        
        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(uiManager.AnimasiDialog("Pada " + namaBangun + " ini, sisi merupakan area yang berwarna merah."));
        yield return StartCoroutine(TungguInputUser());
        
        if (efekSisi != null) efekSisi.SetGlow(false); // Mulai bersinar Merah
        if (efekSisiPrisma != null) efekSisiPrisma.SetGlow(false); // Mulai bersinar Merah
        if (efekSisiBalok != null) efekSisiBalok.SetGlow(false); // Mulai bersinar Merah
        
        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(uiManager.AnimasiDialog("Sekarang bantu aku temukan " + targetSisi + " sisi " + namaBangun + "!"));
        yield return StartCoroutine(TungguInputUser());
        
        namaMisi1.text = "Temukan Sisi";
        uiManager.BukaMisi(1);
        
        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(uiManager.AnimasiDialog("Ketuk pada bagian "+ namaBangun +" yang kamu rasa itu sisi nya!"));
        
        if(namaBangun == "kubus")
        {
            foreach (Collider col in semuaCollider)
            {
                if (col != null) col.enabled = true;
            }
        }
        else if(namaBangun == "prisma segitiga")
        {
            // MENCARI SEMUA SISI DAN MENGAKTIFKANNYA
            //facegroup buat jadi dinamis
            FaceController[] semuaSisi = faceGroupPrisma.GetComponentsInChildren<FaceController>();
            foreach (FaceController sisi in semuaSisi)
            {
                if (sisi != null) sisi.AktifkanInteraksi();
            }
        }
        else if(namaBangun == "balok")
        {
            // MENCARI SEMUA SISI DAN MENGAKTIFKANNYA
            //facegroup buat jadi dinamis
            FaceController[] semuaSisi = faceGroupBalok.GetComponentsInChildren<FaceController>();
            foreach (FaceController sisi in semuaSisi)
            {
                if (sisi != null) sisi.AktifkanInteraksi();
            }
        }


        UpdateUISisi();
    }

    public void TambahSisi()
    {
        jumlahSisiDitemukan++;        
        UpdateUISisi();

        if (jumlahSisiDitemukan >= targetSisi && jumlahVertexDitemukan == 0)
        {
            SelesaikanLevelSisi();
        }
    }

    public void UpdateUISisi()
    {
        if(jumlahSisiDitemukan == 1)
        {   
            uiManager.ShowCubeExplanation("Satu ketemu!");
            arpyAnim.SetTrigger("doHU");
        } else if(jumlahSisiDitemukan == 2)
        {
            arpyAnim.SetTrigger("doIdle2");
            uiManager.ShowCubeExplanation("Hebat! Kamu juga bisa memutar bangun ruang dengan jarimu");
        } else if(jumlahSisiDitemukan == 4)
        {
            uiManager.ShowCubeExplanation("Bagus!");

        }
        // else if(jumlahSisiDitemukan >= 6)
        // {
        //     uiManager.ShowCubeExplanation("Semua sisi ditemukan!");
        // }
        counterSisi.text = "(" + jumlahSisiDitemukan + " / " + targetSisi + ")";
        
    }

    void SelesaikanLevelSisi()
    {
        arpyAnim.SetTrigger("doHU");
        SFXManager.Instance.MainkanArpyNoise(1);
        // MENCARI SEMUA SISI DAN MERESET WARNANYA
        
        uiManager.ShowCompletionPopup("Misi Sisi Selesai!", teksPopupSelesaiSisi, 0);
    }


    // Sudut ------------------------------------------------------------------------------
    // Sudut ------------------------------------------------------------------------------
    // Sudut ------------------------------------------------------------------------------
    public void StartLevelSudut()
    {
        // if (isSequenceActive) return;
        // Jalankan urutan kejadian di bawah
        StartCoroutine(SequenceStartLevelSudut());

    }
    IEnumerator SequenceStartLevelSudut()
    {
        // Pastikan grup lain mati agar tidak menghalangi klik
        // faceGroupPrisma.SetActive(false);
        Collider[] semuaCollider1 = vertexGroup.GetComponentsInChildren<Collider>();
        if(namaBangun == "prisma segitiga")
        {
            FaceController[] semuaSisi = faceGroupPrisma.GetComponentsInChildren<FaceController>();
            foreach (FaceController sisi in semuaSisi)
            {
                if (sisi != null) sisi.ResetSisi();
            }        
        }
        else if(namaBangun == "balok")
        {
            FaceController[] semuaSisi = faceGroupBalok.GetComponentsInChildren<FaceController>();
            foreach (FaceController sisi in semuaSisi)
            {
                if (sisi != null) sisi.ResetSisi();
            }        
        }
        else if(namaBangun == "kubus")
        {
            CubeAnimation faceGroup1 = faceGroupKubus.GetComponent<CubeAnimation>();
            faceGroup1.HilangkanKubus();
            edgeGroup.SetActive(false); // Tambahkan ini sebagai pengaman tambahan
        }
        
        // 1. Jalankan dialog pertama dan TUNGGU sampai selesai
        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(1);
        yield return StartCoroutine(uiManager.AnimasiDialog("Sekarang saatnya untuk mencari sudut!"));
        yield return StartCoroutine(TungguInputUser()); // Jeda pendek antar kalimat agar tidak terlalu cepat

        // 2. Jalankan dialog kedua dan TUNGGU
        // 3. Kalimat terakhir
        if(namaBangun == "kubus")
        {
            vertexGroup.SetActive(true);
            semuaCollider1 = vertexGroup.GetComponentsInChildren<Collider>();
            // 2. Matikan satu per satu menggunakan perulangan
            foreach (Collider col in semuaCollider1)
            {
                if (col != null) col.enabled = false; // Matikan
                
            }
            if (efekSudut != null) efekSudut.SetGlow(true); // Mulai bersinar
        } 
        else if( namaBangun == "prisma segitiga")
        {
            if (efekSudutPrisma != null) efekSudutPrisma.SetGlow(true); // Mulai bersinar
            
        }
        else if( namaBangun == "balok")
        {
            if (efekSudutBalok != null) efekSudutBalok.SetGlow(true); // Mulai bersinar
            
        }

        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(uiManager.AnimasiDialog("Sudut merupakan titik pertemuan beberapa rusuk atau bisa disebut juga 'pojok' dari bangun ruang."));
        yield return StartCoroutine(TungguInputUser());


        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(uiManager.AnimasiDialog("Iya, yang berwarna biru itu."));
        yield return StartCoroutine(TungguInputUser());

        if (efekSudut != null) efekSudut.SetGlow(false); //
        if (efekSudutPrisma != null) efekSudutPrisma.SetGlow(false); //
        if (efekSudutBalok != null) efekSudutBalok.SetGlow(false); //

        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(uiManager.AnimasiDialog("Temukan "+ targetSudut +" titik sudut pada "+namaBangun+"!"));
        
        if(namaBangun == "kubus")
        {
            foreach (Collider col in semuaCollider1)
            {
                if (col != null) col.enabled = true;
                
            }
        }
        else if(namaBangun == "prisma segitiga")
        {
            VertexController[] semuaSudut = vertexGroupPrisma.GetComponentsInChildren<VertexController>();
            foreach (VertexController sudut in semuaSudut)
            {
                if (sudut != null) sudut.AktifkanInteraksi();
            }
        }
        else if(namaBangun == "balok")
        {
            VertexController[] semuaSudut = vertexGroupBalok.GetComponentsInChildren<VertexController>();
            foreach (VertexController sudut in semuaSudut)
            {
                if (sudut != null) sudut.AktifkanInteraksi();
            }
        }
        // Aktifkan vertex setelah penjelasan selesai agar user tidak bingung
        namaMisi2.text = "Temukan Sudut";
        uiManager.BukaMisi(2);
        
        UpdateUISudut();
    }

    public void TambahSudut()
    {
        jumlahVertexDitemukan++;
        UpdateUISudut();

        if (jumlahVertexDitemukan >= targetSudut && jumlahSisiDitemukan >= targetSisi && jumlahRusukDitemukan == 0)
        {
            SelesaikanLevelSudut();
        }
    }

    void UpdateUISudut()
    {
        if(jumlahVertexDitemukan == 1)
        {
            arpyAnim.SetTrigger("doIdle2");
            uiManager.ShowCubeExplanation("Itu dia!");      
        }
        else if( jumlahVertexDitemukan == 3)
        {
            arpyAnim.SetTrigger("doIdle2");
            uiManager.ShowCubeExplanation("Ayo Semangat!");            
        }
        else if( jumlahVertexDitemukan == 6)
        {
            arpyAnim.SetTrigger("doIdle2");
            uiManager.ShowCubeExplanation("Kamu hebat!");            
        }
        counterText.text = "(" + jumlahVertexDitemukan + " / " + targetSudut + ")";
    }

    void SelesaikanLevelSudut()
    {
        arpyAnim.SetTrigger("doHU");
        SFXManager.Instance.MainkanArpyNoise(1);
        
        uiManager.ShowCompletionPopup("Misi Sudut Selesai!", "Hebat! Kamu berhasil menemukan "+ targetSudut +" titik sudut "+namaBangun+".", 0);
    }

    // Rusuk ------------------------------------------------------------------------------
    // Rusuk ------------------------------------------------------------------------------
    // Rusuk ------------------------------------------------------------------------------

    public void StartLevelRusuk()
    {
        // if (isSequenceActive) return;
        // Jalankan urutan kejadian di bawah
        StartCoroutine(SequenceStartLevelRusuk());
    }
    IEnumerator SequenceStartLevelRusuk()
    {  
        if(namaBangun == "kubus")
        {
            faceGroupKubus.SetActive(false);
            CubeAnimation vertexGroup1 = vertexGroup.GetComponent<CubeAnimation>();
            vertexGroup1.HilangkanKubus();
                        
        } 
        else if(namaBangun == "prisma segitiga")
        {
            VertexController[] semuaSudut = vertexGroupPrisma.GetComponentsInChildren<VertexController>();
            foreach (VertexController sudut in semuaSudut)
            {
                if (sudut != null) sudut.ResetSudut();
            }
        }
        else if(namaBangun == "balok")
        {
            VertexController[] semuaSudut = vertexGroupBalok.GetComponentsInChildren<VertexController>();
            foreach (VertexController sudut in semuaSudut)
            {
                if (sudut != null) sudut.ResetSudut();
            }
        }
        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(uiManager.AnimasiDialog("Terakhir mari mempelajari rusuk "+namaBangun+"!"));
        yield return StartCoroutine(TungguInputUser());
        
        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        edgeGroup.SetActive(true);
        Collider[] semuaCollider2 = edgeGroup.GetComponentsInChildren<Collider>();
        Collider[] semuaCollider2Prisma = edgeGroupPrisma.GetComponentsInChildren<Collider>();

        if(namaBangun == "kubus")
        {
            // 2. Matikan satu per satu menggunakan perulangan
            foreach (Collider col in semuaCollider2)
            {
                if (col != null) col.enabled = false; // Matikan
                
            }
            if (efekRusuk != null) efekRusuk.SetGlow(true); // Mulai bersinar
        }
        else if(namaBangun == "prisma segitiga")
        {
            if (efekRusukPrisma != null) efekRusukPrisma.SetGlow(true); // Mulai bersinar
        }
        else if(namaBangun == "balok")
        {
            if (efekRusukPrisma != null) efekRusukBalok.SetGlow(true); // Mulai bersinar
        }
        
        yield return StartCoroutine(uiManager.AnimasiDialog("Rusuk merupakan garis pertemuan antara dua sisi."));
        yield return StartCoroutine(TungguInputUser());
        
        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(uiManager.AnimasiDialog("Pada "+namaBangun+" kita, garis-garis ini berwarna putih."));
        yield return StartCoroutine(TungguInputUser());
        
        if (efekRusuk != null) efekRusuk.SetGlow(false); // Mati bersinar
        if (efekRusukPrisma != null) efekRusukPrisma.SetGlow(false); // Mati bersinar
        if (efekRusukBalok != null) efekRusukBalok.SetGlow(false); // Mati bersinar

        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(uiManager.AnimasiDialog("Temukan "+targetRusuk+" rusuk yang membentuk kerangka "+namaBangun+" ini."));
        
        if(namaBangun == "kubus")
        {
            
            foreach (Collider col in semuaCollider2)
            {
                if (col != null) col.enabled = true; // hidupkan
                
            }
        }
        else if(namaBangun == "prisma segitiga")
        {
            EdgeController[] semuaRusuk = edgeGroupPrisma.GetComponentsInChildren<EdgeController>();
            foreach (EdgeController rusuk in semuaRusuk)
            {
                if (rusuk != null) rusuk.AktifkanInteraksi();
            }
        }
        else if(namaBangun == "balok")
        {
            EdgeController[] semuaRusuk = edgeGroupBalok.GetComponentsInChildren<EdgeController>();
            foreach (EdgeController rusuk in semuaRusuk)
            {
                if (rusuk != null) rusuk.AktifkanInteraksi();
            }
        }
        
        namaMisi3.text = "Temukan Rusuk";
        uiManager.BukaMisi(3);
        
        UpdateUIRusuk();
    }

    public void TambahRusuk()
    {
        jumlahRusukDitemukan++;

        UpdateUIRusuk();

        if (jumlahRusukDitemukan >= targetRusuk && jumlahVertexDitemukan >= targetSudut && jumlahSisiDitemukan >= targetSisi)
        {
            SelesaikanLevelRusuk();
        }
    }

    public void UpdateUIRusuk()
    {
        if(jumlahRusukDitemukan == 1)
        {
            arpyAnim.SetTrigger("doIdle2");
            uiManager.ShowCubeExplanation("Kamu memang hebat!"); 
        } else if(jumlahRusukDitemukan == 3)
        {
            arpyAnim.SetTrigger("doExpla");
            uiManager.ShowCubeExplanation("Ayo semangat!"); 
        }
        else if(jumlahRusukDitemukan == 6)
        {
            arpyAnim.SetTrigger("doExpla");
            uiManager.ShowCubeExplanation("Sedikit lagi!"); 
        }
        else if(jumlahRusukDitemukan == 9)
        {
            arpyAnim.SetTrigger("doIdle2");
            uiManager.ShowCubeExplanation("Kamu pasti bisa!"); 
        }
        
        counterRusuk.text = "(" + jumlahRusukDitemukan + " / " + targetRusuk + ")";
        
    }

    void SelesaikanLevelRusuk()
    {
        if(namaBangun == "kubus")
        {
            CubeAnimation edgeGroup1 = edgeGroup.GetComponent<CubeAnimation>();
            edgeGroup1.HilangkanKubus();
                        
        } 
        else if(namaBangun == "prisma segitiga")
        {
            EdgeController[] semuaRusuk = edgeGroupPrisma.GetComponentsInChildren<EdgeController>();
            foreach (EdgeController rusuk in semuaRusuk)
            {
                if (rusuk != null) rusuk.ResetRusuk();
            }
        }
        else if(namaBangun == "balok")
        {
            EdgeController[] semuaRusuk = edgeGroupBalok.GetComponentsInChildren<EdgeController>();
            foreach (EdgeController rusuk in semuaRusuk)
            {
                if (rusuk != null) rusuk.ResetRusuk();
            }
        }

        int levelYangBaruTerbuka = 2; // Selesai tahap rusuk -> lanjut ke tahap 2

        SaveManager.Instance.BukaLevel(namaBangun, levelYangBaruTerbuka);
        
        arpyAnim.SetTrigger("doHU");
        SFXManager.Instance.MainkanArpyNoise(1);

        // 2. Kalkulasi jumlah bintang untuk UI (Level saat ini dikurangi 1)
        int bintangUntukDitampilkan = levelYangBaruTerbuka - 1;

        uiManager.TutupMisi();
        uiManager.ShowCompletionPopup(
            "Level Kerangka Selesai!", 
            "Kamu telah menguasai seluruh kerangka "+ namaBangun +" ("+targetSisi+" Sisi, "+targetSudut+" Sudut, "+targetRusuk+" Rusuk).", 
            bintangUntukDitampilkan
        );
    }

    void DoRaycast(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            SFXManager.Instance.MainkanPop();
            EdgeController ec = hit.collider.GetComponent<EdgeController>();
            if (ec != null) 
            {
                ec.OnEdgeClicked();
                return; // Hentikan pengecekan jika rusuk sudah kena
            }
            // Panggil controller (Vertex/Face) seperti biasa
            FaceController fc = hit.collider.GetComponent<FaceController>();
            if (fc != null) fc.OnFaceClicked();
            
            VertexController vc = hit.collider.GetComponent<VertexController>();
            if (vc != null) vc.OnMouseDown();
        }
    }    


    // Level 2 --------------------------------------------------------------------------------------------------
    // Level 2 --------------------------------------------------------------------------------------------------
    // Level 2 --------------------------------------------------------------------------------------------------
    public void StartLevelVolume()
    {
        StartCoroutine(SequenceStartLevelVolume());
    }

    IEnumerator SequenceStartLevelVolume()
    {
        uiManager.OnMulaiLevel(namaBangun, 2);
        yield return new WaitForSeconds(1.5f);

        if(namaBangun == "prisma segitiga")
        {
            objekRumusPrisma.SetActive(false);
            penggarisXPrisma.SetActive(false);
            penggarisYPrisma.SetActive(false);
            penggarisZPrisma.SetActive(false);
        }
        else if(namaBangun == "kubus")
        {
            faceGroupKubus.SetActive(false);
            vertexGroup.SetActive(false);
            objekRumus.SetActive(false);
            penggarisX.SetActive(false);
            penggarisY.SetActive(false);
            penggarisZ.SetActive(false);
        }        
        else if(namaBangun == "balok")
        {
            
        }        

        // 2. Siapkan data awal
        currentVolume = 0;
        isLevel2Active = false; // Belum aktif sampai instruksi selesai
        TagLevel.text = "Level 2";
        uiManager.BukaKotakMisi();

        // 3. Jalankan dialog pengantar (Gunakan sistem Tap jika sudah kamu terapkan)
        
        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(uiManager.AnimasiDialog("Di level ini kita akan belajar tentang VOLUME."));
        yield return StartCoroutine(TungguInputUser()); 
        
        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(uiManager.AnimasiDialog("Volume adalah isi dari bangun ruang."));
        yield return StartCoroutine(TungguInputUser());

        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(uiManager.AnimasiDialog("Untuk mempelajarinya mari kita ubah "+namaBangun+" kita!"));
        yield return StartCoroutine(TungguInputUser());

        // KubusLevel1.SetActive(false);
        if(namaBangun == "kubus")
        {
            CubeAnimation animLevel1 = KubusLevel1.GetComponent<CubeAnimation>();
            animLevel1.HilangkanKubus();
        } 
        else if(namaBangun == "prisma segitiga")
        {
            CubeAnimation animLevel1 = prismaLevel1.GetComponent<CubeAnimation>();
            animLevel1.HilangkanKubus();
            
        }
        else if(namaBangun == "balok")
        {
            CubeAnimation animLevel1 = balokLevel1.GetComponent<CubeAnimation>();
            animLevel1.HilangkanKubus();
            
        }

        LoadLevel2(1);

        if (namaBangun=="kubus"&&efekVolume != null) efekVolume.SetGlow(true); // Mulai bersinar
        if (namaBangun=="prisma segitiga"&&efekVolumePrisma != null) efekVolumePrisma.SetGlow(true); // Mulai bersinar
        if (namaBangun=="balok"&&efekVolumeBalok != null) efekVolumeBalok.SetGlow(true); // Mulai bersinar
        
        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(uiManager.AnimasiDialog("Disini kita memiliki sebuah "+namaBangun+" kosong."));
        yield return StartCoroutine(TungguInputUser());

        LoadLevel2(2);

        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(uiManager.AnimasiDialog("Lalu kita akan mengisi "+namaBangun+" kosong ini."));
        yield return StartCoroutine(TungguInputUser());
        
        if (namaBangun=="kubus" && efekVolume != null) efekVolume.SetGlow(false); // bersinar berhenti
        if (namaBangun=="prisma segitiga" && efekVolumePrisma != null) efekVolumePrisma.SetGlow(false); // bersinar berhenti
        if (namaBangun=="balok" && efekVolumeBalok != null) efekVolumeBalok.SetGlow(false); // bersinar berhenti
    
        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(uiManager.AnimasiDialog("Ketuk "+namaBangun+" untuk mengisi"));

        // 4. Aktifkan interaksi
        isLevel2Active = true;
        namaMisi1.text = "Isi Volume";
        uiManager.BukaMisi(1);

        UpdateUIVolume();
    }
    public void LoadLevel2(int set)
    {
        if(set == 1)
        {
            if(namaBangun == "kubus")
            {
                // 1. Ciptakan objeknya
                KubusLevel2.SetActive(true);
                // 2. Ambil skrip CubeAnimation yang ada di prefab tersebut
                CubeAnimation anim = KubusLevel2.GetComponent<CubeAnimation>();
                // 3. Jalankan animasinya
                if (anim != null)
                {
                    anim.MunculkanKubus();
                }
            }
            else if(namaBangun == "balok")
            {
                // 1. Ciptakan objeknya
                BalokLevel2.SetActive(true);
                // 2. Ambil skrip CubeAnimation yang ada di prefab tersebut
                CubeAnimation anim = BalokLevel2.GetComponent<CubeAnimation>();
                // 3. Jalankan animasinya
                if (anim != null)
                {
                    anim.MunculkanKubus();
                }
            }
            else if(namaBangun == "prisma segitiga")
            {
                // 1. Ciptakan objeknya
                PrismaLevel2.SetActive(true);

                // 2. Ambil skrip CubeAnimation yang ada di prefab tersebut
                CubeAnimation anim = PrismaLevel2.GetComponent<CubeAnimation>();

                // 3. Jalankan animasinya
                if (anim != null)
                {
                    anim.MunculkanKubus();
                }
                
            }
            
        }
        else if (set == 2)
        {
            if(namaBangun == "kubus")
            {
                // 1. Ciptakan objeknya
                KubusKecil.SetActive(true);
                // 2. Ambil skrip CubeAnimation yang ada di prefab tersebut
                CubeAnimation anim2 = KubusKecil.GetComponent<CubeAnimation>();

                // 3. Jalankan animasinya
                if (anim2 != null)
                {
                    anim2.MunculkanKubus();
                }
            }
            else if(namaBangun == "balok")
            {
                // 1. Ciptakan objeknya
                BalokKecil.SetActive(true);
                // 2. Ambil skrip CubeAnimation yang ada di prefab tersebut
                CubeAnimation anim2 = BalokKecil.GetComponent<CubeAnimation>();

                // 3. Jalankan animasinya
                if (anim2 != null)
                {
                    anim2.MunculkanKubus();
                }
            }
        }
    }
    public void HandleTapVolume()
    {
        SFXManager.Instance.MainkanPop();
        // Cek: Apakah sudah penuh? Apakah sedang ada dialog berjalan?
        if ((namaBangun == "kubus" || namaBangun == "balok") && currentVolume < targetLapis && isLevel2Active) 
        {
            SpawnUnitCube(currentVolume);
            currentVolume++;
            UpdateUIVolume();

            // CEK MILESTONE: Setiap kelipatan 9 (9, 18, 27)
            if (namaBangun == "kubus" && currentVolume % 9 == 0)
            {
                StartCoroutine(SequencePenjelasanPerLapis(currentVolume));
            }
            if (namaBangun == "balok" && currentVolume % 12 == 0)
            {
                StartCoroutine(SequencePenjelasanBalok(currentVolume));
            }
        }
        // Cek apakah belum penuh dan interaksi sedang aktif
        else if (namaBangun == "prisma segitiga" && currentVolume < targetLapis && isLevel2Active) 
        {            
            // SpawnLapisPrisma(currentVolume); // Panggil fungsi spawn prisma tipis
            IsiVolumePrisma(currentVolume); // Panggil fungsi yang baru
            
            currentVolume++;
            UpdateUIVolume();

            // Panggil penjelasan KHUSUS saat lapis pertama, dan saat penuh
            if (currentVolume == 1 || currentVolume == targetLapis)
            {
                StartCoroutine(SequencePenjelasanPrisma(currentVolume));
            }
        }
    }

    IEnumerator SequencePenjelasanPerLapis(int count)
    {
        // isSequenceActive = true; // Kunci gerbang agar user tidak bisa tap saat dialog
        isLevel2Active = false;

        if (count == 9)
        {
            arpyAnim.SetTrigger("doHU");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(uiManager.AnimasiDialog("Lantai pertama penuh! Alas ini berisi 3 x 3 = 9 kubus satuan."));
            yield return StartCoroutine(TungguInputUser()); 
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(uiManager.AnimasiDialog("Ini yang kita sebut Luas Alas"));
            yield return StartCoroutine(TungguInputUser()); 
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(uiManager.AnimasiDialog("Karena bentuknya persegi maka rumusnya adalah sisi x sisi."));
            yield return StartCoroutine(TungguInputUser()); 
            yield return StartCoroutine(uiManager.AnimasiDialog("Lanjut isi kubus"));
        }
        else if (count == 18)
        {
            arpyAnim.SetTrigger("doHU");
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(uiManager.AnimasiDialog("Lantai kedua selesai! Sekarang totalnya 9 + 9 = 18 atau 9 x 2 = 18 kubus satuan."));
            yield return StartCoroutine(TungguInputUser()); 
            SFXManager.Instance.MainkanArpyNoise(3);
            yield return StartCoroutine(uiManager.AnimasiDialog("Ayo Lanjutkan!"));
        }
        else if (count == 27)
        {
            arpyAnim.SetTrigger("doHU");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(uiManager.AnimasiDialog("Lantai ketiga penuh! Sekarang tingginya sudah 3 lapis dan berisi 9 + 9 + 9 = 27 atau 9 x 3 = 27 kubus satuan."));
            yield return StartCoroutine(TungguInputUser()); 
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(4);
            yield return StartCoroutine(uiManager.AnimasiDialog("Dari sini kita bisa memahami bahwa volume adalah banyaknya kubus satuan yang dapat mengisi suatu bangun ruang."));
            yield return StartCoroutine(TungguInputUser()); 
            arpyAnim.SetTrigger("doIdle2");
            SFXManager.Instance.MainkanArpyNoise(3);
            yield return StartCoroutine(uiManager.AnimasiDialog("Lalu rumusnya adalah Sisi x Sisi x Sisi"));
            yield return StartCoroutine(TungguInputUser()); 
            SelesaikanLevelVolume();
        }

        // Jika belum selesai (9 atau 18), buka kembali interaksinya
        if (count < 27)
        {
            // isSequenceActive = false;
            isLevel2Active = true;
        }
    }
    IEnumerator SequencePenjelasanBalok(int count)
    {
        // isSequenceActive = true; // Kunci gerbang agar user tidak bisa tap saat dialog
        isLevel2Active = false;

        if (count == 12)
        {
            arpyAnim.SetTrigger("doHU");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(uiManager.AnimasiDialog("Lantai pertama penuh! Alas ini berisi 3 x 4 = 12 kubus satuan."));
            yield return StartCoroutine(TungguInputUser());

            teksRumusBalok.text = "Luas Alas = Panjang x Lebar";
            objekRumusBalok.SetActive(true);
            penggarisXBalok.SetActive(true);
            penggarisYBalok.SetActive(true);

            CubeAnimation animp1 = objekRumusBalok.GetComponent<CubeAnimation>();
            CubeAnimation animp2 = penggarisXBalok.GetComponent<CubeAnimation>();
            CubeAnimation animp3 = penggarisYBalok.GetComponent<CubeAnimation>();

            animp1.MunculkanKubus();
            animp2.MunculkanKubus();
            animp3.MunculkanKubus();

            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(uiManager.AnimasiDialog("Ini yang kita sebut luas alas"));
            yield return StartCoroutine(TungguInputUser()); 
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(uiManager.AnimasiDialog("Karena bentuknya persegi panjang maka rumusnya adalah panjang x lebar."));
            yield return StartCoroutine(TungguInputUser()); 
            yield return StartCoroutine(uiManager.AnimasiDialog("Lanjut isi balok"));
        }
        else if (count == 24)
        {
            teksRumusBalok.text = "V = Luas Alas x Tinggi\nV = Panjang x Lebar x Tinggi";
            penggarisZBalok.SetActive(true);
            CubeAnimation animp4 = penggarisZBalok.GetComponent<CubeAnimation>();

            animp4.MunculkanKubus();

            arpyAnim.SetTrigger("doHU");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(uiManager.AnimasiDialog("Lantai kedua penuh! Sekarang tingginya sudah 2 lapis dan berisi 12 + 12 = 24 atau 4 x 3 x 2 = 24 kubus satuan."));
            yield return StartCoroutine(TungguInputUser()); 
            arpyAnim.SetTrigger("doIdle2");
            SFXManager.Instance.MainkanArpyNoise(3);
            yield return StartCoroutine(uiManager.AnimasiDialog("Sehingga didapatkan rumus Volume Balok yaitu panjang x lebar x tinggi"));
            yield return StartCoroutine(TungguInputUser()); 
            SelesaikanLevelVolume();
        }

        // Jika belum selesai (9 atau 18), buka kembali interaksinya
        if (count < 24)
        {
            // isSequenceActive = false;
            isLevel2Active = true;
        }
    }

    IEnumerator SequencePenjelasanPrisma(int count)
    {
        // 1. Kunci interaksi agar siswa tidak mengetuk layar saat Arpy sedang bicara
        isLevel2Active = false;

        if (count == 1)
        {
            // --- PENJELASAN SAAT LAPIS PERTAMA (ALAS) ---
            arpyAnim.SetTrigger("doHU");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(uiManager.AnimasiDialog("Hebat! Satu lapis prisma sudah terisi."));
            yield return StartCoroutine(TungguInputUser()); 
            
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(uiManager.AnimasiDialog("Lapis pertama yang berbentuk segitiga ini menutupi seluruh dasar prisma. Ini yang kita sebut Luas Alas!"));
            yield return StartCoroutine(TungguInputUser()); 
            
            teksRumusPrisma.text = "Luas Segitiga = 1/2 x alas segitiga x tinggi segitiga";
            objekRumusPrisma.SetActive(true);
            penggarisXPrisma.SetActive(true);
            penggarisYPrisma.SetActive(true);
            
            CubeAnimation animp1 = objekRumusPrisma.GetComponent<CubeAnimation>();
            CubeAnimation animp2 = penggarisXPrisma.GetComponent<CubeAnimation>();
            CubeAnimation animp3 = penggarisYPrisma.GetComponent<CubeAnimation>();

            animp1.MunculkanKubus();
            animp2.MunculkanKubus();
            animp3.MunculkanKubus();
                

            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(uiManager.AnimasiDialog("Karena bentuknya segitiga maka rumus luas alasnya adalah (1/2 x alas segitiga x tinggi segitiga)"));
            yield return StartCoroutine(TungguInputUser()); 
            
            
            arpyAnim.SetTrigger("doIdle2");
            SFXManager.Instance.MainkanArpyNoise(3);
            yield return StartCoroutine(uiManager.AnimasiDialog("Ayo ketuk layarnya lagi untuk menumpuk Luas Alas ini ke atas!"));
            yield return StartCoroutine(TungguInputUser()); 
            
            // 2. Buka kembali interaksinya agar siswa bisa lanjut mengetuk sampai penuh
            isLevel2Active = true;
        }
        else if (count == targetLapis) // Gunakan variabel targetLapis (misal 5)
        {
            teksRumusPrisma.text = "V = Luas Alas x Tinggi\nV = 1/2 x a_segitiga x t_segitiga x t_prisma";
            penggarisZPrisma.SetActive(true);
            CubeAnimation animp4 = penggarisZPrisma.GetComponent<CubeAnimation>();
            animp4.MunculkanKubus();

            // --- PENJELASAN SAAT PRISMA PENUH (VOLUME) ---
            arpyAnim.SetTrigger("doHU");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(uiManager.AnimasiDialog("Luar biasa! Prismanya sudah terisi penuh!"));
            yield return StartCoroutine(TungguInputUser()); 
            
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(4);
            yield return StartCoroutine(uiManager.AnimasiDialog("Seperti yang kamu lihat, volume bangun ini terbentuk dari Luas Alas yang ditumpuk setinggi prisma."));
            yield return StartCoroutine(TungguInputUser()); 
            
            arpyAnim.SetTrigger("doIdle2");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(uiManager.AnimasiDialog("Oleh karena itu, kita bisa menemukan rumus Volume Prisma Segitiga..."));
            yield return StartCoroutine(TungguInputUser()); 
            
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(3);
            yield return StartCoroutine(uiManager.AnimasiDialog("Yaitu Luas Alas x Tinggi!"));
            yield return StartCoroutine(TungguInputUser()); 
            
            SelesaikanLevelVolume();
        }
    }

    private void SpawnUnitCube(int index)
    {
        int xGrid = 0, zGrid = 0, yGrid = 0;
        float xPos = 0, yPos = 0, zPos = 0;

        if (namaBangun == "kubus")
        {
            xGrid = index % 3;          // Kolom (0, 1, 2)
            zGrid = (index / 3) % 3;    // Baris (0, 1, 2)
            yGrid = index / 9;          // Lantai (0, 1, 2)

            xPos = (xGrid - 1) * 0.0002f;
            yPos = (yGrid - 1) * 0.0002f;
            zPos = (zGrid - 1) * 0.00016f;

            GameObject unit = Instantiate(unitCubePrefab, unitContainer);
            unit.transform.localPosition = new Vector3(xPos, yPos, zPos);     
            Renderer rend = unit.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                // Pewarnaan lantai balok
                if (yGrid == 0) rend.material.color = Color.blue;       // Lantai 1 (Alas)
                else if (yGrid == 1) rend.material.color = Color.cyan;                  // Lantai 2
                else rend.material.color = Color.white;                  // Lantai 2
            }

            StartCoroutine(AnimateScale(unit.transform));
        }
        else if (namaBangun == "balok")
        {
            // Matematika Grid Balok (Ukuran 4 x 3 x 2)
            xGrid = index % 4;          // Kolom (0, 1, 2, 3) -> Panjang
            zGrid = (index / 4) % 3;    // Baris (0, 1, 2)    -> Lebar
            yGrid = index / 12;         // Lantai (0, 1)       -> Tinggi

            // Atur posisi lokal (Sesuaikan nilai offset perkaliannya dengan ukuran prefab balokmu)
            xPos = (xGrid - 1.5f) * 0.0002f; // -1.5f agar simetris di tengah wadah ukuran 4 kolom
            yPos = (yGrid - 0.5f) * 0.0002f; // -0.5f agar simetris di tengah wadah ukuran 2 lantai
            zPos = (zGrid - 1.0f) * 0.0002f;

            GameObject unit = Instantiate(unitCubePrefabBalok, unitContainerBalok);
            unit.transform.localPosition = new Vector3(xPos, yPos, zPos);     
            Renderer rend = unit.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                // Pewarnaan lantai balok
                if (yGrid == 0) rend.material.color = Color.blue;       // Lantai 1 (Alas)
                else rend.material.color = Color.cyan;                  // Lantai 2
            }

            StartCoroutine(AnimateScale(unit.transform));
        }

    }

    IEnumerator AnimateScale(Transform target)
    {
        target.localScale = Vector3.zero;
        float elapsed = 0;
        float duration = 0.15f;
        float zScaleValue = 0;
        
        if(namaBangun == "kubus")
        {
            zScaleValue = 0.00016f;
        }
        else if(namaBangun == "balok")
        {
            zScaleValue = 0.0002f;
        }

        Vector3 targetScale = new Vector3(0.0002f, 0.0002f, zScaleValue);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsed / duration);
            yield return null;
        }
        target.localScale = targetScale; // Pastikan angkanya pas di akhir
    }
    private void IsiVolumePrisma(int index)
    {
        if (index == 0)
        {
            isianVolumeAktif = Instantiate(unitPrismaPrefab, PrismaLevel2.transform);
            
            // Posisikan tepat di lantai
            isianVolumeAktif.transform.localPosition = new Vector3(0f, 0f, posisiAwalZ);
            
            // Pipihkan ketebalannya di awal menjadi 0
            Vector3 skalaBawaan = isianVolumeAktif.transform.localScale;
            Vector3 targetSkalaAlas = new Vector3(skalaBawaan.x, skalaBawaan.y, 0.007f);
            // KUNCI ANIMASI: Mulai dari ukuran 0 (tidak terlihat)
            isianVolumeAktif.transform.localScale = Vector3.zero;
            
            isianVolumeAktif.SetActive(true);
            // Jalankan animasi membesar khusus untuk alas
            StartCoroutine(AnimasiMunculAlas(isianVolumeAktif.transform, targetSkalaAlas));
            return;
        }

        // Hitung target panjang (Skala Z)
        float skalaTargetZ = (index + 1) * tambahSkalaZPerTap;

        // POSISI KUNCI DI LANTAI (Tidak digeser lagi!)
        float posisiTargetZ = posisiAwalZ + (skalaTargetZ * pengaliPosisi);;

        StartCoroutine(AnimasiTarikPrisma(isianVolumeAktif.transform, skalaTargetZ, posisiTargetZ));
    }

    IEnumerator AnimasiMunculAlas(Transform target, Vector3 targetScale)
    {
        float elapsed = 0;
        float duration = 0.3f; // Kecepatan munculnya alas (0.3 detik)

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // Animasi membesar dari 0 ke ukuran target alas
            target.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
            yield return null;
        }
        
        // Pastikan ukuran akhirnya pas
        target.localScale = targetScale;
    }

    IEnumerator AnimasiTarikPrisma(Transform target, float targetScaleZ, float targetPosZ)
    {
        Vector3 skalaAwal = target.localScale;
        Vector3 posisiAwal = target.localPosition;
        
        // Tetap pertahankan skala X dan Y aslinya agar tidak gepeng
        Vector3 skalaAkhir = new Vector3(skalaAwal.x, skalaAwal.y, targetScaleZ);
        Vector3 posisiAkhir = new Vector3(0f, 0f, targetPosZ);

        float elapsed = 0;
        float duration = 0.3f; // Kecepatan memanjang (0.3 detik)

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            target.localScale = Vector3.Lerp(skalaAwal, skalaAkhir, t);
            target.localPosition = Vector3.Lerp(posisiAwal, posisiAkhir, t);
            yield return null;
        }
        
        target.localScale = skalaAkhir;
        target.localPosition = posisiAkhir;
    }
    void UpdateUIVolume()
    {
        if(namaBangun == "kubus")
        {
            
            // Update teks counter di layar, misal: "(12 / 27) unit³"
            counterSisi.text = "(" + currentVolume + " / 27)";
            if (currentVolume == 9)
            {
                objekRumus.SetActive(true);
                teksRumus.text = "Luas Alas = s x s";
                penggarisX.SetActive(true);
                penggarisY.SetActive(true);
            }
            else if (currentVolume == 27)
            {
                penggarisZ.SetActive(true);
                teksRumus.text = "V = Luas Alas x Tinggi\nV = s x s x s";
            }
        }
        else
        {
            counterSisi.text = "(" + currentVolume + " / "+ targetLapis +")";
        }
        
    }

    void SelesaikanLevelVolume()
    {
        int levelYangBaruTerbuka = 3; // Selesai tahap rusuk -> lanjut ke tahap 2

        SaveManager.Instance.BukaLevel(namaBangun, levelYangBaruTerbuka);
        
        arpyAnim.SetTrigger("doHU");
        SFXManager.Instance.MainkanArpyNoise(1);

        // 2. Kalkulasi jumlah bintang untuk UI (Level saat ini dikurangi 1)
        int bintangUntukDitampilkan = levelYangBaruTerbuka - 1;

        uiManager.TutupMisi();
        if(namaBangun == "kubus")
        {
            uiManager.ShowCompletionPopup(
                "Level Volume Selesai!", 
                "Hebat! Kamu mengisi 3 lapis berisi 9 kubus. Jadi 3 x 3 x 3 = 27!", 
                bintangUntukDitampilkan);
        } 
        else if(namaBangun == "prisma segitiga")
        {
            uiManager.ShowCompletionPopup(
                "Level Volume Selesai!", 
                "Hebat! Kamu telah membuktikan bahwa Volume Prisma = Luas Alas x Tinggi.", 
                bintangUntukDitampilkan);
        }
        else if(namaBangun == "balok")
        {
            uiManager.ShowCompletionPopup(
                "Level Volume Selesai!", 
                "Hebat! Kamu telah membuktikan bahwa Volume Balok = Panjang x Lebar x Tinggi.", 
                bintangUntukDitampilkan);
        }
    }

    // LEvel 3 Jaring jaring Kubus -----------------------------------------------------------------------------------------------
    // LEvel 3 Jaring jaring Kubus -----------------------------------------------------------------------------------------------
    // LEvel 3 Jaring jaring Kubus -----------------------------------------------------------------------------------------------
    // ---------------------------------------------------------
    // 1. INISIALISASI LEVEL
    // ---------------------------------------------------------

    public void StartLevel3()
    {
        KubusLevel1.SetActive(false);
        prismaLevel1.SetActive(false);
        balokLevel1.SetActive(false);
        
        if (namaBangun == "kubus") KubusLevel3.SetActive(false);
        else if (namaBangun == "prisma segitiga") PrismaLevel3.SetActive(false); 
        else if (namaBangun == "balok") BalokLevel3.SetActive(false); 

        // Reset Data
        countSisiTerbuka = 0;
        openStep = 0;
        varianTerbuka = 0;
        TagLevel.text = "Level 3";
        
        uiManager.BukaKotakMisi();
        UpdateUIPermukaan();

        StartCoroutine(SequenceIntroLevel3());
    }

    IEnumerator SequenceIntroLevel3()
    {
        uiManager.OnMulaiLevel(namaBangun, 3);
        yield return new WaitForSeconds(1.5f);

        faseSekarang = FaseLevel3.Pengenalan;
        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(1);
        yield return StartCoroutine(uiManager.AnimasiDialog("Selamat kamu sudah mencapai level 3!"));
        yield return StartCoroutine(TungguInputUser());
        

        if (namaBangun == "kubus") 
        {
            CubeAnimation animLevel2 = KubusLevel2.GetComponent<CubeAnimation>();
            animLevel2.HilangkanKubus();
        }
        else if (namaBangun == "prisma segitiga")
        {
            CubeAnimation animLevel2 = PrismaLevel2.GetComponent<CubeAnimation>();
            animLevel2.HilangkanKubus();
        }
        else if (namaBangun == "balok")
        {
            CubeAnimation animLevel2 = BalokLevel2.GetComponent<CubeAnimation>();
            animLevel2.HilangkanKubus();
        }
        LoadLevel3();

        if (namaBangun == "kubus") efekJaring.SetGlow(true); // Mulai bersinar
        else if (namaBangun == "prisma segitiga") efekJaringPrisma.SetGlow(true); // Mulai bersinar
        if (namaBangun == "balok") efekJaringBalok.SetGlow(true); // Mulai bersinar
        
        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(uiManager.AnimasiDialog("Di level ini kita akan mempelajari jaring-jaring dan luas permukaan!"));
        yield return StartCoroutine(TungguInputUser());
        
        if (namaBangun == "kubus") efekJaring.SetGlow(false); // Mulai bersinar
        else if (namaBangun == "prisma segitiga") efekJaringPrisma.SetGlow(false); // Mulai bersinar
        else if (namaBangun == "balok") efekJaringBalok.SetGlow(false); // Mulai bersinar
        
        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(uiManager.AnimasiDialog("Jaring-jaring adalah bentuk bangun ruang yang dibuka menjadi bangun datar."));
        yield return StartCoroutine(TungguInputUser());
        
        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(uiManager.AnimasiDialog("Sekarang coba ketuk "+namaBangun+" ini untuk mengupasnya!"));
        
        faseSekarang = FaseLevel3.KupasUtama;
        isLevel3Active = true;
    }

    public void LoadLevel3()
    {
        if (namaBangun == "kubus")
        {
            KubusLevel3.SetActive(true);
            CubeAnimation anim = KubusLevel3.GetComponent<CubeAnimation>();
            if (anim != null) anim.MunculkanKubus();
        }
        else if (namaBangun == "prisma segitiga")
        {
            PrismaLevel3.SetActive(true);
            CubeAnimation anim = PrismaLevel3.GetComponent<CubeAnimation>();
            if (anim != null) anim.MunculkanKubus();
        }
        else if (namaBangun == "balok")
        {
            BalokLevel3.SetActive(true);
            CubeAnimation anim = BalokLevel3.GetComponent<CubeAnimation>();
            if (anim != null) anim.MunculkanKubus();
        }
    
    }

    // ---------------------------------------------------------
    // 2. INPUT & RAYCAST
    public void DoRaycastLevel3(Vector2 screenPosition)
    {
        if (!isLevel3Active) return;

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // A. Logika untuk Variasi (Fase TampilVariasi)
            VarianController vCtrl = hit.collider.GetComponentInParent<VarianController>();
            if (vCtrl != null && faseSekarang == FaseLevel3.TampilVariasi)
            {
                SFXManager.Instance.MainkanPop();

                vCtrl.TerkenaTap();
                return;
            }

            // B. Logika untuk Kubus Utama (Tag: SisiKubus)
            if (hit.collider.CompareTag("SisiKubus") || hit.collider.CompareTag("SisiPrisma") || hit.collider.CompareTag("SisiBalok"))
            {
                SFXManager.Instance.MainkanClick2();
                switch (faseSekarang)
                {
                    case FaseLevel3.KupasUtama:
                        LogikaKupasUtama();
                        break;
                    case FaseLevel3.HitungLuas:
                        HandleColoringSisi(hit.collider.gameObject);
                        break;
                }
            }
        }
    }

    // ---------------------------------------------------------
    // 3. LOGIKA PER FASE
    // FASE 1: Mengupas Kubus Utama Step-by-Step
    void LogikaKupasUtama()
    {
        namaMisi1.text = "Buka "+namaBangun;
        uiManager.BukaMisi(1);

        openStep++;
        if (namaBangun == "kubus") jaringAnimator.SetTrigger("doBuka");
        else if (namaBangun == "prisma segitiga") jaringAnimatorPrisma.SetTrigger("doBuka"); // Pastikan referensi ini ada
        else if (namaBangun == "balok") jaringAnimatorBalok.SetTrigger("doBuka"); // Pastikan referensi ini ada
        UpdateUIPermukaan();

        if (openStep >= targetBuka)
        {
            StartCoroutine(TransisiKeVariasi());
        }
    }

    // FASE 2: Memunculkan 3 Variasi
    IEnumerator TransisiKeVariasi()
    {
        faseSekarang = FaseLevel3.TampilVariasi;
        yield return new WaitForSeconds(1f);
        arpyAnim.SetTrigger("doHU");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(uiManager.AnimasiDialog("Bagus! Sekarang "+namaBangun+" telah menjadi jaring-jaring."));
        yield return StartCoroutine(TungguInputUser());
        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(uiManager.AnimasiDialog("Tapi pola jaring-jaring tidak hanya satu lho."));
        yield return StartCoroutine(TungguInputUser());
        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(uiManager.AnimasiDialog("Ayo kita lihat pola lainnya."));
        MunculkanTigaVariasi();
        yield return StartCoroutine(TungguInputUser());
        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(uiManager.AnimasiDialog("Ketuk "+namaBangun+" lain untuk melihat jaring-jaring lain."));

        namaMisi2.text = "Buka Variasi";
        uiManager.BukaMisi(2);
        UpdateUIPermukaan();
        
    }

    void MunculkanTigaVariasi()
    {
        SFXManager.Instance.MainkanPop();
        if(namaBangun == "kubus")
        {
            CubeAnimation animLevel3 = KubusLevel3.GetComponent<CubeAnimation>();
            animLevel3.HilangkanKubus();
        }
        else if(namaBangun == "prisma segitiga")
        {
            CubeAnimation animLevel3P = PrismaLevel3.GetComponent<CubeAnimation>();
            animLevel3P.HilangkanKubus();
        }
        else if(namaBangun == "balok")
        {
            CubeAnimation animLevel3 = BalokLevel3.GetComponent<CubeAnimation>();
            animLevel3.HilangkanKubus();
        }

        float spacing = 0.22f; // Jarak antar kubus
        if(namaBangun == "kubus")
        {
            for (int i = 0; i < prefabVariasi.Length; i++)
            {
                Vector3 offset = new Vector3((i - 1) * spacing, 0, 0.1f); // Muncul sedikit di depan
                Vector3 spawnPos = jaringAnimator.transform.position + offset;

                GameObject varCube = Instantiate(prefabVariasi[i], spawnPos, Quaternion.identity);
                // ========================================================
                // INTEGRASI ANIMASI IN (MUNCUL)
                // ========================================================
                CubeAnimation anim = varCube.GetComponent<CubeAnimation>();
                if (anim != null)
                {
                    // Kunci spawnPos sebagai koordinat target akhir animasi
                    anim.InisialisasiPosisi(); 
                    // Jalankan efek tumbuh dari kecil ke besar
                    anim.MunculkanKubus();
                }
                // ========================================================
                // Berikan ID secara otomatis berdasarkan urutan Array
                VarianController ctrl = varCube.GetComponent<VarianController>();
                if (ctrl != null) ctrl.variantID = i; 

                spawnedVariations.Add(varCube);
            }
        }
        else if(namaBangun == "prisma segitiga")
        {
            for (int i = 0; i < prefabVariasiPrisma.Length; i++)
            {
                Vector3 offset = new Vector3((i - 1) * spacing, 0, 0.5f); // Muncul sedikit di depan
                Vector3 spawnPos = jaringAnimatorPrisma.transform.position + offset;

                GameObject varCube = Instantiate(prefabVariasiPrisma[i], spawnPos, Quaternion.identity);
                // ========================================================
                // INTEGRASI ANIMASI IN (MUNCUL)
                // ========================================================
                CubeAnimation anim = varCube.GetComponent<CubeAnimation>();
                if (anim != null)
                {
                    // Kunci spawnPos sebagai koordinat target akhir animasi
                    anim.InisialisasiPosisi(); 
                    // Jalankan efek tumbuh dari kecil ke besar
                    anim.MunculkanKubus();
                }
                // ========================================================
                // Berikan ID secara otomatis berdasarkan urutan Array
                VarianController ctrl = varCube.GetComponent<VarianController>();
                if (ctrl != null) ctrl.variantID = i; 

                spawnedVariations.Add(varCube);
            }
            
        }
        else if(namaBangun == "balok")
        {
            for (int i = 0; i < prefabVariasiBalok.Length; i++)
            {
                Vector3 offset = new Vector3((i - 1) * spacing, 0, 0.1f); // Muncul sedikit di depan
                Vector3 spawnPos = jaringAnimatorPrisma.transform.position + offset;

                GameObject varCube = Instantiate(prefabVariasiBalok[i], spawnPos, Quaternion.identity);
                // ========================================================
                // INTEGRASI ANIMASI IN (MUNCUL)
                // ========================================================
                CubeAnimation anim = varCube.GetComponent<CubeAnimation>();
                if (anim != null)
                {
                    // Kunci spawnPos sebagai koordinat target akhir animasi
                    anim.InisialisasiPosisi(); 
                    // Jalankan efek tumbuh dari kecil ke besar
                    anim.MunculkanKubus();
                }
                // ========================================================
                // Berikan ID secara otomatis berdasarkan urutan Array
                VarianController ctrl = varCube.GetComponent<VarianController>();
                if (ctrl != null) ctrl.variantID = i; 

                spawnedVariations.Add(varCube);
            }
            
        }
    }
    public void LaporVarianTerbuka()
    {
        varianTerbuka++;
        UpdateUIPermukaan();

        if (varianTerbuka >= prefabVariasi.Length)
        {
            StartCoroutine(LanjutKeFaseHitungLuas());
        }
    }

    // FASE 3: Menghitung Luas Permukaan
    IEnumerator LanjutKeFaseHitungLuas()
    {
        yield return new WaitForSeconds(1f);
        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(uiManager.AnimasiDialog("Lihat ada beragam pola jaring-jaring!"));
        yield return StartCoroutine(TungguInputUser());
        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(uiManager.AnimasiDialog("Meskipun beragam, ini masih jaring-jaring dari "+namaBangun+"!"));
        yield return StartCoroutine(TungguInputUser());
        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(uiManager.AnimasiDialog("Dan masih banyak bentuk lainnya."));
        yield return StartCoroutine(TungguInputUser());
        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(uiManager.AnimasiDialog("Sekarang mari kembali fokus ke "+namaBangun+" utama kita dan pelajari luas permukaannya!"));
        yield return StartCoroutine(TungguInputUser());
        
        SembunyikanVariasi();
        LoadLevel3();
        faseSekarang = FaseLevel3.HitungLuas;
        namaMisi3.text = "Hitung "+targetSisi+" Sisi";
        uiManager.BukaMisi(3);
        
        if (namaBangun == "kubus") efekJaring.SetGlow(true); // Mulai bersinar
        else if (namaBangun == "prisma segitiga") efekJaringPrisma.SetGlow(true); // Mulai bersinar
        if (namaBangun == "balok") efekJaringBalok.SetGlow(true); // Mulai bersinar

        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(uiManager.AnimasiDialog("Kita tahu bahwa "+namaBangun+" memiliki "+targetSisi+" sisi. Sisi ini bisa juga disebut sebagai permukaan."));
        yield return StartCoroutine(TungguInputUser());
    
        arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(uiManager.AnimasiDialog("Bantu aku memilih ke-"+targetSisi+" permukaan ini lagi! Ketuk permukaannya."));
        yield return StartCoroutine(TungguInputUser());

        if (namaBangun == "kubus") efekJaring.SetGlow(false); // Matikan bersinar
        else if (namaBangun == "prisma segitiga") efekJaringPrisma.SetGlow(false); // Matikan bersinar
        if (namaBangun == "balok") efekJaringBalok.SetGlow(false); // Matikan bersinar

    }

    void SembunyikanVariasi()
    {
        SFXManager.Instance.MainkanTransisiWoosh();
        foreach (GameObject g in spawnedVariations)
        {
            g.SetActive(false); // Objek hilang dari layar tapi masih ada di memori
        }
        
    }

    void HandleColoringSisi(GameObject boneYangDitap)
    {
        string n = boneYangDitap.name;
        Renderer rend = null;

        // Pemetaan Bone ke Mesh Visual
        if(namaBangun == "kubus")
        {
            if (n.Contains("001")) rend = GameObject.Find("CubeBawah")?.GetComponent<Renderer>();
            else if (n.Contains("002")) rend = GameObject.Find("Sisi2")?.GetComponent<Renderer>();
            else if (n.Contains("003")) rend = GameObject.Find("Sisi1")?.GetComponent<Renderer>();
            else if (n.Contains("004")) rend = GameObject.Find("Sisi4")?.GetComponent<Renderer>();
            else if (n.Contains("005")) rend = GameObject.Find("Sisi5")?.GetComponent<Renderer>();
            else if (n.Contains("006")) rend = GameObject.Find("Sisi3")?.GetComponent<Renderer>();
        }
        else if(namaBangun == "prisma segitiga")
        {
            
            if (n == "Bone") rend = GameObject.Find("CubeBawah")?.GetComponent<Renderer>();
            else if (n.Contains("001")) rend = GameObject.Find("Sisi1")?.GetComponent<Renderer>();
            else if (n.Contains("002")) rend = GameObject.Find("Sisi2")?.GetComponent<Renderer>();
            else if (n.Contains("003")) rend = GameObject.Find("Sisi3")?.GetComponent<Renderer>();
            else if (n.Contains("004")) rend = GameObject.Find("Sisi4")?.GetComponent<Renderer>();
        }
        else if(namaBangun == "balok")
        {
            if (n.Contains("001")) rend = GameObject.Find("CubeBawah")?.GetComponent<Renderer>();
            else if (n.Contains("002")) rend = GameObject.Find("CubeBelakang")?.GetComponent<Renderer>();
            else if (n.Contains("003")) rend = GameObject.Find("CubeAtas")?.GetComponent<Renderer>();
            else if (n.Contains("004")) rend = GameObject.Find("CubeKanan")?.GetComponent<Renderer>();
            else if (n.Contains("005")) rend = GameObject.Find("CubeKiri")?.GetComponent<Renderer>();
            else if (n.Contains("006")) rend = GameObject.Find("CubeDepan")?.GetComponent<Renderer>();
        }

        if (rend != null && rend.material.color != Color.yellow)
        {
            rend.material.color = Color.yellow;
            countSisiTerbuka++;
            UpdateUIPermukaan();

            // Matikan collider bone agar tidak di-tap dua kali
            if (boneYangDitap.TryGetComponent<BoxCollider>(out BoxCollider col)) col.enabled = false;

            if (countSisiTerbuka >= targetSisi) StartCoroutine(SequenceSelesaiLevel3());
        }
    }

    // FASE 4: Penjelasan Rumus & Selesai
    IEnumerator SequenceSelesaiLevel3()
    {
        isLevel3Active = false;
        if(namaBangun == "kubus")
        {
            penggarisXPermukaan.SetActive(true);
            penggarisYPermukaan.SetActive(true);
            papanRumusLP.SetActive(true);
        }
        else if(namaBangun == "prisma segitiga")
        {
            papanRumusLPPrisma.SetActive(true);   
        }
        else if(namaBangun == "balok")
        {
            papanRumusLPBalok.SetActive(true);   
        }

        yield return new WaitForSeconds(0.5f);
        
        if(namaBangun == "kubus")
        {
            arpyAnim.SetTrigger("doHU");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(uiManager.AnimasiDialog("Lengkap! Ada 6 persegi yang membentuk luas permukaan."));
            yield return StartCoroutine(TungguInputUser());
            
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(uiManager.AnimasiDialog("Karena rumus persegi sisi x sisi dan ada 6 persegi,"));
            yield return StartCoroutine(TungguInputUser());
            
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(uiManager.AnimasiDialog("Maka rumusnya adalah: Luas = 6 x (sisi x sisi)"));
            yield return StartCoroutine(TungguInputUser());
            
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(4);
            yield return StartCoroutine(uiManager.AnimasiDialog("Jika sisi = 3, maka luas permukaannya adalah\n= 6 x (3 x 3) \n= 6 x 9 = 54."));
            yield return StartCoroutine(TungguInputUser());
        }
        else if(namaBangun == "prisma segitiga")
        {
            arpyAnim.SetTrigger("doHU");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(uiManager.AnimasiDialog("Lengkap! Ada 3 persegi panjang dan 2 segitiga yang membentuk luas permukaan."));
            yield return StartCoroutine(TungguInputUser());
            
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(uiManager.AnimasiDialog("Karena rumus luas persegi panjang = panjang x lebar dan luas segitiga = 1/2 x alas x tinggi"));
            yield return StartCoroutine(TungguInputUser());
            
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(uiManager.AnimasiDialog("Maka rumus luas permukaan = 3 (panjang x lebar) + 2 (1/2 x alas x tinggi)"));
            yield return StartCoroutine(TungguInputUser());
        }
        else if(namaBangun == "balok")
        {
            efekJaringBalok.SetGlow(true); // Mulai bersinar

            arpyAnim.SetTrigger("doHU");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(uiManager.AnimasiDialog("Lengkap! Ada 6 persegi panjang yang membentuk luas permukaan."));
            yield return StartCoroutine(TungguInputUser());
            
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(uiManager.AnimasiDialog("Pada balok tidak semua sisinya harus persegi panjang."));
            yield return StartCoroutine(TungguInputUser());
            
            arpyAnim.SetTrigger("doIdle2");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(uiManager.AnimasiDialog("Tetapi jika ada 4 dari 6 sisinya berbentuk persegi panjang sedangkan sisanya persegi, maka itu adalah balok."));
            yield return StartCoroutine(TungguInputUser());

            WarnaiSisiBalokLevel3();
            
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(uiManager.AnimasiDialog("Untuk menghitung luas permukaannya kita cukup menghitung setiap bangun datar yang ada."));
            yield return StartCoroutine(TungguInputUser());

            efekJaringBalok.SetGlow(false);

            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(uiManager.AnimasiDialog("Pada balok terdapat 3 pasang sisi yang luasnya sama."));
            yield return StartCoroutine(TungguInputUser());
            
            arpyAnim.SetTrigger("doIdle2");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(uiManager.AnimasiDialog("Karena rumus luas persegi panjang = panjang x lebar dan disini kita memiliki panjang, lebar dan tinggi."));
            yield return StartCoroutine(TungguInputUser());
            
            arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(uiManager.AnimasiDialog("Maka rumus luas permukaan = 2 (panjang x lebar) + 2 (lebar x tinggi) + 2(panjang x tinggi)"));
            yield return StartCoroutine(TungguInputUser());
        }

        DialogSelesaiLevel();
        TampilkanGaleriVariasi();
    }
    void WarnaiSisiBalokLevel3()
    {
        // Pastikan objek BalokLevel3 sedang aktif di Scene
        if (BalokLevel3 != null)
        {
            // Pindai semua anak langsung dari BalokLevel3
            foreach (Transform sisi in BalokLevel3.transform)
            {
                Renderer rend = sisi.GetComponent<Renderer>();
                if (rend == null) continue;

                string namaAman = sisi.name.ToLower();

                // Eksekusi penyamaan warna berdasarkan nama anak
                if (namaAman.Contains("atas") || namaAman.Contains("bawah"))
                {
                    rend.material.color = Color.blue;
                }
                else if (namaAman.Contains("kiri") || namaAman.Contains("kanan"))
                {
                    rend.material.color = new Color(1.0f, 0.5f, 0.0f); // Oranye
                }
                else if (namaAman.Contains("depan") || namaAman.Contains("belakang"))
                {
                    rend.material.color = Color.white;
                }
            }
            Debug.Log("🎨 [VISUALISASI] Sisi balok berhasil dikelompokkan berdasarkan warna!");
        }
    }
    public void TampilkanGaleriVariasi()
    {
        float spacing = 0.2f; // Beri jarak lebih lebar agar terlihat seperti galeri
        float jarak = 0f;
        for (int i = 0; i < spawnedVariations.Count; i++)
        {
            GameObject varCube = spawnedVariations[i];
            
            // 1. Aktifkan kembali
            varCube.SetActive(true);

            if(namaBangun == "kubus") jarak = 0.3f;
            else if(namaBangun == "prisma segitiga") jarak = 0.2f;
            else if(namaBangun == "balok") jarak = 0.2f;
            // 2. Atur posisi melingkar atau berjajar rapi
            Vector3 offset = new Vector3((i - 1) * spacing, 0, jarak);
            if(namaBangun == "kubus") varCube.transform.position = jaringAnimator.transform.position + offset;
            if(namaBangun == "prisma segitiga") varCube.transform.position = jaringAnimatorPrisma.transform.position + offset;
            if(namaBangun == "balok") varCube.transform.position = jaringAnimatorBalok.transform.position + offset;
            
            // ========================================================
            // INTEGRASI ANIMASI IN (MUNCUL)
            // ========================================================
            CubeAnimation anim = varCube.GetComponent<CubeAnimation>();
            if (anim != null)
            {
                // Kunci spawnPos sebagai koordinat target akhir animasi
                anim.InisialisasiPosisi(); 
                // Jalankan efek tumbuh dari kecil ke besar
                anim.MunculkanKubus();
            }
            // ========================================================
            // Panggil fungsi PaksaBuka dari VarianController
            VarianController ctrl = varCube.GetComponent<VarianController>();
            if (ctrl != null)
            {
                ctrl.PaksaBuka();
            } 
        }

    }

    // ---------------------------------------------------------
    // 4. HELPER FUNCTIONS
    // ---------------------------------------------------------

    void UpdateUIPermukaan()
    {
        if (faseSekarang == FaseLevel3.KupasUtama)
            counterSisi.text = "( " + openStep + " / "+targetBuka+" )";
        else if (faseSekarang == FaseLevel3.TampilVariasi)
            counterText.text = "( " + varianTerbuka + " / 3 )";
        else if (faseSekarang == FaseLevel3.HitungLuas)
            counterRusuk.text = "( " + countSisiTerbuka + " / "+targetSisi+" )";
    }
    

    public void DialogSelesaiLevel()
    {
        int levelYangBaruTerbuka = 4;

        SaveManager.Instance.BukaLevel(namaBangun, levelYangBaruTerbuka);
        
        arpyAnim.SetTrigger("doHU");
        SFXManager.Instance.MainkanArpyNoise(1);

        // 2. Kalkulasi jumlah bintang untuk UI (Level saat ini dikurangi 1)
        int bintangUntukDitampilkan = levelYangBaruTerbuka - 1;

        uiManager.TutupMisi();
        uiManager.ShowCompletionPopup(
            "Level "+namaBangun+" Selesai!", 
            "Hebat! Kamu telah menyelesaikan seluruh level "+namaBangun+"!", 
            bintangUntukDitampilkan);
        faseSekarang = FaseLevel3.Selesai;
    }

    void Update()
    {
        if (Pointer.current == null) return;

        // A. Catat posisi awal saat jari menyentuh layar
        if (Pointer.current.press.wasPressedThisFrame)
        {
            startTapPosition = Pointer.current.position.ReadValue();
        }

        // B. Lakukan Raycast HANYA saat jari dilepas (wasReleasedThisFrame)
        if (Pointer.current.press.wasReleasedThisFrame)
        {
            Vector2 endTapPosition = Pointer.current.position.ReadValue();
            float distance = Vector2.Distance(startTapPosition, endTapPosition);

            // Cek: Jika jari bergeser lebih dari threshold, berarti user sedang "Swap/Rotate"
            // Jika kurang, berarti user memang berniat "Tap/Click"
            if (distance < tapThreshold)
            {
                if (isLevel3Active) // Tambahkan logika Level 3 di sini
                {
                    
                    DoRaycastLevel3(endTapPosition);
                }
                else if (isLevel2Active) 
                {
                    HandleTapVolume();
                }
                else
                {
                    DoRaycast(endTapPosition);
                }
                ;
            }
        }
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

        if (jumlahVertexDitemukan == 0 && jumlahSisiDitemukan >= targetSisi) {
            StartLevelSudut();
        } 
        else if (jumlahVertexDitemukan >= targetSudut && jumlahRusukDitemukan == 0) {
            StartLevelRusuk();
        }
        else if (jumlahRusukDitemukan >= targetRusuk && currentVolume == 0) {
            StartLevelVolume();
        }
        else if (currentVolume >= targetLapis && openStep == 0)
        {
            StartLevel3();
        }
        else {
            StartCoroutine(SequenceKembaliKeMenu());
        }
    }

    IEnumerator SequenceKembaliKeMenu()
    {
        uiManager.OnMulaiLevel(namaBangun, 4);
        yield return new WaitForSeconds(1.5f);

        SceneTransition.Instance.PindahScene("MainMenu");
    }
 
    private IEnumerator TungguInputUser()
    {
        // 1. Munculkan petunjuk visual "Tap untuk Lanjut" di UI
        uiManager.SetTapIndicator(true);

        // 2. Beri jeda sangat singkat agar klik tombol sebelumnya tidak langsung terhitung
        yield return new WaitForSeconds(0.2f);

        // 3. Loop terus selama user BELUM menekan layar
        while (!Pointer.current.press.wasPressedThisFrame)
        {
            yield return null; // Tunggu ke frame berikutnya
        }

        // 4. Sembunyikan petunjuk setelah diklik
        uiManager.SetTapIndicator(false);
        SFXManager.Instance.MainkanClick();
        
        // 5. Beri jeda sedikit agar tidak langsung loncat ke dialog berikutnya secara brutal
        yield return new WaitForSeconds(0.1f);
    }

}
