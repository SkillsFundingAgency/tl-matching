using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Sfa.Tl.Matching.Tests.Common.Extensions
{
    public static class JsonResultExtensions
    {
        public static T GetValue<T>(this JsonResult jsonResult, string propertyName)
        {
            var property = jsonResult
                .Value
                .GetType()
                .GetProperties()
                .FirstOrDefault(p => 
                    string.CompareOrdinal(p.Name, propertyName) == 0);

            if (property == null)
                throw new ArgumentException("propertyName not found", nameof(propertyName));
            
            return (T)property.GetValue(jsonResult.Value, null);
        }
    }
}
