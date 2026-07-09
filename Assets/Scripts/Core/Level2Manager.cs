using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class Level2Manager : MonoBehaviour
{
    private GameSessionManager gsm => GameSessionManager.Instance;

    [Header("Status Level 2")]
    public int currentVolume = 0;
    
    [Header("Level 1")]
    public GameObject KubusLevel1;
    public GameObject PrismaLevel1;
    public GameObject BalokLevel1;
    public GameObject limasLevel1;

    [Header("Level 2 Kubus")]
    public GameObject KubusLevel2;
    public GameObject KubusKecil;
    public GameObject unitCubePrefab;
    public Transform unitContainer;
    public GameObject objekRumus; 
    public TextMeshProUGUI teksRumus; 
    public GameObject penggarisX; 
    public GameObject penggarisY; 
    public GameObject penggarisZ; 
    
    [Header("Level 2 Balok")]
    public GameObject BalokLevel2;
    public GameObject BalokKecil;
    public GameObject unitCubePrefabBalok;
    public Transform unitContainerBalok;
    public GameObject objekRumusBalok; 
    public TextMeshProUGUI teksRumusBalok; 
    public GameObject penggarisXBalok; 
    public GameObject penggarisYBalok; 
    public GameObject penggarisZBalok; 

    [Header("Level 2 Volume Prisma")]
    public GameObject PrismaLevel2;     
    public GameObject unitPrismaPrefab; 
    public float posisiAwalZ = 0.01f;
    public float tambahSkalaZPerTap = 0.5f; 
    public float pengaliPosisi = 0.5f;
    private GameObject isianVolumeAktif;
    public GameObject objekRumusPrisma; 
    public TextMeshProUGUI teksRumusPrisma; 
    public GameObject penggarisXPrisma; 
    public GameObject penggarisYPrisma; 
    public GameObject penggarisZPrisma; 

    
    [Header("Level 2 Volume Limas")]
    public GameObject LimasLevel2;     
    public GameObject LimasBalokTujuan;     
    public Transform limasIsi;
    public Transform balokIsi;
    public GameObject canvasTombolLimas;
    public GameObject canvasRumusLimas;
    private bool sedangAnimasi = false;
    private bool limasPenuh = false;
    

    [Header("Referensi Unsur (Glow)")]
    public ObjectGlow efekVolume;
    public ObjectGlow efekVolumePrisma;
    public ObjectGlow efekVolumeBalok;
    public ObjectGlow efekVolumeLimas;

    // =========================================================
    // INPUT KHUSUS LEVEL 2
    // =========================================================
    void Update()
    {
        if (Pointer.current == null) return;

        if (Pointer.current.press.wasPressedThisFrame)
        {
            gsm.startTapPosition = Pointer.current.position.ReadValue();
        }

        if (Pointer.current.press.wasReleasedThisFrame)
        {
            Vector2 endTapPosition = Pointer.current.position.ReadValue();
            float distance = Vector2.Distance(gsm.startTapPosition, endTapPosition);

            if (distance < gsm.tapThreshold)
            {
                // Eksekusi pengisian volume jika tidak sedang ada dialog (script sedang menyala)
                HandleTapVolume();
            }
        }
    }

    // =========================================================
    // AWAL LEVEL VOLUME
    // =========================================================
    public void StartLevelVolume()
    {
        StartCoroutine(SequenceStartLevelVolume());
    }

    IEnumerator SequenceStartLevelVolume()
    {
        gsm.uiManager.OnMulaiLevel(gsm.namaBangun, 2);
        yield return new WaitForSeconds(1.5f);

        if(gsm.namaBangun == "prisma segitiga")
        {
            objekRumusPrisma.SetActive(false);
            penggarisXPrisma.SetActive(false);
            penggarisYPrisma.SetActive(false);
            penggarisZPrisma.SetActive(false);
        }
        else if(gsm.namaBangun == "kubus")
        {
            objekRumus.SetActive(false);
            penggarisX.SetActive(false);
            penggarisY.SetActive(false);
            penggarisZ.SetActive(false);
        } 
        else if(gsm.namaBangun == "balok")
        {
            objekRumusBalok.SetActive(false);
            penggarisXBalok.SetActive(false);
            penggarisYBalok.SetActive(false);
            penggarisZBalok.SetActive(false);
        }         
        else if(gsm.namaBangun == "limas persegi")
        {
            limasIsi.localScale = Vector3.zero;
            balokIsi.localScale = new Vector3(118.1f, 87.3f, 0f); 
            canvasTombolLimas.SetActive(false);
            LimasBalokTujuan.SetActive(false);
            canvasRumusLimas.SetActive(false);
        }         

        currentVolume = 0;
        this.enabled = false; // Matikan input sementara selama dialog awal
        gsm.TagLevel.text = "Level 2";
        gsm.uiManager.BukaKotakMisi();
        
        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Di level ini kita akan belajar tentang VOLUME."));
        yield return StartCoroutine(gsm.TungguInputUser()); 
        
        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Volume adalah isi dari bangun ruang."));
        yield return StartCoroutine(gsm.TungguInputUser());

        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Untuk mempelajarinya mari kita ubah "+gsm.namaBangun+" kita!"));
        yield return StartCoroutine(gsm.TungguInputUser());

        LoadLevel2(1);

        if (gsm.namaBangun=="kubus" && efekVolume != null) efekVolume.SetGlow(true); 
        if (gsm.namaBangun=="prisma segitiga" && efekVolumePrisma != null) efekVolumePrisma.SetGlow(true); 
        if (gsm.namaBangun=="balok" && efekVolumeBalok != null) efekVolumeBalok.SetGlow(true); 
        if (gsm.namaBangun=="limas persegi" && efekVolumeLimas != null) efekVolumeLimas.SetGlow(true); 
        
        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Disini kita memiliki sebuah "+gsm.namaBangun+" kosong."));
        yield return StartCoroutine(gsm.TungguInputUser());

        LoadLevel2(2);

        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lalu kita akan mengisi "+gsm.namaBangun+" kosong ini."));
        yield return StartCoroutine(gsm.TungguInputUser());
        
        if (efekVolume != null) efekVolume.SetGlow(false); 
        if (efekVolumePrisma != null) efekVolumePrisma.SetGlow(false); 
        if (efekVolumeBalok != null) efekVolumeBalok.SetGlow(false); 
        if (efekVolumeLimas != null) efekVolumeLimas.SetGlow(false); 
    
        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Ketuk "+gsm.namaBangun+" untuk mengisi"));

        // Aktifkan interaksi
        this.enabled = true;
        gsm.uiManager.namaMisi1.text = "Isi Volume"; // Meminjam UI dari Level 1 jika struktur UI-mu sama
        gsm.uiManager.BukaMisi(1);

        UpdateUIVolume();
    }

    public void LoadLevel2(int set)
    {
        if(set == 1)
        {
            if(gsm.namaBangun == "kubus") {
                CubeAnimation animLevel1 = KubusLevel1.GetComponent<CubeAnimation>();
                animLevel1.HilangkanKubus();

                KubusLevel2.SetActive(true);
                CubeAnimation anim = KubusLevel2.GetComponent<CubeAnimation>();
                if (anim != null) anim.MunculkanKubus();
            }
            else if(gsm.namaBangun == "balok") {
                CubeAnimation animLevel1 = BalokLevel1.GetComponent<CubeAnimation>();
                animLevel1.HilangkanKubus();

                BalokLevel2.SetActive(true);
                CubeAnimation anim = BalokLevel2.GetComponent<CubeAnimation>();
                if (anim != null) anim.MunculkanKubus();
            }
            else if(gsm.namaBangun == "prisma segitiga") {
                CubeAnimation animLevel1 = PrismaLevel1.GetComponent<CubeAnimation>();
                animLevel1.HilangkanKubus();

                PrismaLevel2.SetActive(true);
                CubeAnimation anim = PrismaLevel2.GetComponent<CubeAnimation>();
                if (anim != null) anim.MunculkanKubus();
            }
            else if(gsm.namaBangun == "limas persegi") {
                CubeAnimation animLevel1 = limasLevel1.GetComponent<CubeAnimation>();
                animLevel1.HilangkanKubus();

                LimasLevel2.SetActive(true);
                CubeAnimation anim = LimasLevel2.GetComponent<CubeAnimation>();
                if (anim != null) anim.MunculkanKubus();
            }
        }
        else if (set == 2)
        {
            if(gsm.namaBangun == "kubus") {
                KubusKecil.SetActive(true);
                CubeAnimation anim2 = KubusKecil.GetComponent<CubeAnimation>();
                if (anim2 != null) anim2.MunculkanKubus();
            }
            else if(gsm.namaBangun == "balok") {
                BalokKecil.SetActive(true);
                CubeAnimation anim2 = BalokKecil.GetComponent<CubeAnimation>();
                if (anim2 != null) anim2.MunculkanKubus();
            }
        }
    }

    // =========================================================
    // LOGIKA PENGISIAN VOLUME
    // =========================================================
    public void HandleTapVolume()
    {
        SFXManager.Instance.MainkanPop();
        
        if ((gsm.namaBangun == "kubus" || gsm.namaBangun == "balok") && currentVolume < gsm.targetLapis) 
        {
            SpawnUnitCube(currentVolume);
            currentVolume++;
            UpdateUIVolume();

            if (gsm.namaBangun == "kubus" && currentVolume % 9 == 0)
                StartCoroutine(SequencePenjelasanPerLapisKubus(currentVolume));
            if (gsm.namaBangun == "balok" && currentVolume % 12 == 0)
                StartCoroutine(SequencePenjelasanBalok(currentVolume));
        }
        else if (gsm.namaBangun == "prisma segitiga" && currentVolume < gsm.targetLapis) 
        {            
            IsiVolumePrisma(currentVolume); 
            currentVolume++;
            UpdateUIVolume();

            if (currentVolume == 1 || currentVolume == gsm.targetLapis)
                StartCoroutine(SequencePenjelasanPrisma(currentVolume));
        }
        else if (gsm.namaBangun == "limas persegi" && currentVolume < gsm.targetLapis) 
        {        
            StartCoroutine(AnimasiIsiLimas());    
        }
    }

    private void SpawnUnitCube(int index)
    {
        int xGrid = 0, zGrid = 0, yGrid = 0;
        float xPos = 0, yPos = 0, zPos = 0;

        if (gsm.namaBangun == "kubus")
        {
            xGrid = index % 3;          
            zGrid = (index / 3) % 3;    
            yGrid = index / 9;          

            xPos = (xGrid - 1) * 0.0002f;
            yPos = (yGrid - 1) * 0.0002f;
            zPos = (zGrid - 1) * 0.00016f;

            GameObject unit = Instantiate(unitCubePrefab, unitContainer);
            unit.transform.localPosition = new Vector3(xPos, yPos, zPos);     
            Renderer rend = unit.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                if (yGrid == 0) rend.material.color = Color.blue;       
                else if (yGrid == 1) rend.material.color = Color.cyan;                  
                else rend.material.color = Color.white;                  
            }
            StartCoroutine(AnimateScale(unit.transform));
        }
        else if (gsm.namaBangun == "balok")
        {
            xGrid = index % 4;          
            zGrid = (index / 4) % 3;    
            yGrid = index / 12;         

            xPos = (xGrid - 1.5f) * 0.0002f; 
            yPos = (yGrid - 0.5f) * 0.0002f; 
            zPos = (zGrid - 1.0f) * 0.0002f;

            GameObject unit = Instantiate(unitCubePrefabBalok, unitContainerBalok);
            unit.transform.localPosition = new Vector3(xPos, yPos, zPos);     
            Renderer rend = unit.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                if (yGrid == 0) rend.material.color = Color.blue;       
                else rend.material.color = Color.cyan;                  
            }
            StartCoroutine(AnimateScale(unit.transform));
        }
    }

    private void IsiVolumePrisma(int index)
    {
        if (index == 0)
        {
            isianVolumeAktif = Instantiate(unitPrismaPrefab, PrismaLevel2.transform);
            isianVolumeAktif.transform.localPosition = new Vector3(0f, 0f, posisiAwalZ);
            
            Vector3 skalaBawaan = isianVolumeAktif.transform.localScale;
            Vector3 targetSkalaAlas = new Vector3(skalaBawaan.x, skalaBawaan.y, 0.007f);
            isianVolumeAktif.transform.localScale = Vector3.zero;
            isianVolumeAktif.SetActive(true);
            
            StartCoroutine(AnimasiMunculAlas(isianVolumeAktif.transform, targetSkalaAlas));
            return;
        }

        float skalaTargetZ = (index + 1) * tambahSkalaZPerTap;
        float posisiTargetZ = posisiAwalZ + (skalaTargetZ * pengaliPosisi);

        StartCoroutine(AnimasiTarikPrisma(isianVolumeAktif.transform, skalaTargetZ, posisiTargetZ));
    }

    IEnumerator AnimateScale(Transform target)
    {
        target.localScale = Vector3.zero;
        float elapsed = 0;
        float duration = 0.15f;
        float zScaleValue = (gsm.namaBangun == "kubus") ? 0.00016f : 0.0002f;

        Vector3 targetScale = new Vector3(0.0002f, 0.0002f, zScaleValue);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsed / duration);
            yield return null;
        }
        target.localScale = targetScale;
    }

    IEnumerator AnimasiMunculAlas(Transform target, Vector3 targetScale)
    {
        float elapsed = 0;
        float duration = 0.3f; 
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsed / duration);
            yield return null;
        }
        target.localScale = targetScale;
    }

    IEnumerator AnimasiTarikPrisma(Transform target, float targetScaleZ, float targetPosZ)
    {
        Vector3 skalaAwal = target.localScale;
        Vector3 posisiAwal = target.localPosition;
        Vector3 skalaAkhir = new Vector3(skalaAwal.x, skalaAwal.y, targetScaleZ);
        Vector3 posisiAkhir = new Vector3(0f, 0f, targetPosZ);

        float elapsed = 0;
        float duration = 0.3f; 

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.localScale = Vector3.Lerp(skalaAwal, skalaAkhir, elapsed / duration);
            target.localPosition = Vector3.Lerp(posisiAwal, posisiAkhir, elapsed / duration);
            yield return null;
        }
        target.localScale = skalaAkhir;
        target.localPosition = posisiAkhir;
    }

    private IEnumerator AnimasiIsiLimas()
    {
        sedangAnimasi = true;
        this.enabled = false;
        
        float waktu = 0;
        float durasi = 1.0f;

        // KUNCI PERBAIKAN: Ambil skala penuh asli dari Inspector milikmu
        Vector3 skalaPenuhLimas = new Vector3(191.666f, 191.666f, 96.666f);
        
        while (waktu < durasi)
        {
            // Lerp dari (0,0,0) menuju skala asli objek (191.66, 191.66, 96.66)
            limasIsi.localScale = Vector3.Lerp(Vector3.zero, skalaPenuhLimas, waktu / durasi);
            waktu += Time.deltaTime;
            yield return null;
        }
        
        limasIsi.localScale = skalaPenuhLimas;
        limasPenuh = true;
        sedangAnimasi = false;


        StartCoroutine(SequencePenjelasanLimas());
    }

    // 2. Fungsi ini dipasang di event OnClick() pada Button "Pindahkan Isi"
    public void PindahkanIsiKeBalok()
    {
        SFXManager.Instance.MainkanPop();
        if (!sedangAnimasi && limasPenuh)
        {
            StartCoroutine(AnimasiTransferVolume());
        }
    }

    private IEnumerator AnimasiTransferVolume()
    {
        sedangAnimasi = true;
        canvasTombolLimas.SetActive(false); // Sembunyikan tombol saat transfer terjadi

        float waktu = 0;
        float durasi = 1.5f;

        // Hitung target tinggi balok berdasarkan gsm.targetLapis saat ini
        float tinggiBalokAwal = (float)currentVolume / 3f;
        float tinggiBalokAkhir = (float)(currentVolume + 1) / 3f;
        
        Vector3 skalaPenuhLimas = new Vector3(191.666f, 191.666f, 96.666f);
        Vector3 skalaBalokAwal = new Vector3(118.1f, 87.3f, 55.5f*tinggiBalokAwal);
        Vector3 skalaBalokAkhir = new Vector3(118.1f, 87.3f, 55.5f*tinggiBalokAkhir);

        while (waktu < durasi)
        {
            float progres = waktu / durasi;

            // Limas menyusut ke 0
            limasIsi.localScale = Vector3.Lerp(skalaPenuhLimas, Vector3.zero, progres);
            
            // BalokIsi naik perlahan (hanya sumbu Y)
            balokIsi.localScale = Vector3.Lerp(skalaBalokAwal, skalaBalokAkhir, progres);

            waktu += Time.deltaTime;
            yield return null;
        }

        limasIsi.localScale = Vector3.zero;
        balokIsi.localScale = skalaBalokAkhir;
        
        limasPenuh = false;
        sedangAnimasi = false;
        
        // Tambah jumlah lapisan yang sudah diisi
        currentVolume++;
        UpdateUIVolume();

        //Penjelasan
        if (currentVolume >= gsm.targetLapis) 
        {
            gsm.arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lihat! untuk mengisi full balok ini perlu 3 isi limas."));
            yield return StartCoroutine(gsm.TungguInputUser());

            canvasRumusLimas.SetActive(true);
            
            gsm.arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Ini artinya volume limas adalah 1/3 dari volume balok"));
            yield return StartCoroutine(gsm.TungguInputUser());
            
            gsm.arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Kita tahu bahwa rumus volume balok adalah p×l×t atau bisa kita bilang luas alas × tinggi."));
            yield return StartCoroutine(gsm.TungguInputUser());
            gsm.arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Dengan begitu kita dapatkan rumus volume limas adalah\n1/3 × luas alas × tinggi"));
            yield return StartCoroutine(gsm.TungguInputUser());

            // Jika sudah 3 kali, panggil fungsi tamat level 2
            SelesaikanLevelVolume();
        }
        else
        {
            gsm.arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Isi telah berpindah ke balok!"));
            yield return StartCoroutine(gsm.TungguInputUser()); 

            if(currentVolume == 1)
            {
                gsm.arpyAnim.SetTrigger("doIdle2");
                SFXManager.Instance.MainkanArpyNoise(2);
                yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Balok disini memiliki tinggi dan luas alas yang sama dengan limas."));
                yield return StartCoroutine(gsm.TungguInputUser()); 
                
                gsm.arpyAnim.SetTrigger("doIdle2");
                SFXManager.Instance.MainkanArpyNoise(2);
                yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lihat bahwa 1 isi limas sama dengan sedikit isi dari balok."));
                yield return StartCoroutine(gsm.TungguInputUser()); 
                
                gsm.arpyAnim.SetTrigger("doExpla");
                SFXManager.Instance.MainkanArpyNoise(2);
                yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Disini kita ingin melihat perlu berapa isi limas untuk mengisi penuh balok ini."));
                yield return StartCoroutine(gsm.TungguInputUser()); 
            }

            gsm.arpyAnim.SetTrigger("doIdle2");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Ayo isi limas lagi!"));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            // Jika belum 3 kali, izinkan user mengetuk layar untuk mengisi limas kembali
            this.enabled = true;
        }

    }
    // =========================================================
    // DIALOG & UI UPDATE
    // =========================================================
    void UpdateUIVolume()
    {
        // Karena UI counter aslinya nyambung ke Sisi di kodemu, pastikan referensinya ditautkan di Inspector
        if(gsm.namaBangun == "kubus")
        {
            gsm.uiManager.counterMisi1.text = "(" + currentVolume + " / 27)";
            if (currentVolume == 9)
            {
                objekRumus.SetActive(true);
                teksRumus.text = "Luas Alas = s × s";
                penggarisX.SetActive(true);
                penggarisY.SetActive(true);
            }
            else if (currentVolume == 27)
            {
                penggarisZ.SetActive(true);
                teksRumus.text = "V = Luas Alas × Tinggi\nV = s × s × s";
            }
        }
        else
        {
            gsm.uiManager.counterMisi1.text = "(" + currentVolume + " / "+ gsm.targetLapis +")";
        }
    }

    IEnumerator SequencePenjelasanPerLapisKubus(int count)
    {
        this.enabled = false; // Matikan input selama dialog

        if (count == 9)
        {
            gsm.arpyAnim.SetTrigger("doHU");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lantai pertama penuh! Alas ini berisi 3 × 3 = 9 kubus satuan."));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            gsm.arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Ini yang kita sebut Luas Alas"));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            gsm.arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Karena bentuknya persegi maka rumusnya adalah sisi × sisi."));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lanjut isi kubus"));
        }
        else if (count == 18)
        {
            gsm.arpyAnim.SetTrigger("doHU");
            gsm.arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lantai kedua selesai! Sekarang totalnya 9 + 9 = 18 atau 9 × 2 = 18 kubus satuan."));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            SFXManager.Instance.MainkanArpyNoise(3);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Ayo Lanjutkan!"));
        }
        else if (count == 27)
        {
            gsm.arpyAnim.SetTrigger("doHU");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lantai ketiga penuh! Sekarang tingginya sudah 3 lapis dan berisi 9 + 9 + 9 = 27 atau 9 × 3 = 27 kubus satuan."));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            gsm.arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(4);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Dari sini kita bisa memahami bahwa volume adalah banyaknya kubus satuan yang dapat mengisi suatu bangun ruang."));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            gsm.arpyAnim.SetTrigger("doIdle2");
            SFXManager.Instance.MainkanArpyNoise(3);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lalu rumusnya adalah Sisi × Sisi × Sisi"));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            SelesaikanLevelVolume();
        }

        if (count < 27) this.enabled = true;
    }

    IEnumerator SequencePenjelasanBalok(int count)
    {
        this.enabled = false;

        if (count == 12)
        {
            gsm.arpyAnim.SetTrigger("doHU");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lantai pertama penuh! Alas ini berisi 3 × 4 = 12 kubus satuan."));
            yield return StartCoroutine(gsm.TungguInputUser());

            teksRumusBalok.text = "Luas Alas = Panjang × Lebar";
            objekRumusBalok.SetActive(true);
            penggarisXBalok.SetActive(true);
            penggarisYBalok.SetActive(true);

            objekRumusBalok.GetComponent<CubeAnimation>()?.MunculkanKubus();
            penggarisXBalok.GetComponent<CubeAnimation>()?.MunculkanKubus();
            penggarisYBalok.GetComponent<CubeAnimation>()?.MunculkanKubus();

            gsm.arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Ini yang kita sebut luas alas"));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            gsm.arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Karena bentuknya persegi panjang maka rumusnya adalah panjang × lebar."));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lanjut isi balok"));
        }
        else if (count == 24)
        {
            teksRumusBalok.text = "V = Luas Alas × Tinggi\nV = Panjang × Lebar × Tinggi";
            penggarisZBalok.SetActive(true);
            penggarisZBalok.GetComponent<CubeAnimation>()?.MunculkanKubus();

            gsm.arpyAnim.SetTrigger("doHU");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lantai kedua penuh! Sekarang tingginya sudah 2 lapis dan berisi 12 + 12 = 24 atau 4 × 3 × 2 = 24 kubus satuan."));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            gsm.arpyAnim.SetTrigger("doIdle2");
            SFXManager.Instance.MainkanArpyNoise(3);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Sehingga didapatkan rumus Volume Balok yaitu panjang × lebar × tinggi"));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            SelesaikanLevelVolume();
        }

        if (count < 24) this.enabled = true;
    }

    IEnumerator SequencePenjelasanPrisma(int count)
    {
        this.enabled = false;

        if (count == 1)
        {
            gsm.arpyAnim.SetTrigger("doHU");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Hebat! Satu lapis prisma sudah terisi."));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            
            gsm.arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Lapis pertama yang berbentuk segitiga ini menutupi seluruh dasar prisma. Ini yang kita sebut Luas Alas!"));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            
            teksRumusPrisma.text = "Luas Segitiga = 1/2 × alas segitiga × tinggi segitiga";
            objekRumusPrisma.SetActive(true);
            penggarisXPrisma.SetActive(true);
            penggarisYPrisma.SetActive(true);
            
            objekRumusPrisma.GetComponent<CubeAnimation>()?.MunculkanKubus();
            penggarisXPrisma.GetComponent<CubeAnimation>()?.MunculkanKubus();
            penggarisYPrisma.GetComponent<CubeAnimation>()?.MunculkanKubus();

            gsm.arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Karena bentuknya segitiga maka rumus luas alasnya adalah (1/2 × alas segitiga × tinggi segitiga)"));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            
            gsm.arpyAnim.SetTrigger("doIdle2");
            SFXManager.Instance.MainkanArpyNoise(3);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Ayo ketuk layarnya lagi untuk menumpuk Luas Alas ini ke atas!"));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            
            this.enabled = true;
        }
        else if (count == gsm.targetLapis) 
        {
            teksRumusPrisma.text = "V = Luas Alas × Tinggi\nV = 1/2 × a_segitiga × t_segitiga × t_prisma";
            penggarisZPrisma.SetActive(true);
            penggarisZPrisma.GetComponent<CubeAnimation>()?.MunculkanKubus();

            gsm.arpyAnim.SetTrigger("doHU");
            SFXManager.Instance.MainkanArpyNoise(1);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Luar biasa! Prismanya sudah terisi penuh!"));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            
            gsm.arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(4);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Seperti yang kamu lihat, volume bangun ini terbentuk dari Luas Alas yang ditumpuk setinggi prisma."));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            
            gsm.arpyAnim.SetTrigger("doIdle2");
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Oleh karena itu, kita bisa menemukan rumus Volume Prisma Segitiga..."));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            
            gsm.arpyAnim.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(3);
            yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Yaitu Luas Alas × Tinggi!"));
            yield return StartCoroutine(gsm.TungguInputUser()); 
            
            SelesaikanLevelVolume();
        }
    }
    IEnumerator SequencePenjelasanLimas()
    {
        gsm.arpyAnim.SetTrigger("doHU");
        SFXManager.Instance.MainkanArpyNoise(1);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Limas Terisi."));
        yield return StartCoroutine(gsm.TungguInputUser()); 
        
        canvasTombolLimas.SetActive(true);
        if (!LimasBalokTujuan.activeSelf)
        {
            LimasBalokTujuan.SetActive(true);

            CubeAnimation anim = LimasBalokTujuan.GetComponent<CubeAnimation>();
            if (anim != null)
            {
                anim.MunculkanKubus();
            }
        }

        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Ketuk tombol pindahkan isi!"));
        yield return StartCoroutine(gsm.TungguInputUser()); 
    }

    void SelesaikanLevelVolume()
    {
        int levelYangBaruTerbuka = 3; 
        SaveManager.Instance.BukaLevel(gsm.namaBangun, levelYangBaruTerbuka);
        
        gsm.arpyAnim.SetTrigger("doHU");
        SFXManager.Instance.MainkanArpyNoise(1);

        int bintangUntukDitampilkan = levelYangBaruTerbuka - 1;

        gsm.uiManager.TutupMisi();
        if(gsm.namaBangun == "kubus")
        {
            gsm.uiManager.ShowCompletionPopup(
                "Level Volume Selesai!", 
                "Hebat! Kamu mengisi 3 lapis berisi 9 kubus. Jadi 3 × 3 × 3 = 27!", 
                bintangUntukDitampilkan);
        } 
        else if(gsm.namaBangun == "prisma segitiga")
        {
            gsm.uiManager.ShowCompletionPopup(
                "Level Volume Selesai!", 
                "Hebat! Kamu telah membuktikan bahwa Volume Prisma = Luas Alas × Tinggi.", 
                bintangUntukDitampilkan);
        }
        else if(gsm.namaBangun == "balok")
        {
            gsm.uiManager.ShowCompletionPopup(
                "Level Volume Selesai!", 
                "Hebat! Kamu telah membuktikan bahwa Volume Balok = Panjang × Lebar × Tinggi.", 
                bintangUntukDitampilkan);
        }
        else if(gsm.namaBangun == "limas persegi")
        {
            gsm.uiManager.ShowCompletionPopup(
                "Level Volume Selesai!", 
                "Hebat! Kamu telah membuktikan bahwa Volume Limas = 1/3 × Luas Alas × Tinggi.", 
                bintangUntukDitampilkan);
        }
    }
}