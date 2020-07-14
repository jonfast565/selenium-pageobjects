// Learn more about F# at http://fsharp.org

open System
open AngleSharp
open AngleSharp.Dom

let rootSelector = "body"
let elementSelectors = [ "a"; "button"; "input"; "select"; "textarea" ]
let forbiddenInputTypes = [ "hidden" ]

let getPageByUrl (pageUrl : string) = 
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

[<EntryPoint>]
let main argv =
    printfn "-- Selenium Page Object Generator --"

    printfn "Please enter the URL of a page to process: "
    let pageUrl = Console.ReadLine()

    let page = getPageByUrl pageUrl
    let elements = getElements page rootSelector

    for element in elements do
        printfn "%A" element.OuterHtml

    for element in elements do
        printfn "%A" element.OuterHtml
    
    0
