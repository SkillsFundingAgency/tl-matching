using System;
using AutoFixture.Kernel;

namespace Sfa.Tl.Matching.Tests.Common.AutoDomain
{
    public class PostprocessorBehavior : ISpecimenBuilderTransformation
    {
        private readonly ISpecimenCommand _command;
        private readonly IRequestSpecification _specification;

        public PostprocessorBehavior(ISpecimenCommand command, IRequestSpecification specification)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _specification = specification ?? throw new ArgumentNullException(nameof(specification));
        }

        public ISpecimenBuilderNode Transform(ISpecimenBuilder builder)
        {
            return new Postprocessor(builder, _command, _specification);
        }
    }
}