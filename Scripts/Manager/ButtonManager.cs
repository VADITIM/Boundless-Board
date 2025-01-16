using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameManager gameManager;

    public void OnClick()
    {
        gameManager.PlayerMove(GetComponent<Image>());
    }
}
