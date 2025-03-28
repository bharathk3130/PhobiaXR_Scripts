using UnityEngine;

public class MoveValueLinearlyStrategy : IMoveValueStrategy
{
    public float ScalePercent { get; set; }

    float _moveSpeed;

    public MoveValueLinearlyStrategy(float moveSpeed)
    {
        _moveSpeed = moveSpeed;

        ScalePercent = 1;
    }

    public bool TryUpdateValue(float dt, bool increase)
    {
        if ((increase && CanIncreaseSize()) || (!increase && CanDecreaseSize()))
        {
            float increment = _moveSpeed * dt * (increase ? 1 : -1);
            ScalePercent = Mathf.Clamp01(ScalePercent + increment);

            return true;
        }

        return false;
    }

    public bool CanIncreaseSize() => ScalePercent < 1;
    public bool CanDecreaseSize() => ScalePercent > 0;
}