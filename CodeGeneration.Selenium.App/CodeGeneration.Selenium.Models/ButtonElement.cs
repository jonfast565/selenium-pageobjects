using System;
using System.Collections.Generic;
using System.Text;
using DotLiquid;

namespace CodeGeneration.Selenium.Models
{
    public class ButtonElement : DomElement, ILiquidizable
    {
        private static int _number;

        public ButtonElement(string id, string classes, string variableName) : base("button", id, classes, $"Button{_number++}", variableName)
        {

        }

        public object ToLiquid()
        {
            return new
            {
                InternalValue,
                Tag,
                Id,
                Classes,
                VariableName
            };
        }
    }
}
