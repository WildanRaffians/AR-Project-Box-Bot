using UnityEngine;

public class EdgeController : MonoBehaviour
{
     public Level1Manager manager;
    private bool sudahDiklik = false;
    private Collider col;
    private Color warnaAsli;
    private Renderer rend;
    void Start()
    {
        rend = GetComponent<Renderer>();
        
        // 1. Simpan warna asli bawaan model ke dalam memori
        if (rend != null)
        {
            warnaAsli = rend.material.color;
        }
        if (manager == null)
        {
            manager = Object.FindFirstObjectByType<Level1Manager>();
        }
    }

    // Fungsi ini terpanggil otomatis jika objek punya Collider dan disentuh (di Android/Editor)
    public void OnEdgeClicked()
    {
        if (!sudahDiklik)
        {
            sudahDiklik = true;
            
            // Beri umpan balik visual (Ubah warna jadi Hijau)
            GetComponent<Renderer>().material.color = Color.yellow;
            
            // Beritahu manager bahwa satu titik ditemukan
            manager.TambahRusuk();
            
            // Efek tambahan (opsional: matikan collider agar tidak bisa diklik lagi)
            Debug.Log("Titik Sudut Ditemukan!");
        }
    }

    public void AktifkanInteraksi()
    {
        if (!sudahDiklik && col == null)
        {
            //OPSI DENGAN MESHCOLLIDER

            // MeshCollider meshCol = gameObject.AddComponent<MeshCollider>();
            
            // // Wajib dinyalakan agar bentuk miringnya terdeteksi sempurna oleh Raycast
            // meshCol.convex = true; 
            
            // // Simpan ke variabel global jika kamu menggunakan 'col' untuk Destroy nanti
            // col = meshCol;

            //OPSI DENGAN BOXCOLLIDER

            // 1. Tambahkan BoxCollider
            BoxCollider boxCol = gameObject.AddComponent<BoxCollider>();
            
            // 2. Trik UX: Ambil ukuran aslinya yang sangat tipis
            Vector3 ukuranAsli = boxCol.size;
            
            // 3. Gemukkan sisi-sisinya (kecuali panjangnya) agar gampang disentuh jari!
            // Mathf.Max akan memastikan ketebalannya minimal 0.05f
            float ketebalanSentuh = 0.00005f; 
            
            // Asumsi: Sumbu Y adalah panjang rusuk. Kita gemukkan sumbu X dan Z.
            // (Jika yang panjang adalah sumbu Z, ubah kodenya menyesuaikan sumbu yang pendek)
            boxCol.size = new Vector3(
                Mathf.Max(ukuranAsli.x, ketebalanSentuh), 
                ukuranAsli.y, 
                Mathf.Max(ukuranAsli.z, ketebalanSentuh)
            );

            // Simpan ke variabel global
            col = boxCol;
            
            Debug.Log($"[SUNTIK EDGE] BoxCollider otomatis terpasang pada {gameObject.name}");
        }
    }
    public void ResetRusuk()
    {
        sudahDiklik = false;
        
        // Kembalikan ke warna asli
        if (rend != null) rend.material.color = warnaAsli;
        
        // Pastikan collider mati
        // if (col != null) col.enabled = false; 
        if (col != null) Destroy(col);
    }
}