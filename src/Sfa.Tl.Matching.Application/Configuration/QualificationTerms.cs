using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.Configuration
{
    public static class QualificationTerms
    {
        public static readonly List<string> Ignored = new()
        {
            "&",
            "a",
            "advanced",
            "an",
            "and",
            "ascentis",
            "asdan",
            "award",
            "btec",
            "cambridge",
            "certificate",
            "city & guilds",
            "city and guilds",
            "-credit",
            "cskills",
            "diploma",
            "extended",
            "foundation",
            "gce",
            "ietc",
            "in",
            "introduction",
            "level",
            "national",
            "nocn",
            "nvq",
            "pearson",
            "qcf",
            "subsidiary",
            "technical",
            "the",
            "vrq",
            "vtct",
            "with"
        };
    }
}