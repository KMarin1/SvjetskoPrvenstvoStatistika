using System.Globalization;

namespace SvjetskoPrvenstvoPobjedeForm
{
    internal class MyCulture
    {
        public string Name { get; set; }
        public CultureInfo Value  { get; set; }

        public MyCulture(string name, CultureInfo value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString() => $"{Value.TwoLetterISOLanguageName}:{Name}";
    }
}