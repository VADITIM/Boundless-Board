using UnityEngine;
using TMPro;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private AnimationManagerGame animationManager;
    
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
            turnText.color = Color.green;
            animationManager.TurnRotateForward();
        }
        else
        {
            turnText.text = "AI's Thinking...";
            turnText.color = Color.red;
            animationManager.TurnRotateBack();
        }
    }
    
}