using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public TicTacToeManager gameManager;

    public void OnClick()
    {
        gameManager.PlayerMove(GetComponent<Image>());
    }
}
