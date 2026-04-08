using UnityEngine;

public abstract class StatStorage : MonoBehaviour
{
    public abstract void Save(StatData data);
    public abstract StatData Load();
}