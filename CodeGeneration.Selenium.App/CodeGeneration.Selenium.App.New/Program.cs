using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using CodeGeneration.Selenium.Models;
using DotLiquid;
using Newtonsoft.Json;

namespace CodeGeneration.Selenium.App.New
{
    internal class Program
    {
        private static string[] _elementSelectors = { "a", "button", "input", "select", "textarea", "form" };
        private static int _counter = 0;

        private static async Task Main(string[] args)
        {
            Console.WriteLine("-- Selenium Page Object Generator --");

            var dir = "C:\\Users\\jfast\\Desktop\\";
            var files = Directory.GetFiles(dir);

            foreach (var file in files.Where(x => x.EndsWith(".html")))
            {
                var pageObjectName = file.Split("\\")
                    .Last()
                    .Replace(".html", string.Empty)
                    .Split('_')
                    .Aggregate((x, y) => x + " " + y);
                pageObjectName = new CultureInfo("en-US", false).TextInfo.ToTitleCase(pageObjectName).Replace(" ", string.Empty);
                var pageUrl = file;

                var page = await GetPageByString(pageUrl);
                var elements = GetElements(page);
                var splitElements = DivvyElementsByType(elements);

                Console.WriteLine(JsonConvert.SerializeObject(splitElements, Formatting.Indented));
                PageObjectGenerator.GenerateCode(pageObjectName, dir, splitElements);
            }
        }

        private static async Task<IDocument> GetPageByString(string pageUrl)
        {
            if (pageUrl == null)
            {
                throw new Exception("Page url is null");
            }

            var pageContents = await File.ReadAllTextAsync(pageUrl);
            var htmlParser = new HtmlParser(options: new HtmlParserOptions { IsStrictMode = false });
            var document = await htmlParser.ParseDocumentAsync(pageContents);

            return document;

        }

        private static async Task<IDocument> GetPageByUrl(string pageUrl)
        {
            if (pageUrl == null)
            {
                throw new Exception("Page url is null");
            }

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(pageUrl);

            return document;
        }

        private static List<IElement> GetElements(IDocument page)
        {
            return _elementSelectors.SelectMany(page.QuerySelectorAll).ToList();
        }

        private static string ProcessIdToVariable(IElement el)
        {
            var name = el.Id ?? $"UnnamedElement{_counter++}";
            var textInfo = new CultureInfo("en-US", false).TextInfo;
            var splitted = name.Replace("_", " ").Replace("-", " ").ToLowerInvariant();
            var titleCase = textInfo.ToTitleCase(splitted);
            var result = titleCase.Replace(" ", string.Empty);
            return result;
        }

        private static SplitDomElements DivvyElementsByType(List<IElement> elements)
        {
            var inputElements = new List<InputElement>();
            var buttonElements = new List<ButtonElement>();
            var selectElements = new List<SelectElement>();
            var checkboxElements = new List<InputCheckboxElement>();
            var linkElements = new List<LinkElement>();
            var radioElements = new List<RadioElement>();
            var textAreaElements = new List<TextAreaElement>();

            var filteredElements = elements.Where(x => !string.IsNullOrEmpty(x.Id));
            foreach (var el in filteredElements)
            {
                var tagName = el.TagName.ToLower();
                var idValues = el.Id;
                var classValues = el.ClassName;
                var variableName = ProcessIdToVariable(el);

                switch (tagName)
                {
                    case "input" when el.Attributes["type"].Value != "hidden":
                        {
                            if (el.Attributes["type"].Value == "checkbox")
                            {
                                var name = el.Attributes["name"]?.Value;
                                checkboxElements.Add(new InputCheckboxElement(idValues, classValues,
                                    name,
                                    name == "checked",
                                    variableName));
                            }
                            else
                            {
                                var name = el.Attributes["name"]?.Value;
                                var type = el.Attributes["type"]?.Value;
                                inputElements.Add(new InputElement(idValues, classValues, name,
                                    type, el.InnerHtml, variableName));
                            }

                            break;
                        }
                    case "button":
                        buttonElements.Add(new ButtonElement(idValues, classValues, variableName));
                        break;
                    case "select":
                        {
                            var optionsElements = new List<OptionsElement>();
                            var options = el.QuerySelectorAll("option");

                            optionsElements.AddRange(
                                options.Select(x =>
                                {
                                    var @class = x.Attributes["class"]?.Value;
                                    var value = x.Attributes["value"]?.Value;
                                    var optionsVariableName = ProcessIdToVariable(x);

                                    return new OptionsElement(x.Id, @class, x.InnerHtml, value, optionsVariableName);
                                }));

                            selectElements.Add(new SelectElement(idValues, classValues, optionsElements, variableName));
                            break;
                        }
                    case "a":
                        var href = el.Attributes["href"]?.Value;
                        linkElements.Add(new LinkElement(idValues, classValues, href, el.InnerHtml, variableName));
                        break;
                    case "textarea":
                        textAreaElements.Add(new TextAreaElement(idValues, classValues, variableName));
                        break;
                }
            }

            var result = new SplitDomElements(
                inputElements,
                selectElements,
                buttonElements,
                checkboxElements,
                linkElements,
                radioElements,
                textAreaElements);

            return result;
        }
    }
}
