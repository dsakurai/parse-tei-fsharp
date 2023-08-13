// For more information see https://aka.ms/fsharp-console-apps

open System
open System.Xml.Linq

let processXml (filePath: string)  =
    let xdoc = XDocument.Load(filePath)
    xdoc.Descendants("message") // Replace with the name of the parent element
        |> Seq.collect (fun parentElement ->
            parentElement.Value
        )
        |> Seq.toList
        
let xml_path = "/Users/Shared/work/shared/code-dev/learn-german-fsharp/learn_german/resources/sample_xml/test.xml"
// processXml xml_path

let xml_key_values (filePath: string) =
    let xdoc = XDocument.Load(filePath)
    xdoc.Descendants("message")
        |> Seq.map (fun parentElement ->
            parentElement.Descendants("to")
            |> Seq.map (fun elem -> elem.Value)
            )
        
for value in (xml_key_values xml_path) do
    printfn "%A" value

let elements = ["apple"; "apple"; "banana"; "cherry"]

let keyValueMap =
    elements
    |> Seq.groupBy id // Group by the element
    |> Seq.map (fun (key, values) ->
                                      (key,
                                       Seq.map (fun (value: string) -> value.Length) values
                                      )
    ) // Map the values to their lengths
    |> Map.ofSeq
    
// Print the map to verify the result
keyValueMap |> Map.iter (fun key values -> printfn "Key: %s, Value: %A" key values)
