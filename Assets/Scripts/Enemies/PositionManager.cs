using UnityEngine;

public class PositionManager : MonoBehaviour
{
    private bool _isTaked = false;

    public bool GetIsTaked() {
        return _isTaked;
    }

    public void TakePosition() {
        _isTaked = true;
    }

    public void FreePosition()
    {
        _isTaked = false;
    }
}
