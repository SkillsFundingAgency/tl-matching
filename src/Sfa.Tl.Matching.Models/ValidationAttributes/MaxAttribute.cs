using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ValidationAttributes
{
    public class MaxAttribute : ValidationAttribute
    {
        private readonly short _maxValue;

        public MaxAttribute(short maxValue)
        {
            _maxValue = maxValue;
        }

        public override bool IsValid(object value)
        {
            return (short)value <= _maxValue;
        }
    }
}