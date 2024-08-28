using LanguageExt;

namespace LordOfTheRings.App;

public static class Program
{
    public static void Run()
    {
        var fellowship = new FellowshipOfTheRingService();

        try
        {
            AddMemberToFellowship("Frodo", "Hobbit",
                "Sting",30, fellowship);
            
            AddMemberToFellowship("Sam", "Hobbit",
                "Dagger",10, fellowship);
            
            AddMemberToFellowship("Merry", "Hobbit",
                "Short Sword",24, fellowship);
            
            AddMemberToFellowship("Pippin", "Hobbit",
                "Bow",8, fellowship);
            
            AddMemberToFellowship("Aragorn", "Human",
                "Anduril",100, fellowship);
            
            AddMemberToFellowship("Boromir", "Human",
                "Sword",90, fellowship);
            
            AddMemberToFellowship("Legolas", "Elf",
                "Bow",100, fellowship);
            
            AddMemberToFellowship("Gimli", "Dwarf",
                "Axe",100, fellowship);
            
            AddMemberToFellowship("Gandalf the 🐐", "Wizard",
                "Staff",200, fellowship);

            Console.WriteLine(fellowship.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        var group1 = new List<string> {"Frodo", "Sam"};
        var group2 = new List<string> {"Merry", "Pippin", "Aragorn", "Boromir"};
        var group3 = new List<string> {"Legolas", "Gimli", "Gandalf the 🐐"};

        fellowship.MoveMembersToRegion(group1, "Rivendell");
        fellowship.MoveMembersToRegion(group2, "Moria");
        fellowship.MoveMembersToRegion(group3, "Lothlorien");

        try
        {
            var group4 = new List<string> {"Frodo", "Sam"};
            fellowship.MoveMembersToRegion(group4, "Mordor");
            fellowship.MoveMembersToRegion(group4, "Shire"); // This should fail for "Frodo"
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        fellowship.PrintMembersInRegion("Rivendell");
        fellowship.PrintMembersInRegion("Moria");
        fellowship.PrintMembersInRegion("Lothlorien");
        fellowship.PrintMembersInRegion("Mordor");
        fellowship.PrintMembersInRegion("Shire");

        try
        {
            fellowship.RemoveMember("Frodo");
            fellowship.RemoveMember("Sam"); // This should throw an exception
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static void AddMemberToFellowship(string name, string race, string weaponName, int weaponDamage, FellowshipOfTheRingService fellowship)
    {
        Weapon.AddWeapon(weaponName, weaponDamage)
            .IfRight(weapon => CharacterWantsToJoin(name, race, weapon)
                .IfRight(character => JoiningTheFellowship(character, fellowship)));
    }

    private static Either<ErrorResult, Character> CharacterWantsToJoin(string name, string race, Weapon weapon)
    {
        return Character.Join(name, race, weapon);
    }

    private static void JoiningTheFellowship(Character character, FellowshipOfTheRingService fellowship)
    {
        fellowship.AddMember(character);
    }

    public static void Main()
    {
        Run();
    }
}

