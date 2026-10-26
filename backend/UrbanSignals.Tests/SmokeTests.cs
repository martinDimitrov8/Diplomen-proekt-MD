using Xunit;
using UrbanSignals.Domain.Entities;
namespace UrbanSignals.Tests;
public class SmokeTests
{
    [Fact] public void NewReport_DefaultStatus_IsSubmitted() => Assert.Equal(ReportStatus.Submitted, new Report().Status);
    [Fact] public void NewAssignment_DefaultStatus_IsPending() => Assert.Equal(AssignmentStatus.Pending, new Assignment().Status);
}
