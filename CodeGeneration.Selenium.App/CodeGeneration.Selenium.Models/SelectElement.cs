using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLiquid;
using Microsoft.FSharp.Collections;

namespace CodeGeneration.Selenium.Models
{
    public class SelectElement : DomElement, ILiquidizable
    {
        private static int _number;
        public SelectElement(string id, string classes, List<OptionsElement> options, string variableName) : base("select", id, classes, $"Select{_number++}", variableName)
        {
            Options = options;
        }

        public SelectElement(string id, string classes, FSharpList<OptionsElement> options, string variableName) : base("select", id, classes, $"Select{_number++}", variableName)
        {
            Options = options.ToList();
        }

        public List<OptionsElement> Options { get; set; }

        public object ToLiquid()
        {
            return new
            {
                InternalValue,
                Tag,
                Id,
                Classes,
                Options,
                VariableName
            };
        }
    }
}
