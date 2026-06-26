using UnityEngine;
using UnityEditor;
using System.IO;

public class ResetDataTool
{
    // Ini akan memunculkan menu baru di bagian paling atas Unity Editor-mu!
    [MenuItem("BoxBot/🔥 Reset Data JSON Lokal")]
    public static void HapusDataLokal()
    {
        // Sesuaikan nama file ini jika di OfflineDatabaseManager-mu berbeda
        string path = Application.persistentDataPath + "/database_boxbot.json"; 

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("✅ [RESET] Data JSON lokal berhasil dihapus total!");
        }
        else
        {
            Debug.Log("⚠️ [RESET] Aman! File JSON tidak ditemukan atau memang sudah kosong.");
        }
    }
}