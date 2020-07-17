using System;
using System.Collections.Generic;
using System.Text;
using DotLiquid;

namespace CodeGeneration.Selenium.Models
{
    public class RadioElement : DomElement, ILiquidizable
    {
        private static int _number;

        public RadioElement(string id, string classes) : base("radio", id, classes, $"Radio{_number++}")
        {

        }

        public object ToLiquid()
        {
            return new
            {
                InternalValue,
                Tag,
                Id,
                Classes
            };
        }
    }
}
