public interface IPassiveEffect
{
    void Apply(GameManager gameManager);
    string GetEffectName();
    string GetEffectDescription();
    int GetBonusValue();
}
