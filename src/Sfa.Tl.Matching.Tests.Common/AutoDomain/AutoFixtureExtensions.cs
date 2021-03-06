﻿using System;
using AutoFixture;
using AutoFixture.Kernel;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    // ReSharper disable UnusedMember.Global
    public static class AutoFixtureExtensions
    {
        public static void PostprocessorFor<T>(this IFixture fixture, Action<T> postProcessor)
        {
            fixture.Behaviors.Add(
                new PostprocessorBehavior(
                    new ActionSpecimenCommand<T>(postProcessor), new ExactTypeSpecification(typeof(T))));
        }
    }
}