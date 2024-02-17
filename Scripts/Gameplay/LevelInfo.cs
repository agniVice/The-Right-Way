using UnityEngine;

[CreateAssetMenu(fileName = "Info", menuName = "Infos/EnemyInfo")]
public class LevelInfo : ScriptableObject
{
    public BoardType BoardType;
    public GameObject LevelPrefab;
}
