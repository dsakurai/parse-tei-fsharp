// For more information see https://aka.ms/fsharp-console-apps

open System
open System.Xml.Linq

let processXml (filePath: string) =
    let xdoc = XDocument.Load(filePath)
    xdoc.Descendants("message") // Replace with the name of the parent element
        |> Seq.iter (fun parentElement ->
            parentElement.Descendants("to") // Replace with the name of the grandchild element
                |> Seq.iter (fun grandchildElement ->
                    // Process each grandchild element here
                 Console.WriteLine(grandchildElement.Value)
                )
        )
        
processXml "/Users/Shared/work/shared/code-dev/learn-german-fsharp/learn_german/resources/sample_xml/test.xml"