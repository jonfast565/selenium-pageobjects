using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DotLiquid;

namespace CodeGeneration.Selenium.Models
{
    public class OptionsElement : DomElement, ILiquidizable
    {
        private static int _number;

        public OptionsElement(string id, string classes, string name, string value, string variableName) : base("option", id, classes, $"Option{_number++}", variableName)
        {
            Name = name;
            Value = value;
        }

        public string Value { get; set; }

        public string Name { get; set; }


        public object ToLiquid()
        {
            return new
            {
                InternalValue,
                Tag,
                Id,
                Classes,
                Name,
                Value,
                VariableName
            };
        }
    }
}
