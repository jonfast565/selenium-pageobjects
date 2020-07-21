﻿using System;
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
using Newtonsoft.Json;

namespace CodeGeneration.Selenium.App.New
{
    internal class Program
    {
        private static string[] _elementSelectors = {"a", "button", "input", "select", "textarea", "form"};

        private static async Task Main(string[] args)
        {
            Console.WriteLine("-- Selenium Page Object Generator --");

            var dir = "C:\\Users\\jfast\\Desktop\\test_pages\\";
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
            var htmlParser = new HtmlParser(options: new HtmlParserOptions {IsStrictMode = false});
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

        private static SplitDomElements DivvyElementsByType(List<IElement> elements)
        {
            var inputElements = new List<InputElement>();
            var buttonElements = new List<ButtonElement>();
            var selectElements = new List<SelectElement>();
            var checkboxElements = new List<InputCheckboxElement>();
            var linkElements = new List<LinkElement>();
            var radioElements = new List<RadioElement>();

            foreach (var el in elements)
            {
                var tagName = el.TagName.ToLower();
                var idValues = el.Id;
                var classValues = el.ClassName;

                switch (tagName)
                {
                    case "input" when el.Attributes["type"].Value != "hidden":
                    {
                        if (el.Attributes["type"].Value == "checkbox")
                        {
                            checkboxElements.Add(new InputCheckboxElement(idValues, classValues,
                                el.Attributes["name"]?.Value,
                                el.Attributes["checked"]?.Value == "checked"));
                        }
                        else
                        {
                            inputElements.Add(new InputElement(idValues, classValues, el.Attributes["name"].Value,
                                el.Attributes["type"].Value, el.InnerHtml));
                        }

                        break;
                    }
                    case "button":
                        buttonElements.Add(new ButtonElement(idValues, classValues));
                        break;
                    case "select":
                    {
                        var optionsElements = new List<OptionsElement>();
                        var options = el.QuerySelectorAll("option");

                        optionsElements.AddRange(
                            options.Select(x =>
                                new OptionsElement(x.Id, x.Attributes["class"]?.Value, x.InnerHtml,
                                    x.Attributes["value"]?.Value)));
                        selectElements.Add(new SelectElement(idValues, classValues, optionsElements));
                        break;
                    }
                    case "a":
                        linkElements.Add(new LinkElement(idValues, classValues, el.Attributes["href"]?.Value,
                            el.InnerHtml));
                        break;
                }
            }

            var result = new SplitDomElements(
                inputElements, 
                selectElements, 
                buttonElements, 
                checkboxElements, 
                linkElements,
                radioElements);

            return result;
        }
    }
}
