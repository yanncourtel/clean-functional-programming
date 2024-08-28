using LanguageExt;

using static LordOfTheRings.Errors.Character;
using static LordOfTheRings.ErrorResult;

namespace LordOfTheRings
{
    public sealed class Character
    {
        private Character(string name, string race, Weapon weapon)
        {
            Name = name;
            Race = race;
            Weapon = weapon;
        }

        public string Name { get; }
        public string Race { get; }
        public Weapon Weapon { get; }
        public string Region { get; set; } = "Shire";

        public static Either<ErrorResult, Character> Join(string name, string race, Weapon weapon)
        {
            if (string.IsNullOrWhiteSpace(name))
                return FailedBecause(characterMustHaveANameMessage);
            
            if (string.IsNullOrWhiteSpace(race))
                return FailedBecause(characterMustHaveARaceMessage);

            if (weapon == null)
                return FailedBecause(characterMustHaveAWeaponMessage);
            
            return new Character(name, race, weapon);
        }
    }
}