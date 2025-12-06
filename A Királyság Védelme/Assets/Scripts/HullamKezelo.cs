using UnityEngine;
using System.Collections;

[System.Serializable]
public class Hullam // Egy hullám adatai
{
    public string nev = "1. Hullám";
    public GameObject ellensegPrefab; // Milyen ellenség?
    public int darab = 5;             // Mennyi jöjjön?
    public float gyakorisag = 2f;     // Milyen gyorsan jöjjenek egymás után?
}

public class HullamKezelo : MonoBehaviour
{
    [Header("Hullámok Beállítása")]
    public Hullam[] hullamok; // Itt sorolod fel a hullámokat az Inspectorban
    public float hullamSzunet = 5f; // Szünet két hullám között

    [Header("Spawn Pozíciók")]
    public float[] oszlopXPoziciok;
    public float startY = -6f;

    private int jelenlegiHullamIndex = 0;
    private bool hullamFolyamatban = false;

    void Start()
    {
        // 5 másodperc múlva indul az els? hullám
        StartCoroutine(HullamInditas(5f));
    }

    void Update()
    {
        // Ha vége a spawningnak, figyeljük, mikor hal meg mindenki
        if (!hullamFolyamatban && jelenlegiHullamIndex < hullamok.Length)
        {
            // Megszámoljuk, hány ellenség maradt a pályán
            int eloEllensegek = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (eloEllensegek == 0)
            {
                // Ha 0 maradt, jöhet a következ? hullám!
                hullamFolyamatban = true; // Hogy ne indítsa el többször
                jelenlegiHullamIndex++;

                if (jelenlegiHullamIndex < hullamok.Length)
                {
                    Debug.Log("Hullám legy?zve! Jön a következ?...");
                    StartCoroutine(HullamInditas(hullamSzunet));
                }
                else
                {
                    Debug.Log("PÁLYA TELJESÍTVE!");
                    GameManager.instance.LevelComplete(); // GY?ZELEM!
                }
            }
        }
    }

    IEnumerator HullamInditas(float varakozas)
    {
        yield return new WaitForSeconds(varakozas);

        if (jelenlegiHullamIndex < hullamok.Length)
        {
            hullamFolyamatban = true;
            Hullam aktualisHullam = hullamok[jelenlegiHullamIndex];
            Debug.Log($"Indul a {aktualisHullam.nev}!");

            // Lespawnoljuk a darabszámot
            for (int i = 0; i < aktualisHullam.darab; i++)
            {
                SpawnEnemy(aktualisHullam.ellensegPrefab);
                yield return new WaitForSeconds(aktualisHullam.gyakorisag);
            }

            hullamFolyamatban = false; // Befejeztük a spawnolást, most a játékos öl
        }
    }

    void SpawnEnemy(GameObject prefab)
    {
        int randomIndex = Random.Range(0, oszlopXPoziciok.Length);
        Vector2 pos = new Vector2(oszlopXPoziciok[randomIndex], startY);
        Instantiate(prefab, pos, Quaternion.identity);
    }
}