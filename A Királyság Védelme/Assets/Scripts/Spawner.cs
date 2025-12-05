using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject goblinPrefab; // Mit gyártsunk?
    public float spawnIdokoz = 3f;  // Milyen gyakran?

    // Ezeket a GridManagerb?l is kiolvashatnánk, de most egyszer?sítünk:
    // Milyen X koordinátákon vannak az oszlopok közepei?
    // (Ezt majd kézzel beállítjuk a Unity-ben)
    public float[] oszlopXPoziciok;

    public float startY = -6f; // Milyen magasan kezdjenek (alul)?

    void Start()
    {
        StartCoroutine(SpawnFolyamat());
    }

    IEnumerator SpawnFolyamat()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnIdokoz);
            SpawnGoblin();
        }
    }

    void SpawnGoblin()
    {
        // 1. Választunk egy véletlenszer? oszlopot
        int randomIndex = Random.Range(0, oszlopXPoziciok.Length);
        float randomX = oszlopXPoziciok[randomIndex];

        // 2. Összerakjuk a pozíciót (Véletlen X, Fix Y alul)
        Vector2 spawnPos = new Vector2(randomX, startY);

        // 3. Létrehozzuk a Goblint
        Instantiate(goblinPrefab, spawnPos, Quaternion.identity);
    }
}