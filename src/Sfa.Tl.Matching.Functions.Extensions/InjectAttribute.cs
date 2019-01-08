using System;
using Microsoft.Azure.WebJobs.Description;

namespace Sfa.Tl.Matching.Functions.Extensions
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class InjectAttribute : Attribute
    {
    }
}
