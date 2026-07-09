using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Level3Manager : MonoBehaviour
{
    private GameSessionManager gsm => GameSessionManager.Instance;

    public enum FaseLevel3 { Pengenalan, KupasUtama, TampilVariasi, HitungLuas, Selesai }
    [Header("Level 1 & 2")]
    public GameObject KubusLevel1;
    public GameObject KubusLevel2;
    public GameObject PrismaLevel1;
    public GameObject PrismaLevel2;
    public GameObject BalokLevel1;
    public GameObject BalokLevel2;
    public GameObject LimasLevel1;
    public GameObject LimasLevel2;

    [Header("Status Level 3")]
    public FaseLevel3 faseSekarang = FaseLevel3.Pengenalan;
    public int countSisiTerbuka = 0;
    public int varianTerbuka = 0;
    public int openStep = 0;

    [Header("Level 3 Kubus")]
    public GameObject KubusLevel3;
    public Animator jaringAnimator;
    public GameObject penggarisXPermukaan;
    public GameObject penggarisYPermukaan;
    public GameObject papanRumusLP; 
    public GameObject[] prefabVariasi; 
    private List<GameObject> spawnedVariations = new List<GameObject>(); 

    [Header("Level 3 Prisma")]
    public GameObject PrismaLevel3;
    public Animator jaringAnimatorPrisma;
    public GameObject papanRumusLPPrisma; 
    public GameObject[] prefabVariasiPrisma; 
    
    [Header("Level 3 Balok")]
    public GameObject BalokLevel3;
    public Animator jaringAnimatorBalok;
    public GameObject papanRumusLPBalok; 
    public GameObject[] prefabVariasiBalok; 


    [Header("Level 3 Limas")]
    public GameObject LimasLevel3;
    public Animator jaringAnimatorLimas;
    public GameObject papanRumusLPLimas; 
    public GameObject[] prefabVariasiLimas; 


    [Header("Referensi Unsur (Glow)")]
    public ObjectGlow efekJaring;
    public ObjectGlow efekJaringPrisma;
    public ObjectGlow efekJaringBalok;
    public ObjectGlow efekJaringLimas;

    // =========================================================
    // INPUT KHUSUS LEVEL 3
    // =========================================================
    void Update()
    {
        if (Pointer.current == null) return;

        if (Pointer.current.press.wasPressedThisFrame)
            gsm.startTapPosition = Pointer.current.position.ReadValue();

        if (Pointer.current.press.wasReleasedThisFrame)
        {
            Vector2 endTapPosition = Pointer.current.position.ReadValue();
            if (Vector2.Distance(gsm.startTapPosition, endTapPosition) < gsm.tapThreshold)
            {
                DoRaycastLevel3(endTapPosition);
            }
        }
    }

    public void DoRaycastLevel3(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            VarianController vCtrl = hit.collider.GetComponentInParent<VarianController>();
            if (vCtrl != null && faseSekarang == FaseLevel3.TampilVariasi)
            {
                SFXManager.Instance.MainkanPop();
                vCtrl.TerkenaTap();
                return;
            }

            if (hit.collider.CompareTag("SisiKubus") || hit.collider.CompareTag("SisiPrisma") || hit.collider.CompareTag("SisiBalok") || hit.collider.CompareTag("SisiLimas"))
            {
                SFXManager.Instance.MainkanClick2();
                switch (faseSekarang)
                {
                    case FaseLevel3.KupasUtama: LogikaKupasUtama(); break;
                    case FaseLevel3.HitungLuas: HandleColoringSisi(hit.collider.gameObject); break;
                }
            }
        }
    }

    // =========================================================
    // AWAL LEVEL 3
    // =========================================================
    public void StartLevel3()
    {
        // Matikan objek level 1 yang mungkin masih menyala
        Level1Manager lvl1 = gsm.level1Manager as Level1Manager;
        if (lvl1 != null) {
            lvl1.KubusLevel1.SetActive(false);
            lvl1.prismaLevel1.SetActive(false);
            lvl1.balokLevel1.SetActive(false);
            lvl1.limasLevel1.SetActive(false);
        }
        
        if (gsm.namaBangun == "kubus") KubusLevel3.SetActive(false);
        else if (gsm.namaBangun == "prisma segitiga") PrismaLevel3.SetActive(false); 
        else if (gsm.namaBangun == "balok") BalokLevel3.SetActive(false); 
        else if (gsm.namaBangun == "limas persegi") LimasLevel3.SetActive(false); 

        countSisiTerbuka = 0; openStep = 0; varianTerbuka = 0;
        gsm.TagLevel.text = "Level 3";
        gsm.uiManager.BukaKotakMisi();
        UpdateUIPermukaan();

        StartCoroutine(SequenceIntroLevel3());
    }

    IEnumerator SequenceIntroLevel3()
    {
        gsm.uiManager.OnMulaiLevel(gsm.namaBangun, 3);
        yield return new WaitForSeconds(1.5f);

        faseSekarang = FaseLevel3.Pengenalan;
        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(1);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Selamat kamu sudah mencapai level 3!"));
        yield return StartCoroutine(gsm.TungguInputUser());

        LoadLevel3();

        if (gsm.namaBangun == "kubus") efekJaring.SetGlow(true); 
        else if (gsm.namaBangun == "prisma segitiga") efekJaringPrisma.SetGlow(true); 
        if (gsm.namaBangun == "balok") efekJaringBalok.SetGlow(true); 
        if (gsm.namaBangun == "limas persegi") efekJaringLimas.SetGlow(true); 
        
        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Di level ini kita akan mempelajari jaring-jaring dan luas permukaan!"));
        yield return StartCoroutine(gsm.TungguInputUser());
        
        if (efekJaring != null) efekJaring.SetGlow(false); 
        if (efekJaringPrisma != null) efekJaringPrisma.SetGlow(false); 
        if (efekJaringBalok != null) efekJaringBalok.SetGlow(false); 
        if (efekJaringLimas != null) efekJaringLimas.SetGlow(false); 
        
        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Jaring-jaring adalah bentuk bangun ruang yang dibuka menjadi bangun datar."));
        yield return StartCoroutine(gsm.TungguInputUser());
        
        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Sekarang coba ketuk "+gsm.namaBangun+" ini untuk mengupasnya!"));
        
        faseSekarang = FaseLevel3.KupasUtama;
    }

    public void LoadLevel3()
    {
        if (gsm.namaBangun == "kubus") {
            KubusLevel1.GetComponent<CubeAnimation>()?.HilangkanKubus();
            KubusLevel2.GetComponent<CubeAnimation>()?.HilangkanKubus();

            KubusLevel3.SetActive(true);
            KubusLevel3.GetComponent<CubeAnimation>()?.MunculkanKubus();
        } else if (gsm.namaBangun == "prisma segitiga") {
            PrismaLevel1.GetComponent<CubeAnimation>()?.HilangkanKubus();
            PrismaLevel2.GetComponent<CubeAnimation>()?.HilangkanKubus();

            PrismaLevel3.SetActive(true);
            PrismaLevel3.GetComponent<CubeAnimation>()?.MunculkanKubus();
        } else if (gsm.namaBangun == "balok") {
            BalokLevel1.GetComponent<CubeAnimation>()?.HilangkanKubus();
            BalokLevel2.GetComponent<CubeAnimation>()?.HilangkanKubus();

            BalokLevel3.SetActive(true);
            BalokLevel3.GetComponent<CubeAnimation>()?.MunculkanKubus();
        }
        else if (gsm.namaBangun == "limas persegi") {
            LimasLevel1.GetComponent<CubeAnimation>()?.HilangkanKubus();
            LimasLevel2.GetComponent<CubeAnimation>()?.HilangkanKubus();

            LimasLevel3.SetActive(true);
            LimasLevel3.GetComponent<CubeAnimation>()?.MunculkanKubus();
        }
    }

    // =========================================================
    // LOGIKA PER FASE LEVEL 3
    // =========================================================
    void LogikaKupasUtama()
    {
        gsm.uiManager.namaMisi1.text = "Buka "+gsm.namaBangun; // Meminjam slot teks UI Misi 1
        gsm.uiManager.BukaMisi(1);

        openStep++;
        if (gsm.namaBangun == "kubus") jaringAnimator.SetTrigger("doBuka");
        else if (gsm.namaBangun == "prisma segitiga") jaringAnimatorPrisma.SetTrigger("doBuka"); 
        else if (gsm.namaBangun == "balok") jaringAnimatorBalok.SetTrigger("doBuka"); 
        else if (gsm.namaBangun == "limas persegi") jaringAnimatorLimas.SetTrigger("doBuka"); 
        
        UpdateUIPermukaan();

        if (openStep >= gsm.targetBuka) StartCoroutine(TransisiKeVariasi());
    }

    IEnumerator TransisiKeVariasi()
    {
        faseSekarang = FaseLevel3.TampilVariasi;
        yield return new WaitForSeconds(1f);
        gsm.arpyAnim.SetTrigger("doHU");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Bagus! Sekarang "+gsm.namaBangun+" telah menjadi jaring-jaring."));
        yield return StartCoroutine(gsm.TungguInputUser());
        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Tapi pola jaring-jaring tidak hanya satu lho."));
        yield return StartCoroutine(gsm.TungguInputUser());
        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Ayo kita lihat pola lainnya."));
        
        MunculkanTigaVariasi();
        
        yield return StartCoroutine(gsm.TungguInputUser());
        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Ketuk "+gsm.namaBangun+" lain untuk melihat jaring-jaring lain."));

        gsm.uiManager.namaMisi2.text = "Buka Variasi";
        gsm.uiManager.BukaMisi(2);
        UpdateUIPermukaan();
    }

    void MunculkanTigaVariasi()
    {
        SFXManager.Instance.MainkanPop();
        if(gsm.namaBangun == "kubus") KubusLevel3.GetComponent<CubeAnimation>()?.HilangkanKubus();
        else if(gsm.namaBangun == "prisma segitiga") PrismaLevel3.GetComponent<CubeAnimation>()?.HilangkanKubus();
        else if(gsm.namaBangun == "balok") BalokLevel3.GetComponent<CubeAnimation>()?.HilangkanKubus();
        else if(gsm.namaBangun == "limas persegi") LimasLevel3.GetComponent<CubeAnimation>()?.HilangkanKubus();

        float spacing = 0.22f; 
        GameObject[] arrayTarget = null;
        Transform titikPusat = null;
        float zOffset = 0.1f;

        if(gsm.namaBangun == "kubus") { arrayTarget = prefabVariasi; titikPusat = jaringAnimator.transform; }
        else if(gsm.namaBangun == "prisma segitiga") { arrayTarget = prefabVariasiPrisma; titikPusat = jaringAnimatorPrisma.transform; zOffset = 0.5f; }
        else if(gsm.namaBangun == "balok") { arrayTarget = prefabVariasiBalok; titikPusat = jaringAnimatorBalok.transform; }
        else if(gsm.namaBangun == "limas persegi") { arrayTarget = prefabVariasiLimas; titikPusat = jaringAnimatorLimas.transform; }

        if (arrayTarget != null && titikPusat != null)
        {
            for (int i = 0; i < arrayTarget.Length; i++)
            {
                Vector3 offset = new Vector3((i - 1) * spacing, 0, zOffset); 
                Vector3 spawnPos = titikPusat.position + offset;

                GameObject varCube = Instantiate(arrayTarget[i], spawnPos, Quaternion.identity);
                CubeAnimation anim = varCube.GetComponent<CubeAnimation>();
                if (anim != null) { anim.InisialisasiPosisi(); anim.MunculkanKubus(); }
                
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
        if (varianTerbuka >= prefabVariasi.Length) StartCoroutine(LanjutKeFaseHitungLuas());
    }

    IEnumerator LanjutKeFaseHitungLuas()
    {
        yield return new WaitForSeconds(1f);
        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lihat ada beragam pola jaring-jaring!"));
        yield return StartCoroutine(gsm.TungguInputUser());
        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Meskipun beragam, ini masih jaring-jaring dari "+gsm.namaBangun+"!"));
        yield return StartCoroutine(gsm.TungguInputUser());
        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Dan masih banyak bentuk lainnya."));
        yield return StartCoroutine(gsm.TungguInputUser());
        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Sekarang mari kembali fokus ke "+gsm.namaBangun+" utama kita dan pelajari luas permukaannya!"));
        yield return StartCoroutine(gsm.TungguInputUser());
        
        SFXManager.Instance.MainkanTransisiWoosh();
        foreach (GameObject g in spawnedVariations) g.SetActive(false); 
        
        LoadLevel3();
        faseSekarang = FaseLevel3.HitungLuas;
        gsm.uiManager.namaMisi3.text = "Hitung "+gsm.targetSisi+" Sisi";
        gsm.uiManager.BukaMisi(3);
        
        if (gsm.namaBangun == "kubus") efekJaring.SetGlow(true); 
        else if (gsm.namaBangun == "prisma segitiga") efekJaringPrisma.SetGlow(true); 
        if (gsm.namaBangun == "balok") efekJaringBalok.SetGlow(true); 
        if (gsm.namaBangun == "limas persegi") efekJaringLimas.SetGlow(true); 

        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Kita tahu bahwa "+gsm.namaBangun+" memiliki "+gsm.targetSisi+" sisi. Sisi ini bisa juga disebut sebagai permukaan."));
        yield return StartCoroutine(gsm.TungguInputUser());
    
        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Bantu aku memilih ke-"+gsm.targetSisi+" permukaan ini lagi! Ketuk permukaannya."));
        yield return StartCoroutine(gsm.TungguInputUser());

        if (efekJaring != null) efekJaring.SetGlow(false); 
        if (efekJaringPrisma != null) efekJaringPrisma.SetGlow(false); 
        if (efekJaringBalok != null) efekJaringBalok.SetGlow(false); 
        if (efekJaringLimas != null) efekJaringLimas.SetGlow(false); 
    }

    void HandleColoringSisi(GameObject boneYangDitap)
    {
        string n = boneYangDitap.name;
        Renderer rend = null;

        if(gsm.namaBangun == "kubus") {
            if (n.Contains("001")) rend = GameObject.Find("CubeBawah")?.GetComponent<Renderer>();
            else if (n.Contains("002")) rend = GameObject.Find("Sisi2")?.GetComponent<Renderer>();
            else if (n.Contains("003")) rend = GameObject.Find("Sisi1")?.GetComponent<Renderer>();
            else if (n.Contains("004")) rend = GameObject.Find("Sisi4")?.GetComponent<Renderer>();
            else if (n.Contains("005")) rend = GameObject.Find("Sisi5")?.GetComponent<Renderer>();
            else if (n.Contains("006")) rend = GameObject.Find("Sisi3")?.GetComponent<Renderer>();
        }
        else if(gsm.namaBangun == "prisma segitiga") {
            if (n == "Bone") rend = GameObject.Find("CubeBawah")?.GetComponent<Renderer>();
            else if (n.Contains("001")) rend = GameObject.Find("Sisi1")?.GetComponent<Renderer>();
            else if (n.Contains("002")) rend = GameObject.Find("Sisi2")?.GetComponent<Renderer>();
            else if (n.Contains("003")) rend = GameObject.Find("Sisi3")?.GetComponent<Renderer>();
            else if (n.Contains("004")) rend = GameObject.Find("Sisi4")?.GetComponent<Renderer>();
        }
        else if(gsm.namaBangun == "balok") {
            if (n.Contains("001")) rend = GameObject.Find("CubeBawah")?.GetComponent<Renderer>();
            else if (n.Contains("002")) rend = GameObject.Find("CubeBelakang")?.GetComponent<Renderer>();
            else if (n.Contains("003")) rend = GameObject.Find("CubeAtas")?.GetComponent<Renderer>();
            else if (n.Contains("004")) rend = GameObject.Find("CubeKanan")?.GetComponent<Renderer>();
            else if (n.Contains("005")) rend = GameObject.Find("CubeKiri")?.GetComponent<Renderer>();
            else if (n.Contains("006")) rend = GameObject.Find("CubeDepan")?.GetComponent<Renderer>();
        }
        else if(gsm.namaBangun == "limas persegi") {
            if (n == "Bone") rend = GameObject.Find("LimasSisiAlas")?.GetComponent<Renderer>();
            else if (n.Contains("001")) rend = GameObject.Find("LimasSisi3")?.GetComponent<Renderer>();
            else if (n.Contains("004")) rend = GameObject.Find("LimasSisi1")?.GetComponent<Renderer>();
            else if (n.Contains("005")) rend = GameObject.Find("LimasSisi4")?.GetComponent<Renderer>();
            else if (n.Contains("006")) rend = GameObject.Find("LimasSisi2")?.GetComponent<Renderer>();
        }

        if (rend != null && rend.material.color != Color.yellow)
        {
            rend.material.color = Color.yellow;
            countSisiTerbuka++;
            UpdateUIPermukaan();

            if (boneYangDitap.TryGetComponent<BoxCollider>(out BoxCollider col)) col.enabled = false;
            if (countSisiTerbuka >= gsm.targetSisi) StartCoroutine(SequenceSelesaiLevel3());
        }
    }

    IEnumerator SequenceSelesaiLevel3()
    {
        if(gsm.namaBangun == "kubus") {
            penggarisXPermukaan.SetActive(true); penggarisYPermukaan.SetActive(true); papanRumusLP.SetActive(true);
        } else if(gsm.namaBangun == "prisma segitiga") {
            papanRumusLPPrisma.SetActive(true);   
        } else if(gsm.namaBangun == "balok") {
            papanRumusLPBalok.SetActive(true);   
        }
        else if(gsm.namaBangun == "limas persegi") {
            papanRumusLPLimas.SetActive(true);   
        }

        yield return new WaitForSeconds(0.5f);
        
        if(gsm.namaBangun == "kubus")
        {
            gsm.arpyAnim.SetTrigger("doHU"); SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lengkap! Ada 6 persegi yang membentuk luas permukaan."));
            yield return StartCoroutine(gsm.TungguInputUser());
            gsm.arpyAnim.SetTrigger("doExpla"); SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Maka rumusnya adalah: Luas = 6 × (sisi × sisi)"));
            yield return StartCoroutine(gsm.TungguInputUser());
        }
        else if(gsm.namaBangun == "prisma segitiga")
        {
            gsm.arpyAnim.SetTrigger("doHU"); SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lengkap! Ada 3 persegi panjang dan 2 segitiga yang membentuk luas permukaan."));
            yield return StartCoroutine(gsm.TungguInputUser());
            gsm.arpyAnim.SetTrigger("doExpla"); SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Maka rumus luas permukaan = 3 (panjang × lebar) + 2 (1/2 × alas × tinggi)"));
            yield return StartCoroutine(gsm.TungguInputUser());
        }
        else if(gsm.namaBangun == "balok")
        {
            if (efekJaringBalok != null) efekJaringBalok.SetGlow(true);
            gsm.arpyAnim.SetTrigger("doHU"); SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lengkap! Ada 6 persegi panjang yang membentuk luas permukaan."));
            yield return StartCoroutine(gsm.TungguInputUser());
            WarnaiSisiBalokLevel3();
            gsm.arpyAnim.SetTrigger("doExpla"); SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Untuk menghitung luas permukaannya kita cukup menghitung setiap bangun datar yang ada."));
            yield return StartCoroutine(gsm.TungguInputUser());
            if (efekJaringBalok != null) efekJaringBalok.SetGlow(false);
            gsm.arpyAnim.SetTrigger("doExpla"); SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Maka rumus luas permukaan = 2(pl) + 2(pt) + 2(lt)"));
            yield return StartCoroutine(gsm.TungguInputUser());
        }
        else if(gsm.namaBangun == "limas persegi")
        {
            gsm.arpyAnim.SetTrigger("doHU"); SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lengkap! Ada 1 persegi dan 4 segitiga."));
            yield return StartCoroutine(gsm.TungguInputUser());
            gsm.arpyAnim.SetTrigger("doExpla"); SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Maka rumus luas permukaan = (sisi × sisi) + 4 (1/2 × alas segitiga × tinggi segitiga)"));
            yield return StartCoroutine(gsm.TungguInputUser());
        }

        int levelYangBaruTerbuka = 4;
        SaveManager.Instance.BukaLevel(gsm.namaBangun, levelYangBaruTerbuka);
        gsm.arpyAnim.SetTrigger("doHU");
        SFXManager.Instance.MainkanArpyNoise(1);

        gsm.uiManager.TutupMisi();
        gsm.uiManager.ShowCompletionPopup(
            "Level "+gsm.namaBangun+" Selesai!", 
            "Hebat! Kamu telah menyelesaikan seluruh level "+gsm.namaBangun+"!", 
            levelYangBaruTerbuka - 1);
        faseSekarang = FaseLevel3.Selesai;
    }

    void WarnaiSisiBalokLevel3()
    {
        if (BalokLevel3 != null)
        {
            foreach (Transform sisi in BalokLevel3.transform)
            {
                Renderer rend = sisi.GetComponent<Renderer>();
                if (rend == null) continue;
                string namaAman = sisi.name.ToLower();
                if (namaAman.Contains("atas") || namaAman.Contains("bawah")) rend.material.color = Color.blue;
                else if (namaAman.Contains("kiri") || namaAman.Contains("kanan")) rend.material.color = new Color(1.0f, 0.5f, 0.0f); 
                else if (namaAman.Contains("depan") || namaAman.Contains("belakang")) rend.material.color = Color.white;
            }
        }
    }

    void UpdateUIPermukaan()
    {
        if (faseSekarang == FaseLevel3.KupasUtama)
            gsm.uiManager.counterMisi1.text = "( " + openStep + " / "+gsm.targetBuka+" )";
        else if (faseSekarang == FaseLevel3.TampilVariasi)
            gsm.uiManager.counterMisi2.text = "( " + varianTerbuka + " / 3 )";
        else if (faseSekarang == FaseLevel3.HitungLuas)
            gsm.uiManager.counterMisi3.text = "( " + countSisiTerbuka + " / "+gsm.targetSisi+" )";
    }
}