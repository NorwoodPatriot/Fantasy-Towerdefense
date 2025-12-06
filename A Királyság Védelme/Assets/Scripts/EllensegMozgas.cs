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

    // --- ÚJ VÁLTOZÓ ---
    private Animator anim;

    void Start()
    {
        // --- ÚJ SOR: Megkeressük az animátort az induláskor ---
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Mozgás (csak ha nem támad)
        if (!tamad)
        {
            transform.Translate(Vector2.up * sebesseg * Time.deltaTime);
        }

        // --- HA FELÉR A TET?RE ---
        if (transform.position.y > 5.5f)
        {
            // 1. Szólunk a GameManagernek, hogy vesztettünk 1 életet
            if (GameManager.instance != null) // Biztonsági ellen?rzés
            {
                GameManager.instance.PlayerTakeDamage(1);
            }

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

                // --- ÚJ SOR: ANIMÁCIÓ VÁLTÁS (TÁMADÁS) ---
                if (anim != null) anim.SetBool("isAttacking", true);
                // -----------------------------------------

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

            // Várunk a következ? harapásig
            yield return new WaitForSeconds(tamadasSebesseg);
        }

        // Ha a ciklusnak vége, az azt jelenti, a célpont meghalt (null lett).

        // 3. Újra elindulunk!
        tamad = false;
        celpont = null;

        // --- ÚJ SOR: ANIMÁCIÓ VÁLTÁS (FUTÁS) ---
        // Kikapcsoljuk a támadást, így a nyilak visszaviszik a Run-ba
        if (anim != null) anim.SetBool("isAttacking", false);
        // --------------------------------------
    }
}