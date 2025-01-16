using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AILogic aiLogic;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private AnimationManagerGame animationManager;

    [SerializeField] private GameObject waitPanel;
    [SerializeField] private GameObject turnPanel;

    [SerializeField] public Sprite playerSprite; 
    [SerializeField] public Sprite aiSprite;
    [SerializeField] public Sprite tintedPlayerSprite;
    [SerializeField] public Sprite tintedAiSprite;

    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] public Image[] buttonImages;

    public Queue<int> moveHistory = new Queue<int>();
    public int totalMoves;
    
    public int movePosition = -1;
    public string currentPlayer = "X";

    public bool isHardMode;
    public bool gameOver;
    private bool deactivated;
    private float elapsedTime;
    
    void Start()
    {
        resultText.text = "";
        foreach (var image in buttonImages)
        {
            image.sprite = null;
            image.color = new Color(1f, 1f, 1f, 0f);
        }
        isHardMode = PlayerPrefs.GetInt("HardMode", 0) == 1;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1.5f && !deactivated)
        {
            deactivated = true;
            elapsedTime = 0;
            waitPanel.SetActive(false);
        }
    }

    public Sprite GetCurrentSprite()
    {
        return (currentPlayer == "X") ? playerSprite : aiSprite;
    }

    public void PlayerMove(Image clickedButton)
    {
        if (gameOver || clickedButton.sprite != null) return;
        animationManager.ScoreRotate();

        clickedButton.sprite = GetCurrentSprite();
        clickedButton.color = new Color(1f, 1f, 1f, 1f);
        RegisterMove(Array.IndexOf(buttonImages, clickedButton));

        scoreManager.IncrementScore(100);
        animationManager.OnScorePopInEnter();

        if (CheckWinCondition())
        {
            gameOver = true;
            resultText.text = "You Win!";
            turnPanel.SetActive(false);
            waitPanel.SetActive(false);
            return;
        }
        
        waitPanel.SetActive(true);
        turnManager.isPlayerTurn = false;
        currentPlayer = "O";
        StartCoroutine(AIMove());
    }

    private IEnumerator AIMove()
    {
        float thinkTime = UnityEngine.Random.Range(1f, 3f);
        yield return new WaitForSeconds(thinkTime);

        if (isHardMode)
            aiLogic.HardAILogic();
        else
            aiLogic.EasyAILogic();

        if (movePosition != -1)
        {
            buttonImages[movePosition].sprite = GetCurrentSprite();
            RegisterMove(movePosition);
            buttonImages[movePosition].color = new Color(1f, 1f, 1f, 1f); // Fully opaque
        }

        if (CheckWinCondition())
        {
            gameOver = true;
            resultText.text = "AI Wins!";
            turnPanel.SetActive(false);
            waitPanel.SetActive(false);
            yield break;
        }

        currentPlayer = "X";
        turnManager.isPlayerTurn = true;
        waitPanel.SetActive(false);
    }

    private void RegisterMove(int buttonIndex)
    {
        moveHistory.Enqueue(buttonIndex);
        totalMoves++;
    
        // after 6 moves, start removing the oldest move
        if (moveHistory.Count > 6)
        {
            int oldestIndex = moveHistory.Dequeue();
            buttonImages[oldestIndex].sprite = null;
            buttonImages[oldestIndex].color = new Color(1f, 1f, 1f, 0f); 
        }
    
        // the oldest move will be tinted
        int[] movesArray = moveHistory.ToArray();
        int tintedCount = Mathf.Max(movesArray.Length - 4, 0);
    
        for (int i = 0; i < tintedCount; i++)
        {
            int oldIndex = movesArray[i];
            var sprite = buttonImages[oldIndex].sprite;
    
            if (sprite == playerSprite) buttonImages[oldIndex].sprite = tintedPlayerSprite;
            else if (sprite == aiSprite) buttonImages[oldIndex].sprite = tintedAiSprite;
        }
    }

    private bool CheckWinCondition()
    {
        int[,] winConditions = new int[,]
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // Rows
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // Columns
            {0, 4, 8}, {2, 4, 6}             // Diagonals
        };
    
        for (int i = 0; i < winConditions.GetLength(0); i++)
        {
            Sprite firstSprite = buttonImages[winConditions[i, 0]].sprite;
            if (firstSprite == null) continue;

            // get the corresponding tinted version based on the first sprite
            Sprite correspondingTinted = (firstSprite == playerSprite || firstSprite == tintedPlayerSprite) 
                ? tintedPlayerSprite 
                : tintedAiSprite;
            
            Sprite correspondingRegular = (firstSprite == playerSprite || firstSprite == tintedPlayerSprite)
                ? playerSprite
                : aiSprite;

            // check if all three positions have same sprite
            if ((buttonImages[winConditions[i, 1]].sprite == correspondingRegular || 
                 buttonImages[winConditions[i, 1]].sprite == correspondingTinted) &&
                (buttonImages[winConditions[i, 2]].sprite == correspondingRegular || 
                 buttonImages[winConditions[i, 2]].sprite == correspondingTinted))
            {
                return true;
            }
        }
        return false;
    }
}