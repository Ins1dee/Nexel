using Nexel.Domain.Errors;
using Nexel.Domain.Shared;

namespace Tests.Domain.Shared;

public class ResultTests
{
    [Fact]
    public void Success_Should_CreateSuccessResult()
    {
        // Arrange & Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void Failure_Should_CreateFailureResultWithSpecifiedError()
    {
        // Arrange
        var expectedError = Error.DefaultError;

        // Act
        var result = Result.Failure(expectedError);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void SuccessWithValue_Should_CreateSuccessResultWithTValue()
    {
        // Arrange
        var expectedValue = "SampleValue";

        // Act
        var result = Result.Success(expectedValue);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);
        Assert.Equal(expectedValue, result.Value);
    }

    [Fact]
    public void FailureWithValue_Should_CreateFailureResultWithTValue()
    {
        // Arrange
        var expectedValue = "SampleValue";
        var expectedError = Error.DefaultError;

        // Act
        var result = Result.Failure(expectedValue);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(expectedError, result.Error);
        Assert.Equal(expectedValue, result.Value);
    }

    [Fact]
    public void FailureWithTValueAndError_Should_CreateFailureResultWithTValueAndError()
    {
        // Arrange
        var expectedError = DomainErrors.Cell.NotFound("id");

        // Act
        var result = Result.Failure<string>(expectedError);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(expectedError, result.Error);
        Assert.Equal(default, result.Value);
    }
}