using System;
using System.Collections.Generic;
using FluentAssertions;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.Event;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Extensions
{
    [Trait("ValueParser", "Data Tests")]
    public class ValueParserDataTests
    {
        [Theory(DisplayName = "QualificationSearch Data Tests")]
        [InlineData("and", "")]
        [InlineData("BTEC Level", "")]
        [InlineData("and BTEC LEVEL", "")]
        [InlineData("and BTEC Level", "")]
        [InlineData("city & guilds", "")]
        [InlineData("Level 2 Diploma Health Social Care (Adults) for England", "HealthSocialCareAdultsforEngland")]
        [InlineData("skills for professions in building and construction", "skillsforprofessionsbuildingconstruction")]
        [InlineData("BTEC skills for professions and BTEC skills for building and BTEC skills for construction", "skillsforprofessionsskillsforbuildingskillsforconstruction")]
        [InlineData("BTEC skills city & guilds professions", "skillsprofessions")]
        public void QualificationSearchDataTests(string searchTerm, string result)
        {
            searchTerm.ToQualificationSearch().Should().Be(result);
        }

        [Theory(DisplayName = "AllSpecialCharactersOrNumbers Data Tests")]
        [InlineData("Test", false)]
        [InlineData("Test2342423", false)]
        [InlineData("Test Test2", false)]
        [InlineData("Test Test2 Test3 Test4", false)]
        [InlineData("Test 4234 Test2 33", false)]
        [InlineData("$£%$£$ $£$", true)]
        [InlineData("123213", true)]
        [InlineData("$", true)]
        [InlineData("$£%$£$", true)]
        public void AllSpecialCharactersOrNumbersDataTests(string searchTerm, bool result)
        {
            searchTerm.IsAllSpecialCharactersOrNumbers().Should().Be(result);
        }

        [Theory(DisplayName = "ToTitleCase Data Tests")]
        [InlineData("Word", "Word")]
        [InlineData("an interesting idea", "An Interesting Idea")]
        [InlineData("war and peace", "War and Peace")]
        [InlineData("Sentence with an ACRONYM.", "Sentence With an Acronym.")]
        [InlineData("Kensington And Chelsea College", "Kensington and Chelsea College")]
        [InlineData("new year’s day", "New Year’s Day")]
        [InlineData("new year's day", "New Year's Day")]
        [InlineData("NEW YEAR'S DAY", "New Year's Day")]
        [InlineData("CHILDRENS'S SCHOOL", "Childrens's School")]
        [InlineData("CHILDRENS’S SCHOOL", "Childrens’s School")]
        [InlineData("Bob’s Burger’s", "Bob’s Burger’s")]
        [InlineData("QUEEN ELIZABETH'S GRAMMAR SCHOOL", "Queen Elizabeth's Grammar School")]
        [InlineData("BRIGHT & BROTHERS ACADEMY", "Bright & Brothers Academy")]
        public void ToTitleCaseDataTests(string input, string result)
        {
            input.ToTitleCase().Should().Be(result);
        }

        public static IEnumerable<object[]> SfaAupaData =>
            new List<object[]>
            {
                new object[] { new SfaAupa { Value = 229660000 }, AupaStatus.Aware },
                new object[] { new SfaAupa { Value = 229660001 }, AupaStatus.Understand },
                new object[] { new SfaAupa { Value = 229660002 }, AupaStatus.Planning },
                new object[] { new SfaAupa { Value = 229660003 }, AupaStatus.Active }
            };

        [Theory(DisplayName = "ToAupaStatus Data Tests")]
        [MemberData(nameof(SfaAupaData))]
        public void ToAupaDataTests(SfaAupa input, AupaStatus result)
        {
            input.ToAupaStatus().Should().Be(result);
        }

        [Theory(DisplayName = "ToBool Data Tests")]
        [InlineData("Yes", true)]
        [InlineData("True", true)]
        [InlineData("", false)]
        [InlineData("No", false)]
        [InlineData("False", false)]
        [InlineData("-", false)]
        public void ToBoolDataTests(string input, bool result)
        {
            input.ToBool().Should().Be(result);
        }
        
        public static IEnumerable<object[]> GuidData =>
            new List<object[]>
            {
                new object[] { "998F7B21-4929-4B75-9A45-C40D4F5D0F48", new Guid("998F7B21-4929-4B75-9A45-C40D4F5D0F48"),  }
            };

        [Theory(DisplayName = "ToGuid Data Tests")]
        [MemberData(nameof(GuidData))]
        public void ToGuidDataTests(string input, Guid result)
        {
            input.ToGuid().Should().Be(result);
        }

        [Theory(DisplayName = "ToDecimal Data Tests")]
        [InlineData("0", 0)]
        [InlineData("1.234", 1.234)]
        public void ToDecimalDataTests(string input, decimal result)
        {
            input.ToDecimal().Should().Be(result);
        }

        [Theory(DisplayName = "ToLong Data Tests")]
        [InlineData("0", 0)]
        [InlineData("1", 1)]
        public void ToLongDataTests(string input, long result)
        {
            input.ToLong().Should().Be(result);
        }
    }
}