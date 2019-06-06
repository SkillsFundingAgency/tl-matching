using System.Collections.Generic;

namespace Sfa.Tl.Matching.Application.Configuration
{
    public static class QualificationTerms
    {
        public static readonly List<string> Ignored = new List<string>
        {
            " and ",
            "BTEC",
            "Level",
            "advanced",
            "national",
            "diploma",
            " in ",
            "certificate",
            " the ",
            "QCF",
            "extended",
            "-credit",
            "&",
            "Pearson",
            "subsidiary",
            "foundation",
            "NVQ",
            "award",
            "City & Guilds",
            "City and guilds",
            "VTCT",
            "IETC",
            "technical",
            "VRQ",
            "Ascentis",
            "NOCN",
            "Cskills",
            "Cambridge",
            "introduction",
            " the ",
            " an ",
            " a ",
            " with ",
            "ASDAN",
            "GCE"
        };
    }
}