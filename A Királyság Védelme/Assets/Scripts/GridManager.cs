using UnityEngine;

public class GridManager : MonoBehaviour
{
    // --- SINGLETON (Hogy mindenki megtalálja) ---
    public static GridManager instance;

    void Awake()
    {
        instance = this;
    }

    [Header("Rács Beállítások")]
    public int rows = 9;       // Sorok száma (függőleges)
    public int cols = 5;       // Oszlopok száma (vízszintes)
    public float cellWidth = 2.86f;  // Egy mező szélessége
    public float cellHeight = 1.9f;  // Egy mező magassága

    [Header("Pozíció")]
    public Vector2 gridOrigin = new Vector2(-7.15f, 8.55f); // Bal felső sarok

    // Ez tárolja, melyik mező foglalt (true/false)
    private bool[,] isOccupied;

    void Start()
    {
        // Létrehozzuk az üres rács-memóriát
        isOccupied = new bool[rows, cols];
    }

    void Update()
    {
        // Ha kattintunk
        if (Input.GetMouseButtonDown(0))
        {
            // 1. Ellenőrizzük, van-e egység a "kezünkben" (kiválasztva a boltban)
            if (GameManager.instance.unitToPlace == null)
            {
                Debug.Log("Nincs kiválasztva egység! Kattints egy kártyára.");
                return;
            }

            // 2. Kiszámoljuk hova kattintottunk
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int indices = GetGridIndexes(worldPoint);
            int row = indices.x;
            int col = indices.y;

            // 3. Ellenőrizzük, hogy szabad-e a hely
            if (CanPlaceUnit(row, col))
            {
                // 4. Megpróbálunk fizetni érte
                bool sikeresVasarlas = GameManager.instance.SpendMana(GameManager.instance.unitCost);

                if (sikeresVasarlas)
                {
                    // 5. Ha sikerült, lerakjuk az egységet
                    PlaceUnit(row, col, GameManager.instance.unitToPlace);

                    // (Opcionális: itt kinullázhatnád a unitToPlace-t, ha azt akarod, 
                    // hogy minden lerakás után újra rá kelljen kattintani a kártyára)
                }
                else
                {
                    Debug.Log("Nincs elég manád!");
                }
            }
            else
            {
                Debug.Log("Ide nem építhetsz (foglalt vagy pályán kívül)!");
            }
        }
    }

    // --- FŐ FÜGGVÉNYEK ---

    // Kiszámolja a világ-koordinátát a rács indexekből
    public Vector2 GetWorldPosition(int row, int col)
    {
        float x = gridOrigin.x + col * cellWidth + cellWidth / 2f;
        float y = gridOrigin.y - row * cellHeight - cellHeight / 2f;
        return new Vector2(x, y);
    }

    // Kiszámolja a rács indexeket a kattintás helyéből
    public Vector2Int GetGridIndexes(Vector2 worldPosition)
    {
        float deltaX = worldPosition.x - gridOrigin.x;
        float deltaY = gridOrigin.y - worldPosition.y;

        int col = Mathf.FloorToInt(deltaX / cellWidth);
        int row = Mathf.FloorToInt(deltaY / cellHeight);

        return new Vector2Int(row, col);
    }

    // Ellenőrzi, hogy lerakható-e ide
    private bool CanPlaceUnit(int row, int col)
    {
        // Pályán belül van?
        if (row < 0 || row >= rows || col < 0 || col >= cols) return false;

        // Foglalt már?
        if (isOccupied[row, col]) return false;

        return true;
    }

    // Tényleges lerakás
    private void PlaceUnit(int row, int col, GameObject unitPrefab)
    {
        Vector2 placePosition = GetWorldPosition(row, col);

        // Létrehozás (Instantiate) - FIGYELD: használjuk a prefab saját forgatását!
        GameObject newUnit = Instantiate(unitPrefab, placePosition, unitPrefab.transform.rotation);

        // --- FONTOS: Megmondjuk az egységnek, hol lakik ---
        // Így amikor meghal, tudni fogja, melyik mezőt kell felszabadítani
        EletEro unitElet = newUnit.GetComponent<EletEro>();
        if (unitElet != null)
        {
            unitElet.BeallitRacsPozicio(row, col);
        }

        // Bejelöljük a rácsot foglaltnak
        isOccupied[row, col] = true;
        Debug.Log($"Egység telepítve: Sor: {row}, Oszlop: {col}");
    }

    // --- EZT HÍVJA A HALDOKLÓ EGYSÉG (EletEro.cs) ---
    public void CellFelszabadit(int row, int col)
    {
        if (row >= 0 && row < rows && col >= 0 && col < cols)
        {
            isOccupied[row, col] = false; // Újra szabaddá tesszük!
            Debug.Log($"Mező felszabadítva: {row}, {col} - Újra építhetsz ide!");
        }
    }

    // --- SEGÉDVONALAK (GIZMOS) ---
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                float x = gridOrigin.x + col * cellWidth + cellWidth / 2f;
                float y = gridOrigin.y - row * cellHeight - cellHeight / 2f;
                Gizmos.DrawWireCube(new Vector3(x, y, 0), new Vector3(cellWidth, cellHeight, 1));
            }
        }
    }
}