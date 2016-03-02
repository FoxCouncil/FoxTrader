namespace FoxTrader.Game.Utils
{
    public static class Generator
    {
        private static readonly global::System.Random m_random = new global::System.Random((int)global::System.DateTime.Now.Ticks & (0x0000FFFF + (int)global::System.DateTime.Now.Ticks));

        public static int RandomRange(int c_min, int c_max)
        {
            return m_random.Next(c_min, c_max);
        }

        public static string Name()
        {
            var a_name = Constants.kNameGenPrefix[RandomRange(0, Constants.kNameGenPrefix.Length)];
            a_name += Constants.kNameGenSuffix[RandomRange(0, Constants.kNameGenSuffix.Length)];
            a_name += Constants.kNameGenStems[RandomRange(0, Constants.kNameGenStems.Length)];

            return UppercaseFirst(a_name);
        }

        public static string FullName()
        {
            return Name() + " " + Name();
        }

        public static string CatalogueName()
        {
            return Name() + "-" + m_random.Next(1, 257);
        }

        static string UppercaseFirst(string c_s)
        {
            if (string.IsNullOrEmpty(c_s))
            {
                return string.Empty;
            }

            var a_a = c_s.ToCharArray();
            a_a[0] = char.ToUpper(a_a[0]);

            return new string(a_a);
        }
    }
}
