using UnityEngine;

public class VarianController : MonoBehaviour
{
    private Animator anim;
    private bool isOpened = false;
    public LevelManager levelManager;
    
    [Header("Settings")]
    public int variantID; // Set di Inspector: 0, 1, atau 2

    void Start()
    {
        anim = GetComponent<Animator>();
        
        if (levelManager == null)
        {
            levelManager = Object.FindFirstObjectByType<LevelManager>();
        }
    }

    // Fungsi baru untuk mendapatkan nama State Animator yang tepat
    public string AmbilNamaStateAnimasi()
    {
        // Logika ini harus SAMA PERSIS dengan nama kotak (State) di jendela Animator
        // Berdasarkan gambar kamu:
        if (variantID == 0) return "Armature|Buka";
        if (variantID == 1) return "Armature|Buka 0";
        if (variantID == 2) return "Armature|Buka 1";
        return "Armature|Buka"; // Untuk varian sisanya
    }

    public void TerkenaTap()
    {
        if (!isOpened)
        {
            // Trigger untuk transisi (saat diklik manual)
            anim.SetTrigger("doBuka" + variantID); 
            isOpened = true;
            
            if (levelManager != null)
                levelManager.LaporVarianTerbuka();
        }
    }

    // Fungsi untuk memaksa terbuka di akhir level
    public void PaksaBuka()
    {
        if (anim == null) anim = GetComponent<Animator>();
        
        // Langsung lompat ke frame terakhir animasi yang benar
        anim.Play(AmbilNamaStateAnimasi(), 0, 1f);
        isOpened = true;
    }
}