#clear
open System
open System.IO

let fileout location func =
    let d = (FileInfo location).Directory
    if not d.Exists then d.Create()

    let out = Console.Out

    try
        use ws = File.CreateText location
        Console.SetOut ws
        func ()
        printfn $"\nMiss Americana --- {DateTime.Now:s}"
    finally
        Console.SetOut out

let banner () =
    //https://textart4u.blogspot.com/2013/03/taylor-swift-ascii-text-picture.html
    printfn
        "
█▒░▓▒░░░░══░═░▒▒▒▒██▓▒░░▒▓▓▓▓▓▓█▓▓▓█████▒
█▒▒▒═══───────░░▒░▓▓▓▒▒▒▓▓▓▓▓▓▓▓▓▓██████▓
█▓▓▒░░░░░══░══▒▒▒▒▒▓▓▒▓▓████▓▓▓▓█████████
▓▓▓▒░░░░══════░▒▒▓▒▓▓▓▒▒▓▓▓███████▓▓▓██▓▓
██▓░░░░░═══════▒▒▓▒░▒▓▒▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓█▓█
██▓░░══░░══════░▒▒▒▒▒▒▓▓▓▓▓▓▓▓▓▒▒▓▓▓▓█▓▓█
██▓░═══░░═════─═▒▒▒▓▒▒▒▓▓▓▓▓▓▓▒▓▓▓▓▒▓▓▒██
█▓▒░═══════════─░▒▒▒▒▒▒▒▓▓▓▓▓▓▓▓▒▓▒▓█░▓██
█▓▒░══░══════════▒▒░▒▒▒▒▒▒▒▓▓▒▒▒▒▓▓▓░░██▓
█▓▒░═══════════──═▒▒░▒▒▒▒▒▒▒▓▒▓▒▒▓▓░░▓▓▒▒
█▓▒░═══════════───░▒▒░░▒▒▒▒▒▒▒▒▒▒▒░░▒▓▒░▒
█▓▓░═════════════──░▒▒▒░░░░░░▒▒▒░░▒▓██▓▓▒
██▓░═══░══════════─═▒▒▓▒▒▒▒▒▒▒▒▒▒▒▓▓▓▓▓▓▒
██▓░══▒▒▒░──═══════─═▒▒▓▒▒▒▒▒▒▒▒▒▒▒▒▒░▒▒▒
██▒░═░░═▒▓▒░════════─═▒▒▒▒▓▓█▓▓▓▓▒░░░▒▒▒▓
█▓▒░═══░═░▓█▓▒════════░▒▓████▓█▓▓▓▓▓▓▓▓██
██▒═══░▒▒▒▒▓█▓▒░░═════░▒▓████████████████
█▓▒══░▒▓██████▓▒▒░═══▒▒▓██████████▓▓████▓
█▓░─══▒█████████▒════░▓███▓██████▓▓▓▓███▓
█▓░─══▓██▓▓█▓▒▓▓█░───░▓▒█▒░▓█▒▓██▓▒▒▓███▓
██░─══▒▓█░░▓▒─▓▒▒▒───▒▒─▒▒▒▒▒▓▓▓▒▒▒▒▒▓██▓
██▒─═══░▓▒▒▒░░░──▒═──▒═──░░░░▒▒▒▒░▒▒▒▓██▓
▓█▒─════▒▒▒░░░═──░═─═░═─══░▒▒▒▒▒░░░▒▒▓███
██▓──═══▒▒▒▒▒░═─═░═─═░═───══░░░════▒▒▓███
███──════░░░══──═░═─═░══──────═════░▒▓███
█▓▒──══════─────═══─════───════════░▒▓███
▓▒▒═─══════────════─═════───═══════░▒▓█▓█
█═░═─════════──════──════────═════░░▒▓▓▓█
█░═░─═░════─═──═░══──════───══════░▒▒▓▓▓▓
█▒─░──░════────═░══──═░░═───════░═░▒▒▓▓▓█
██▒▓──░░════───═░══──═░▒═───═══░░░▒▒▒▓█░▓
█▒░█▒─░░░═══───░░═───═░░═─════░░░▒▒▒▒█▓▒▒
█▒░▓▒─░░░═════─══─────░░═─════░░░▒▒▒▒▓▓▒▒
█▓═▒▒─═░░═════─═░░───═▒▒═─═══░░░░▒▒▒▒█▓░█
██▒▓█─═░░░════─═▒▓░─═▒▒▒░─════░░░░▒▒▒████
█████─═░░░════──▓█▓░▒███═─════░░░░▒▒▒████
█████──░░░░════─▓██████▓──═══░░░░▒▒▒▒████
█████░─░░░═════─═▒▓██▒░═─════░░░░▒▒▒▒████
█████▒─░░░══════──═▒▒═──════░░░░░▒▒░▓██▓▓
█▓▓▓█▓─░░░════════──═───════░░░░▒▒▒░▓██▓▓
█▓▓▓██─═░░░═════──═──═░───══░░░░▒▒░▒███▓▓
█▓▓▓██░═░░░░░══──▓█▓▒██▓▒═░░░░░░▒░▒▒███▓▓
▓▒▓▓██▒═░░░░░░═░███████████▒░░░░▒▒░▓███▓▓
▒░▒▓██▓░░░░══░▓███████████▒═░░░▒░▒▒████▓▓
▒░▒▒▓█▓▒░░░░══▒██▓▓▓█▓▓▒▓▒─═░░▒▒░▒▓███▓▓▓
░░▒▒██▒▓▒░░░░═─▒▓▒▒▒▓▒░▒█▒─═░░▒░▒▒█████▓▓
░░▒▒█▓░█▓░▒░░══▒█▓░▒▒░▒██░═░░▒▒▒▒▓█████▓▓
▒░░▓█▒░██▒▒▒▒░═░██▓▓▓███▓═░░░▒▒▒▒██████▓▓
▒░░▓▓░░▓█▓▒▒▒░░░▓███████░═░░░▒▒▒▓███████▓
▒░▒▒▒═░▓█▓▒▒▒░░░░▓████▓▒─═░░░▒▒▓▓███████▓
▒▒▒▒░═▒▓█▓▒▒▒░░░═░▒▒▒░───══░░▒▓▓▓█████▓██
▒▒▒▒═░▒▒▓▓▒▓▒░░░═────────══░▒▒█▓▓█████▓▓█
▓▒═▒═▒▒▒▓▒▒██▒░═══──────═══░▒██▓▓██████▓▓
▓░═▒═▒░▒▒▒▒▓█▓▒░░════════░░▒██▓▓▓██████▓▓
▓═░▒░░═▒▒▒▒▓██▓░░═─══════░▒▓█▓▓▓▓█████▓█▓
▒░▒▒░░═▓▒▒▒▒▓██▓▒░═══░░░▒▒▓██▓▓▓▓██████▓▓
▒─▒▒░─▓█░▒▒▒▓▓██▓▒▒▒▒▒▒▒▒▓██▓▓▓▓▓▓█████▓▓
▒─░▒═▒█▓░▒▒▒▒▓▓▓██▓▓▓▓▓▓██▓▓▓▓▓▓▒▓█████▓▓
▒═▒▒░▓█▒░▒▒▒▒▒▓▓▓████████▓▓▓▓▓▓▓▒▓█████▓▓
▒─══▒██░─═░░░░▒▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▒░▓█████▓▓"

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
    member this.Ratio =
        float this.Frequencies.Keys.Count / float this.Words.Length

// Data access

let processLyrics (input: string seq) =
    let invalidChars = set [ '.'; ':'; ';'; '?'; '!'; ','; '"'; '('; ')'; '\n'; '\r'; '…' ]

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
        (stringBetween "[Writers:" encoded "]").Split ';'
        |> Seq.toList
        |> Some
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
    |> Array.where (fun x -> ((x.Name.Replace("-3am", "").Replace("-taylors-version", "").Replace("-edition", "").Replace("-deluxe", "").Replace('-', ' ').Split('_')[1]).ToLower()) = name.ToLower())
    |> Array.rev
    |> Array.collect (fun album -> album.GetFiles())
    |> Array.Parallel.map (fun song ->
        File.ReadAllText song.FullName
        |> processSong song.Name)
    |> Array.toList

let tracksStats tracks =
    printfn "\nTrack List:"

    let w =
        List.map (fun x -> x.Name.Length) tracks
        |> List.max
        |> (+) 1

    for t in tracks do
        List.sort t.Writers
        |> String.concat ", "
        |> printfn "%2i %-*s%-4i%s" t.Index w t.Name t.Writers.Length

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

    let x = linreg (List.map (fun (x, y) -> float x, y) points)

    printfn $"\ny = {x.M} * x + {x.B}\nr = {x.R}"
    printfn "\nAverage Distinct Words, Writers"

    let points =
        List.groupBy (fun x -> x.Writers.Length) tracks
        |> List.sortBy fst
        |> List.map (fun (w, x) -> w, List.averageBy (fun y -> float y.Frequencies.Keys.Count) x)

    for x, y in points do
        printfn "%i, %f" x y

    let x = linreg (List.map (fun (x, y) -> float x, y) points)

    printfn $"\ny = {x.M} * x + {x.B}\nr = {x.R}"
    printfn "\nAll Words:"
    List.collect (fun x -> x.Words) tracks
    |> List.countBy id
    |> List.sortByDescending (fun (a, b) -> b, a)
    |> List.iter (fun (x, y) -> printfn $"%-15s{x} {y}")

    for track in tracks do
        printfn $"\nWords of \"{track.Name}\":"
        Seq.map (fun (KeyValue(x, y)) -> x, y) track.Frequencies
        |> Seq.sortByDescending (fun (a, b) -> b, a)
        |> Seq.iter (fun (x, y) -> printfn $"%-15s{x} {y}")

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

let s = DateTime.Now
banner ()

fileout "./output/stats/ALL.txt" generateDiscographyReport

for album in ts do
    fileout $"./output/stats/{album}.txt" (fun _ -> generateReport album)

let f = DateTime.Now

printfn $"Evaluated in {(f - s).TotalSeconds} seconds"