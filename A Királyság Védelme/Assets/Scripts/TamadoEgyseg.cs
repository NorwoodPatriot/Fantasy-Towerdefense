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