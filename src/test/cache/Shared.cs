using ApiClient.Cache.Entity;
using System;

namespace ApiClient.Test.Cache;

public sealed record TestResult(string Value);
public sealed record OtherResult(string Value);

internal static class TestRequests
{
    // Adapt to StructuredRequest's real shape. Calls with the same key must
    // produce distinct instances that compare as equal.
    public static StructuredRequest Create(string key) => 
        new(Guid.NewGuid(), key, new DateTime(2026,1,1));
}
