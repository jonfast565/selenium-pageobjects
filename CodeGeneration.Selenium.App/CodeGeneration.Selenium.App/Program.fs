open System
open System.Linq
open AngleSharp
open AngleSharp.Dom
open Newtonsoft.Json
open CodeGeneration.Selenium
open CodeGeneration.Selenium.Models

let rootSelector = "body"
let elementSelectors = [ //"a"; "button"; 
    "input"; //"select"; "textarea" 
]
let forbiddenInputTypes = [ "hidden" ]

let getPageByUrl (pageUrl : string) = 
    if pageUrl = null
    then raise <| Exception("Page url is null")

    let config = Configuration.Default.WithDefaultLoader()
    let context = BrowsingContext.New(config)
    let document = context.OpenAsync(pageUrl)
    let documentResult = document.Result

    documentResult

let getElements (page : IDocument) (root : string) = 
    let mutable elements = []
    let pageSelection = page.QuerySelector(root)
    for elementSelector in elementSelectors do
        let pageElements = pageSelection.QuerySelectorAll(elementSelector)
        for element in pageElements do
            elements <- elements @ [ element ]
    elements

let divvyElementsByType (elements: List<IElement>) =
    let mutable inputElements = []
    let mutable buttonElements = []
    let mutable selectElements = []
    let mutable checkboxElements = []

    for el in elements do  
        let tagName = el.TagName.ToLower()
        let idValues = el.Id
        let classValues = el.ClassName        

        if tagName = "input" && el.Attributes.["type"].Value <> "hidden" then 
            if el.Attributes.["type"].Value = "checkbox" 
            then checkboxElements <- checkboxElements @ [InputCheckboxElement(idValues, classValues, el.Attributes.["name"].Value, el.Attributes.["checked"].Value = "checked")]
            else inputElements <- inputElements @ [InputElement(idValues, classValues, el.Attributes.["name"].Value, el.Attributes.["type"].Value, el.InnerHtml)]

        if tagName = "button" 
            then buttonElements <- buttonElements @ [ButtonElement(idValues, classValues)]

        if tagName = "select"
            then
                let mutable optionElements = []
                let options = el.QuerySelectorAll("option")
                for option in options do
                    optionElements <- optionElements @ [option]
                let optionElements = optionElements |> List.map(fun (x : IElement) -> 
                    OptionsElement(x.Id, x.Attributes.["class"].Value, x.InnerHtml, x.Attributes.["value"].Value))
                selectElements <- selectElements @ [SelectElement(idValues, classValues, optionElements)]

        (* if tagName = "textarea" *)
        (*    then "" *)

    let splitDomElements = SplitDomElements(inputElements.ToList(), selectElements.ToList(), buttonElements.ToList(), checkboxElements.ToList())
    splitDomElements

[<EntryPoint>]
let main argv =
    let debug = true
    printfn "-- Selenium Page Object Generator --"

    // printfn "Please enter the URL of a page to process: "
    let pageObjectName = "NcdrLoginPage"
    let pageUrl = "https://www.ncdr.com/webncdr/login"

    let page = getPageByUrl pageUrl
    let elements = getElements page rootSelector

    let splitElements = divvyElementsByType elements

    if debug then
        printfn "%s" <| JsonConvert.SerializeObject(splitElements, Formatting.Indented)

    PageObjectGenerator.GenerateCode(pageObjectName, splitElements);
    
    0
