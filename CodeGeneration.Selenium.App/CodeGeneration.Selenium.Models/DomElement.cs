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

        public DomElement(string tag, string id, string classes)
        {
            Tag = tag;
            Id = id;
            Classes = classes;
        }
    }
}
