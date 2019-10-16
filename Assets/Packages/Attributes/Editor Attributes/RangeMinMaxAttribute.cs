namespace AdditionalAttributes
{
    public sealed class RangeMinMaxAttribute : RangeAttribute
    {
        public RangeMinMaxAttribute(float min, float max, float step = 0, bool showRandomButton = true) : base(min, max, step, showRandomButton) { }
    }
}
