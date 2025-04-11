namespace Template_NetCoreWeb.WebApi;

/// <summary>
/// 專案使用的常數
/// </summary>
internal static class Const
{
    /// <summary>
    /// 代表允許呼叫 API 的前台 Audience 的 Policy
    /// </summary>
    internal const string frontendAudiencePolicyName = "Frontend";
    /// <summary>
    /// Specifies the Development environment.
    /// </summary>
    /// <remarks>The development environment can enable features that shouldn't be exposed in production. Because of the performance cost, scope validation and dependency validation only happens in development.</remarks>
    internal const string developmentEnvironmentName = "Development";
    /// <summary>
    /// Specifies the Staging environment.
    /// </summary>
    /// <remarks>The staging environment can be used to validate app changes before changing the environment to production.</remarks>
    internal const string stagingEnvironmentName = "Staging";
    /// <summary>
    /// Specifies the Production environment.
    /// </summary>
    /// <remarks>The production environment should be configured to maximize security, performance, and application robustness.</remarks>
    internal const string productionEnvironmentName = "Production";
}
