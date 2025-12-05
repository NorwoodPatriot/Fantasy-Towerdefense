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
    public bool isGameOver = false;

    [Header("UI Referenciák")]
    public TMP_Text manaText;
    public TMP_Text livesText;
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
        Time.timeScale = 0;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    void UpdateUI()
    {
        if (manaText != null) manaText.text = currentMana.ToString();
        if (livesText != null) livesText.text = currentLives.ToString();
    }
}