using UnityEngine;
using TMPro;

public class FaluManager : MonoBehaviour
{
    [Header("UI Elemek")]
    public TMP_Text goldText;
    public TMP_Text minerBtnText;
    public TMP_Text elfBtnText;

    // A jelenlegi szintek és pénz
    int totalGold;
    int minerLevel;
    int elfLevel;

    // Mennyibe kerül a fejlesztés? (Alap ár * szint)
    int minerCost => minerLevel * 100;
    int elfCost => elfLevel * 150;

    void Start()
    {
        // Betöltjük az adatokat a "jegyzetfüzetb?l"
        totalGold = PlayerPrefs.GetInt("TotalGold", 0); // Ha nincs mentés, 0
        minerLevel = PlayerPrefs.GetInt("MinerLevel", 1); // Alapból 1-es szint
        elfLevel = PlayerPrefs.GetInt("ElfLevel", 1);

        UpdateUI();
    }

    void UpdateUI()
    {
        goldText.text = "Aranyad: " + totalGold;

        minerBtnText.text = $"Bányász Lv {minerLevel + 1}\nÁr: {minerCost}";
        elfBtnText.text = $"Elf Lv {elfLevel + 1}\nÁr: {elfCost}";
    }

    // --- GOMB FUNKCIÓK ---

    public void BuyMinerUpgrade()
    {
        if (totalGold >= minerCost)
        {
            totalGold -= minerCost;       // Pénz levonása
            minerLevel++;                 // Szint növelése

            // Mentés
            PlayerPrefs.SetInt("TotalGold", totalGold);
            PlayerPrefs.SetInt("MinerLevel", minerLevel);
            PlayerPrefs.Save();

            UpdateUI();
            Debug.Log("Bányász fejlesztve!");
        }
    }

    public void BuyElfUpgrade()
    {
        if (totalGold >= elfCost)
        {
            totalGold -= elfCost;
            elfLevel++;

            PlayerPrefs.SetInt("TotalGold", totalGold);
            PlayerPrefs.SetInt("ElfLevel", elfLevel);
            PlayerPrefs.Save();

            UpdateUI();
            Debug.Log("Elf fejlesztve!");
        }
    }
}