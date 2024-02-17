using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FinishWindow : MonoBehaviour, IInitializable, ISubscriber
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _nextButton;
    [SerializeField] private TextMeshProUGUI _levelNumber;

    private bool _isInitialized;

    private void OnEnable()
    {
        if (!_isInitialized)
            return;

        SubscribeAll();
    }
    private void OnDisable()
    {
        UnsubscribeAll();
    }
    public void Initialize()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;

        Hide();

        _isInitialized = true;
    }
    public void SubscribeAll()
    {
        GameState.Instance.GameFinished += Show;
    }
    public void UnsubscribeAll()
    {
        GameState.Instance.GameFinished -= Show;
    }
    private void Show()
    {
        _panel.SetActive(true);

        _levelNumber.text = "LEVEL: " + LevelManager.Instance.CurrentLevelId;

        if (LevelManager.Instance.CurrentLevelId >= LevelManager.Instance.GetLevelsCount())
            _nextButton.interactable = false;
    }
    private void Hide()
    {
        _panel.SetActive(false);
    }
    public void OnRestartButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Gameplay");
    }
    public void OnMenuButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Menu");
    }
    public void OnNextButtonClicked()
    {
        LevelManager.Instance.LoadNextLevel();
        SceneLoader.Instance.LoadScene("Gameplay");
    }
}