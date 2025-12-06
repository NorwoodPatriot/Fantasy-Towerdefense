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

    public void RestartGame()
    {
        // 1. Visszaállítjuk az id?t normálisra (mert Game Overkor megállítottuk 0-ra!)
        Time.timeScale = 1f;

        // 2. Újratöltjük az aktuális pályát (Scene-t)
        // Ehhez kell a "using UnityEngine.SceneManagement;" a fájl tetején (az már ott van nálad)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}