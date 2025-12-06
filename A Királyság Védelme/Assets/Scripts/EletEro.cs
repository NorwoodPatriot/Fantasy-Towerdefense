using UnityEngine;

public class EletEro : MonoBehaviour
{
    [Header("Élet Beállítások")]
    public int maxElet = 100;
    public int jelenlegiElet;

    [Header("Jutalom")]
    public int aranyErtek = 0; // Mennyi pénzt dobjon halálkor? (Véd?knél 0, Goblinnál pl. 10)

    // Rács pozíció (hogy tudjuk, honnan töröljük)
    private int myRow = -1;
    private int myCol = -1;

    void Start()
    {
        jelenlegiElet = maxElet;
    }

    public void BeallitRacsPozicio(int row, int col)
    {
        myRow = row;
        myCol = col;
    }

    public void SebzestKap(int sebzes)
    {
        jelenlegiElet -= sebzes;

        if (jelenlegiElet <= 0)
        {
            Meghal();
        }
    }

    void Meghal()
    {
        // 1. PÉNZ KIOSZTÁSA
        if (aranyErtek > 0 && GameManager.instance != null)
        {
            GameManager.instance.AddGold(aranyErtek);
            // Ha a manát is így kezeled, maradhat:
            GameManager.instance.AddMana(aranyErtek * 2);
            Debug.Log($"Hulla-jutalék: +{aranyErtek} Gold!");
        }

        // 2. RÁCS FELSZABADÍTÁSA (Csak Véd?knél fontos)
        // Az ellenségeknél a myRow/myCol általában -1 marad, így ez nem fut le náluk, ami helyes.
        if (myRow != -1 && myCol != -1)
        {
            if (GridManager.instance != null)
            {
                GridManager.instance.CellFelszabadit(myRow, myCol);
            }
        }

        // --- EZ AZ ÚJ RÉSZ AZ ORKOK MIATT! ---
        // 3. ÜTKÖZ? KIKAPCSOLÁSA
        // Ez azért kell, hogy a "halott" test már ne fogja fel az egérkattintást.
        // Így azonnal tudsz építeni a helyére, még miel?tt a Destroy teljesen eltüntetné.
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
        // -------------------------------------

        Destroy(gameObject);
    }
}