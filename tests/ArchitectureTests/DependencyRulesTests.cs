using System.Reflection;
using NetArchTest.Rules;
using Xunit;

namespace EduCrm.ArchitectureTests;

public sealed class DependencyRulesTests
{
    private static readonly Assembly SharedKernel =
        typeof(EduCrm.SharedKernel.Results.Result).Assembly;

    private static readonly Assembly PeopleDomain =
        typeof(EduCrm.Modules.People.Domain.AssemblyMarker).Assembly;

    private static readonly Assembly PeopleApplication =
        typeof(EduCrm.Modules.People.Application.AssemblyMarker).Assembly;
    
    private static readonly Assembly PeopleContracts =
        typeof(EduCrm.Modules.People.Contracts.AssemblyMarker).Assembly;


    [Fact]
    public void SharedKernel_should_not_depend_on_WebApi_Infrastructure_or_Modules()
    {
        var result = Types.InAssembly(SharedKernel)
            .ShouldNot()
            .HaveDependencyOnAny(
                "EduCrm.WebApi",
                "EduCrm.Infrastructure",
                "EduCrm.Modules.")
            .GetResult();

        Assert.True(result.IsSuccessful, result.FormatFailures());
    }

    [Fact]
    public void People_Domain_should_not_depend_on_AspNetCore_WebApi_or_Infrastructure()
    {
        var result = Types.InAssembly(PeopleDomain)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Microsoft.AspNetCore.",
                "EduCrm.WebApi",
                "EduCrm.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful, result.FormatFailures());
    }

    [Fact]
    public void People_Application_should_not_depend_on_AspNetCore_or_WebApi()
    {
        var result = Types.InAssembly(PeopleApplication)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Microsoft.AspNetCore.",
                "EduCrm.WebApi")
            .GetResult();

        Assert.True(result.IsSuccessful, result.FormatFailures());
    }
    
    [Fact]
    public void People_Contracts_should_not_depend_on_AspNetCore_WebApi_Infrastructure_or_EFCore()
    {
        var result = Types.InAssembly(PeopleContracts)
            .ShouldNot()
            .HaveDependencyOnAny(
                "Microsoft.AspNetCore.",
                "EduCrm.WebApi",
                "EduCrm.Infrastructure",
                "Microsoft.EntityFrameworkCore",
                "Npgsql.EntityFrameworkCore")
            .GetResult();

        Assert.True(result.IsSuccessful, result.FormatFailures());
    }
}

internal static class NetArchTestFailureFormatting
{
    public static string FormatFailures(this TestResult result)
    {
        if (result.IsSuccessful) return string.Empty;

        // NetArchTest can return null here depending on version / rule type.
        var names = result.FailingTypeNames ?? Array.Empty<string>();
        if (names.Count > 0)
            return "Failing types:" + Environment.NewLine + string.Join(Environment.NewLine, names);

        // Fallback if names are not populated
        return "Architecture rule failed, but no failing type names were provided by NetArchTest.";
    }
}