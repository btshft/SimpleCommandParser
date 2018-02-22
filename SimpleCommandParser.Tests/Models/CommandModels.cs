using SimpleCommandParser.Attributes;

namespace SimpleCommandParser.Tests.Models
{
    public class SingleCommandModel
    {
        [Argument("arg1")]
        public string RequiredArg1 { get; set; }
        
        [Argument("arg2")]
        public string RequiredArg2 { get; set; }
        
        [Argument("arg3", required: false)]
        public string OptionalArg3 { get; set; } 
        
        [Argument("arg4", required: false)]
        public int OptionalArg4 { get; set; }
        
        [Argument("arg5", required: false)]
        public int OptionalArg5 { get; }
        
        [Argument("arg6", required: false)]
        public CustomType OptionalArg6 { get; }
    }
    
    public class CustomType { }

    [Verb("first")]
    public class FirstCommand
    {
        [Argument("arg1")]
        public string RequiredArg1 { get; set; }
        
        [Argument("arg2", required: false)]
        public int OptionalArg2 { get; set; }
    }
    
    [Verb("second")]
    public class SecondCommand
    {
        [Argument("arg1")]
        public string RequiredArg1 { get; set; }
        
        [Argument("arg2", required: false)]
        public int OptionalArg2 { get; set; }
    }
    
    [Verb("second")]
    public class SecondCommandDuplicate
    {
        [Argument("arg1")]
        public string RequiredArg1 { get; set; }
        
        [Argument("arg2", required: false)]
        public int OptionalArg2 { get; set; }
    }

    public class VerbMissingCommand
    {
        [Argument("arg1")]
        public string RequiredArg1 { get; set; }
        
        [Argument("arg2", required: false)]
        public int OptionalArg2 { get; set; }
    }
}