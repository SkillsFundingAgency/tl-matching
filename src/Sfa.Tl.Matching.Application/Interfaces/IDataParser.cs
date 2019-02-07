using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IDataParser<out T> where T : class
    {
        IEnumerable<T> Parse(string[] cells);
    }
}