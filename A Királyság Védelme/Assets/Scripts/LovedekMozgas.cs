using UnityEngine;

public class LovedekMozgas : MonoBehaviour
{
    public float sebesseg = 5f;
    public int sebzes = 1;

    void Update()
    {
        // Lefelé mozog
        transform.Translate(Vector2.down * sebesseg * Time.deltaTime, Space.World);

        // Ha kimegy a pályáról alul, elt?nik
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    // --- EZ A RÉSZ FIGYELI AZ ÜTKÖZÉST ---
    void OnTriggerEnter2D(Collider2D other)
    {
        // Megnézzük, hogy akinek nekiütköztünk, az Ellenség-e?
        if (other.CompareTag("Enemy"))
        {
            // Megkeressük rajta az Élet scriptet
            EletEro enemyElet = other.GetComponent<EletEro>();

            // Ha van élete, sebezzük meg!
            if (enemyElet != null)
            {
                enemyElet.SebzestKap(sebzes);
            }

            // A golyó becsapódott, tehát megsemmisül
            Destroy(gameObject);
        }
    }
}