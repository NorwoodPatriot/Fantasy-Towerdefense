using UnityEngine;
using TMPro; // Kell a szöveghez
using UnityEngine.SceneManagement; // Kell majd az újraindításhoz

public class GameManager : MonoBehaviour
{
    // --- SINGLETON ---
    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    [Header("Játékos Adatai")]
    public int currentMana = 100;
    public int currentLives = 3;
    public int currentGold = 0;
    public bool isGameOver = false;

    [Header("UI Referenciák")]
    public TMP_Text manaText;
    public TMP_Text livesText;
    public TMP_Text goldText;
    public GameObject gameOverPanel;

    [Header("Bolt")]
    public GameObject unitToPlace; // Ez van most a "kezedben"
    public int unitCost;           // Ennyibe kerül

    void Start()
    {
        UpdateUI();
    }

    // --- FÜGGVÉNYEK ---

    public void SelectUnit(GameObject unit, int cost)
    {
        unitToPlace = unit;
        unitCost = cost;
        Debug.Log($"Kiválasztva: {unit.name}, Ára: {cost}");
    }

    public void AddMana(int amount)
    {
        currentMana += amount;
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateUI();

        int bankGold = PlayerPrefs.GetInt("TotalGold", 0);
        PlayerPrefs.SetInt("TotalGold", bankGold + amount);
        PlayerPrefs.Save();
    }

    public bool SpendMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            UpdateUI();
            return true;
        }
        return false;
    }

    public void PlayerTakeDamage(int damage)
    {
        if (isGameOver) return;

        currentLives -= damage;
        UpdateUI();

        Debug.Log($"Jaj! Egy Goblin bejutott! Maradt {currentLives} életed.");

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER! Vesztettél!");
        if (AudioManager.instance != null)
        {
            AudioManager.instance.musicSource.Stop();
            AudioManager.instance.PlaySFX(AudioManager.instance.gameOverHang);
        }
        Time.timeScale = 0;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        // --- ÚJ SOR: GAME OVER HANG ---
        // El?ször leállítjuk a zenét, hogy drámai legyen
       
    }
  
    void UpdateUI()
    {
        if (manaText != null) manaText.text = currentMana.ToString();
        if (livesText != null) livesText.text = currentLives.ToString();
        if (goldText != null) goldText.text = currentGold.ToString();
    }


    [Header("Szint Rendszer")]
    public int currentLevelIndex = 1; // Hányas pálya ez? (Inspectorban állítsd be!)
    public GameObject winPanel;       // Húzz be ide egy "Gy?zelem" panelt!

   

    public void RestartGame()
    {
        // 1. Visszaállítjuk az id?t normálisra (mert Game Overkor megállítottuk 0-ra!)
        Time.timeScale = 1f;

        // 2. Újratöltjük az aktuális pályát (Scene-t)
        // Ehhez kell a "using UnityEngine.SceneManagement;" a fájl tetején (az már ott van nálad)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        // 1. Nagyon fontos: Visszaállítjuk az id?t!
        // Ha Game Over közben (amikor áll az id?) lépsz ki, a Menü is "fagyott" lenne.
        Time.timeScale = 1f;

        // 2. Betöltjük a menüt
        // Gy?z?dj meg róla, hogy a Scene neve pontosan "MainMenu"!
        SceneManager.LoadScene("MainMenu");
    }


    public void LevelComplete()
    {
        if (isGameOver) return; // Ha már meghaltál, ne nyerj

        Debug.Log("SZÉP VOLT! PÁLYA KÉSZ!");

        // Elmentjük, hogy ezt a pályát megcsináltad
        // Ha a 1-es pályán vagy, akkor a "LevelReached" legyen 2
        int mentettSzint = PlayerPrefs.GetInt("LevelReached", 1);
        if (currentLevelIndex >= mentettSzint)
        {
            PlayerPrefs.SetInt("LevelReached", currentLevelIndex + 1);
            PlayerPrefs.Save();
        }

        // Megjelenítjük a Gy?zelem Panelt
        if (winPanel != null) winPanel.SetActive(true);
        Time.timeScale = 0; // Játék megáll
    }

    public void NextLevel()
    {
        // 1. Visszaállítjuk az id?t (nagyon fontos!)
        Time.timeScale = 1f;

        // 2. Megnézzük, hányas számú pályán vagyunk most
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 3. Kiszámoljuk a következ?t
        int nextSceneIndex = currentSceneIndex + 1;

        // 4. Ellen?rizzük, hogy létezik-e a következ? pálya
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            // Ha van, betöltjük
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // Ha elfogytak a pályák (nincs több), visszamegyünk a menübe
            Debug.Log("Nincs több pálya, gratulálok! Vissza a menübe.");
            SceneManager.LoadScene("MainMenu");
        }
    }
}