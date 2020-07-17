﻿using DotLiquid;

namespace CodeGeneration.Selenium.Models
{
    public class InputElement : DomElement, ILiquidizable
    {
        private static int _number;

        public InputElement(string id, string classes, string name, string typeName, string value) : base("input", id, classes, $"Input{_number++}")
        {
            Name = name;
            TypeName = typeName;
            Value = value;
        }

        public string Value { get; set; }

        public string Name { get; set; }

        public string TypeName { get; set; }

        public object ToLiquid()
        {
            return new
            {
                InternalValue,
                Tag,
                Id,
                Classes,
                Name,
                TypeName
            };
        }
    }
}
