using DotLiquid;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneration.Selenium.Models
{
    public class DomElement
    {
        public string Tag { get; set; }
        public string Id { get; set; }
        public string Classes { get; set; }
        public string InternalValue { get; set; }
        public string VariableName { get; set; }

        public DomElement(string tag, string id, string classes, string internalValue, string variableName)
        {
            Tag = tag;
            Id = id;
            Classes = classes;
            InternalValue = internalValue;
            VariableName = variableName;
        }
    }
}
