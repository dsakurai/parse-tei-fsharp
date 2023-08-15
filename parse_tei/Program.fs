// For more information see https://aka.ms/fsharp-console-apps

open System
open System.Xml
open System.Xml.Linq
open System.Diagnostics

let printMemoryUsage () =
    let process = Process.GetCurrentProcess()
    let memoryInBytes = process.WorkingSet64
    let memoryInMegabytes = memoryInBytes / 1024L / 1024L
    printfn "Memory usage: %d MB" memoryInMegabytes


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

printMemoryUsage()
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

printMemoryUsage()
let entries = xdoc.Descendants("{http://www.tei-c.org/ns/1.0}entry")
printMemoryUsage()

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
                

                 
let genders_der_die_das (entry: XElement) =
    
    // masculine, feminine, neuter as 'm', 'f', 'n'
    let genders_mfn (entry: XElement) =
                     let gramGrp = entry.Elements(XName.Get(tag "gramGrp"))
                     if Seq.isEmpty gramGrp then
                         Seq.empty // empty gender is given for, e.g., a verb.
                     else
                         gramGrp
                         |> Seq.exactlyOne
                         |> (fun gramGrp -> get_elements gramGrp "pos")
                         |> Seq.map (fun pos -> pos.Value)

    let genders = genders_mfn entry
    
    [ if Seq.contains "m" genders then yield "der"
      if Seq.contains "f" genders then yield "die"
      if Seq.contains "n" genders then yield "das"
       ]


type Word = {
    spelling: string
    grammar_group: string
}

let words_map =
    entries
    // the spell
    |> Seq.groupBy (fun (entry: XElement) -> spell entry)
    |> Seq.map (fun (spelling: string, entries: seq<XElement>) ->
            (spelling,
             entries
            )
        )
    |> Map.ofSeq
    
printMemoryUsage()
    
let A_s = Map.find "A" words_map

printMemoryUsage()
    
for each in A_s do
    Console.WriteLine(each)

let test entries =
    entries |> Seq.iter (fun (entry: XElement) ->
        let spelling: string = spell entry
        printfn "%s" (spell entry)
        
        // let x = genders_der_die_das entry
        )

test entries

let xml_key_values (filePath: string) =
    let xdoc = XDocument.Load(filePath)
    xdoc.Descendants("message")
        |> Seq.map (fun parentElement ->
            parentElement.Descendants("to")
            |> Seq.map (fun elem -> elem.Value)
            )
        
for value in (xml_key_values test_xml_path) do
    printfn "%A" value

