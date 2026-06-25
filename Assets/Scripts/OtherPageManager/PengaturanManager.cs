using UnityEngine;
using UnityEngine.SceneManagement; // Penting untuk pindah scene
using System.Collections;
using TMPro;
public class PengaturanManager : MonoBehaviour
{
    [Header("UI Utama Data Diri")]
    public TextMeshProUGUI teksNamaSaatIni;
    public TextMeshProUGUI teksKelasSaatIni;
    
    [Header("Popup Ganti Nama")]
    public GameObject popupGantiNama;
    public TMP_InputField inputNamaBaru;
    public UnityEngine.UI.Button tombolSimpanNama;
    
    [Header("Popup Ganti Kelas")]
    public GameObject popupGantiKelas;
    public TMP_InputField inputKelasBaru;
    public UnityEngine.UI.Button tombolSimpanKelas;
    void Start()
    {
        // Tutup popup di awal
        popupGantiNama.SetActive(false);
        popupGantiKelas.SetActive(false);

        // Ambil nama dari SaveManager saat menu pengaturan dibuka
        MuatDataPemain();

        // Pasang pendengar untuk validasi tombol simpan (sama seperti di layar Login)
        inputNamaBaru.onValueChanged.AddListener(ValidasiInputNama);
        inputKelasBaru.onValueChanged.AddListener(ValidasiInputKelas);
    }

    void MuatDataPemain()
    {
        if (SaveManager.Instance.pemainAktif != null)
        {
            // Ambil Nama
            string namaTersimpan = SaveManager.Instance.pemainAktif.nama_siswa;
            teksNamaSaatIni.text = !string.IsNullOrEmpty(namaTersimpan) ? namaTersimpan : "Username";

            // Ambil Kelas
            string kelasTersimpan = SaveManager.Instance.pemainAktif.kelas;
            teksKelasSaatIni.text = (!string.IsNullOrEmpty(kelasTersimpan) && kelasTersimpan != "Belum Diatur") ? kelasTersimpan : "Belum diatur";
        }
        else
        {
            teksNamaSaatIni.text = "Username";
            teksKelasSaatIni.text = "Belum diatur";
        }
    }

    // ========================================================
    // --- KONTROL POPUP GANTI NAMA ---
    // ========================================================
    public void BukaPopupUbahNama()
    {
        if (SFXManager.Instance != null) SFXManager.Instance.MainkanClick();
        
        inputNamaBaru.text = SaveManager.Instance.AmbilNama(); 
        popupGantiKelas.SetActive(false);
        if(popupGantiNama.activeSelf == true)
        {
            popupGantiNama.SetActive(false);
        }
        else
        {
            popupGantiNama.SetActive(true);
        }
        ValidasiInputNama(inputNamaBaru.text);
    }

    void ValidasiInputNama(string teks)
    {
        string teksBersih = teks.Trim();
        tombolSimpanNama.interactable = (teksBersih.Length >= 2 && teksBersih.Length <= 12);
    }

    public void SimpanNamaBaru()
    {
        if (SFXManager.Instance != null) SFXManager.Instance.MainkanClick();
        
        string namaFinal = inputNamaBaru.text.Trim();

        // Panggil fungsi ganti nama yang aman untuk ID di SaveManager
        SaveManager.Instance.GantiNamaPemainAktif(namaFinal);

        MuatDataPemain();
        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.TampilkanNotifikasi("Nama Diperbarui");
        }
        popupGantiNama.SetActive(false);
    }

    // ========================================================
    // --- KONTROL POPUP GANTI KELAS ---
    // ========================================================
    public void BukaPopupUbahKelas()
    {
        if (SFXManager.Instance != null) SFXManager.Instance.MainkanClick();
        
        // Ambil kelas saat ini jika ada, jika tidak biarkan kosong
        string kelasSekarang = SaveManager.Instance.pemainAktif != null ? SaveManager.Instance.pemainAktif.kelas : "";
        inputKelasBaru.text = kelasSekarang == "Belum Diatur" ? "" : kelasSekarang;
        
        popupGantiNama.SetActive(false);
        if(popupGantiKelas.activeSelf == true)
        {
            popupGantiKelas.SetActive(false);
        }
        else
        {
            popupGantiKelas.SetActive(true);
        }
        ValidasiInputKelas(inputKelasBaru.text);
    }

    void ValidasiInputKelas(string teks)
    {
        string teksBersih = teks.Trim();
        // Kelas valid jika tidak kosong dan maksimal 5 karakter (Contoh: "4A", "IV-B")
        tombolSimpanKelas.interactable = (teksBersih.Length > 0 && teksBersih.Length <= 5);
    }

    public void SimpanKelasBaru()
    {
        if (SFXManager.Instance != null) SFXManager.Instance.MainkanClick();
        
        string kelasFinal = inputKelasBaru.text.Trim();

        // Simpan kelas baru ke data aktif
        SaveManager.Instance.GantiKelasPemainAktif(kelasFinal);

        MuatDataPemain();
        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.TampilkanNotifikasi("Kelas Diperbarui");
        }
        popupGantiKelas.SetActive(false);
    }

    // ========================================================
    // --- NAVIGASI ---
    // ========================================================
    public void TombolKembali()
    {
        if (SFXManager.Instance != null) SFXManager.Instance.MainkanClick();
        
        if (SceneTransition.Instance != null)
            SceneTransition.Instance.PindahScene("MainMenu");
        else
            SceneManager.LoadScene("MainMenu");
    }
    
    
}