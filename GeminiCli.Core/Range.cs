
namespace BenScr.GeminiCli.Core
{
    public struct RangeInt
    {
        public int Min;
        public int Max;

        public RangeInt(int min, int max)
        {
            if (min > max)
                throw new ArgumentException("min can't be more than max");

            Min = min;
            Max = max;
        }

        public int Clamped(int value)
            => Math.Clamp(value, Min, Max);

        public override string ToString()
        {
            return Min + "-" + Max;
        }
    }
}
