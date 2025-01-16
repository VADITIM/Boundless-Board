using UnityEngine;
using TMPro;
using DG.Tweening;

public class AnimationManagerMainMenu : MonoBehaviour
{
    private float elapsedTime = 0f;

    [SerializeField] private RectTransform startButton;
    private float startButtonTweenDuration = 1.5f;
    private float startButtonInitialPosition = -520f;
    private float startButtonFinalPosition = 0f;
    private bool startButtonHasEntered;
    private float startButtonFloatSpeed = .5f;
    private float startButtonFloatAmplitude = 15f;

    // ------------------------------------------------------------------------------------------------------

    [SerializeField] private RectTransform gameNamePanel;
    private float gameNameTweenDuration = .7f;
    private float gameNameInitialPosition = 1000f;
    private float gameNameFinalPosition = 0f;

    // ------------------------------------------------------------------------------------------------------

    [SerializeField] private RectTransform difficultyPanel;
    private float difficultyTweenDuration = .9f;
    private float difficultyInitialPosition = -5020f;
    private float difficultyFinalPosition = -100f;

    // ------------------------------------------------------------------------------------------------------

    [SerializeField] private TMP_Text floatingText; 
    private float charFloatSpeed = 2f;             
    private float charFloatAmplitude = 10f;       
    private bool isAnimatingCharacters = true;    

    // ------------------------------------------------------------------------------------------------------

    public void OnStartEnter()
    {
        startButton.DOAnchorPosY(startButtonFinalPosition, startButtonTweenDuration).OnComplete(() => startButtonHasEntered = true);
    }

    private void StartButtonFloat()
    {
        if (!startButtonHasEntered) return;
        float newY = startButtonFinalPosition + Mathf.Sin(Time.time * startButtonFloatSpeed) * startButtonFloatAmplitude;
        startButton.anchoredPosition = new Vector2(startButton.anchoredPosition.x, newY);
    }

    public void OnGameNameEnter()
    {
            gameNamePanel.DOAnchorPosX(gameNameFinalPosition, gameNameTweenDuration);
    }

    private void AnimateFloatingText()
    {
        floatingText.ForceMeshUpdate();
        TMP_TextInfo textInfo = floatingText.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            // get vertex of each char
            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;
            Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

            // calculate floating effect
            float offset = Mathf.Sin(Time.time * charFloatSpeed + i * 0.3f) * charFloatAmplitude;

            // apply
            vertices[vertexIndex + 0].y += offset;
            vertices[vertexIndex + 1].y += offset;
            vertices[vertexIndex + 2].y += offset;
            vertices[vertexIndex + 3].y += offset;
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            floatingText.UpdateGeometry(meshInfo.mesh, i);
        }
    }

    private void OnDifficultyEnter()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= .8f)
        {
            difficultyPanel.DOAnchorPosY(difficultyFinalPosition, difficultyTweenDuration);
        }
    }
    
    private void Start()
    {
        SetupInitialPosition();
        OnStartEnter();
        OnGameNameEnter();
    }

    private void Update()
    {
        OnDifficultyEnter();
        StartButtonFloat();

        if (isAnimatingCharacters)
        {
            AnimateFloatingText();
        }
    }

    private void SetupInitialPosition()
    {
        startButton.anchoredPosition = new Vector2(startButton.anchoredPosition.y, startButtonInitialPosition);
        difficultyPanel.anchoredPosition = new Vector2(difficultyPanel.anchoredPosition.x, difficultyInitialPosition);

        Vector2 gameNamePosition = gameNamePanel.anchoredPosition;
        gameNamePosition.x = -gameNameInitialPosition * 2;
        gameNamePanel.anchoredPosition = gameNamePosition;
    }
}
