using UnityEngine;

public class FaceController : MonoBehaviour
{
    [Header("Referensi Manager")]
    public LevelManager manager;
    
    private bool sudahDiklik = false;
    private Color warnaAsli;
    private MeshCollider col;
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

    public void OnFaceClicked()
    {
        if (!sudahDiklik)
        {
            sudahDiklik = true;
            
            // Ubah warna jadi Hijau
            if (rend != null) rend.material.color = Color.green;
            
            // Beritahu manager
            if (manager != null) manager.TambahSisi();
            
            // Matikan collider agar tidak bisa diklik dua kali oleh jari yang jail
            // if (col != null) col.enabled = false; 
            if (col != null) Destroy(col);
            
            Debug.Log("Sisi Ditemukan!"); 
        }
    }

    // 3. Fungsi baru: Dipanggil LevelManager SAAT MISI DIMULAI
    public void AktifkanInteraksi()
    {
        if (col != null && !sudahDiklik) 
        {
            col.enabled = true;
        } 
        else if (!sudahDiklik && col == null) 
        {
            // TEMBAKKAN SUNTIKAN KOMPONEN BARU LEWAT CODING!
            col = gameObject.AddComponent<MeshCollider>();
        }
    }
    
    // 4. Fungsi baru: Dipanggil LevelManager SAAT MISI SELESAI
    public void ResetSisi()
    {
        sudahDiklik = false;
        
        // Kembalikan ke warna asli
        if (rend != null) rend.material.color = warnaAsli;
        
        // Pastikan collider mati
        // if (col != null) col.enabled = false; 
        if (col != null) Destroy(col);
    }
}