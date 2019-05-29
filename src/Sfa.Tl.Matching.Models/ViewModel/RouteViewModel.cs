
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class RouteViewModel
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public IList<string> PathNames { get; set; }
    }
}
