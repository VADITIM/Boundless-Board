using System.Collections.Generic;
using UnityEngine;

public class AILogic : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public void EasyAILogic()
    {
        List<int> availableButtons = new List<int>();
        for (int i = 0; i < gameManager.buttonImages.Length; i++)
        {
            if (gameManager.buttonImages[i].sprite == null)
                availableButtons.Add(i);
        }

        if (availableButtons.Count > 0)
            gameManager.movePosition = availableButtons[Random.Range(0, availableButtons.Count)];
    }

    public void HardAILogic()
    {
        gameManager.movePosition = FindWinningMove("O"); // Try to win

        if (gameManager.movePosition == -1)
            gameManager.movePosition = FindBlockingMove(); // Block player's winning move

        if (gameManager.movePosition == -1)
            gameManager.movePosition = FindBestStrategicMove(); // Use strategic move logic
    }

    public int FindBestStrategicMove()
    {
        // prioritize center
        if (gameManager.buttonImages[4].sprite == null) 
            return 4;

        // then corners
        int[] corners = { 0, 2, 6, 8 };
        foreach (int corner in corners)
        {
            if (gameManager.buttonImages[corner].sprite == null) 
                return corner;
        }

        // then edges
        int[] edges = { 1, 3, 5, 7 };
        foreach (int edge in edges)
        {
            if (gameManager.buttonImages[edge].sprite == null) 
                return edge;
        }
        return -1;
    }
    
    private int FindWinningMove(string player) // find a move to win the game
    {
        return FindCriticalMove(player, ignoreTinted: false);
    }
    
    private int FindBlockingMove() // find a move to prevent player from winning
    {
        return FindCriticalMove("X", ignoreTinted: true);
    }

private int FindCriticalMove(string player, bool ignoreTinted)
{
    int[,] winConditions = new int[,]
    {
        {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, // rows
        {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, // columns
        {0, 4, 8}, {2, 4, 6}             // diagonals
    };

    Sprite checkSprite = player == "X" ? gameManager.playerSprite : gameManager.aiSprite;
    Sprite tintedCheckSprite = player == "X" ? gameManager.tintedPlayerSprite : gameManager.tintedAiSprite;

    for (int i = 0; i < winConditions.GetLength(0); i++)
    {
        int count = 0;
        int emptyPosition = -1;

        for (int j = 0; j < 3; j++)
        {
            int pos = winConditions[i, j];
            Sprite currentSprite = gameManager.buttonImages[pos].sprite;

            if (currentSprite == checkSprite || 
                (!ignoreTinted && player == "X" && currentSprite == tintedCheckSprite))
            {
                count++;
            }
            else if (currentSprite == null)
            {
                emptyPosition = pos;
            }
        }

        if (count == 2 && emptyPosition != -1)
            return emptyPosition;
    }
    return -1;
}}
