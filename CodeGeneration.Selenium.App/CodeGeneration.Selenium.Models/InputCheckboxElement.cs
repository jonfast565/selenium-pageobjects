using System;
using System.Collections.Generic;
using System.Text;
using DotLiquid;

namespace CodeGeneration.Selenium.Models
{
    public class InputCheckboxElement : DomElement, ILiquidizable
    {
        public InputCheckboxElement(string id, string classes, string name, bool enabled) : base("checkbox", id, classes)
        {
            Name = name;
            Enabled = enabled;
        }

        public string Name { get; set; }
        public bool Enabled { get; set; }

        public object ToLiquid()
        {
            return new
            {
                Tag,
                Id,
                Classes,
                Name
            };
        }
    }
}
