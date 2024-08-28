using LanguageExt;
using static LordOfTheRings.Errors.Weapon;
using static LordOfTheRings.ErrorResult;

namespace LordOfTheRings;


public record Weapon
{
    private Weapon(string named, int dealDamage)
    {
        Name = named;
        Damage = dealDamage;
    }

    public string Name { get; }
    public int Damage { get; }

    public static Either<ErrorResult, Weapon> AddWeapon(string named, int dealDamage)
    {
        if (string.IsNullOrWhiteSpace(named))
        {
            return FailedBecause(weaponShouldHaveAName);
        }
        
        if (dealDamage <= 0)
        {
            return FailedBecause(weaponShouldDealDamage);
        }
        
        return new Weapon(named, dealDamage);
    }
}