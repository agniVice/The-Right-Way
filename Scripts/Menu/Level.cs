using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public bool IsLocked { get; private set; }

    [SerializeField] private int _id;
    [SerializeField] private Sprite _enabled;
    [SerializeField] private Sprite _disabled;

    private TextMeshProUGUI _idText;

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        _idText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        GetComponent<Button>().onClick.AddListener(LoadLevel);

        if (_id == 1) PlayerPrefs.SetInt("LevelLocked" + _id, 0);

        IsLocked = Convert.ToBoolean(PlayerPrefs.GetInt("LevelLocked" + _id, 1));

        _idText.text = _id.ToString();

        if (IsLocked)
            GetComponent<Image>().sprite = _disabled;
        else
            GetComponent<Image>().sprite = _enabled;
    }
    public void LoadLevel()
    {
        LevelManager.Instance.LoadLevel(this);
    }
    public int Id() => _id;
}