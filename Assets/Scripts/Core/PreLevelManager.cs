using UnityEngine;
using Vuforia;
using System.Collections;

public class PreLevelManager : MonoBehaviour
{
    public LevelManager levelManager;
    public UIManager uiManager;
    private GameObject objekBangunAktif;
    private string namaBangun = "";

    [Header("Vuforia Plane")]
    public GameObject planeFinder;      
    public GameObject groundStage;      
    
    [Header("Tracker")]
    public GameObject rubikTracker;    
    public GameObject prismTracker;
    public GameObject balokTracker;
    
    [Header("Object Bangun Ruang")]
    public GameObject kubusSolid;
    public GameObject prismaSolid;
    public GameObject balokSolid;
    
    [Header("Lainnya")]
    string pesanArpy;
    public GameObject nextButton;
    public Animator arpyAnim; 

    void Start()
    {
        arpyAnim.SetTrigger("doExpla");
        SFXManager.Instance.MainkanArpyNoise(1);
        
        rubikTracker.SetActive(true);
        prismTracker.SetActive(true);
        balokTracker.SetActive(true);

        planeFinder.SetActive(false);
        groundStage.SetActive(false); 
        uiManager.SetTombolNextLevel(false);
    }

//TRACKER
    public void OnRubikFound()
    {
        levelManager.SetupDataBangun(LevelManager.TipeBangun.Kubus);
        MatikanSemuaTracker();
        namaBangun = "kubus";
        
        kubusSolid.SetActive(true); // Siapkan kubus virtual
        prismaSolid.SetActive(false); 
        balokSolid.SetActive(false); 
        objekBangunAktif = kubusSolid;
        
        NyalakanLantai();

    }
    public void OnPrismFound()
    {
        
        levelManager.SetupDataBangun(LevelManager.TipeBangun.PrismaSegitiga);
        MatikanSemuaTracker();
        namaBangun = "prisma segitiga";

        prismaSolid.SetActive(true); // Siapkan prisma virtual
        // AturColliderBangunRuang();
        kubusSolid.SetActive(false);
        balokSolid.SetActive(false);
        objekBangunAktif = prismaSolid;
        NyalakanLantai(); 
    }
    public void OnBalokFound()
    {
        
        levelManager.SetupDataBangun(LevelManager.TipeBangun.Balok);
        MatikanSemuaTracker();
        namaBangun = "balok";

        balokSolid.SetActive(true); // Siapkan prisma virtual
        kubusSolid.SetActive(false);
        prismaSolid.SetActive(false);
        objekBangunAktif = balokSolid;
        NyalakanLantai(); 
    }
    private void MatikanSemuaTracker()
    {
        if (rubikTracker != null) rubikTracker.SetActive(false);
        if (prismTracker != null) prismTracker.SetActive(false);
        if (balokTracker != null) balokTracker.SetActive(false);
        Debug.Log("traker mati!");
    }

// LANTAI
    private void NyalakanLantai()
    {
        planeFinder.SetActive(true);
        groundStage.SetActive(true);
        Debug.Log("lantai nyala!");
    }
    public void OnLevelPlaced()
    {
        SFXManager.Instance.MainkanTransisiWoosh();
        // 1. Matikan Plane Finder
        if (planeFinder != null) planeFinder.SetActive(false);

        arpyAnim.SetTrigger("doHU");
        SFXManager.Instance.MainkanArpyNoise(1);

        // 2. Beri pesan awal (Static)
        pesanArpy = "Yeay bangun ruang sudah diletakan!";
        uiManager.ShowCubeExplanation(pesanArpy);

        uiManager.ShowPopupPilihLevel(namaBangun);
        
    }

// MULAI LEVEL
    public void MulaiLevel1()
    {
        uiManager.TutupPopupPilihLevel();
        SFXManager.Instance.MainkanClick();
        levelManager.MulaiLevel(1);
    }
    public void MulaiLevel2()
    {
        uiManager.TutupPopupPilihLevel();
        SFXManager.Instance.MainkanClick();
        levelManager.MulaiLevel(2);
    }
    public void MulaiLevel3()
    {
        uiManager.TutupPopupPilihLevel();
        SFXManager.Instance.MainkanClick();
        levelManager.MulaiLevel(3);

    }
}