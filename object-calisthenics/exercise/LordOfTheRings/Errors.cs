namespace LordOfTheRings;

public static class Errors
{
    public static class Character
    {

        public static string characterMustHaveANameMessage = "Character must have a name.";
        public static string characterMustHaveARaceMessage = "Character must have a race.";
        public static string characterMustHaveAWeaponMessage = "Character must have a race.";
    }

    public static class Weapon
    {
        public static string weaponShouldHaveAName = "A weapon must have a name.";
        public static string weaponShouldDealDamage = "A weapon must have a damage level.";
    }
}