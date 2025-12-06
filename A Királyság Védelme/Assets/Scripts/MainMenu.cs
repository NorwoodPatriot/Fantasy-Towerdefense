using UnityEngine;
using UnityEngine.SceneManagement; // FONTOS: Ez kell a pályaváltáshoz!

public class MainMenu : MonoBehaviour
{
    [Header("Panelek")]
    public GameObject mapPanel;
    public GameObject villagePanel;

    [Header("Gombok")]
    // Ha van Level 2 gombod, húzd be ide, ha nincs, hagyd üresen
    public UnityEngine.UI.Button level2Button;

    void Start()
    {
        // Megnézzük, meddig jutott a játékos (alapból 1)
        int levelReached = PlayerPrefs.GetInt("LevelReached", 1);

        // Csak akkor foglalkozunk ezzel, ha be van kötve a gomb
        if (level2Button != null)
        {
            if (levelReached >= 2)
                level2Button.interactable = true;
            else
                level2Button.interactable = false;
        }
    }

    // --- PANELEK ---
    public void OpenMap() { mapPanel.SetActive(true); }
    public void CloseMap() { mapPanel.SetActive(false); }

    public void OpenVillage() { villagePanel.SetActive(true); }
    public void CloseVillage() { villagePanel.SetActive(false); }

    // --- INDÍTÁS ---
    public void StartLevel1()
    {
        Debug.Log("Indul a GameScene..."); // Ellen?rzés a konzolon

        Time.timeScale = 1f; // Id? újraindítása (ha Game Over miatt állna)
        SceneManager.LoadScene("GameScene"); // Pálya betöltése
    }

    // Ha van második pálya, ezt kösd a 2-es gombra
    public void StartLevel2()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level2"); // Gy?z?dj meg róla, hogy ez a neve!
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}