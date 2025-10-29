using UnityEngine;

public abstract class GameManagerComponent : MonoBehaviour
{
    public abstract int Order { get; }
    public abstract bool IsInitialized { get; protected set; }
    public abstract void Init();
}