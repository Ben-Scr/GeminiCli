
namespace BenScr.GeminiCli.Core;
    public readonly struct RangeInt
    {
        public int Min { get; }
        public int Max { get; }

        public RangeInt(int min, int max)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException(nameof(min), min, "min cannot be greater than max.");

            Min = min;
            Max = max;
        }

        public int Clamp(int value) => Math.Clamp(value, Min, Max);
        public override string ToString()
        {
            return $"{Min}-{Max}";
        }
    }

