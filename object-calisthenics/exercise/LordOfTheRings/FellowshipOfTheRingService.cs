namespace LordOfTheRings
{
    public class FellowshipOfTheRingService
    {
        private readonly Fellowship fellowship = new();

        public void AddMember(Character character)
        {
            if (fellowship.Members.Any(member => member.Name == character.Name))
            {
                throw new InvalidOperationException(
                    "A character with the same name already exists in the fellowship.Members.");
            }

            fellowship.AddMember(character);
        }

        public void RemoveMember(string name)
        {
            var characterToRemove = 
                fellowship.Members
                    .FirstOrDefault(character => character.Name == name);

            if (characterToRemove == null)
            {
                throw new InvalidOperationException($"No character with the name '{name}' exists in the fellowship.Members.");
            }

            fellowship.Members.Remove(characterToRemove);
        }

        public void MoveMembersToRegion(List<string> memberNames, string region)
        {
            memberNames
                .SelectMany(name => fellowship.Members.Where(character => character.Name == name))
                .ToList()
                .ForEach(character => MoveMemberToRegion(region, character));
        }

        private void MoveMemberToRegion(string region, Character character)
        {
            if (character.Region == "Mordor" && region != "Mordor")
            {
                throw new InvalidOperationException(
                    $"Cannot move {character.Name} from Mordor to {region}. Reason: There is no coming back from Mordor.");
            }

            character.Region = region;
            
            Console.WriteLine(region != "Mordor"
                ? $"{character.Name} moved to {region}."
                : $"{character.Name} moved to {region} 💀.");
        }

        public void PrintMembersInRegion(string region)
        {
            var charactersInRegion = fellowship.Members
                    .Where(character => character.Region == region)
                    .ToList();

            if (charactersInRegion.Count == 0)
            {
                Console.WriteLine($"No members in {region}");
                return;
            }

            Console.WriteLine($"Members in {region}:");
            foreach (var character in charactersInRegion)
            {
                Console.WriteLine($"{character.Name} ({character.Race}) " +
                                  $"with {character.Weapon.Name}");
            }
        }

        public override string ToString()
        {
            return fellowship.Members.Aggregate(
                "Fellowship of the Ring Members:\n", (current, member) => 
                    current + ($"{member.Name} ({member.Race}) with {member.Weapon.Name} in {member.Region}" + "\n"));
        }
    }
}