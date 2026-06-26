using UnityEngine;
using System.IO; // Wajib untuk fitur File (Tulis/Baca)
using System.Collections.Generic;
using System;

// ==========================================
// 1. STRUKTUR DATA (MODEL)
// ==========================================
[Serializable]
public class LevelDetail
{
    public int level_tertinggi = 1; // Default selalu 1
}

[Serializable]
public class PencapaianLevel
{
    public LevelDetail prisma_segitiga;
    public LevelDetail kubus;
    public LevelDetail balok;
    public LevelDetail limas_persegi;
    public LevelDetail tabung;
    public LevelDetail kerucut;
    public LevelDetail bola;
}

[Serializable]
public class PemainData
{
    public string id_pemain; // ID kita pindahkan ke dalam sini
    public string nama_siswa;
    public string kelas;
    public string terakhir_main;
    public PencapaianLevel pencapaian_level;
}

// "Kardus" besar untuk membungkus semua pemain agar mudah dibaca Unity
[Serializable]
public class DatabaseLokal
{
    public List<PemainData> daftar_pemain = new List<PemainData>();
}

// ==========================================
// 2. SISTEM MANAJER (MESIN PENYIMPAN)
// ==========================================
public class OfflineDatabaseManager : MonoBehaviour
{
    // Agar bisa dipanggil dari script mana saja (Singleton sederhana)
    public static OfflineDatabaseManager Instance;

    private string filePath;
    public DatabaseLokal dbLokal = new DatabaseLokal();

    private void Awake()
    {
        Instance = this;
        // Menentukan lokasi brankas rahasia di HP/PC
        filePath = Application.persistentDataPath + "/database_boxbot.json";
        
        // Langsung muat data saat game baru dibuka
        MuatDatabase(); 
    }

    // Fungsi membaca file dari HP
    public void MuatDatabase()
    {
        if (File.Exists(filePath))
        {
            string jsonTeks = File.ReadAllText(filePath);
            dbLokal = JsonUtility.FromJson<DatabaseLokal>(jsonTeks);
            Debug.Log("✅ [DATABASE] Data dimuat dari: " + filePath);
        }
        else
        {
            Debug.Log("⚠️ [DATABASE] File belum ada, membuat brankas baru...");
            dbLokal = new DatabaseLokal();
        }
    }

    // Fungsi menyimpan atau menimpa data pemain ke HP
    public void SimpanAtauUpdatePemain(PemainData dataPemain)
    {
        // Cek apakah pemain dengan ID ini sudah ada di dalam brankas?
        int index = dbLokal.daftar_pemain.FindIndex(p => p.id_pemain == dataPemain.id_pemain);

        if (index >= 0)
        {
            // Jika sudah ada, timpa data lamanya dengan progres yang baru
            dbLokal.daftar_pemain[index] = dataPemain;
        }
        else
        {
            // Jika ini pemain baru, tambahkan ke dalam daftar
            dbLokal.daftar_pemain.Add(dataPemain);
        }

        // Kunci brankasnya: Ubah ke teks JSON lalu tulis ke dalam file HP
        string jsonTeks = JsonUtility.ToJson(dbLokal, true);
        File.WriteAllText(filePath, jsonTeks);

        // Lempar salinan teks JSON tersebut ke awan!
        if (CloudManager.Instance != null)
        {
            CloudManager.Instance.SinkronisasiKeCloud(jsonTeks);
        }
        
    }
}