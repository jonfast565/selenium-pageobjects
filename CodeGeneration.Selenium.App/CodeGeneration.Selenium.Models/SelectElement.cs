using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLiquid;
using Microsoft.FSharp.Collections;

namespace CodeGeneration.Selenium.Models
{
    public class SelectElement : DomElement, ILiquidizable
    {
        public SelectElement(string id, string classes, List<OptionsElement> options) : base("select", id, classes)
        {
            Options = options;
        }

        public SelectElement(string id, string classes, FSharpList<OptionsElement> options) : base("select", id, classes)
        {
            Options = options.ToList();
        }

        public List<OptionsElement> Options { get; set; }

        public object ToLiquid()
        {
            return new
            {
                Tag,
                Id,
                Classes,
                Options
            };
        }
    }
}
