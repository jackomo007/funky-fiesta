using Microsoft.AspNetCore.Authorization;

namespace Lil.TimeTracker.Auth;

public class EmailDomainRequirement : IAuthorizationRequirement
{
    public EmailDomainRequirement(string domain) => Domain = domain;
    public string Domain { get; set;}
}