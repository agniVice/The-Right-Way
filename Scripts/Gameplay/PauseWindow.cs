using System.Collections;
using TMPro;
using UnityEngine;

public class PauseWindow : MonoBehaviour, IInitializable, ISubscriber
{
    [SerializeField] private GameObject _panel;
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
        GameState.Instance.GamePaused += Show;
        GameState.Instance.GameUnpaused += Hide;
        GameState.Instance.GameFinished += Hide;
    }
    public void UnsubscribeAll()
    {
        GameState.Instance.GamePaused -= Show;
        GameState.Instance.GameUnpaused -= Hide;
        GameState.Instance.GameFinished -= Hide;
    }
    private void Show()
    {
        _panel.SetActive(true);
        _levelNumber.text = "LEVEL: " + LevelManager.Instance.CurrentLevelId;
    }
    private void Hide()
    {
        _panel.SetActive(false);
    }
    public void OnContinueButtonClicked()
    {
        Time.timeScale = 1f;
        GameState.Instance.UnpauseGame();
    }
    public void OnRestartButtonClicked()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene("Gameplay");
    }
    public void OnMenuButtonClicked()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene("Menu");
    }
}