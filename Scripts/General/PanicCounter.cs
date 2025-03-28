using UnityEngine;

public class PanicCounter : MonoBehaviour
{
    int _panicCount;

    public int PanicCount => _panicCount;

    void Start()
    {
        PanicHandler.IsPanicking.AddListener(OnPanicChange);
    }

    void OnPanicChange(bool panic)
    {
        if (panic)
        {
            _panicCount++;
        }
    }
}
