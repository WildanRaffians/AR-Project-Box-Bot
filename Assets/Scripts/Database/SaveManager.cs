using UnityEngine;
using System;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    
    // Variabel untuk mengingat siapa yang sedang main sekarang
    public PemainData pemainAktif; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- FITUR LOGIN / PILIH NAMA ---
    public void SimpanNama(string namaPemain)
    {
        // 1. Buat ID unik sederhana (misal: "Budi Santoso" jadi "budi_santoso")
        string idUnik = namaPemain.ToLower().Replace(" ", "_");

        // 2. Cek di Database Lokal, apakah ID ini sudah pernah main sebelumnya?
        PemainData dataLama = OfflineDatabaseManager.Instance.dbLokal.daftar_pemain.Find(p => p.id_pemain == idUnik);

        if (dataLama != null)
        {
            // Jika sudah ada, muat data lamanya sebagai pemain aktif
            pemainAktif = dataLama;
            Debug.Log($"Selamat datang kembali, {pemainAktif.nama_siswa}!");
        }
        else
        {
            // Jika belum ada, buat profil baru!
            pemainAktif = new PemainData();
            pemainAktif.id_pemain = idUnik;
            pemainAktif.nama_siswa = namaPemain;
            pemainAktif.kelas = "Belum Diatur"; // Bisa disesuaikan nanti lewat UI
            pemainAktif.pencapaian_level = new PencapaianLevel();
            
            // Inisialisasi level agar tidak error Null
            pemainAktif.pencapaian_level.prisma_segitiga = new LevelDetail();
            pemainAktif.pencapaian_level.kubus = new LevelDetail();
            
            Debug.Log($"Profil baru dibuat untuk: {pemainAktif.nama_siswa}");
        }

        // Catat waktu login hari ini
        pemainAktif.terakhir_main = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);
        
        // Simpan langsung ke JSON
        OfflineDatabaseManager.Instance.SimpanAtauUpdatePemain(pemainAktif);
    }

    public string AmbilNama()
    {
        if (pemainAktif != null && !string.IsNullOrEmpty(pemainAktif.nama_siswa))
        {
            return pemainAktif.nama_siswa;
        }
        return ""; 
    }

    // --- FITUR LEVEL PROGRESS ---
    public void BukaLevel(string namaBangun, int levelBaru)
    {
        if (pemainAktif == null) return; // Cegah error jika belum login

        bool adaPerubahan = false;

        // Cek bangun ruang apa yang sedang dimainkan
        if (namaBangun.ToLower().Contains("prisma"))
        {
            if (levelBaru > pemainAktif.pencapaian_level.prisma_segitiga.level_tertinggi)
            {
                pemainAktif.pencapaian_level.prisma_segitiga.level_tertinggi = levelBaru;
                adaPerubahan = true;
            }
        }
        else if (namaBangun.ToLower().Contains("kubus"))
        {
            if (levelBaru > pemainAktif.pencapaian_level.kubus.level_tertinggi)
            {
                pemainAktif.pencapaian_level.kubus.level_tertinggi = levelBaru;
                adaPerubahan = true;
            }
        }
        else if (namaBangun.ToLower().Contains("balok"))
        {
            if (levelBaru > pemainAktif.pencapaian_level.balok.level_tertinggi)
            {
                pemainAktif.pencapaian_level.balok.level_tertinggi = levelBaru;
                adaPerubahan = true;
            }
        }
        else if (namaBangun.ToLower().Contains("limas"))
        {
            if (levelBaru > pemainAktif.pencapaian_level.limas_persegi.level_tertinggi)
            {
                pemainAktif.pencapaian_level.limas_persegi.level_tertinggi = levelBaru;
                adaPerubahan = true;
            }
        }
        else if (namaBangun.ToLower().Contains("tabung"))
        {
            if (levelBaru > pemainAktif.pencapaian_level.tabung.level_tertinggi)
            {
                pemainAktif.pencapaian_level.tabung.level_tertinggi = levelBaru;
                adaPerubahan = true;
            }
        }
        else if (namaBangun.ToLower().Contains("kerucut"))
        {
            if (levelBaru > pemainAktif.pencapaian_level.kerucut.level_tertinggi)
            {
                pemainAktif.pencapaian_level.kerucut.level_tertinggi = levelBaru;
                adaPerubahan = true;
            }
        }
        else if (namaBangun.ToLower().Contains("bola"))
        {
            if (levelBaru > pemainAktif.pencapaian_level.bola.level_tertinggi)
            {
                pemainAktif.pencapaian_level.bola.level_tertinggi = levelBaru;
                adaPerubahan = true;
            }
        }

        // Hanya menyuruh Database menulis ulang file JSON jika memang rekornya tembus
        if (adaPerubahan)
        {
            pemainAktif.terakhir_main = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);
            OfflineDatabaseManager.Instance.SimpanAtauUpdatePemain(pemainAktif);
            Debug.Log($"Level {namaBangun} berhasil dinaikkan ke level {levelBaru} untuk {pemainAktif.nama_siswa}!");
        }
    }

    public int AmbilLevelTertinggi(string namaBangun)
    {
        if (pemainAktif == null) return 1;

        if (namaBangun.ToLower().Contains("prisma"))
        {
            return pemainAktif.pencapaian_level.prisma_segitiga.level_tertinggi;
        }
        else if (namaBangun.ToLower().Contains("kubus"))
        {
            return pemainAktif.pencapaian_level.kubus.level_tertinggi;
        }
        else if (namaBangun.ToLower().Contains("balok"))
        {
            return pemainAktif.pencapaian_level.balok.level_tertinggi;
        }

        return 1; // Default
    }

    public void GantiNamaPemainAktif(string namaBaru)
    {
        if (pemainAktif == null) return;

        // 1. Ambil ID lama untuk dihapus nanti dari database lokal
        string idLama = pemainAktif.id_pemain;

        // 2. Buat ID baru berdasarkan nama baru
        string idBaru = namaBaru.ToLower().Replace(" ", "_");

        // 3. Update data di dalam objek pemainAktif
        pemainAktif.id_pemain = idBaru;
        pemainAktif.nama_siswa = namaBaru;
        pemainAktif.terakhir_main = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);

        // 4. Perintahkan database lokal untuk menghapus ID lama dan menyimpan data dengan ID baru
        OfflineDatabaseManager.Instance.dbLokal.daftar_pemain.RemoveAll(p => p.id_pemain == idLama);
        OfflineDatabaseManager.Instance.SimpanAtauUpdatePemain(pemainAktif);
        
        Debug.Log($"[SaveManager] Berhasil mengubah nama dari {idLama} ke {idBaru}");
    }

    public void GantiKelasPemainAktif(string kelasBaru)
    {
        if (pemainAktif == null) return;

        pemainAktif.kelas = kelasBaru;
        pemainAktif.terakhir_main = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);

        // Langsung simpan perubahan kelas ke JSON
        OfflineDatabaseManager.Instance.SimpanAtauUpdatePemain(pemainAktif);
    }
}