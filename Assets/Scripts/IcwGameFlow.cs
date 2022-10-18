using System.Collections.Generic;
using UnityEngine;

public class IcwGameFlow : MonoBehaviour, IGame
{
    private GameObject player;
    private GameObject routeBuilder;
    public GameObject Flowers;
    public GameObject Coins;
    public GameObject FlowerPrefab;
    public GameObject CoinPrefab;
    public GameObject PlayerPrefab;
    public GameObject StartGameWindowPrefab;
    public GameObject RouteBuilderPrefab;

    public IGame.EnumGameState gameState { get; set; }

    public int ItemsCount;
    private Rect field;
    private List<Vector3> flowersPositionList = new List<Vector3>();
    private List<Vector3> coinsPositionList = new List<Vector3>();


    void SetFieldSize()
    {
        Camera cam = Camera.main;
        // каждый объект размером 1 unit.
        // отнимаем 0.5 с каждой стороны, чтобы влезали полностью
        float x = cam.ViewportToWorldPoint(Vector2.zero).x + 0.5f;
        float y = cam.ViewportToWorldPoint(Vector2.zero).y + 0.5f;
        float w = cam.ViewportToWorldPoint(Vector2.one).x - 0.5f;
        float h = cam.ViewportToWorldPoint(Vector2.one).y - 0.5f;

        field = new Rect(x, y, w - x, h - y);
        Debug.LogWarning(field.ToString());
    }

    private void Awake()
    {
        SetFieldSize();
        gameState = IGame.EnumGameState.FirstStart;
        ItemsCount = 5;
        ShowUiWindow();
    }

    private Vector3 GetRandomPos(List<Vector3> objectsPositionList)
    {
        Vector3 pos = Vector3.zero;
        do
        {
            pos.x = Random.Range(field.xMin, field.xMax);
            pos.y = Random.Range(field.yMin, field.yMax);
        }
        while (objectsPositionList.Exists((Vector3 o) => Vector3.Distance(o, pos) < 2f));
        objectsPositionList.Add(pos);
        return pos;
    }

    public void EndGame()
    {
        foreach (Transform child in Flowers.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in Coins.transform)
        {
            Destroy(child.gameObject);
        }
        if (player != null) Destroy(player);
        if (routeBuilder != null) Destroy(routeBuilder);
    }

    public void StartNewGame()
    {
        Vector3 pos;
        if (gameState != IGame.EnumGameState.GameOver && gameState != IGame.EnumGameState.GameWin)
            EndGame();
        if (gameState == IGame.EnumGameState.GameWin && ItemsCount < 11) 
            ItemsCount++;
        List<Vector3> objectsPositionList = new List<Vector3>();
        player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);
        objectsPositionList.Add(Vector3.zero);
        
        routeBuilder = Instantiate(RouteBuilderPrefab, this.transform);
        routeBuilder.GetComponent<IcwRouteBuilder>().walker = player.GetComponent<IWalker>();
        
        if (gameState == IGame.EnumGameState.GameOver)
        {
            // Reset previous field
            for (int i = 0; i < flowersPositionList.Count; i++)
            {
                pos = flowersPositionList[i];
                Instantiate(FlowerPrefab, pos, Quaternion.identity, Flowers.transform);
            }
            for (int i = 0; i < coinsPositionList.Count; i++)
            {
                pos = coinsPositionList[i];
                Instantiate(CoinPrefab, pos, Quaternion.identity, Coins.transform);
            }
        }
        else
        {
            flowersPositionList.Clear();
            coinsPositionList.Clear();
            // Generate new field
            for (int i = 0; i < ItemsCount; i++)
            {
                pos = GetRandomPos(objectsPositionList);
                Instantiate(FlowerPrefab, pos, Quaternion.identity, Flowers.transform);
                flowersPositionList.Add(pos);

                pos = GetRandomPos(objectsPositionList);
                Instantiate(CoinPrefab, pos, Quaternion.identity, Coins.transform);
                coinsPositionList.Add(pos);
            }
        }
    }

    public IWalker GetPlayer()
    {
        if (player != null)
            return player.GetComponent<IWalker>();
        else
            return null;
    }

    void ShowUiWindow()
    {
        GameObject UiWindow = Instantiate(StartGameWindowPrefab);
        if (UiWindow == null)
        {
            Debug.LogWarning("UiWindow Gameobject didn't instatiate.");
            return;
        }
        IUiWindow UiWindowClass = UiWindow.GetComponent<IUiWindow>();
        if (UiWindowClass == null)
        {
            Debug.LogWarning("Can't find UiWindow component. Check uiScreem prefab");
            Destroy(UiWindow);
            return;
        }

        string atext = "";

        if (gameState == IGame.EnumGameState.FirstStart)
            atext = $"Tap on the screen to build a route.\r\n\r\nAvoid contacs with angry flowers.\r\n\r\nGet all coins by {ItemsCount + 1} clicks!\r\n";
        if (gameState == IGame.EnumGameState.GameOver)
            atext = $"U'are dead.\r\n\r\nAvoid contacs with angry flowers.\r\n\r\nGet all coins by {ItemsCount + 1} clicks!\r\n";
        if (gameState == IGame.EnumGameState.GameWin)
        {
            int numclicks = routeBuilder.GetComponent<IcwRouteBuilder>().RouteItemsCount;
            if (numclicks <= ItemsCount + 1)
                atext = $"Good job.\r\n\r\nYou've got it by {numclicks} of {ItemsCount + 1} click.\r\n\r\nGet all coins by {ItemsCount + 2} clicks!\r\n";
            else
            {
                atext = $"Not bad.\r\n\r\nBut you've got it just by {numclicks} of {ItemsCount + 1} click.\r\n\r\nGet all coins by {ItemsCount + 1} clicks!\r\n";
                gameState = IGame.EnumGameState.GameOver;
            }
        }

        UiWindowClass.SetStateText(atext);
        UiWindowClass.SetGame(this);
    }

    public void CheckGameState()
    {
        if (Coins.transform.childCount == 0) gameState = IGame.EnumGameState.GameWin; 
        if (player == null) gameState = IGame.EnumGameState.GameOver;
    }

    private void Update()
    {
        if (gameState == IGame.EnumGameState.InProgress)
        {
            CheckGameState();
            if (gameState == IGame.EnumGameState.GameOver || gameState == IGame.EnumGameState.GameWin)
                EndGame();
            if (gameState != IGame.EnumGameState.InProgress) ShowUiWindow();
        }
    }

    private void OnGUI()
    {
        if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
        {
            if (gameState != IGame.EnumGameState.InProgress)
                Application.Quit();
        }
    }
}
