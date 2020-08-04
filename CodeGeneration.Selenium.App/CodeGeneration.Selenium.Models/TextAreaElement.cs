using DotLiquid;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneration.Selenium.Models
{
    public class TextAreaElement : DomElement, ILiquidizable
    {
        private static int _number;

        public TextAreaElement(string id, string classes, string variableName) : base("textarea", id, classes, $"TextArea{_number++}", variableName)
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
