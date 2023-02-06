open System
open System.IO

let fileout location func =
    let d = (FileInfo location).Directory

    if not d.Exists then
        d.Create()

    let out = Console.Out

    try
        use ws = File.CreateText location
        Console.SetOut ws
        func ()
    finally
        Console.SetOut out

let tstaylorswift = "Taylor Swift" // 1
let tsfearless = "Fearless" // 2
let tsspeakNow = "Speak Now" // 3
let tsred = "Red" // 4
let ts1989 = "1989" // 5
let tsreputation = "reputation" // 6
let tslover = "Lover" // 7
let tsfolklore = "folklore" // 8
let tsevermore = "evermore" // 9
let tsmidnights = "Midnights" // 10

let ts =
    [ tstaylorswift
      tsfearless
      tsspeakNow
      tsred
      ts1989
      tsreputation
      tslover
      tsfolklore
      tsevermore
      tsmidnights ]

assert (ts.Length = 10)

let stringBetween (start: string) (x: string) (finish: string) =
    let pFrom = x.IndexOf start + start.Length
    let pTo = x.LastIndexOf finish
    x.Substring(pFrom, pTo - pFrom)

let linreg points =
    let n = List.length points |> float
    let sumX = List.sumBy fst points
    let sumY = List.sumBy snd points
    let meanX = List.averageBy fst points
    let meanY = List.averageBy snd points
    let a = List.sumBy (fun (x, y) -> (x - meanX) * (y - meanY)) points
    let b = List.sumBy (fun (x, _) -> (x - meanX) ** 2.) points
    let c = List.sumBy (fun (_, y) -> (y - meanY) ** 2.) points
    let r = a / (sqrt (b * c))
    let sumOfXY = List.sumBy (fun (x, y) -> x * y) points
    let squareOfSumOfX = sumX ** 2.
    let sumOfSquaresOfX = List.sumBy (fun (x, _) -> x ** 2.) points
    let m = (n * sumOfXY - sumX * sumY) / (n * sumOfSquaresOfX - squareOfSumOfX)
    let b = (sumY - m * sumX) / n
    {| M = m; B = b; R = r |}

/// Song details
type Track =
    { Name: string
      Words: string list
      Index: int
      Lyrics: string
      Frequencies: Map<string, int>
      Writers: string list }

    member this.Ratio = float this.Frequencies.Keys.Count / float this.Words.Length

// Data access

let processLyrics (input: string seq) =
    let invalidChars =
        set [ '.'; ':'; ';'; '?'; '!'; ','; '"'; '('; ')'; '\n'; '\r'; '…' ]

    input
    |> Seq.choose (fun x ->
        if x.StartsWith '[' || String.IsNullOrWhiteSpace x then
            None
        else
            String.filter (fun c -> Set.contains c invalidChars |> not) x
            |> fun x -> x.ToLower().Split ' '
            |> Some)
    |> Seq.concat

let parseWriters (encoded: string) =
    if encoded.StartsWith "[Writers:" && encoded.Trim().EndsWith ']' then
        (stringBetween "[Writers:" encoded "]").Split ';' |> Seq.toList |> Some
    else
        None

let parseName (encoded: string) =
    (encoded.Split('_')[1]).Replace("-", " ").Replace(".txt", "")

let processSong name (text: string) =
    let lines = text.Replace('е', 'e').Replace(' ', ' ').Replace('’', '\'').Split '\n'

    let writers =
        parseWriters lines[0]
        |> function
            | Some x -> x
            | None -> failwithf "Missing writers header on '%s'" name

    let words = processLyrics lines
    let freqs = Seq.countBy id words |> Map.ofSeq

    { Name = parseName name
      Index = name.Split('_')[0] |> int
      Words = List.ofSeq words
      Lyrics = text
      Frequencies = freqs
      Writers = writers }

let fromAlbum (name: string) =
    DirectoryInfo("./data/taylor-swift").GetDirectories()
    |> Array.where (fun x ->
        ((x
            .Name
            .Replace("-3am", "")
            .Replace("-taylors-version", "")
            .Replace("-edition", "")
            .Replace("-deluxe", "")
            .Replace('-', ' ')
            .Split('_')[1])
            .ToLower()) = name.ToLower())
    |> Array.rev
    |> Array.collect (fun album -> album.GetFiles())
    |> Array.Parallel.map (fun song -> File.ReadAllText song.FullName |> processSong song.Name)
    |> Array.toList

let tracksStats tracks =
    printfn "\nTrack List:"

    let w = List.map (fun x -> x.Name.Length) tracks |> List.max |> (+) 1

    for t in tracks do
        List.sort t.Writers
        |> String.concat ", "
        |> printfn "%2i %-*s%-4i%s" t.Index w t.Name t.Writers.Length

    printfn "\nIndex, Writers, Words, Distinct Words, Track"

    tracks
    |> List.sortBy (fun x -> x.Index)
    |> List.iter (fun x ->
        printfn $"{x.Index}, {x.Writers.Length}, {x.Words.Length}, {x.Frequencies.Keys.Count}, {x.Name}")

    printfn "\nTrack, Writers, Words, Distinct Words"

    tracks
    |> List.sortBy (fun x -> x.Index)
    |> List.iter (fun x -> printfn $"{x.Name}, {x.Writers.Length}, {x.Words.Length}, {x.Frequencies.Keys.Count}")

    printfn "\nWriters, Number of tracks"

    tracks
    |> List.countBy (fun x -> x.Writers.Length)
    |> List.sortBy fst
    |> List.iter (fun (w, c) -> printfn $"{w}, {c}")

    printfn ""

    printfn "\nWriters, Distinct Words, Name"

    List.sortBy (fun x -> (x.Writers.Length, x.Frequencies.Keys.Count)) tracks
    |> List.iter (fun x -> printfn $"{x.Writers.Length},\t {x.Frequencies.Keys.Count},\t\t {x.Name}")

    printfn "\nDistict, Writers, Name"

    List.sortBy (fun x -> (x.Frequencies.Keys.Count, x.Writers.Length)) tracks
    |> List.iter (fun x -> printfn $"{x.Frequencies.Keys.Count} / {x.Words.Length},\t\t {x.Writers.Length},\t {x.Name}")

    printfn "\nDistict / Total Words, Writers, Name"

    tracks
    |> List.sortBy (fun x -> x.Ratio)
    |> List.iter (fun x -> printfn $"%f{x.Ratio},\t\t {x.Writers.Length},\t {x.Name}")

    List.averageBy (fun x -> float x.Words.Length) tracks
    |> printfn "\nAverage Track Length: %f"

    List.averageBy (fun x -> float x.Frequencies.Keys.Count) tracks
    |> printfn "\nAverage Number of Distinct Words: %f"

    printfn "\nAverage Total Words, Writers"

    let points =
        List.groupBy (fun x -> x.Writers.Length) tracks
        |> List.sortBy fst
        |> List.map (fun (w, x) -> w, List.averageBy (fun y -> float y.Words.Length) x)

    for x, y in points do
        printfn "%i, %f" x y

    let x =
        List.map (fun x -> float x.Writers.Length, float x.Words.Length) tracks
        |> linreg

    printfn $"\nlinreg: \ny = {x.M} * x + {x.B}\nr = {x.R}"

    printfn "\nAverage Distinct Words, Writers"

    let points =
        List.groupBy (fun x -> x.Writers.Length) tracks
        |> List.sortBy fst
        |> List.map (fun (w, x) -> w, List.averageBy (fun y -> float y.Frequencies.Keys.Count) x)

    for x, y in points do
        printfn "%i, %f" x y

    let x =
        List.map (fun x -> float x.Writers.Length, float x.Frequencies.Keys.Count) tracks
        |> linreg

    printfn $"\nlinreg: \ny = {x.M} * x + {x.B}\nr = {x.R}"

    printfn "\nAll Words:"

    List.collect (fun x -> x.Words) tracks
    |> List.countBy id
    |> List.sortBy (fun (a, b) -> -b, a)
    |> List.iter (fun (x, y) -> printfn $"%-15s{x} {y}")

    for track in tracks do
        printfn $"\nWords of \"{track.Name}\":"

        Seq.map (fun (KeyValue(x, y)) -> x, y) track.Frequencies
        |> Seq.sortBy (fun (a, b) -> -b, a)
        |> Seq.iter (fun (x, y) -> printfn $"{x}, {y}")

let generateReport album =
    printfn $"* **\n** *\n **  Project Miss Americana\n** *\n* **\n\nStatistics for {album}"

    let tracks = fromAlbum album |> List.sortBy (fun x -> x.Index)

    tracksStats tracks

let generateDiscographyReport () =
    printfn "* **\n** *\n **  Project Miss Americana\n** *\n* **\n\nStatistics for entire discography"

    let albums = List.map fromAlbum ts
    let tracks = List.concat albums

    printfn $"{albums.Length} albums"
    printfn $"{tracks.Length} tracks"

    tracks |> tracksStats


let albums = List.map fromAlbum ts

albums |> List.map (fun x -> x.Length)

albums
|> List.iteri (fun i x -> x |> List.iter (fun t -> printfn $"{i + 1},{t.Words.Length}"))

albums
|> List.iteri (fun i x ->
    x
    |> List.iter (fun t -> printfn $"{i + 1},{float t.Frequencies.Keys.Count / float t.Words.Length}"))

albums
|> List.iteri (fun i x ->
    x
    |> List.iter (fun t -> printfn $"{i + 1},{t.Words |> List.map (fun x -> float x.Length) |> List.average}"))


List.map (fun x -> x |> List.averageBy (fun x -> float x.Words.Length)) albums
|> List.iter (printfn "%f")


List.map (fun x -> x |> List.sumBy (fun x -> float x.Words.Length)) albums
|> List.iter (printfn "%f")


// let s = DateTime.Now

// fileout "./output/stats/ALL.txt" generateDiscographyReport

// for album in ts do
//     fileout $"./output/stats/{album}.txt" (fun _ -> generateReport album)

// fileout $"./output/data.md" (fun _ ->
//     printfn "# Additional Data\n"
//     printfn "## Taylor Swift"
//     let tracks = fromAlbum tstaylorswift |> List.sortBy (fun x -> x.Index)

//     List.map (fun x -> string x.Writers.Length) tracks
//     |> String.concat ", "
//     |> printfn "\nwriters = {%s}"

//     List.sumBy (fun x -> x.Writers.Length) tracks
//     |> printfn "sum = %i"

//     let squares = List.map (fun x -> float x.Writers.Length ** 2) tracks
//     List.map string squares
//     |> String.concat ", "
//     |> printfn "squares = {%s}"
//     List.sum squares
//     |> printfn "sum of squares = %f"

//     List.map (fun x -> string x.Words.Length) tracks
//     |> String.concat ", "
//     |> printfn "\nwords = {%s}"

//     List.sumBy (fun x -> x.Words.Length) tracks
//     |> printfn "sum = %i"

//     let squares2 = List.map (fun x -> float x.Words.Length ** 2) tracks
//     List.map string squares2
//     |> String.concat ", "
//     |> printfn "squares = {%s}"
//     List.sum squares2
//     |> printfn "sum of squares = %f"
//     let xy = List.map (fun x -> x.Writers.Length * x.Words.Length) tracks
//     List.map string xy
//     |> String.concat ", "
//     |> printfn "\nxy = {%s}"

//     List.sum xy
//     |> printfn "sum xy = %i"

// )


// printfn $"Evaluated in {(DateTime.Now - s).TotalSeconds} seconds"
