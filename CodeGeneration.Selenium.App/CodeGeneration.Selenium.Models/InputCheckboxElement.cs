using System;
using System.Collections.Generic;
using System.Text;
using DotLiquid;

namespace CodeGeneration.Selenium.Models
{
    public class InputCheckboxElement : DomElement, ILiquidizable
    {
        private static int _number;

        public InputCheckboxElement(string id, string classes, string name, bool enabled) : base("checkbox", id, classes, $"Checkbox{_number++}")
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
                InternalValue,
                Tag,
                Id,
                Classes,
                Name
            };
        }
    }
}
