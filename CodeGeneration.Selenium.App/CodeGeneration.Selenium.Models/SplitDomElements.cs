﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLiquid;

namespace CodeGeneration.Selenium.Models
{
    public class SplitDomElements : ILiquidizable
    {
        public List<InputElement> InputElements { get; set; }
        public List<ButtonElement> ButtonElements { get; set; }
        public List<InputCheckboxElement> CheckboxElements { get; }
        public List<SelectElement> SelectElements { get; set; }
        public List<LinkElement> LinkElements { get; set; }
        public List<RadioElement> RadioElements { get; set; }

        public SplitDomElements(
            List<InputElement> inputElements, 
            List<SelectElement> selectElements,
            List<ButtonElement> buttonElements, 
            List<InputCheckboxElement> checkboxElements,
            List<LinkElement> linkElements,
            List<RadioElement> radioElements)
        {
            InputElements = inputElements;
            SelectElements = selectElements;
            ButtonElements = buttonElements;
            CheckboxElements = checkboxElements;
            LinkElements = linkElements.Where(x => x.Href != null).ToList();
            RadioElements = radioElements;
        }

        public object ToLiquid()
        {
            return new
            {
                InputElements,
                SelectElements,
                ButtonElements,
                CheckboxElements,
                LinkElements
            };
        }
    }
}
