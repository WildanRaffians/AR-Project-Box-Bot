using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class Level1Manager : MonoBehaviour
{
    private GameSessionManager gsm => GameSessionManager.Instance;

    [Header("Objek Utama Level 1")]
    public GameObject KubusLevel1; 
    public GameObject prismaLevel1; 
    public GameObject balokLevel1; 
    
    [Header("Misi Sisi")]
    public GameObject faceGroupKubus;
    public GameObject faceGroupPrisma;
    public GameObject faceGroupBalok;
    public int jumlahSisiDitemukan = 0;

    [Header("Misi Sudut")]
    public GameObject vertexGroup; 
    public GameObject vertexGroupPrisma; 
    public GameObject vertexGroupBalok; 
    public int jumlahVertexDitemukan = 0;

    [Header("Misi Rusuk")]
    public GameObject edgeGroup;
    public GameObject edgeGroupPrisma;
    public GameObject edgeGroupBalok;
    public int jumlahRusukDitemukan = 0;

    [Header("Referensi Unsur (Glow)")]
    public ObjectGlow efekSisi; public ObjectGlow efekSisiPrisma; public ObjectGlow efekSisiBalok; 
    public ObjectGlow efekSudut; public ObjectGlow efekSudutPrisma; public ObjectGlow efekSudutBalok;
    public ObjectGlow efekRusuk; public ObjectGlow efekRusukPrisma; public ObjectGlow efekRusukBalok;

    // =========================================================
    // INPUT RAYCAST KHUSUS LEVEL 1
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
                DoRaycast(endTapPosition);
            }
        }
    }

    void DoRaycast(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            SFXManager.Instance.MainkanPop();
            
            EdgeController ec = hit.collider.GetComponent<EdgeController>();
            if (ec != null) { ec.OnEdgeClicked(); return; }
            
            FaceController fc = hit.collider.GetComponent<FaceController>();
            if (fc != null) fc.OnFaceClicked();
            
            VertexController vc = hit.collider.GetComponent<VertexController>();
            if (vc != null) vc.OnMouseDown();
        }
    }    

    // =========================================================
    // SISI
    // =========================================================
    public void StartLevelSisi()
    {
        StartCoroutine(SequenceStartLevelSisi());
    }

    IEnumerator SequenceStartLevelSisi()
    {
        gsm.uiManager.OnMulaiLevel(gsm.namaBangun, 1);
        yield return new WaitForSeconds(1.5f);

        Collider[] semuaCollider = faceGroupKubus.GetComponentsInChildren<Collider>();
        vertexGroup.SetActive(false); 
        gsm.TagLevel.text = "Level 1";
        gsm.uiManager.BukaKotakMisi();

        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Mari kita mengenal " + gsm.namaBangun + "!"));
        yield return StartCoroutine(gsm.TungguInputUser());
        
        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Seperti yang kamu lihat inilah "+ gsm.namaBangun + "!"));
        yield return StartCoroutine(gsm.TungguInputUser());
        
        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Pertama, mari kita amati kerangka "+ gsm.namaBangun +"!"));
        yield return StartCoroutine(gsm.TungguInputUser());

        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog(gsm.dialogKerangka));
        yield return StartCoroutine(gsm.TungguInputUser());
        
        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(2);

        if(gsm.namaBangun == "kubus")
        {
            faceGroupKubus.SetActive(true);
            foreach (Collider col in semuaCollider) { if (col != null) col.enabled = false; }
            if (efekSisi != null) efekSisi.SetGlow(true); 
        } 
        else if(gsm.namaBangun == "prisma segitiga") { if (efekSisiPrisma != null) efekSisiPrisma.SetGlow(true); }
        else if(gsm.namaBangun == "balok") { if (efekSisiBalok != null) efekSisiBalok.SetGlow(true); }

        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Sisi merupakan permukaan atau “kulit luar” pada bangun ruang."));
        yield return StartCoroutine(gsm.TungguInputUser());
        
        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Pada " + gsm.namaBangun + " ini, sisi merupakan area yang berwarna merah."));
        yield return StartCoroutine(gsm.TungguInputUser());
        
        if (efekSisi != null) efekSisi.SetGlow(false); 
        if (efekSisiPrisma != null) efekSisiPrisma.SetGlow(false); 
        if (efekSisiBalok != null) efekSisiBalok.SetGlow(false); 
        
        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Sekarang bantu aku temukan " + gsm.targetSisi + " sisi " + gsm.namaBangun + "!"));
        yield return StartCoroutine(gsm.TungguInputUser());
        
        // MENGGUNAKAN UIMANAGER SEKARANG
        gsm.uiManager.namaMisi1.text = "Temukan Sisi"; 
        gsm.uiManager.BukaMisi(1);
        
        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Ketuk pada bagian "+ gsm.namaBangun +" yang kamu rasa itu sisi nya!"));
        
        if(gsm.namaBangun == "kubus")
        {
            foreach (Collider col in semuaCollider) { if (col != null) col.enabled = true; }
        }
        else if(gsm.namaBangun == "prisma segitiga")
        {
            FaceController[] semuaSisi = faceGroupPrisma.GetComponentsInChildren<FaceController>();
            foreach (FaceController sisi in semuaSisi) { if (sisi != null) sisi.AktifkanInteraksi(); }
        }
        else if(gsm.namaBangun == "balok")
        {
            FaceController[] semuaSisi = faceGroupBalok.GetComponentsInChildren<FaceController>();
            foreach (FaceController sisi in semuaSisi) { if (sisi != null) sisi.AktifkanInteraksi(); }
        }

        UpdateUISisi();
    }

    public void TambahSisi()
    {
        jumlahSisiDitemukan++;        
        UpdateUISisi();

        if (jumlahSisiDitemukan >= gsm.targetSisi && jumlahVertexDitemukan == 0)
        {
            SelesaikanLevelSisi();
        }
    }

    public void UpdateUISisi()
    {
        if(jumlahSisiDitemukan == 1) {   
            gsm.uiManager.ShowCubeExplanation("Satu ketemu!");
            gsm.arpyAnim.SetTrigger("doHU");
        } else if(jumlahSisiDitemukan == 2) {
            gsm.arpyAnim.SetTrigger("doIdle2");
            gsm.uiManager.ShowCubeExplanation("Hebat! Kamu juga bisa memutar bangun ruang dengan jarimu");
        } else if(jumlahSisiDitemukan == 4) {
            gsm.uiManager.ShowCubeExplanation("Bagus!");
        }
        // MENGGUNAKAN UIMANAGER SEKARANG
        gsm.uiManager.counterMisi1.text = "(" + jumlahSisiDitemukan + " / " + gsm.targetSisi + ")";
    }

    void SelesaikanLevelSisi()
    {
        gsm.arpyAnim.SetTrigger("doHU");
        SFXManager.Instance.MainkanArpyNoise(1);
        gsm.uiManager.ShowCompletionPopup("Misi Sisi Selesai!", gsm.teksPopupSelesaiSisi, 0);
    }


    // =========================================================
    // SUDUT
    // =========================================================
    public void StartLevelSudut()
    {
        StartCoroutine(SequenceStartLevelSudut());
    }

    IEnumerator SequenceStartLevelSudut()
    {
        Collider[] semuaCollider1 = vertexGroup.GetComponentsInChildren<Collider>();
        if(gsm.namaBangun == "prisma segitiga")
        {
            FaceController[] semuaSisi = faceGroupPrisma.GetComponentsInChildren<FaceController>();
            foreach (FaceController sisi in semuaSisi) { if (sisi != null) sisi.ResetSisi(); }        
        }
        else if(gsm.namaBangun == "balok")
        {
            FaceController[] semuaSisi = faceGroupBalok.GetComponentsInChildren<FaceController>();
            foreach (FaceController sisi in semuaSisi) { if (sisi != null) sisi.ResetSisi(); }        
        }
        else if(gsm.namaBangun == "kubus")
        {
            CubeAnimation faceGroup1 = faceGroupKubus.GetComponent<CubeAnimation>();
            faceGroup1.HilangkanKubus();
            edgeGroup.SetActive(false); 
        }
        
        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(1);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Sekarang saatnya untuk mencari sudut!"));
        yield return StartCoroutine(gsm.TungguInputUser());

        if(gsm.namaBangun == "kubus")
        {
            vertexGroup.SetActive(true);
            semuaCollider1 = vertexGroup.GetComponentsInChildren<Collider>();
            foreach (Collider col in semuaCollider1) { if (col != null) col.enabled = false; }
            if (efekSudut != null) efekSudut.SetGlow(true);
        } 
        else if(gsm.namaBangun == "prisma segitiga") { if (efekSudutPrisma != null) efekSudutPrisma.SetGlow(true); }
        else if(gsm.namaBangun == "balok") { if (efekSudutBalok != null) efekSudutBalok.SetGlow(true); }

        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Sudut merupakan titik pertemuan beberapa rusuk atau bisa disebut juga 'pojok' dari bangun ruang."));
        yield return StartCoroutine(gsm.TungguInputUser());

        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Iya, yang berwarna biru itu."));
        yield return StartCoroutine(gsm.TungguInputUser());

        if (efekSudut != null) efekSudut.SetGlow(false); 
        if (efekSudutPrisma != null) efekSudutPrisma.SetGlow(false); 
        if (efekSudutBalok != null) efekSudutBalok.SetGlow(false); 

        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Temukan "+ gsm.targetSudut +" titik sudut pada "+gsm.namaBangun+"!"));
        
        if(gsm.namaBangun == "kubus")
        {
            foreach (Collider col in semuaCollider1) { if (col != null) col.enabled = true; }
        }
        else if(gsm.namaBangun == "prisma segitiga")
        {
            VertexController[] semuaSudut = vertexGroupPrisma.GetComponentsInChildren<VertexController>();
            foreach (VertexController sudut in semuaSudut) { if (sudut != null) sudut.AktifkanInteraksi(); }
        }
        else if(gsm.namaBangun == "balok")
        {
            VertexController[] semuaSudut = vertexGroupBalok.GetComponentsInChildren<VertexController>();
            foreach (VertexController sudut in semuaSudut) { if (sudut != null) sudut.AktifkanInteraksi(); }
        }
        
        // MENGGUNAKAN UIMANAGER SEKARANG
        gsm.uiManager.namaMisi2.text = "Temukan Sudut"; 
        gsm.uiManager.BukaMisi(2);
        UpdateUISudut();
    }

    public void TambahSudut()
    {
        jumlahVertexDitemukan++;
        UpdateUISudut();

        if (jumlahVertexDitemukan >= gsm.targetSudut && jumlahSisiDitemukan >= gsm.targetSisi && jumlahRusukDitemukan == 0)
        {
            SelesaikanLevelSudut();
        }
    }

    void UpdateUISudut()
    {
        if(jumlahVertexDitemukan == 1) {
            gsm.arpyAnim.SetTrigger("doIdle2");
            gsm.uiManager.ShowCubeExplanation("Itu dia!");      
        }
        else if( jumlahVertexDitemukan == 3) {
            gsm.arpyAnim.SetTrigger("doIdle2");
            gsm.uiManager.ShowCubeExplanation("Ayo Semangat!");            
        }
        else if( jumlahVertexDitemukan == 6) {
            gsm.arpyAnim.SetTrigger("doIdle2");
            gsm.uiManager.ShowCubeExplanation("Kamu hebat!");            
        }
        // MENGGUNAKAN UIMANAGER SEKARANG
        gsm.uiManager.counterMisi2.text = "(" + jumlahVertexDitemukan + " / " + gsm.targetSudut + ")";
    }

    void SelesaikanLevelSudut()
    {
        gsm.arpyAnim.SetTrigger("doHU");
        SFXManager.Instance.MainkanArpyNoise(1);
        gsm.uiManager.ShowCompletionPopup("Misi Sudut Selesai!", "Hebat! Kamu berhasil menemukan "+ gsm.targetSudut +" titik sudut "+gsm.namaBangun+".", 0);
    }


    // =========================================================
    // RUSUK
    // =========================================================
    public void StartLevelRusuk()
    {
        StartCoroutine(SequenceStartLevelRusuk());
    }

    IEnumerator SequenceStartLevelRusuk()
    {  
        if(gsm.namaBangun == "kubus")
        {
            faceGroupKubus.SetActive(false);
            CubeAnimation vertexGroup1 = vertexGroup.GetComponent<CubeAnimation>();
            vertexGroup1.HilangkanKubus();
        } 
        else if(gsm.namaBangun == "prisma segitiga")
        {
            VertexController[] semuaSudut = vertexGroupPrisma.GetComponentsInChildren<VertexController>();
            foreach (VertexController sudut in semuaSudut) { if (sudut != null) sudut.ResetSudut(); }
        }
        else if(gsm.namaBangun == "balok")
        {
            VertexController[] semuaSudut = vertexGroupBalok.GetComponentsInChildren<VertexController>();
            foreach (VertexController sudut in semuaSudut) { if (sudut != null) sudut.ResetSudut(); }
        }

        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(2);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Terakhir mari mempelajari rusuk "+gsm.namaBangun+"!"));
        yield return StartCoroutine(gsm.TungguInputUser());
        
        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(3);
        edgeGroup.SetActive(true);
        Collider[] semuaCollider2 = edgeGroup.GetComponentsInChildren<Collider>();

        if(gsm.namaBangun == "kubus")
        {
            foreach (Collider col in semuaCollider2) { if (col != null) col.enabled = false; }
            if (efekRusuk != null) efekRusuk.SetGlow(true); 
        }
        else if(gsm.namaBangun == "prisma segitiga") { if (efekRusukPrisma != null) efekRusukPrisma.SetGlow(true); }
        else if(gsm.namaBangun == "balok") { if (efekRusukBalok != null) efekRusukBalok.SetGlow(true); }
        
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Rusuk merupakan garis pertemuan antara dua sisi."));
        yield return StartCoroutine(gsm.TungguInputUser());
        
        gsm.arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(4);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Pada "+gsm.namaBangun+" kita, garis-garis ini berwarna putih."));
        yield return StartCoroutine(gsm.TungguInputUser());
        
        if (efekRusuk != null) efekRusuk.SetGlow(false); 
        if (efekRusukPrisma != null) efekRusukPrisma.SetGlow(false);
        if (efekRusukBalok != null) efekRusukBalok.SetGlow(false);

        gsm.arpyAnim.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(3);
        yield return StartCoroutine(gsm.uiManager.AnimasiDialog("Temukan "+gsm.targetRusuk+" rusuk yang membentuk kerangka "+gsm.namaBangun+" ini."));
        
        if(gsm.namaBangun == "kubus")
        {
            foreach (Collider col in semuaCollider2) { if (col != null) col.enabled = true; }
        }
        else if(gsm.namaBangun == "prisma segitiga")
        {
            EdgeController[] semuaRusuk = edgeGroupPrisma.GetComponentsInChildren<EdgeController>();
            foreach (EdgeController rusuk in semuaRusuk) { if (rusuk != null) rusuk.AktifkanInteraksi(); }
        }
        else if(gsm.namaBangun == "balok")
        {
            EdgeController[] semuaRusuk = edgeGroupBalok.GetComponentsInChildren<EdgeController>();
            foreach (EdgeController rusuk in semuaRusuk) { if (rusuk != null) rusuk.AktifkanInteraksi(); }
        }
        
        // MENGGUNAKAN UIMANAGER SEKARANG
        gsm.uiManager.namaMisi3.text = "Temukan Rusuk";
        gsm.uiManager.BukaMisi(3);
        UpdateUIRusuk();
    }

    public void TambahRusuk()
    {
        jumlahRusukDitemukan++;
        UpdateUIRusuk();

        if (jumlahRusukDitemukan >= gsm.targetRusuk && jumlahVertexDitemukan >= gsm.targetSudut && jumlahSisiDitemukan >= gsm.targetSisi)
        {
            SelesaikanLevelRusuk();
        }
    }

    public void UpdateUIRusuk()
    {
        if(jumlahRusukDitemukan == 1) {
            gsm.arpyAnim.SetTrigger("doIdle2");
            gsm.uiManager.ShowCubeExplanation("Kamu memang hebat!"); 
        } else if(jumlahRusukDitemukan == 3) {
            gsm.arpyAnim.SetTrigger("doExpla");
            gsm.uiManager.ShowCubeExplanation("Ayo semangat!"); 
        }
        else if(jumlahRusukDitemukan == 6) {
            gsm.arpyAnim.SetTrigger("doExpla");
            gsm.uiManager.ShowCubeExplanation("Sedikit lagi!"); 
        }
        else if(jumlahRusukDitemukan == 9) {
            gsm.arpyAnim.SetTrigger("doIdle2");
            gsm.uiManager.ShowCubeExplanation("Kamu pasti bisa!"); 
        }
        
        // MENGGUNAKAN UIMANAGER SEKARANG
        gsm.uiManager.counterMisi3.text = "(" + jumlahRusukDitemukan + " / " + gsm.targetRusuk + ")";
    }

    void SelesaikanLevelRusuk()
    {
        if(gsm.namaBangun == "kubus")
        {
            CubeAnimation edgeGroup1 = edgeGroup.GetComponent<CubeAnimation>();
            edgeGroup1.HilangkanKubus();
        } 
        else if(gsm.namaBangun == "prisma segitiga")
        {
            EdgeController[] semuaRusuk = edgeGroupPrisma.GetComponentsInChildren<EdgeController>();
            foreach (EdgeController rusuk in semuaRusuk) { if (rusuk != null) rusuk.ResetRusuk(); }
        }
        else if(gsm.namaBangun == "balok")
        {
            EdgeController[] semuaRusuk = edgeGroupBalok.GetComponentsInChildren<EdgeController>();
            foreach (EdgeController rusuk in semuaRusuk) { if (rusuk != null) rusuk.ResetRusuk(); }
        }

        int levelYangBaruTerbuka = 2; // Selesai tahap rusuk -> lanjut ke tahap 2 (Volume)
        SaveManager.Instance.BukaLevel(gsm.namaBangun, levelYangBaruTerbuka);
        
        gsm.arpyAnim.SetTrigger("doHU");
        SFXManager.Instance.MainkanArpyNoise(1);

        int bintangUntukDitampilkan = levelYangBaruTerbuka - 1;

        gsm.uiManager.TutupMisi();
        gsm.uiManager.ShowCompletionPopup(
            "Level Kerangka Selesai!", 
            "Kamu telah menguasai seluruh kerangka "+ gsm.namaBangun +" ("+gsm.targetSisi+" Sisi, "+gsm.targetSudut+" Sudut, "+gsm.targetRusuk+" Rusuk).", 
            bintangUntukDitampilkan
        );
    }
}