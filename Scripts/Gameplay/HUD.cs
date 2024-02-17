using System.Collections;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour, IInitializable, ISubscriber
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _levelNumberText;

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

        Show();

        _levelNumberText.text = "LEVEL: " + LevelManager.Instance.CurrentLevelId;

        _isInitialized = true;
    }
    public void SubscribeAll()
    {
        GameState.Instance.GameFinished += Hide;
        GameState.Instance.GamePaused += Hide;
        GameState.Instance.GameUnpaused += Show;
    }
    public void UnsubscribeAll()
    {
        GameState.Instance.GameFinished -= Hide;
        GameState.Instance.GamePaused -= Hide;
        GameState.Instance.GameUnpaused -= Show;
    }
    private void Show()
    {
        _panel.SetActive(true);
    }
    private void Hide()
    {
        _panel.SetActive(false);
    }
    public void OnRestartButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Gameplay");
    }
    public void OnPauseButtonClicked()
    {
        GameState.Instance.PauseGame();
    }
}