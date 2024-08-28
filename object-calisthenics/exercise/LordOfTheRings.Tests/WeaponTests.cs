using FluentAssertions;
using FluentAssertions.LanguageExt;

using static LordOfTheRings.Errors.Weapon;
    
namespace LordOfTheRings.Tests;

public class WeaponTests
{
    [Fact]
    public void Should_Not_Add_Weapon_If_No_Name()
    {
        Weapon.AddWeapon("", 10)
            .Should()
            .BeLeft()
            .Which
            .Message
            .Should()
            .Be(weaponShouldHaveAName);
    }
    
    [Fact]
    public void Should_Not_Add_Weapon_If_Its_Dealing_No_Damage()
    {
        Weapon.AddWeapon("Broken Sword", 0)
            .Should()
            .BeLeft()
            .Which
            .Message
            .Should()
            .Be(weaponShouldDealDamage);
    }
}