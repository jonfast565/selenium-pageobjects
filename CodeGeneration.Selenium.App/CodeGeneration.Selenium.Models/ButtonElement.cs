using System;
using System.Collections.Generic;
using System.Text;
using DotLiquid;

namespace CodeGeneration.Selenium.Models
{
    public class ButtonElement : DomElement, ILiquidizable
    {
        public ButtonElement(string id, string classes) : base("button", id, classes)
        {

        }

        public object ToLiquid()
        {
            return new
            {
                Tag,
                Id,
                Classes
            };
        }
    }
}
