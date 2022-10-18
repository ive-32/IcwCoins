using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class IcwStartGameWindow : MonoBehaviour, IUiWindow
{
    public GameObject textfield;
    IGame game;
    TextMeshProUGUI maintext;

    private void Awake()
    {
        maintext = textfield.GetComponent<TextMeshProUGUI>();
        if (maintext==null)
        {
            Destroy(this.gameObject);
            Debug.LogWarning("MainText component not founded in parent. Check prefab");
            return;
        }
    }

    public void SetGame(IGame agame)
    {
        game = agame;
    }

    public void SetStateText(string atext)
    {
        // устанавливаем текст для окна 
        maintext.text = atext;
    }

    public void OnClick()
    {
        if (game==null)
        {
            Debug.LogWarning("Something wrong! UiWindow hasn't game object");
            return;
        }
        if (game.gameState != IGame.EnumGameState.Paused)
        {
            game.StartNewGame();
            Destroy(this.gameObject);
            return;
        }
        if (game.gameState == IGame.EnumGameState.Paused)
        {
            Destroy(this.gameObject);
            return;
        }
    }
    void OnDestroy()
    {
        game.gameState = IGame.EnumGameState.InProgress;
    }
}
