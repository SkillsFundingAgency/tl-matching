using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ValidationAttributes
{
    public class MinAttribute : ValidationAttribute
    {
        private readonly short _minValue;

        public MinAttribute(short minValue)
        {
            _minValue = minValue;
        }

        public override bool IsValid(object value)
        {
            return (short)value >= _minValue;
        }
    }
}