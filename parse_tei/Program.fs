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
        
let test_xml_path = "/Users/Shared/work/shared/code-dev/learn-german-fsharp/learn_german/resources/sample_xml/test.xml"
// processXml xml_path

let tei_xml_path = "/Users/Shared/work/shared/code-dev/learn-german-fsharp/learn_german/resources/tu-chemnitz/deu-eng.tei"

let xdoc = XDocument.Load(tei_xml_path)

// for entry in xdoc.Descendants("{http://www.tei-c.org/ns/1.0}entry") do // an entry is a word
//     for form in entry.Elements(XName.Get("{http://www.tei-c.org/ns/1.0}form")) do
//         for orth in form.Elements(XName.Get("{http://www.tei-c.org/ns/1.0}orth")) do
//             printfn "%A" orth.Value
            
            
// An entry can be a word or a phrase
// The entries are not sorted.
// This is especially because some nouns can become a postfix, like
// <<Spiel>> becomes <<Hasardspiel>>.
// <<Hasardspiel>> is placed next to <<Spiel>> in the .tei file.

let entries = xdoc.Descendants("{http://www.tei-c.org/ns/1.0}entry")

let tag localname =
    // namespace
    let ns = "{http://www.tei-c.org/ns/1.0}"
    ns + localname
    
let get_elements (xelement: XElement) (localname: string) =
    xelement.Elements(XName.Get(tag localname))
                 
let spell (entry: XElement) =
                 get_elements entry "form"
                 |> Seq.exactlyOne
                 |> (fun form -> get_elements form "orth")
                 |> Seq.exactlyOne
                 |> (fun orth -> orth.Value)
                
let test entries =
    entries |> Seq.iter (fun (entry: XElement) ->
        printfn "%s" (spell entry)
        )

test entries

let genders (entry: XElement) =
                 entry.Elements(XName.Get(tag "gramGrp"))
                 |> Seq.exactlyOne
                 |> (fun gramGrp -> get_elements gramGrp "pos")
                 |> Seq.map (fun pos -> pos.Value) 

// let orth = forms |> Seq.map (fun form  -> form.Elements(XName.Get("{http://www.tei-c.org/ns/1.0}orth")))
// |> Seq.map (fun orth -> printfn "%A" orth.ToString)

let xml_key_values (filePath: string) =
    let xdoc = XDocument.Load(filePath)
    xdoc.Descendants("message")
        |> Seq.map (fun parentElement ->
            parentElement.Descendants("to")
            |> Seq.map (fun elem -> elem.Value)
            )
        
for value in (xml_key_values test_xml_path) do
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
