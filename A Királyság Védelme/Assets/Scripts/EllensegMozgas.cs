using UnityEngine;
using System.Collections;

public class EllensegMozgas : MonoBehaviour
{
    [Header("Mozgás")]
    public float sebesseg = 1.5f;

    [Header("Támadás")]
    public int sebzes = 10;      // Mennyit harap?
    public float tamadasSebesseg = 1.0f; // Milyen gyorsan harap (mp)?

    private bool tamad = false;  // Éppen eszik valakit?
    private GameObject celpont;  // Kit eszik éppen?

    void Update()
    {
        // Mozgás (csak ha nem támad)
        if (!tamad)
        {
            transform.Translate(Vector2.up * sebesseg * Time.deltaTime);
        }

        // --- ÚJ RÉSZ: HA FELÉR A TET?RE ---
        // (Ellen?rizd, hogy a 10 jó-e, lehet, hogy a te pályádon ez 5 vagy 6)
        if (transform.position.y > 5.5f) // Átírtam 5.5-re, mert a te rácsod teteje kb. ott van!
        {
            // 1. Szólunk a GameManagernek, hogy vesztettünk 1 életet
            GameManager.instance.PlayerTakeDamage(1);

            // 2. A Goblin elt?nik (bement a házba)
            Destroy(gameObject);
        }
    }

    // --- ÜTKÖZÉS A VÉD?VEL ---
    void OnTriggerEnter2D(Collider2D other)
    {
        // Ha beleütközünk egy "Defender" (Véd?) címkéj? dologba
        if (other.CompareTag("Defender"))
        {
            // Megkeressük rajta az életet
            EletEro vedoElet = other.GetComponent<EletEro>();

            if (vedoElet != null)
            {
                // 1. Megállunk
                tamad = true;
                celpont = other.gameObject;

                // 2. Elkezdjük "rágni" (Coroutinnal)
                StartCoroutine(TamadasFolyamat(vedoElet));
            }
        }
    }

    IEnumerator TamadasFolyamat(EletEro vedoElet)
    {
        // Addig csináljuk, amíg a célpont létezik (nem halt meg)
        while (celpont != null)
        {
            // Harapás!
            vedoElet.SebzestKap(sebzes);
            // Debug.Log("Nyam-nyam, eszem a növényt!");

            // Várunk a következ? harapásig
            yield return new WaitForSeconds(tamadasSebesseg);
        }

        // Ha a ciklusnak vége, az azt jelenti, a célpont meghalt (null lett).
        // 3. Újra elindulunk!
        tamad = false;
        celpont = null;
    }
}