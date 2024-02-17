using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioVibrationManager : MonoBehaviour
{
    public static AudioVibrationManager Instance;

    public Action SoundChanged;
    public Action MusicChanged;
    public Action VibrationChanged;

    public bool IsSoundEnabled { get; private set; }
    public bool IsMusicEnabled { get; private set; }
    public bool IsVibrationEnabled { get; private set; }

    [SerializeField] private string SoundGroup = "Sound";
    [SerializeField] private string MusicGroup = "Music";

    [SerializeField] private AudioMixer _mixer;

    public AudioClip ElementPlaced;
    public AudioClip Win;

    [SerializeField] private GameObject _soundPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);

        Initialize();
    }
    private void Initialize()
    {
        IsSoundEnabled = Convert.ToBoolean(PlayerPrefs.GetInt("IsSoundEnabled", 1));
        IsMusicEnabled = Convert.ToBoolean(PlayerPrefs.GetInt("IsMusicEnabled", 1));
        IsVibrationEnabled = Convert.ToBoolean(PlayerPrefs.GetInt("IsVibrationEnabled", 1));

        Invoke("InitializeAudioMixer", 0.1f);
    }
    private void InitializeAudioMixer()
    {
        _mixer.SetFloat(SoundGroup, -80 * Convert.ToInt32(!IsSoundEnabled));
        _mixer.SetFloat(MusicGroup, -80 * Convert.ToInt32(!IsMusicEnabled));
    }
    public void ToggleSound(bool enable)
    {
        IsSoundEnabled = enable;

        _mixer.SetFloat(SoundGroup, -80 * Convert.ToInt32(!IsSoundEnabled));

        SoundChanged?.Invoke();

        PlayerPrefs.SetInt("IsSoundEnabled", Convert.ToInt32(IsSoundEnabled));
    }
    public void ToggleMusic(bool enable)
    {
        IsMusicEnabled = enable;

        _mixer.SetFloat(MusicGroup, -80 * Convert.ToInt32(!IsMusicEnabled));

        MusicChanged?.Invoke();

        PlayerPrefs.SetInt("IsMusicEnabled", Convert.ToInt32(IsMusicEnabled));
    }
    public void ToggleVibration(bool enable)
    {
        IsVibrationEnabled = enable;

        VibrationChanged?.Invoke();

        PlayerPrefs.SetInt("IsVibrationEnabled", Convert.ToInt32(IsVibrationEnabled));
    }
    public void PlaySound(AudioClip clip, float pitch)
    {
        Instantiate(_soundPrefab).GetComponent<Sound>().PlaySound(clip, pitch);
    }
}
