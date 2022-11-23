#r "nuget: FSharp.Stats"

open FSharp.Stats
open FSharp.Stats.Fitting.LinearRegression

open System
open System.IO

let tstaylorswift = "taylor swift"
let tsfearless = "fearless"
let tsspeakNow = "speak now"
let tsred = "red"
let ts1989 = "1989"
let tsreputation = "reputation"
let tsfolklore = "folklore"
let tsevermore = "evermore"
let tsmidnights = "midnights"

/// Inline print.
let iprint x = printfn $"%A{x}"; x

let stringBetween (start: string) (x: string) (finish: string) =
    let pFrom = x.IndexOf start + start.Length
    let pTo = x.LastIndexOf finish
    x.Substring(pFrom, pTo - pFrom)

let linreg (xs: float list) (ys: float list) =
    let ax = List.average xs
    let ay = List.average ys
    let sx = List.map (fun x -> ax - x) xs
    let ssx = List.sumBy (( ** )2.) sx

    let ssy = 
        List.map2 (fun x y -> x * (ay - y)) sx ys
        |> List.sum
    let m = ssy / ssx
    {| M = m |} 

linreg [0;1;2;3] [0;3;6;9]
|> printfn "%A"

/// Song details
type Track =
    { Name: string
      Lyrics: string
      Frequencies: Map<string, int>
      Writers: string list }

// Data access

let processLyrics (input: string seq) =
    let invalidChars = set ['.'; ':'; ';'; '?'; '!'; ','; '"'; '\''; '('; ')'; '\n'; '\r']
    input
    |> Seq.map (fun x -> x.ToLower())
    |> Seq.choose (fun x -> 
        if x.StartsWith '[' || String.IsNullOrWhiteSpace x then
            None
        else
            String.filter (fun c -> Set.contains c invalidChars |> not) x
            |> fun x -> x.Split ' '
            |> Some)
    |> Seq.concat

let parseWriters (encoded: string) =
    if encoded.StartsWith "[Writers:" && encoded.EndsWith ']' then
        (stringBetween "[Writers:" encoded "]").Split ';'
        |> Seq.toList
        |> Some
    else
        None

let parseName (encoded: string) = 
    (encoded.Split('_')[1]).Replace("-", " ").Replace(".txt", "")

let processSong name (text: string) = 
    let lines = text.Split '\n'
    
    let writers = 
        parseWriters lines[0]
        |> function
        | Some x -> x
        | None -> failwithf "Missing writers header on '%s'" name

    let freqs =
        processLyrics lines
        |> Seq.countBy id
        |> Map.ofSeq

    { Name = parseName name
      Lyrics = text
      Frequencies = freqs
      Writers = writers }

let fromAlbum (name: string) =
    let artistDirectory =
        DirectoryInfo("./data/").EnumerateDirectories()
        |> Seq.find (fun x -> x.Name.Replace('-', ' ').Equals("Taylor Swift", StringComparison.InvariantCultureIgnoreCase))
    artistDirectory.GetDirectories()
    |> Seq.where (fun x -> ((x.Name.Replace("-3am", "").Replace("-taylors-version", "").Replace("-edition", "").Replace("-deluxe", "").Replace('-', ' ').Split('_')[1]).ToLower()) = name.ToLower())
    |> Seq.collect (fun album -> album.EnumerateFiles())
    |> Seq.map (fun song -> File.ReadAllText song.FullName |> processSong song.Name)
    |> Seq.toList

// Data processing

[
    tsred
]
|> List.collect fromAlbum
|> List.map (fun x -> x.Writers.Length, x.Frequencies.Keys.Count)
|> List.sort
|> List.iter (printfn "%A")

// fromAlbum tsred

// |>  List.iter (fun x -> printfn "%-32s :::: %i" x.Name x.Writers.Length)

// |> Seq.collect (fun x -> x.Frequencies.Keys)
// |> Seq.distinct
// |> Seq.sort
// |> Seq.iter (printfn "%s")

// debut
// (1, 90)
// (1, 105)
// (1, 138)
// (2, 81)
// (2, 87)
// (2, 102)
// (2, 109)
// (2, 112)
// (2, 122)
// (2, 129)
// (2, 129)
// (3, 67)
// (3, 82)
// (3, 104)
// (3, 144)

// fearless
// (1, 95)
// (1, 110)
// (1, 121)
// (1, 122)
// (1, 125)
// (1, 125)
// (1, 139)
// (1, 147)
// (1, 148)
// (1, 160)
// (1, 169)
// (1, 172)
// (2, 85)
// (2, 85)
// (2, 100)
// (2, 107)
// (2, 109)
// (2, 110)
// (2, 114)
// (2, 118)
// (2, 127)
// (2, 129)
// (2, 146)
// (3, 82)
// (3, 113)
// (4, 79)