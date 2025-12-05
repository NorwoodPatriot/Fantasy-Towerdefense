using UnityEngine;

public class RobbanoEgyseg : MonoBehaviour
{
    [Header("Robbanás Adatai")]
    public int robbanasSebzes = 500; // Jó nagyot sebezzen (azonnal öljön)

    // Amikor valaki belép az Akna területére (Trigger)
    void OnTriggerEnter2D(Collider2D other)
    {
        // Csak akkor robbanunk, ha ELLENSÉG lépett ránk
        if (other.CompareTag("Enemy"))
        {
            // Megkeressük az ellenség életét
            EletEro enemyElet = other.GetComponent<EletEro>();

            if (enemyElet != null)
            {
                // Bumm! Megsebezzük az ellenséget
                enemyElet.SebzestKap(robbanasSebzes);
                Debug.Log("?? BUMM! Az akna felrobbant!");
            }

            // Az akna is megsemmisül a robbanásban (feláldozza magát)
            Destroy(gameObject);

            // TIPP: Ide kés?bb rakhatsz be hangot vagy robbanás effektet!
        }
    }
}