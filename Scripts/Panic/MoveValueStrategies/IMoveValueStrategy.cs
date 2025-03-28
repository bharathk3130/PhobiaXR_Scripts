public interface IMoveValueStrategy
{
    float ScalePercent { get; set; }

    bool TryUpdateValue(float dt, bool increase);
    bool CanIncreaseSize();
    bool CanDecreaseSize();
}