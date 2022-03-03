using UnityEngine;

public class PositionManager : MonoBehaviour
{
    private bool _isTaken = false;

    public bool GetIsTaken() {
        return _isTaken;
    }

    public void TakePosition() {
        _isTaken = true;
    }

    public void FreePosition()
    {
        _isTaken = false;
    }
}
