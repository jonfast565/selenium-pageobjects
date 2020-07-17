using DotLiquid;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneration.Selenium.Models
{
    public class LinkElement : DomElement, ILiquidizable
    {
        private static int _number;

        public LinkElement(string id, string classes, string href, string value) : base("a", id, classes, $"Link{_number++}")
        {
            Href = href;
            Value = value;
        }

        public string Value { get; set; }

        public string Href { get; set; }

        public object ToLiquid()
        {
            return new
            {
                InternalValue,
                Tag,
                Id,
                Classes,
                Href,
                Value
            };
        }
    }
}
