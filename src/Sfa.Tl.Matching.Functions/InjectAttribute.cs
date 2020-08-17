using System;
using Microsoft.Azure.WebJobs.Description;

namespace Sfa.Tl.Matching.Functions
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter)]
    public class InjectAttribute : Attribute
    {
        //TODO: Remove this attribute once the [Inject] bits are moved to ctors
    }
}
