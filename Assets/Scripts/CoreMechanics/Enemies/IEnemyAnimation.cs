using static DirectionExtensions;

public interface IEnemyAnimation
{
    void OnEnable();
    void SetDirection(Direction _direction);
    public void SetHealth(int _health);
    void PlayWalkAnimation();
    void PlayAttackAnimation();
    void PlayDeathAnimation();
}
