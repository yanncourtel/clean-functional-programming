using FluentAssertions;
using LanguageExt.UnsafeValueAccess;

namespace LordOfTheRings.Tests;

public class FellowshipTest
{
    [Fact]
    public void Should_Be_Able_To_Add_Member_To_Fellowship()
    {
        var fellowship = new Fellowship();

        fellowship.AddMember(
            Character.Join("frodo", "hobbit", 
                Weapon.AddWeapon("dede", 10).ValueUnsafe()).ValueUnsafe());

        fellowship.HowManyMembers.Should().Be(1);
    }
}