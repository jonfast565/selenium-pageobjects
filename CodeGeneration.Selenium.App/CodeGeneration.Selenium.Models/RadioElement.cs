﻿using System;
using System.Collections.Generic;
using System.Text;
using DotLiquid;

namespace CodeGeneration.Selenium.Models
{
    public class RadioElement : DomElement, ILiquidizable
    {
        private static int _number;

        public RadioElement(string id, string classes, string variableName) : base("radio", id, classes, $"Radio{_number++}", variableName)
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
