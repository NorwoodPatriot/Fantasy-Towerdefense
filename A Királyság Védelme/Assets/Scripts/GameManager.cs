using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Játékos Adatai")]
    public int currentMana = 100;

    [Header("UI Referenciák")]
    public TMP_Text manaText;

    // --- KIVÁLASZTOTT EGYSÉG (Bolt) ---
    [Header("Bolt")]
    public GameObject unitToPlace; // Ez van most a "kezedben"
    public int unitCost;           // Ennyibe kerül

    void Awake() { instance = this; }

    void Start() { UpdateUI(); }

    // Ezt hívja a Kártya gomb
    public void SelectUnit(GameObject unit, int cost)
    {
        unitToPlace = unit;
        unitCost = cost;
        Debug.Log($"Kiválasztva: {unit.name}, Ára: {cost}");
    }

    // Ezt hívja a GridManager pénztermeléskor
    public void AddMana(int amount)
    {
        currentMana += amount;
        UpdateUI();
    }

    // Ezt hívja a GridManager vásárláskor
    public bool SpendMana(int amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            UpdateUI();
            return true; // Volt elég pénz, levontuk
        }
        return false; // Nem volt elég pénz
    }

    // (OPCIONÁLIS ÚJ RÉSZ)
    // Ezt majd akkor használhatod, ha azt akarod, hogy jobb klikkre "elejtse" az egységet
    public void DeselectUnit()
    {
        unitToPlace = null;
        unitCost = 0;
    }

    void UpdateUI()
    {
        if (manaText != null) manaText.text = currentMana.ToString();
    }
}