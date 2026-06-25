using UnityEngine;

public class VertexController : MonoBehaviour
{
    public LevelManager manager;
    private bool sudahDiklik = false;
    
    // Variabel memori untuk menyimpan status awal
    private Color warnaAsli;
    private Renderer rend;
    private SphereCollider col;

    void Start()
    {
        rend = GetComponent<Renderer>();
        
        // 1. Simpan warna asli bawaan bola sejak awal
        if (rend != null)
        {
            warnaAsli = rend.material.color;
        }
    }

    // Fungsi ini terpanggil otomatis jika objek punya Collider dan disentuh (di Android/Editor)
    public void OnMouseDown()
    {
        if (!sudahDiklik)
        {
            sudahDiklik = true;
            
            // Beri umpan balik visual (Ubah warna jadi Hijau)
            if (rend != null) rend.material.color = Color.green;
            
            // Beritahu manager bahwa satu titik ditemukan
            if (manager != null) manager.TambahSudut();
            
            // Matikan collider agar tidak bisa diklik berkali-kali oleh user
            // if (col != null) col.enabled = false;
            if (col != null) Destroy(col);
            
            Debug.Log("Titik Sudut Ditemukan!");
        }
    }

    // 3. FUNGSI BARU: Dipanggil oleh LevelManager saat misi sudut DIMULAI
    public void AktifkanInteraksi()
    {
        if (col != null && !sudahDiklik)
        {
            col.enabled = true;
        }
        else if (!sudahDiklik && col == null)
        {
            // 1. Suntikkan komponen Sphere Collider secara paksa
            col = gameObject.AddComponent<SphereCollider>();
            
            // 2. Atur radiusnya menjadi sangat kecil sesuai permintaanmu
            col.radius = 0.0015f;
            
            Debug.Log($"[SUNTIK] SphereCollider berhasil dipasang pada {gameObject.name} dengan radius 0.002");
        }
    }

    // 4. FUNGSI BARU: Dipanggil oleh LevelManager saat misi sudut SELESAI / ingin diulang
    public void ResetSudut()
    {
        sudahDiklik = false;
        
        // Kembalikan bola ke warna aslinya
        if (rend != null) rend.material.color = warnaAsli;
        
        // Pastikan collider mati kembali
        // if (col != null) col.enabled = false;
        if (col != null) Destroy(col);

    }
}