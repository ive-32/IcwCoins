using UnityEngine;

public interface IWalker
{
    bool IsMoving { get; }
    Vector3 Position { get; }
    void SetTarget(Vector3? atarget);
    bool TargetReached { get; }
}
