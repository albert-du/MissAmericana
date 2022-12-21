open System

let stringBetween (start: string) (x: string) (finish: string) =
    let pFrom = x.IndexOf start + start.Length
    let pTo = x.LastIndexOf finish
    x.Substring(pFrom, pTo - pFrom)

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
    // let writers =
    //     parseWriters lines[0]
    //     |> function
    //         | Some x -> x
    //         | None -> failwithf "Missing writers header on '%s'" name

    let words = processLyrics lines
    let freqs = Seq.countBy id words |> Map.ofSeq

    { Name = ""
      Index = -1
      Words = List.ofSeq words
      Lyrics = text
      Frequencies = freqs
      Writers = [] }

let x = processSong "Tim McGraw" """[Writers:Liz Rose;Taylor Swift]
[Verse 1]
He said the way my blue eyes shined
Put those Georgia stars to shame that night
I said, "That's a lie"
Just a boy in a Chevy truck
That had a tendency of gettin' stuck
On backroads at night
And I was right there beside him all summer long
And then the time we woke up to find that summer gone

[Chorus]
But when you think Tim McGraw
I hope you think my favorite song
The one we danced to all night long
The moon like a spotlight on the lake
When you think happiness
I hope you think that little black dress
Think of my head on your chest
And my old faded blue jeans
When you think Tim McGraw
I hope you think of me

[Verse 2]
September saw a month of tears
And thankin' God that you weren't here
To see me like that
But in a box beneath my bed
Is a letter that you never read
From three summers back
It's hard not to find it all a little bittersweet
And lookin' back on all of that, it's nice to believe

[Chorus]
When you think Tim McGraw
I hope you think my favorite song
The one we danced to all night long
The moon like a spotlight on the lake
When you think happiness
I hope you think that little black dress
Think of my head on your chest
And my old faded blue jeans
When you think Tim McGraw
I hope you think of me

[Bridge]
And I'm back for the first time since then
I'm standin' on your street
And there's a letter left on your doorstep
And the first thing that you'll read is:
"When you think Tim McGraw
I hope you think my favorite song
Someday you'll turn your radio on
I hope it takes you back to that place"

[Chorus]
When you think happiness
I hope you think that little black dress
Think of my head on your chest
And my old faded blue jeans
When you think Tim McGraw
I hope you think of me
Oh, think of me
Mmmm

[Outro]
He said the way my blue eyes shine
Put those Georgia stars to shame that night
I said, "That's a lie"
"""

x.Words
|> String.concat ", "
|> printfn "[%s]"