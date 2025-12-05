using UnityEngine;

public class UnitCard : MonoBehaviour
{
    public GameObject unitPrefab; // Melyik egység ez? (TorpBanyasz)
    public int cost = 50;         // Mennyibe kerül?

    // Ezt a függvényt kötjük majd be a Gomb-ra
    public void OnClick()
    {
        // Szólunk a GameManagernek, hogy ezt választottuk ki
        GameManager.instance.SelectUnit(unitPrefab, cost);
    }
}