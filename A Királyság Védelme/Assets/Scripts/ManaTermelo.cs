using UnityEngine;
using System.Collections; // Erre szükség van az időzítéshez

public class ManaTermelo : MonoBehaviour
{
    [Header("Beállítások")]
    public int mennyiseg = 25;      // Mennyi manát adjon?
    public float idokoz = 5f;       // Hány másodpercenként? (pl. 5 másodperc)

    void Start()
    {
        // Amint megszületik a bányász, elindítjuk a termelést
        StartCoroutine(TermelesFolyamat());
    }

    // Ez egy speciális ciklus, ami tud várni az időben
    IEnumerator TermelesFolyamat()
    {
        while (true) // Végtelen ciklus, amíg a bányász él
        {
            // Várunk a megadott ideig (pl. 5 mp)
            yield return new WaitForSeconds(idokoz);

            // -- ITT FOGJUK MAJD HOZZÁADNI A JÁTÉKOS PÉNZÉHEZ --
            GameManager.instance.AddMana(mennyiseg);

            // Itt később lejátszhatsz egy hangot vagy animációt is!
        }
    }
}