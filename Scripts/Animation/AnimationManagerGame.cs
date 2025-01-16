using UnityEngine;
using TMPro;
using DG.Tweening;

public class AnimationManagerGame : MonoBehaviour
{
    [SerializeField] private TurnManager turnManager;

    private float elapsedTime = 0f;
    private float fadeOutDuration = 1f;

    [SerializeField] private RectTransform exitButton;
    private Vector2 exitButtonInitialPosition = new Vector2(1000f, 310f);
    private Vector2 exitButtonFinalPosition = new Vector2(360f, 310f);
    private float buttonTweenDuration = 1.5f;
    private float exitButtonFloatSpeed = .3f;
    private float exitButtonFloatAmplitude = 7f;
    private bool exitButtonHasEntered;

    // ------------------------------------------------------------------------------------------------------

    [SerializeField] private RectTransform ticTacToeBoard;
    private float ticTacToeBoardInitialPositionY = 1000f;
    private float ticTacToeBoardFinalPositionY = 0f;
    private float ticTacToeBoardTweenDuration = 1.5f;

    // ------------------------------------------------------------------------------------------------------

    [SerializeField] private RectTransform turn;
    private Vector2 turnInitialPosition = new Vector2(1000f, 310f);
    private Vector2 turnFinalPosition;
    private float turnTweenDuration = 1.6f;
    private float turnFloatSpeed = .8f;
    private float turnFloatAmplitude = 12f;
    private bool turnHasEntered;

    // ------------------------------------------------------------------------------------------------------

    [SerializeField] private RectTransform score;
    private Vector2 scoreInitialPosition = new Vector2(1000f, 310f);
    private Vector2 scoreFinalPosition;
    private float scoreTweenDuration = 1.65f;
    private float scoreFloatSpeed = 1.3f;
    private float scoreFloatAmplitude = 12f;
    private bool scoreHasEntered;
    private bool isRotating;

    // ------------------------------------------------------------------------------------------------------

    [SerializeField] private RectTransform scorePopInText;
    private Vector2 scorePopInInitialPosition = new Vector2(0f, 0f);    
    private Vector2 scorePopInFinalPosition = new Vector2(0f, 80f);    
    private float scorePopInTweenDuration = .5f;
        
    // ------------------------------------------------------------------------------------------------------
    
    public void OnScorePopInEnter()
    {
        float rotationValue = Random.Range(-5f, 5f);
        TextMeshProUGUI textComponent = scorePopInText.GetComponent<TextMeshProUGUI>();

        // reset
        scorePopInText.DOAnchorPosY(scorePopInInitialPosition.y, 0);
        textComponent.DOColor(new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 1), 0);

        // animate
        scorePopInText.localRotation = Quaternion.Euler(0, 0, rotationValue);
        scorePopInText.DOAnchorPosY(scorePopInFinalPosition.y, scorePopInTweenDuration);
        textComponent.DOColor(new Color(textComponent.color.r, textComponent.color.g, textComponent.color.b, 0), fadeOutDuration);

    }

    public void OnTicTacToeBoardEnter()
    {
        ticTacToeBoard.DOAnchorPosY(ticTacToeBoardFinalPositionY, ticTacToeBoardTweenDuration);
    }

    private void OnExitButtonEnter()
    {
        exitButton.DOAnchorPosX(exitButtonFinalPosition.x, buttonTweenDuration).OnComplete(() => exitButtonHasEntered = true);
    }

    private void ExitButtonFloat()
    {
        if (!exitButtonHasEntered) return;
        float newY = exitButtonFinalPosition.y + Mathf.Sin(Time.time * exitButtonFloatSpeed) * exitButtonFloatAmplitude;
        exitButton.anchoredPosition = new Vector2(exitButtonFinalPosition.x, newY);
    }
    

    private void OnTurnEnter()
    {
        turn.DOAnchorPosX(turnFinalPosition.x, turnTweenDuration).OnComplete(() => turnHasEntered = true);
    }

    private void TurnFloat()
    {
        if (!turnHasEntered) return;
        float newY = turnFinalPosition.y + Mathf.Sin(Time.time * turnFloatSpeed) * turnFloatAmplitude;
        turn.anchoredPosition = new Vector2(turnFinalPosition.x, newY);
    }

    public void TurnRotateForward()
    {
        turn.DORotate(new Vector3(0, 40f, 0), 1f);
    }
    
    public void TurnRotateBack()
    {
        turn.DORotate(new Vector3(0, -40f, 0), 1f);
    }

    
    private void OnScoreEnter()
    {
        score.DOAnchorPosX(scoreFinalPosition.x, scoreTweenDuration).OnComplete(() => scoreHasEntered = true);
    }

    private void ScoreFloat()
    {
        elapsedTime += Time.deltaTime;
        if (!scoreHasEntered && elapsedTime < .5f) return;

        float newY = scoreFinalPosition.y + Mathf.Sin(Time.time * scoreFloatSpeed) * scoreFloatAmplitude;
        score.anchoredPosition = new Vector2(scoreFinalPosition.x, newY);
    }

    public void ScoreRotate()
    {
        score.DOLocalRotate(new Vector3(0f, 0f, 360f), .3f, RotateMode.FastBeyond360);
    }


    void Start()
    {
        SetupInitialPosition();

        OnTicTacToeBoardEnter();
        OnExitButtonEnter();
        OnScoreEnter();
        OnTurnEnter();
    }

    void Update()
    {
        ExitButtonFloat();
        ScoreFloat();
        TurnFloat();
    }

    private void SetupInitialPosition()
    {
        Vector2 ticTacToepPosition = ticTacToeBoard.position;
        ticTacToepPosition.y = ticTacToeBoardInitialPositionY;
        ticTacToeBoard.position = ticTacToepPosition;

        Vector2 scorePosition = score.position;
        scorePosition.x = scoreInitialPosition.x;
        score.position = scorePosition;

        Vector2 turnPosition = turn.position;
        turnPosition.x = turnInitialPosition.x;
        turn.position = turnPosition;
        
        Vector2 exitPosition = exitButton.position;
        exitPosition.x = exitButtonInitialPosition.x;
        exitButton.position = exitPosition;

        exitButton.anchoredPosition = new Vector2(exitButtonInitialPosition.x, exitButtonInitialPosition.y);
    }
}
