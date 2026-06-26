using UnityEngine;
using UnityEngine.Networking; // Wajib untuk fitur internet
using System.Collections;
using System.Text; // Wajib untuk konversi teks

public class CloudManager : MonoBehaviour
{
    public static CloudManager Instance;

    [Header("Pengaturan Firebase")]
    [Tooltip("Paste URL Firebase-mu di sini, wajib diakhiri tanda garis miring (/)")]
    public string urlFirebase = "https://boxbot-edu-default-rtdb.asia-southeast1.firebasedatabase.app/"; 
    
    // Nama "folder" di dalam database-mu
    private string namaFolderDatabase = "data_boxbot.json"; 

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

    // Fungsi ini bisa dipanggil dari mana saja
    public void SinkronisasiKeCloud(string jsonTeks)
    {
        // Cek apakah ada koneksi internet sebelum mencoba mengirim
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            StartCoroutine(KirimData(jsonTeks));
        }
        else
        {
            Debug.Log("☁️ [CLOUD] Tidak ada internet. Data hanya tersimpan di HP (Offline).");
        }
    }

    private IEnumerator KirimData(string jsonTeks)
    {
        string urlLengkap = urlFirebase + namaFolderDatabase;

        // Kita gunakan metode PUT agar data di awan selalu sama persis dengan data di HP
        UnityWebRequest request = new UnityWebRequest(urlLengkap, "PUT");

        // Ubah teks JSON menjadi format byte yang bisa dikirim lewat internet
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonTeks);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        
        // Beritahu Firebase bahwa paket yang kita kirim adalah file JSON
        request.SetRequestHeader("Content-Type", "application/json");

        Debug.Log("☁️ [CLOUD] Sedang mengirim data ke Firebase...");

        // Tunggu sampai proses pengiriman selesai
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("✅ [CLOUD] SUKSES! Data berhasil diunggah ke Dashboard Guru!");
        }
        else
        {
            Debug.LogError("🚨 [CLOUD] Gagal mengirim data: " + request.error);
        }
    }
}