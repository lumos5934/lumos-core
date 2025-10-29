using UnityEngine;

[CreateAssetMenu(fileName = "GameManagerComponents", menuName = "Scriptable Objects/Game Manager Components", order = 0)]
public class GameManagerComponentsSO : ScriptableObject
{
    [field: SerializeField] public GameManagerComponent[] ComponentPrefabs { get; private set; }
}