using UnityEngine;
using UnityEngine.UI;

public class MenuUserInterface : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _levelsPanel;
    [SerializeField] private GameObject _settingsPanel;

    [Header("Settings")]
    [SerializeField] private Button _soundEnable;
    [SerializeField] private Button _soundDisable;

    [SerializeField] private Button _musicEnable;
    [SerializeField] private Button _musicDisable;

    [SerializeField] private Button _vibrationEnable;
    [SerializeField] private Button _vibrationDisable;

    [SerializeField] private Sprite _buttonEnabled;
    [SerializeField] private Sprite _buttonDisabled;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        AudioVibrationManager.Instance.SoundChanged += UpdateSoundImage;
        AudioVibrationManager.Instance.MusicChanged += UpdateMusicImage;
        AudioVibrationManager.Instance.VibrationChanged += UpdateVibrationImage;

        _soundEnable.onClick.AddListener(() => AudioVibrationManager.Instance.ToggleSound(true));
        _soundDisable.onClick.AddListener(() => AudioVibrationManager.Instance.ToggleSound(false));

        _musicEnable.onClick.AddListener(() => AudioVibrationManager.Instance.ToggleMusic(true));
        _musicDisable.onClick.AddListener(() => AudioVibrationManager.Instance.ToggleMusic(false));

        _vibrationEnable.onClick.AddListener(() => AudioVibrationManager.Instance.ToggleVibration(true));
        _vibrationDisable.onClick.AddListener(() => AudioVibrationManager.Instance.ToggleVibration(false));

        UpdateSoundImage();
        UpdateMusicImage();
        UpdateVibrationImage();

        _menuPanel.SetActive(true);
        _levelsPanel.SetActive(false);
        _settingsPanel.SetActive(false);

    }
    private void OnDisable()
    {
        AudioVibrationManager.Instance.SoundChanged -= UpdateSoundImage;
        AudioVibrationManager.Instance.MusicChanged -= UpdateMusicImage;
        AudioVibrationManager.Instance.VibrationChanged -= UpdateVibrationImage;
    }
    private void UpdateSoundImage()
    {
        _soundEnable.GetComponent<Image>().sprite = _buttonDisabled;
        _soundDisable.GetComponent<Image>().sprite = _buttonDisabled;

        if (AudioVibrationManager.Instance.IsSoundEnabled)
            _soundEnable.GetComponent<Image>().sprite = _buttonEnabled;
        else
            _soundDisable.GetComponent<Image>().sprite = _buttonEnabled;
    }
    private void UpdateMusicImage()
    {
        _musicEnable.GetComponent<Image>().sprite = _buttonDisabled;
        _musicDisable.GetComponent<Image>().sprite = _buttonDisabled;

        if (AudioVibrationManager.Instance.IsMusicEnabled)
            _musicEnable.GetComponent<Image>().sprite = _buttonEnabled;
        else
            _musicDisable.GetComponent<Image>().sprite = _buttonEnabled;
    }
    private void UpdateVibrationImage()
    {
        _vibrationEnable.GetComponent<Image>().sprite = _buttonDisabled;
        _vibrationDisable.GetComponent<Image>().sprite = _buttonDisabled;

        if (AudioVibrationManager.Instance.IsVibrationEnabled)
            _vibrationEnable.GetComponent<Image>().sprite = _buttonEnabled;
        else
            _vibrationDisable.GetComponent<Image>().sprite = _buttonEnabled;
    }
    public void OnSettingsButtonClicked()
    {
        _menuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }
    public void OnCloseSettingsButtonClicked()
    {
        _menuPanel.SetActive(true);
        _settingsPanel.SetActive(false);
    }
    public void OnCloseLevelsButtonClicked()
    {
        _menuPanel.SetActive(true);
        _levelsPanel.SetActive(false);
    }
    public void OnLevelsButtonClicked()
    {
        _menuPanel.SetActive(false);
        _levelsPanel.SetActive(true);
    }
    public void OnExitButtonClicked()
    { 
        Application.Quit();
    }
}
