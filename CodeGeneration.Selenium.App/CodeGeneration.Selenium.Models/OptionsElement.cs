using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DotLiquid;

namespace CodeGeneration.Selenium.Models
{
    public class OptionsElement : DomElement, ILiquidizable
    {
        public OptionsElement(string id, string classes, string name, string value) : base("option", id, classes)
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
                Tag,
                Id,
                Classes,
                Name,
                Value
            };
        }
    }
}
