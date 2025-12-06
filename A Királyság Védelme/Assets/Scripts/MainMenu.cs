using UnityEngine;
using UnityEngine.SceneManagement; // Kell a pályabetöltéshez

public class MainMenu : MonoBehaviour
{
    [Header("Panelek")]
    public GameObject mapPanel;
    public GameObject villagePanel;

    // --- TÉRKÉP MEGNYITÁSA / BEZÁRÁSA ---
    public void OpenMap()
    {
        mapPanel.SetActive(true);
    }

    public void CloseMap()
    {
        mapPanel.SetActive(false);
    }

    // --- FALU MEGNYITÁSA / BEZÁRÁSA ---
    public void OpenVillage()
    {
        villagePanel.SetActive(true);
    }

    public void CloseVillage()
    {
        villagePanel.SetActive(false);
    }

    // --- PÁLYA INDÍTÁSA ---
    public void StartLevel1()
    {
        // FONTOS: Itt pontosan a te Scene-ed nevét kell megadni!
        // A képeid alapján a neve: "GameScene"
        SceneManager.LoadScene("GameScene");
    }

    // --- KILÉPÉS ---
    public void QuitGame()
    {
        Debug.Log("Kilépés...");
        Application.Quit();
    }
}