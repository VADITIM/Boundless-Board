using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;

public class TicTacToeManager : MonoBehaviour
{
    public static TicTacToeManager Instance { get; private set; }
    
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private AnimationManagerGame animationManager;

    [SerializeField] public Sprite playerSprite; 
    [SerializeField] public Sprite aiSprite;
    [SerializeField] private Sprite tintedPlayerSprite;
    [SerializeField] private Sprite tintedAiSprite;

    [SerializeField] private GameObject waitPanel;
    [SerializeField] private GameObject turnPanel;

    public Queue<int> moveHistory = new Queue<int>();
    public int totalMoves;
    public TextMeshProUGUI resultText;

    public string currentPlayer = "X";
    [SerializeField] private Image[] buttonImages;

    public bool isHardMode;
    public bool gameOver;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        waitPanel.SetActive(false);
        resultText.text = "";

        foreach (var image in buttonImages)
        {
            image.sprite = null;
            image.color = new Color(1f, 1f, 1f, 0f);
        }
        isHardMode = PlayerPrefs.GetInt("HardMode", 0) == 1;

    }

    public Sprite GetCurrentSprite()
    {
        return (currentPlayer == "X") ? playerSprite : aiSprite;
    }

    public void PlayerMove(Image clickedButton)
    {
        Debug.Log("Button clicked!");
        if (gameOver || clickedButton.sprite != null) return;

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

        int movePosition = -1;

        if (isHardMode)
        {
            movePosition = FindWinningMove("O"); 

            if (movePosition == -1)
            {
                movePosition = FindBlockingMove();
            }

            if (movePosition == -1)
            {
                movePosition = FindBestStrategicMove();
            }
        }
        else
        {
            List<int> availableButtons = new List<int>();
            for (int i = 0; i < buttonImages.Length; i++)
            {
                if (buttonImages[i].sprite == null)
                {
                    availableButtons.Add(i);
                }
            }

            if (availableButtons.Count > 0)
            {
                movePosition = availableButtons[UnityEngine.Random.Range(0, availableButtons.Count)];
            }
        }

        if (movePosition != -1)
        {
            buttonImages[movePosition].sprite = GetCurrentSprite();
            RegisterMove(movePosition);
            buttonImages[movePosition].color = new Color(1f, 1f, 1f, 1f); 
            Debug.Log($"AI moved to button {movePosition}");
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

    private int FindBestStrategicMove()
    {
        if (buttonImages[4].sprite == null) return 4;

        int[] corners = { 0, 2, 6, 8 };
        foreach (int corner in corners)
        {
            if (buttonImages[corner].sprite == null) return corner;
        }

        int[] edges = { 1, 3, 5, 7 };
        foreach (int edge in edges)
        {
            if (buttonImages[edge].sprite == null) return edge;
        }

        return -1;
    }

    private int FindWinningMove(string player)
    {
        return FindCriticalMove(player, ignoreTinted: false);
    }

    private int FindBlockingMove()
    {
        return FindCriticalMove("X", ignoreTinted: true);
    }

    private int FindCriticalMove(string player, bool ignoreTinted)
    {
        int[,] winConditions = new int[,]
        {
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8}, 
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, 
            {0, 4, 8}, {2, 4, 6}             
        };

        Sprite checkSprite = player == "X" ? playerSprite : aiSprite;
        Sprite tintedCheckSprite = player == "X" ? tintedPlayerSprite : tintedAiSprite;

        for (int i = 0; i < winConditions.GetLength(0); i++)
        {
            int count = 0;
            int emptyPosition = -1;

            for (int j = 0; j < 3; j++)
            {
                int pos = winConditions[i, j];
                Sprite currentSprite = buttonImages[pos].sprite;

                if (currentSprite == checkSprite || (!ignoreTinted && currentSprite == tintedCheckSprite))
                {
                    count++;
                }
                else if (currentSprite == null)
                {
                    emptyPosition = pos;
                }
            }

            if (count == 2 && emptyPosition != -1)
            {
                return emptyPosition;
            }
        }
        return -1;
    }

    private void RegisterMove(int buttonIndex)
    {
        moveHistory.Enqueue(buttonIndex);
        totalMoves++;
    
        if (moveHistory.Count > 6)
        {
            int oldestIndex = moveHistory.Dequeue();
            buttonImages[oldestIndex].sprite = null;
            buttonImages[oldestIndex].color = new Color(1f, 1f, 1f, 0f); 
        }
    
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
            {0, 1, 2}, {3, 4, 5}, {6, 7, 8},
            {0, 3, 6}, {1, 4, 7}, {2, 5, 8}, 
            {0, 4, 8}, {2, 4, 6}           
        };
    
        for (int i = 0; i < winConditions.GetLength(0); i++)
        {
            Sprite firstSprite = buttonImages[winConditions[i, 0]].sprite;
            if (firstSprite == null) continue;

            Sprite correspondingTinted = (firstSprite == playerSprite || firstSprite == tintedPlayerSprite) 
                ? tintedPlayerSprite 
                : tintedAiSprite;
            
            Sprite correspondingRegular = (firstSprite == playerSprite || firstSprite == tintedPlayerSprite)
                ? playerSprite
                : aiSprite;

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
