using Ardalis.SharedKernel;
using Leuze_AGV_Handling_Service.Core.ContributorAggregate;
using Leuze_AGV_Handling_Service.UseCases.Contributors.Create;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Leuze_AGV_Handling_Service.UnitTests.UseCases.Contributors;

public class CreateContributorHandlerHandle
{
  private readonly string _testName = "test name";
  private readonly IRepository<Contributor> _repository = Substitute.For<IRepository<Contributor>>();
  private CreateContributorHandler _handler;

  public CreateContributorHandlerHandle()
  {
    _handler = new CreateContributorHandler(_repository);
  }

  private Contributor CreateContributor()
  {
    return new Contributor(_testName);
  }

  [Fact]
  public async Task ReturnsSuccessGivenValidName()
  {
    _repository.AddAsync(Arg.Any<Contributor>(), Arg.Any<CancellationToken>())
      .Returns(Task.FromResult(CreateContributor()));
    var result = await _handler.Handle(new CreateContributorCommand(_testName, null), CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
  }
}
