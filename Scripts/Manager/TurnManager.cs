using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnText;
    public bool isPlayerTurn = true;

    void Start()
    {
        UpdateTurnText();
    }

    void Update()
    {
        UpdateTurnText();
    }

    private void UpdateTurnText()
    {
        if (isPlayerTurn)
        {
            turnText.text = "It's Your Turn!";
        }
        else
            turnText.text = "AI's Thinking...";
    }
}