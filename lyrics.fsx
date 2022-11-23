open System
open System.IO

let private processLyrics (input: string) =
    let invalidChars = set ['.'; ';'; '?'; '!'; ','; '"'; '\''; '('; ')'; '\n'; '\r']
    input.ToLower().Split '\n'
    |> Seq.choose (fun x -> 
        if x.StartsWith '[' || String.IsNullOrWhiteSpace x then
            None
        else
            String.filter (fun c -> Set.contains c invalidChars |> not) x
            |> fun x -> x.Split ' '
            |> Some)
    |> Seq.concat

let words (artist: string) (albums: string list) =
    let artistDirectory =
        DirectoryInfo("./data/").EnumerateDirectories()
        |> Seq.find (fun x -> x.Name.Replace('-', ' ').Equals(artist, StringComparison.InvariantCultureIgnoreCase))
    
    let albumDirectories = artistDirectory.EnumerateDirectories()

    let allWords =
        match albums with 
        | [] -> albumDirectories
        | _ ->
            let albums =
                albums
                |> List.map (fun x -> x.ToLower())
                |> set
                
            albumDirectories
            |> Seq.where (fun x -> Set.contains ((x.Name.Replace("-taylors-version", "").Replace("-edition", "").Replace("-deluxe", "").Replace('-', ' ').Split('_')[1]).ToLower()) albums)
            |> Seq.cache
        |> Seq.collect (fun album -> album.EnumerateFiles())
        |> Seq.collect (fun song -> File.ReadAllText song.FullName |> processLyrics)
        |> Seq.cache
    
    let total = Seq.length allWords
    
    allWords
    |> Seq.countBy id
    |> Seq.sortByDescending snd
    |> Seq.map (fun (x,y) -> (x, float y / float total))