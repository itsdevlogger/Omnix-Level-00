namespace Omnix.Damping.Editor
{
    public readonly struct Transformer
    {
        private readonly float _delta;
        private readonly float _min;

        public Transformer(float min, float max)
        {
            _min = min;
            _delta = 1.12f * (max - _min); // 12% offset to make sure the graphs doest touch the box edges
        }

        public float Transform(float value)
        {
            return 0.06f + ((value - _min) / _delta); // 6% offset to make sure the graphs doest touch the box edges
        }
    }
}