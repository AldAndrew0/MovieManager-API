# Istruzioni: creazione entità DAL (MovieManager)

Obiettivo: creare nella cartella `MovieManager.DAL/Entities` le classi necessarie per un catalogo film.

## 1) Struttura del progetto

- Progetto: `MovieManager.DAL`
- Cartella: `Entities`
- Namespace da usare in ogni file: `MovieManager.DAL.Entities`

---

## 2) Entità da creare

Creare un file `.cs` per ciascuna entità:

- `Genre.cs`
- `Director.cs`
- `Actor.cs`
- `Movie.cs`
- `MovieActor.cs`
- `Review.cs`

---

## 3) Proprietà richieste per ogni entità

## Genre

- `int Id`
- `string Name`
- `string? Description`
- `ICollection<Movie> Movies`

## Director

- `int Id`
- `string FirstName`
- `string LastName`
- `DateOnly? BirthDate`
- `string? Country`
- `string? Biography`
- `ICollection<Movie> Movies`

## Actor

- `int Id`
- `string FirstName`
- `string LastName`
- `DateOnly? BirthDate`
- `string? Country`
- `string? Biography`
- `ICollection<MovieActor> MovieActors`

## Movie

- `int Id`
- `string Title`
- `string? OriginalTitle`
- `DateOnly ReleaseDate`
- `int DurationMinutes`
- `string? Synopsis`
- `string Language`
- `string? Country`
- `decimal? Budget`
- `decimal? Revenue`
- `string? PosterUrl`
- `string? AgeRating`
- `int GenreId`
- `Genre Genre`
- `int DirectorId`
- `Director Director`
- `ICollection<MovieActor> MovieActors`
- `ICollection<Review> Reviews`

## MovieActor

- `int MovieId`
- `Movie Movie`
- `int ActorId`
- `Actor Actor`
- `string? CharacterName`
- `bool IsLeadRole`
- `int DisplayOrder`

## Review

- `int Id`
- `int MovieId`
- `Movie Movie`
- `string ReviewerName`
- `int Score`
- `string? Comment`
- `DateTime CreatedAt`

---

## 4) Note importanti

- Usare `string.Empty` come valore iniziale per le stringhe non nullable.
- Usare `null!` per le proprietà di navigazione non nullable (`Genre`, `Director`, `Movie`, `Actor`) quando richiesto.
- Inizializzare le collezioni con `new List<T>()`.
- Impostare in `Review` il valore iniziale di `CreatedAt` a `DateTime.UtcNow`.

---

## 5) Verifica finale

Dopo aver creato tutte le classi:

1. Salvare i file.
2. Compilare la solution.
3. Verificare che non ci siano errori di tipo o namespace.
