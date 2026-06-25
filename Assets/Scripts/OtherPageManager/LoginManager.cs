using UnityEngine;
using UnityEngine.UI;
using TMPro; // Gunakan ini jika Input Field kamu memakai TextMeshPro
using UnityEngine.SceneManagement; // Untuk pindah scene
using System.Collections;
using UnityEngine.InputSystem; // Tambahkan ini di baris paling atas
using System.Collections.Generic;
public class LoginManager : MonoBehaviour
{
    [Header("Referensi UI")]
    public GameObject bubble;
    public TextMeshProUGUI bubbletext;
    public GameObject tombolnext;
    public Animator arpyAnimator;       // Tarik objek Arpy yang punya komponen Animator ke sini
    public float typingSpeed = 0.05f; // Semakin kecil semakin cepat
    private bool sedangMengetik = false;
    private Vector3 originalScale1;
    private Vector3 originalScale2;
    private Vector3 originalScale3;
    
    [Header("Referensi UI List Player")]
    public GameObject playerList;
    public Transform wadahScrollView;
    public GameObject prefabTombolNama;
    public TMP_InputField inputCariNama;
    public Button tombolLanjutUtama; // Masukkan tombol "Lanjut" yang di bawah ke sini

    [Header("Referensi UI Popup Tambah")]
    public GameObject panelPopupTambah;
    public TMP_InputField inputNamaBaru;
    public Button tombolLanjut;

    [Header("Pengaturan Warna Pilihan")]
    public Color warnaNormal = new Color(0.9f, 0.9f, 0.9f); // Abu-abu terang
    public Color warnaTeksNormal = new Color(0f, 0f, 0f);
    public Color warnaTerpilih = new Color(0.2f, 0.5f, 1f); // Biru (sesuaikan di Inspector)

    private List<GameObject> daftarTombolAktif = new List<GameObject>();
    
    // Variabel untuk mengingat status pilihan saat ini
    private string namaYangSedangDipilih = "";
    private GameObject objekTombolTerpilih = null;
    
    void Awake()
    {
        originalScale1 = bubble.transform.localScale;
        originalScale2 = panelPopupTambah.transform.localScale;
        originalScale3 = playerList.transform.localScale;
    }
    void Start()
    {
        StopAllCoroutines();
        StartCoroutine(SequenceStart());        

    }

    IEnumerator SequenceStart()
    {
        int jumlahPemain = -1;
        jumlahPemain = OfflineDatabaseManager.Instance.dbLokal.daftar_pemain.Count;
        if(jumlahPemain == 0)
        {
            bubble.SetActive(true);
            yield return StartCoroutine(ScalePopup(1, 0.5f));

            arpyAnimator.SetTrigger("doHai"); 
            SFXManager.Instance.MainkanArpyNoise(2);
            yield return StartCoroutine(AnimasiDialog("Halo, aku adalah Asisten Robot Pengenal Bangun Ruang ARP-BR 001 atau kamu bisa memanggilku Arpy."));
            arpyAnimator.SetTrigger("doidle2"); 
            yield return StartCoroutine(TungguInputUser());

            arpyAnimator.SetTrigger("doidle2"); 
            SFXManager.Instance.MainkanArpyNoise(3);
            yield return StartCoroutine(AnimasiDialog("Disini aku akan membantumu mengenali setiap bangun ruang yang ada agar kamu menjadi insinyur yang hebat!"));
            arpyAnimator.SetTrigger("doHU");      
            yield return StartCoroutine(TungguInputUser());

            arpyAnimator.SetTrigger("doExpla");
            SFXManager.Instance.MainkanArpyNoise(4);
            yield return StartCoroutine(AnimasiDialog("Sebelum memulai, bagaimana sebaiknya aku memanggilmu?"));
            yield return StartCoroutine(TungguInputUser());

            yield return StartCoroutine (HidePopup(1, 0.1f));
        }
        
        playerList.SetActive(true);
        yield return StartCoroutine (ScalePopup(3, 0.5f));

        arpyAnimator.SetTrigger("doIdle2");
        SFXManager.Instance.MainkanArpyNoise(1);
        
        tombolLanjutUtama.interactable = false;
        
        RefreshDaftarPemain();
        inputCariNama.onValueChanged.AddListener(FilterPencarian);        
    }

    public void RefreshDaftarPemain()
    {
        foreach (Transform child in wadahScrollView) { Destroy(child.gameObject); }
        daftarTombolAktif.Clear();
        namaYangSedangDipilih = ""; // Reset pilihan
        tombolLanjutUtama.interactable = false;

        foreach (var pemain in OfflineDatabaseManager.Instance.dbLokal.daftar_pemain)
        {
            GameObject tombolBaru = Instantiate(prefabTombolNama, wadahScrollView);
            
            // BARIS AJAIB: Pindahkan tombol ini ke urutan paling atas di dalam wadah!
            tombolBaru.transform.SetAsFirstSibling();

            tombolBaru.GetComponentInChildren<TextMeshProUGUI>().text = pemain.nama_siswa;
            tombolBaru.GetComponent<Image>().color = warnaNormal;
            tombolBaru.GetComponentInChildren<TextMeshProUGUI>().color = warnaTeksNormal;

            string namaPemain = pemain.nama_siswa; 
            tombolBaru.GetComponent<Button>().onClick.AddListener(() => PilihPemain(tombolBaru, namaPemain));

            daftarTombolAktif.Add(tombolBaru);
        }
    }

    private void PilihPemain(GameObject tombolYangDiklik, string nama)
    {
        // 1. Kembalikan warna tombol yang sebelumnya dipilih (jika ada) ke warna normal
        if (objekTombolTerpilih != null)
        {
            objekTombolTerpilih.GetComponent<Image>().color = warnaNormal;
            objekTombolTerpilih.GetComponentInChildren<TextMeshProUGUI>().color = warnaTeksNormal;
        }

        // 2. Ubah warna tombol yang baru diklik menjadi biru
        tombolYangDiklik.GetComponent<Image>().color = warnaTerpilih;
        tombolYangDiklik.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;

        // 3. Simpan data pilihan saat ini
        objekTombolTerpilih = tombolYangDiklik;
        namaYangSedangDipilih = nama;

        // 4. Nyalakan tombol "Lanjut" di bawah
        tombolLanjutUtama.interactable = true;
    }

    private void FilterPencarian(string kataKunci)
    {
        kataKunci = kataKunci.ToLower();
        foreach (GameObject tombol in daftarTombolAktif)
        {
            string namaDiTombol = tombol.GetComponentInChildren<TextMeshProUGUI>().text.ToLower();
            tombol.SetActive(namaDiTombol.Contains(kataKunci));
        }
    }

    public void TombolBukaPopupDitekan() 
    {
        SFXManager.Instance.MainkanClick();
        
        StartCoroutine(SequenceTombolBukaPopupDitekan());        
    }
    IEnumerator SequenceTombolBukaPopupDitekan()
    {
        panelPopupTambah.SetActive(true);
        yield return StartCoroutine (ScalePopup(2, 0.5f));

        inputNamaBaru.text = ""; 
    }

    public void TombolTambahLanjutDitekan()
    {
        SFXManager.Instance.MainkanClick();
        string namaBaru = inputNamaBaru.text;
        
        StartCoroutine (SequenceTombolTambahLanjutDitekan());
        if (!string.IsNullOrEmpty(namaBaru))
        {
            // 1. Buat profil baru dan simpan ke database JSON
            SaveManager.Instance.SimpanNama(namaBaru);

            inputNamaBaru.text = ""; 

            RefreshDaftarPemain();

            if (NotificationManager.Instance != null)
            {
                NotificationManager.Instance.TampilkanNotifikasi("Pemain Ditambahkan");
            }
        }
    }
    
    public void TombolBatalTambah()
    {
        SFXManager.Instance.MainkanClick();
        StartCoroutine (SequenceTombolTambahLanjutDitekan());
        inputNamaBaru.text = ""; 
    }

    IEnumerator SequenceTombolTambahLanjutDitekan()
    {
        yield return StartCoroutine (HidePopup(2, 0.1f));
    }

    public void TombolLanjutUtamaDitekan()
    {
        SFXManager.Instance.MainkanClick();
        SFXManager.Instance.MainkanArpyNoise(2);
        if (!string.IsNullOrEmpty(namaYangSedangDipilih))
        {
            LoginSebagai(namaYangSedangDipilih);
        }
    }

    private void LoginSebagai(string nama)
    {
        SaveManager.Instance.SimpanNama(nama);
        Debug.Log("🚀 Memulai permainan sebagai: " + nama);
        // UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
        SceneTransition.Instance.PindahScene("MainMenu");

    }

    void ValidasiInput(string teks)
    {
        // Hapus spasi di awal dan akhir teks
        string teksBersih = teks.Trim();

        // Tombol Lanjut hanya bisa ditekan jika ada minimal 2 huruf dan maksimal 12 huruf
        if (teksBersih.Length >= 2 && teksBersih.Length <= 12)
        {
            tombolLanjut.interactable = true;
        }
        else
        {
            tombolLanjut.interactable = false;
        }
    }

    public IEnumerator AnimasiDialog(string fullText)
    {
        sedangMengetik = true;
        bubbletext.text = "";

        // 2. Efek Mengetik
        foreach (char huruf in fullText.ToCharArray())
        {
            bubbletext.text += huruf;
            yield return new WaitForSeconds(typingSpeed);
        }

        sedangMengetik = false;
    }

    private IEnumerator TungguInputUser()
    {
        // 1. Munculkan petunjuk visual "Tap untuk Lanjut" di UI
        tombolnext.SetActive(true);

        // 2. Beri jeda sangat singkat agar klik tombol sebelumnya tidak langsung terhitung
        yield return new WaitForSeconds(0.2f);

        // 3. Loop terus selama user BELUM menekan layar
        while (!Pointer.current.press.wasPressedThisFrame)
        {
            yield return null; // Tunggu ke frame berikutnya
        }

        // 4. Sembunyikan petunjuk setelah diklik
        tombolnext.SetActive(false);
        SFXManager.Instance.MainkanClick();
        
        // 5. Beri jeda sedikit agar tidak langsung loncat ke dialog berikutnya secara brutal
        yield return new WaitForSeconds(0.1f);
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
                bubble.transform.localScale = originalScale1 * bounceValue;
            } 
            else if(set == 2){
                panelPopupTambah.transform.localScale = originalScale2 * bounceValue;
            } 
            else if(set == 3){
                playerList.transform.localScale = originalScale3 * bounceValue;
            } 

            yield return null;
        }

        // Pastikan skala akhir tepat
        if(set == 1)
        {
            
            bubble.transform.localScale = originalScale1;
        } 
        else if(set == 2)
        {
            
            panelPopupTambah.transform.localScale = originalScale2;
        }
        else if(set == 3)
        {
            
            playerList.transform.localScale = originalScale3;
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
                bubble.transform.localScale = Vector3.Lerp(startScale1, Vector3.zero, Mathf.SmoothStep(0, 1, t));
            } else if(set == 2)
            {
                Vector3 startScale2 = originalScale2;
                panelPopupTambah.transform.localScale = Vector3.Lerp(startScale2, Vector3.zero, Mathf.SmoothStep(0, 1, t));
            } 
            yield return null;
        }

        if(set == 1)
        {
            bubble.SetActive(false);
        } 
        else if( set == 2)
        {
            panelPopupTambah.SetActive(false);
        }
        
    }
}