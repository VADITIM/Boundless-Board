using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyToggle : MonoBehaviour
{    
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip toggleSound; 

    [SerializeField] private Toggle hardModeToggle;
    [SerializeField] private TextMeshProUGUI toggleText; 
    
    void Start()
    {
        hardModeToggle.onValueChanged.AddListener(OnToggleChanged);
         
        bool isHardMode = PlayerPrefs.GetInt("HardMode", 0) == 1; // load saved state
        hardModeToggle.isOn = isHardMode;
        UpdateText(isHardMode);
    }

    private void OnToggleChanged(bool isOn)
    {
        PlaySound(toggleSound);
        UpdateText(isOn);
        PlayerPrefs.SetInt("HardMode", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void UpdateText(bool isOn)
    {
        toggleText.text = isOn ? "Hard Mode!!" : "Hard Mode?";
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}