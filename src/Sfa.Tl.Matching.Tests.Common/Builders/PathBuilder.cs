using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Tests.Common.Extensions;

namespace Sfa.Tl.Matching.Tests.Common.Builders
{
    public class PathBuilder
    {
        private readonly MatchingDbContext _context;

        public IList<Path> Paths { get; }

        public PathBuilder(MatchingDbContext context)
        {
            _context = context;
            Paths = new List<Path>();
        }

        public PathBuilder CreatePaths(int numberOfPaths = 1, string createdBy = null,
            DateTime? createdOn = null, string modifiedBy = null, DateTime? modifiedOn = null)
        {
            for (var i = 0; i < numberOfPaths; i++)
            {
                var pathNumber = i + 1;
                CreatePath(pathNumber, $"Path {pathNumber}",
                    $"Keyword{pathNumber}", $"Path {pathNumber} summary",
                    createdBy, createdOn, modifiedBy, modifiedOn);
            }

            return this;
        }

        public PathBuilder CreatePath(int id, string name, string keywords = "", string summary = "",
            string createdBy = null, DateTime? createdOn = null,
            string modifiedBy = null, DateTime? modifiedOn = null)
        {
            var path = new Path
            {
                Id = id,
                Name = name,
                Keywords = keywords,
                Summary = summary,
                CreatedBy = createdBy,
                CreatedOn = createdOn ?? default(DateTime),
                ModifiedBy = modifiedBy,
                ModifiedOn = modifiedOn,
            };

            Paths.Add(path);

            return this;
        }

        public PathBuilder ClearData()
        {
            if (!Paths.IsNullOrEmpty())
                _context.Path.RemoveRange(Paths);

            _context.SaveChanges();

            Paths.Clear();

            return this;
        }

        public PathBuilder SaveData()
        {
            if (!Paths.IsNullOrEmpty())
                _context.AddRange(Paths);

            _context.SaveChanges();

            return this;
        }
    }
}