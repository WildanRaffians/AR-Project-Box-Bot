using UnityEngine;

public class EdgeController : MonoBehaviour
{
     public LevelManager manager;
    private bool sudahDiklik = false;
    private BoxCollider col;
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
            // 1. Ambil data ukuran visual objek (Renderer)
            Renderer rendr = GetComponent<Renderer>();
            if (rendr == null) return;

            // 2. Suntikkan BoxCollider biasa
            BoxCollider box = gameObject.AddComponent<BoxCollider>();
            
            // 3. MATEMATIKA OTOMATIS: Paksa BoxCollider mengikuti ukuran lokal objek
            // Kita ubah ukuran koordinat dunia (Bounds) menjadi koordinat lokal objek
            Vector3 ukuranLokal = transform.InverseTransformVector(rendr.bounds.size);
            
            // Sifat absolut: pastikan tidak ada nilai minus pada ukuran
            box.size = new Vector3(Mathf.Abs(ukuranLokal.x), Mathf.Abs(ukuranLokal.y), Mathf.Abs(ukuranLokal.z));
            box.center = Vector3.zero; // Titik pusat otomatis di tengah objek lokal

            // 4. BONUS PERTEBAL: Jika rusuk terlalu tipis, kita beri toleransi tebal (misal 0.01 atau 1cm)
            // agar jempol siswa lebih mudah menyentuhnya di AR
            Vector3 s = box.size;
            if (s.x < 0.0002f) s.x = 0.0002f;
            if (s.y < 0.0002f) s.y = 0.0002f;
            if (s.z < 0.0002f) s.z = 0.0002f;
            box.size = s;

            // Simpan referensinya ke variabel global
            col = box; 
            
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