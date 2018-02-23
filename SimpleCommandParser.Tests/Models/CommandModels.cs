using System;
using SimpleCommandParser.Attributes;

namespace SimpleCommandParser.Tests.Models
{
    public class SingleCommandModel
    {
        [Parameter("arg1", "argument1")]
        public string RequiredArg1 { get; set; }
        
        [Parameter("arg2", "argument2")]
        public string RequiredArg2 { get; set; }
        
        [Parameter("arg3", "argument3", Required = false)]
        public string OptionalArg3 { get; set; } 
        
        [Parameter("arg4", "argument4", Required =  false)]
        public int OptionalArg4 { get; set; }
        
        [Parameter("arg5", "argument5", Required = false)]
        public int OptionalArg5 { get; }
        
        [Parameter("arg6", "argument6", Required = false)]
        public CustomType OptionalArg6 { get; set; }
        
        [Option("arg7", "argument7")]
        public bool Option7 { get; set; }
    }
    
    public class CustomType { }

    [Verb("first")]
    public class FirstCommand
    {
        [Parameter("arg1")]
        public string RequiredArg1 { get; set; }
        
        [Parameter("arg2", Required = false)]
        public int OptionalArg2 { get; set; }
    }
    
    [Verb("second")]
    public class SecondCommand
    {
        [Parameter("arg1")]
        public string RequiredArg1 { get; set; }
        
        [Parameter("arg2", Required = false)]
        public int OptionalArg2 { get; set; }
    }
    
    [Verb("second")]
    public class SecondCommandDuplicate
    {
        [Parameter("arg1")]
        public string RequiredArg1 { get; set; }
        
        [Parameter("arg2", Required = false)]
        public int OptionalArg2 { get; set; }
    }

    public class VerbMissingCommand
    {
        [Parameter("arg1")]
        public string RequiredArg1 { get; set; }
        
        [Parameter("arg2", Required = false)]
        public int OptionalArg2 { get; set; }
    }
}