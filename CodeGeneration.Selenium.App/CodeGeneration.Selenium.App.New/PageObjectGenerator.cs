using System.IO;
using CodeGeneration.Selenium.Models;
using DotLiquid;

namespace CodeGeneration.Selenium.App.New
{
    public static class PageObjectGenerator
    {
        public static void GenerateCode(string pageObjectName, SplitDomElements sde)
        {
            var template = Template.Parse(File.ReadAllText("./Templates/CSharpCode.liquid"));
            var hash = Hash.FromAnonymousObject(new { root = sde, name = pageObjectName });
            var rendered = template.Render(hash);
            Directory.CreateDirectory("./Results");
            File.WriteAllText($"./Results/{pageObjectName}.cs", rendered);
        }
    }
}
