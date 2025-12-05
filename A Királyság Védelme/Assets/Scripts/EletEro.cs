using UnityEngine;

public class EletEro : MonoBehaviour
{
    public int maxElet = 100;
    public int jelenlegiElet;

    // --- ÚJ VÁLTOZÓK: Hol vagyok a rácson? ---
    // Alapból -1, ami azt jelenti: "nem vagyok rácson" (pl. Goblin)
    private int myRow = -1;
    private int myCol = -1;

    void Start()
    {
        jelenlegiElet = maxElet;
    }

    // --- ÚJ FÜGGVÉNY: A GridManager hívja meg lerakáskor ---
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
        // --- ÚJ RÉSZ: Szólunk a rácsnak, ha volt helyünk ---
        if (myRow != -1 && myCol != -1)
        {
            // Ha létezik a GridManager, szólunk neki
            if (GridManager.instance != null)
            {
                GridManager.instance.CellFelszabadit(myRow, myCol);
            }
        }

        Destroy(gameObject);
    }
}