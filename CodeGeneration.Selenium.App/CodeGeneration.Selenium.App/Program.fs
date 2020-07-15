﻿// Learn more about F# at http://fsharp.org

open System
open AngleSharp
open AngleSharp.Dom
open Newtonsoft.Json

type DomElement (tag: string, id: string, classes: string) =
    member this.Tag = tag
    member this.Id = id
    member this.Classes = classes

type ButtonElement (id: string, classes: string) =
    inherit DomElement("button", id, classes)

type OptionsElement(id: string, classes: string, name: string, value: string) =
    inherit DomElement("option", id, classes)
    member this.Name = name
    member this.Value = value

type SelectElement (id: string, classes: string, options: List<OptionsElement>) = 
    inherit DomElement("select", id, classes)
    member this.Options = options

type InputCheckboxElement (id: string, classes: string, name: string, currentValue : bool) =
    inherit DomElement("input", id, classes)
    member this.Name = name
    member this.CurrentValue = currentValue
       
type InputElement (id: string, classes: string, name: string, typeName: string, placeholder: string, currentValue: string) =
    inherit DomElement("input", id, classes)
    member this.Name = name
    member this.Placeholder = placeholder
    member this.CurrentValue = currentValue
    member this.TypeName = typeName

type SplitDomElements (inputElements: List<InputElement>, selectElements: List<SelectElement>, buttonElements: List<ButtonElement>, checkboxElements: List<InputCheckboxElement>) =
    member this.InputElements = inputElements
    member this.SelectElements = selectElements
    member this.ButtonElements = buttonElements
    member this.CheckboxElements = checkboxElements

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
            else inputElements <- inputElements @ [InputElement(idValues, classValues, el.Attributes.["name"].Value, el.Attributes.["type"].Value, "", el.InnerHtml)]

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

        // if tagName = "textarea"
        //    then ""

    let splitDomElements = SplitDomElements(inputElements, selectElements, buttonElements, checkboxElements)
    splitDomElements
    
let generateCode splitElements = 
    1=1

[<EntryPoint>]
let main argv =
    let debug = true
    printfn "-- Selenium Page Object Generator --"

    printfn "Please enter the URL of a page to process: "
    let pageUrl = Console.ReadLine()

    let page = getPageByUrl pageUrl
    let elements = getElements page rootSelector

    let splitElements = divvyElementsByType elements

    if debug then
        printfn "%s" <| JsonConvert.SerializeObject(splitElements, Formatting.Indented)

    
    
    0