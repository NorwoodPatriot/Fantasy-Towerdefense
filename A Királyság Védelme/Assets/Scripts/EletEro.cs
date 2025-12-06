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
        // --- ÚJ RÉSZ: PÉNZ KIOSZTÁSA ---
        // Csak akkor adunk pénzt, ha van értéke (pl. Goblin) és létezik a GameManager
        if (aranyErtek > 0 && GameManager.instance != null)
        {
            GameManager.instance.AddGold(aranyErtek);
            GameManager.instance.AddMana(aranyErtek*2);
            Debug.Log($"Hulla-jutalék: +{aranyErtek} Gold!");
        }
        // -------------------------------

        // Szólunk a rácsnak, ha volt helyünk (ez a Véd?kre vonatkozik)
        if (myRow != -1 && myCol != -1)
        {
            if (GridManager.instance != null)
            {
                GridManager.instance.CellFelszabadit(myRow, myCol);
            }
        }

        Destroy(gameObject);
    }
}