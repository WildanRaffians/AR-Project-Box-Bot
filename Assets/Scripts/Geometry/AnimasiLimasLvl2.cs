using UnityEngine;
using System.Collections;

public class AnimasiLimas : MonoBehaviour
{
    [Header("Referensi Objek")]
    public Transform objekIsiLimas; // Masukkan 3D model "Isi Limas" di Inspector

    [Header("Pengaturan Animasi")]
    public float durasiAnimasi = 1.5f; // Berapa detik proses pengisian berlangsung
    
    // Fungsi ini dipanggil saat pemain mengetuk layar
    public void MulaiIsiLimas()
    {
        // Pastikan skala awalnya 0 (kosong) sebelum diisi
        objekIsiLimas.localScale = Vector3.zero;
        objekIsiLimas.gameObject.SetActive(true);
        
        StartCoroutine(ProsesLerpSkala());
    }

    private IEnumerator ProsesLerpSkala()
    {
        Vector3 skalaAwal = Vector3.zero;
        
        // Asumsi skala penuhnya adalah 1 di X, Y, Z. 
        // Jika ukuran aslimu berbeda, ganti Vector3.one dengan ukuran aslimu, misal: new Vector3(0.9f, 0.9f, 0.9f)
        Vector3 skalaAkhir = Vector3.one; 
        
        float waktu = 0;

        while (waktu < durasiAnimasi)
        {
            // Lerp mengubah vektor secara perlahan dari awal ke akhir berdasarkan waktu
            objekIsiLimas.localScale = Vector3.Lerp(skalaAwal, skalaAkhir, waktu / durasiAnimasi);
            
            // Tambahkan waktu setiap frame berjalan
            waktu += Time.deltaTime; 
            
            // Tunggu frame berikutnya
            yield return null; 
        }

        // Pastikan di akhir animasi, ukurannya pas 100% (tidak meleset karena desimal)
        objekIsiLimas.localScale = skalaAkhir;
    }
}