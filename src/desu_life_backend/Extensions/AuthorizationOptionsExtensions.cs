﻿using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace desu.life.Extensions
{
    public static class AuthorizationOptionsExtensions
    {
        // 暴露内部策略
        public static IDictionary<string, AuthorizationPolicy> GetPolicies(this AuthorizationOptions options)
        {
            return options.GetType()
                .GetProperty("PolicyMap", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(options) as IDictionary<string, AuthorizationPolicy>;
        }
    }
}