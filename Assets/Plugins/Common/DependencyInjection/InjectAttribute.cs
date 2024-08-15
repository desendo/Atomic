using System;

namespace Common.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class InjectAttribute : Attribute
    {
    }
}