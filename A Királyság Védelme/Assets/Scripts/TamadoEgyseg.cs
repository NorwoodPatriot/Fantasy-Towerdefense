using UnityEngine;
using System.Collections;

public class TamadoEgyseg : MonoBehaviour
{
    [Header("Lövés Beállítások")]
    public GameObject lovedekPrefab; // Mit l?jön ki? (A Varázsgömb)
    public float lovesIdokoz = 2f;   // Milyen gyakran? (másodperc)
    public Transform lovesPont;      // Honnan induljon a golyó? (Opcionális)

    void Start()
    {
        // --- ÚJ RÉSZ: SZINT LEKÉRDEZÉSE ---
        int myLevel = PlayerPrefs.GetInt("ElfLevel", 1);

        // Minden szinten 0.1 másodperccel gyorsabban l?
        // Lv1 = 2.0s, Lv2 = 1.9s, stb.
        lovesIdokoz = 2.0f - (myLevel - 1) * 0.1f;

        // Biztonsági korlát, ne legyen túl gyors (min 0.5 mp)
        if (lovesIdokoz < 0.5f) lovesIdokoz = 0.5f;
        // ----------------------------------
        StartCoroutine(LovesFolyamat());
    }

    IEnumerator LovesFolyamat()
    {
        while (true) // Végtelen ciklus
        {
            // Kés?bb itt ellen?rizzük majd, hogy VAN-E ellenség a sorban
            // De most tesztelésnek l?jön folyamatosan!

            Loves();

            // Várunk a következ? lövésig
            yield return new WaitForSeconds(lovesIdokoz);
        }
    }

    void Loves()
    {
        Instantiate(lovedekPrefab, transform.position, Quaternion.identity);

        // --- ÚJ SOR: LÖVÉS HANG ---
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.lovesHang);
        }
    }
}