using SimpleCommandParser.Attributes;

namespace SimpleCommandParser.Examples.Models
{
    public class SampleCommand
    {
        [Parameter("f", "foo")]
        public string Foo { get; set; }
    }
}