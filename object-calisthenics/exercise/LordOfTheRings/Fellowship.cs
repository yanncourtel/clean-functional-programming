namespace LordOfTheRings;

public class Fellowship
{
    private readonly List<Character> members = [];

    public List<Character> Members => members;

    public void AddMember(Character character)
    {
        members.Add(character);
    }

    public int HowManyMembers => members.Count;
}