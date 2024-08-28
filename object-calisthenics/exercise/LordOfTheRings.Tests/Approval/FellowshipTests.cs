using ApprovalTests;
using ApprovalTests.Reporters;
using LordOfTheRings.App;

namespace LordOfTheRings.Tests.Approval;

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