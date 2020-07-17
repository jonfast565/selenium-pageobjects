﻿using System;
using System.Collections.Generic;
using System.Text;
using DotLiquid;

namespace CodeGeneration.Selenium.Models
{
    public class ButtonElement : DomElement, ILiquidizable
    {
        private static int _number;

        public ButtonElement(string id, string classes) : base("button", id, classes, $"Button{_number++}")
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
