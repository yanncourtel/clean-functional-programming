I took Pierre and Yoan's exercise on the Object Calisthenics at heart to make this exercise.

Here is the original article -> ...

### The analysis of the topic

In object calisthenics, there is no perfect order. It's all about context.

I feel like the way to look at it is which principle is going to ...
1) fix your design 
VS
2) which principle is going to always drive the way your code looks.

Let's look over the principles

1. Only One Level Of Indentation Per Method
2. Don’t Use The ELSE Keyword
3. Wrap All Primitives And Strings
4. First Class Collections
5. One Dot Per Line
6. Don’t Abbreviate
7. Keep All Entities Small
8. No Classes With More Than Two Instance Variables
9. No Getters/Setters/Properties

We will base our understanding of each principle from this article to summarize 
what Jeff Bay was explaining in the book "The ThoughtWorks Anthology".
https://williamdurand.fr/2013/06/03/object-calisthenics/

Now, we are going to separate them into two list, the design concerns VS the syntax concerns.

Design adjusments
3. Wrap All Primitives And Strings
4. First Class Collections
5. One Dot Per Line
7. Keep All Entities Small
8. No Classes With More Than Two Instance Variables
9. No Getters/Setters/Properties

Syntax enhancements
1. Only One Level Of Indentation Per Method
2. Don’t Use The ELSE Keyword
6. Don’t Abbreviate

For the purpose of the exercise, we will be using TDD to make new code emerge through tests.
I will purposely not start with focusing on syntax enhancements to make new code emerge 
BUT in the refactor phase, I will always go over all the 3 rules in the syntax enhancements. 
We also call the syntax enhancements the "surface refactoring" where the IDE can sugggest 
automated actions while not breaking the code.

When we talk about TDD, we have to think if we are trying to write new code with no concerns 
of breaking the existing API or if we are doing a refactoring.

Here, we are doing a refactoring so I am trying to favor the baby steps which means 
the smallest code change that will help fix my code.

Let's evaluate from the design adjustments list the degree of complexity and change it will imply.
For that, I will use a simple Tshirt size: XS, S, M, L, XL

Design adjusments
3. Wrap All Primitives And Strings 					--> XS
4. First Class Collections 							--> XS
5. One Dot Per Line 								--> S
7. Keep All Entities Small 							--> M
8. No Classes With More Than Two Instance Variables --> XL
9. No Getters/Setters/Properties 					--> L

I assume for instance, one dot per line means fixing encapsulation which will likely question 
the entites themselves so it will add more complexity than maybe wrapping a list into its own class.

On the other side, have less than 3 invariants per class will significantly change my code 
to make it more modular.

### In practice

#### First steps

A golden rule of refactoring is not to refactor a code that is not tested. 
Here we have zero tests but we have a demo in the App class that use all 
the API functionnalities so the best solution for covering as much as possible is to plug 
a Golden Master to the Program itself.

For that, I encapsulate the algorythm in a Program class into a Run method that I run through 
a Main method to keep compatibility.

Then I can write an approval test using Program.Run like this

```csharp
[UseReporter(typeof(DiffReporter))]
public class FellowshipTests
{
    [Fact]
    public void VerifyFellowshipOfTheRingApp()
    {
        using var consoleOutput = new ConsoleOutputCapture();
        
        Program.Run();
        Approvals.Verify(consoleOutput.GetOutput());
    }
}
```

🔴 The test first fails because the Golden Master does not exist. 
A received file has the instructions and the approved file is empty.

🟢 I copy the content of the received in the approved file and voila! 
My golde master is ready and my test is green.

🔵 This allows me to not break the existing code while I'm refactoring.
After a quick look at the coverage, I see that nothing calls the UpdateCharacterWeapon method.
By virtue of YAGNI I can safely remove this method. No need to maintain unused code.

As we are in a refactoring phase, we could use the 3 syntax enhancements principles to 
automate some change. For the sake of respecting the baby step, we will focus in one
API method at a time. The first one is AddMember.

Here, **a lot of else are used**, and we can convert the foreach to **avoid having multiple indentation level**
A bunch of rules are, for now, stack into multiple if statements.
As we look at the Character class, we can see **the property are abbreviated**. 
We can quickly fix it with a rename. (we can see the purpose of the property in the error message - N for Name)

As I moved to the other methods, I noticed a pattern applying the principles for surface refactoring:

1) Don't use the else keyword
2) One indentation level
3) No abbreviation
4) One dot per line

Here, I mainly make my code easier to understand. Let's see if the order sticks.

#### Emerging new code - The Fellowship

The API being simpler, I can see a pattern here. We are managing a group of characters
that are all part of a fellowship. The logical step would be to use the **first-class collection principle**.
We can do that using a sprout method.

🔴 I want to add a Fellowship object and being able to add a member. 
My member is a Character that is for now valid, I then check how many members I have.

Let's write a test that will reflect that

````csharp
[Fact]
public void Should_Be_Able_To_Add_Member_To_Fellowship()
{
    var fellowship = new Fellowship();

    fellowship.AddMember(Character.New("frodo", "hobbit"));

    fellowship.HowManyMembers.Should().Be(1);
}
````

I added a wrinkle to the Character class to be able to instantiate a default character

````csharp
public static Character New(string name, string race)
{
    return new Character()
    {
        Name = name,
        Race = race,
        Weapon = new Weapon()
    };
}
````

The test is failing because the method is not implemented.

🟢 After a quick method generation and code change, I have the Fellowship class

````csharp
public class Fellowship
{
    private readonly List<Character> members = [];

    public void AddMember(Character character)
    {
        members.Add(character);
    }

    public int HowManyMembers => members.Count;
}
````

The test now passes!

🔵 Not much to refactor here. I just move the new classes in the right package.

Interestingly, at this stage, I could test that my GoldenMaster would work if 
I expose the members list and plug it all my code like this.

````csharp
public class FellowshipOfTheRingService
{
    private readonly Fellowship fellowship = new();

    public void AddMember(Character character)
    {
        //...
        //Rules to check character
        //...
        
        fellowship.AddMember(character);
    }

    public void RemoveMember(string name)
    {
        //...

        //Here we use the temporary property Members
        fellowship.Members.Remove(characterToRemove);
    }
    
    //...
}
````

Now of course, we don't want to keep exposing the inner details of the Fellowship object
but we want to make sure our new class can work on the production code.

All the code changes ahead will need to pass two checks:
1) my unit test
2) my integration test > the Golden Master

🔴 Now, let's add the invalid cases for adding members to challenge our design.

````csharp
[Fact]
public void Should_Fail_To_Add_Member_To_Fellowship()
{
    var fellowship = new Fellowship();
    
    var action = () =>
        fellowship.AddMember(Character.New("", "hobbit", new Weapon()));
    
    using (new AssertionScope())
    {
        action.Should().Throw<ArgumentException>().WithMessage("Character must have a name.");
        fellowship.HowManyMembers.Should().Be(0);
    }
}
````

🟢 A simple move of the rule to the Fellowship class and the

````csharp
public void AddMember(Character character)
{
    if (string.IsNullOrWhiteSpace(character.Name))
    {
        throw new ArgumentException("Character must have a name.");
    }
    
    members.Add(character);
}
````

🔵 Here, we can notice something is that we don't necessarily intend to throw an exception,
just to check that a new character has a valid name.

Therefore, let us cheat the system and add the rule in the character factory method.

````csharp
    public static Character New(string name, string race)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Character must have a name.");
        }
        
        return new Character()
        {
            Name = name,
            Race = race,
            Weapon = new Weapon()
        };
    }
````

#### The Character becomes a safe domain object

🔴 Let's move the rule of creating a new character in the character class by giving 
some logic to the character object.

````csharp
[Fact]
public void Should_Not_Create_Member_If_No_Name()
{
    ((Action?)(() => Character
        .New("", "hobbit", new Weapon())))
        .Should()
        .Throw<ArgumentException>()
        .WithMessage("Character must have a name.");
}
````

🟢 The test passes but in this case, it's to be expected.
We could have made the new code emerge from the Character test instead.

🔵 Now, let's remove the logic from the Fellowship and run all the tests. All good!

#### Removing exception

Before we move on to put all the rules in the Character, let's analyse something.
The test is very hard to understand to check an invalid state...

It would be much better to add a structure that return a character or nothing.

Let's see what is at our disposal through the test.
LanguageExt in C# allows us to use the Either monad to switch the return type.
The left member being the error message.
The right member being the character created.

🔴 The test looks like this

````csharp
[Fact]
public void Should_Not_Create_Member_If_No_Name()
{
    Character.TryNew("", "hobbit", new Weapon())
        .Should()
        .BeLeft(error => 
            error.Should().Be("Character must have a name."));
}
````

🟢 We need to create another method, let's call it TryNew.

````csharp
public static Either<String, Character> TryNew(string name, string race, Weapon weapon)
{
    if (string.IsNullOrWhiteSpace(name))
        return "Character must have a name.";
    
    return new Character()
    {
        Name = name,
        Race = race,
        Weapon = weapon
    };
}
````

🔵 We could add a class to store the error message in the application. 

With a bit of renaming and refactoring the signature method, we have this API.

````csharp
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
````

````csharp
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
````

🔴 We could add the same with the Weapon object. Let's add the tests.

````csharp
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
````

🟢 We need to handle the rules in the weapon class. 
We are saying a weapon cannot exist with no name and not deal damage.

````csharp
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
````

🔵 We can adjust the Program class to improve adding characters

````csharp
//...
    public static void Run()
    {
        var fellowship = new FellowshipOfTheRingService();

        try
        {
            AddMemberToFellowship("Frodo", "Hobbit",
                "Sting", 30, fellowship);
            
            AddMemberToFellowship("Sam", "Hobbit",
                "Dagger", 10, fellowship);
            
            //...
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
//...
````

We can do some package renaming