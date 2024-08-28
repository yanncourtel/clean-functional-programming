using FluentAssertions;
using FluentAssertions.LanguageExt;
using LanguageExt.UnsafeValueAccess;
using static LordOfTheRings.Errors.Character;
    
namespace LordOfTheRings.Tests;

public class CharacterTests
{
    private Weapon validWeapon = 
        Weapon.AddWeapon("Sword", 10)
            .ValueUnsafe();

    [Fact]
    public void Should_Not_Join_If_No_Name()
    {
        validWeapon = Weapon.AddWeapon("Sword", 10).ValueUnsafe();
        Character.Join("", "hobbit", validWeapon)
            .Should()
            .BeLeft()
            .Which
            .Message
            .Should()
            .Be(characterMustHaveANameMessage);
    }
    
    [Fact]
    public void Should_Not_Join_If_No_Race()
    {
        Character.Join("frodo", "", validWeapon)
            .Should()
            .BeLeft()
            .Which
            .Message
            .Should()
            .Be(characterMustHaveARaceMessage);
    }
    
    [Fact]
    public void Should_Not_Join_If_No_Weapon()
    {
        Character.Join("frodo", "hobbit", null)
            .Should()
            .BeLeft()
            .Which
            .Message
            .Should()
            .Be(characterMustHaveAWeaponMessage);
    }
}