using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour, IInitializable
{
    [SerializeField] private List<GameObject> _levelPrefabs = new List<GameObject>();
    public void Initialize()
    {
        BuildLevel();
    }
    private void BuildLevel()
    {
        Instantiate(_levelPrefabs[LevelManager.Instance.CurrentLevelId - 1]);
    }
}
