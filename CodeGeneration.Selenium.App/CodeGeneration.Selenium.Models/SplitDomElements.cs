using System;
using System.Collections.Generic;
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

        public SplitDomElements(List<InputElement> inputElements, List<SelectElement> selectElements,
            List<ButtonElement> buttonElements, List<InputCheckboxElement> checkboxElements)
        {
            InputElements = inputElements;
            SelectElements = selectElements;
            ButtonElements = buttonElements;
            CheckboxElements = checkboxElements;
        }

        public object ToLiquid()
        {
            return new
            {
                InputElements,
                SelectElements,
                ButtonElements,
                CheckboxElements
            };
        }
    }
}
