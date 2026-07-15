# MovieManager - Guida completa BLL (dettagliata)

Questa guida spiega **passo per passo** come è stato costruito il BLL nel progetto, perché sono state fatte certe scelte e cosa deve contenere ogni parte.

---

## 1) Obiettivo del BLL

Il Business Logic Layer (BLL) è il livello intermedio tra API (PL) e DAL.

Responsabilità principali:

1. esporre operazioni applicative (CRUD nel caso base)
2. usare repository/unit of work senza esporli direttamente al controller
3. lavorare con **modelli BLL** invece che con entità EF del DAL
4. centralizzare la logica comune in un unico servizio generico

Nel progetto attuale la struttura è:

- `IGenericService<TModel>`: contratto
- `GenericService<TEntity, TModel>`: implementazione

---

## 2) Pacchetti necessari e dove installarli

## 2.1 BLL

Nel progetto `MovieManager.BLL` è richiesto:

- `AutoMapper` (v14.0.0)

Perché serve: converte facilmente `Entity <-> Model`.

## 2.2 API (PL)

Nel progetto `MovieManager.PL.API` è richiesto:

- `Microsoft.EntityFrameworkCore.SqlServer`

Perché serve: collega `DbContext` a SQL Server con `UseSqlServer(...)`.

---

## 3) Cosa devono contenere i Model BLL

I model sono classi in `MovieManager.BLL/Models` e rappresentano i dati usati dal livello applicativo.

Modelli presenti:

- `ActorModel`
- `DirectorModel`
- `GenreModel`
- `MovieModel`
- `ReviewModel`
- `MovieActorModel`

Nota architetturale: `MovieActorModel` ha chiave composta (`MovieId`, `ActorId`) e non implementa `IModelWithId`, quindi non è gestito dal `GenericService` CRUD basato su id singolo.

Stato attuale del progetto: la problematica è stata risolta con un servizio dedicato `IMovieActorService` / `MovieActorService`, che usa metodi basati sulla coppia di chiavi (`MovieId`, `ActorId`).

Regole pratiche usate nel progetto:

1. proprietà coerenti con l’entità DAL corrispondente
2. proprietà stringa non nullable inizializzate con `string.Empty`
3. proprietà opzionali dichiarate nullable (`string?`, `DateOnly?`, ecc.)
4. i model usati dal `GenericService` devono implementare `IModelWithId` (quindi avere `Id` di tipo `int`)

### 3.1 Perché esiste `IModelWithId`

`IModelWithId` è un vincolo di tipo usato per rendere il generic service sicuro a compile-time.

Vantaggi:

- evita reflection per leggere la proprietà `Id`
- evita errori runtime (il compilatore garantisce che `Id` esista)
- rende chiaro che il generic CRUD è pensato per entità a chiave singola

In sintesi: `IModelWithId` non è ridondante, ma è la regola che permette a `GenericService` di funzionare in modo robusto e prevedibile.

Differenza model BLL vs DTO (in questo progetto):

- qui **non** si usano DTO separati
- il model BLL viene usato direttamente come contratto del servizio

---

## 4) Interfaccia del servizio generico

File: `MovieManager.BLL/Services/Interfaces/IGenericService.cs`

Definisce metodi asincroni:

- `Task<TModel?> GetByIdAsync(int id, ...)`
- `Task<IReadOnlyList<TModel>> GetAllAsync(...)`
- `Task<TModel> CreateAsync(TModel model, ...)`
- `Task<bool> UpdateAsync(TModel model, ...)`
- `Task<bool> DeleteAsync(int id, ...)`

Significato del ritorno `bool` su update/delete:

- `true`: operazione eseguita
- `false`: record non trovato

---

## 5) Implementazione di GenericService

File: `MovieManager.BLL/Services/GenericService.cs`

Classe:

- `GenericService<TEntity, TModel> : IGenericService<TModel>`

Vincoli generici usati:

- `where TEntity : class, new()`
- `where TModel : class, IModelWithId, new()`

Significato pratico dei vincoli:

1. `where TEntity : class, new()`
   - `class`: `TEntity` deve essere reference type (coerente con entità EF)
   - `new()`: `TEntity` deve avere costruttore vuoto pubblico

2. `where TModel : class, IModelWithId, new()`
   - `class`: `TModel` è un tipo reference
   - `IModelWithId`: il model deve esporre `Id:int`
   - `new()`: il model deve avere costruttore vuoto pubblico

Questa combinazione permette al service di usare `model.Id` in sicurezza e mantenere un contratto coerente.

Dipendenze iniettate nel costruttore:

- `IUnitOfWork _unitOfWork`
- `IGenericRepository<TEntity> _repository` (ottenuto da `_unitOfWork.Repository<TEntity>()`)
- `IMapper _mapper`

### 5.1 Cosa fa ogni metodo

## GetByIdAsync

1. legge entità da repository tramite id
2. se null ritorna null
3. altrimenti mappa in `TModel`

## GetAllAsync

1. legge tutte le entità
2. mappa la lista in `IReadOnlyList<TModel>`

## CreateAsync

1. mappa `TModel -> TEntity`
2. `AddAsync` su repository
3. `SaveChangesAsync` su unit of work
4. ritorna il model mappato dall’entità salvata

## UpdateAsync

1. legge l’id direttamente da `model.Id` (vincolo `IModelWithId`)
2. carica l’entità esistente
3. se non esiste ritorna `false`
4. mappa i nuovi valori sul record esistente
5. `Update` + `SaveChangesAsync`
6. ritorna `true`

## DeleteAsync

1. carica entità per id
2. se non esiste ritorna `false`
3. `Remove` + `SaveChangesAsync`
4. ritorna `true`

### 5.2 Gestione della chiave composita (`MovieActorModel`)

Problema: `GenericService` è progettato per chiavi singole (`Id:int`), mentre `MovieActor` usa chiave composta (`MovieId`, `ActorId`).

Per questo `MovieActorModel` non deve implementare `IModelWithId` e non deve essere registrato con `IGenericService<TModel>`.

Soluzione architetturale applicata nel progetto:

1. nel DAL è stato introdotto un repository dedicato alla chiave composta:
   - `IMovieActorRepository`
   - `MovieActorRepository`
2. il repository dedicato espone metodi coerenti con la PK composta:
   - `GetByIdsAsync(int movieId, int actorId)`
   - `GetByMovieIdAsync(int movieId)`
   - `ExistsAsync(int movieId, int actorId)`
3. nel BLL è stato introdotto il servizio dedicato:
   - `IMovieActorService`
   - `MovieActorService`
4. `MovieActorService` usa `IMovieActorRepository` + `IUnitOfWork`:
   - repository dedicato per la ricerca con doppia chiave
   - unit of work per il commit (`SaveChangesAsync`)
5. nel `Program.cs` sono registrati entrambi:
   - `AddScoped<IMovieActorRepository, MovieActorRepository>()`
   - `AddScoped<IMovieActorService, MovieActorService>()`

Perché è meglio così:

- non si forza un `Id` artificiale
- non si piega `IGenericRepository` (basato su `GetByIdAsync(int)`) a un caso che non gli appartiene
- si mantiene separazione pulita tra CRUD a chiave singola e CRUD a chiave composta

### 5.3 Perché SaveChanges non è nel repository

Scelta architetturale corretta:

- repository: query e manipolazione entità
- unit of work: commit transazionale (`SaveChangesAsync`)

### 5.4 Allineamento firme tra Repository e Service (refactoring consigliato)

Domanda tipica: nel repository alcuni metodi sono `void` (`Update`, `Remove`), mentre nel service sono `bool` (`UpdateAsync`, `DeleteAsync`). È un errore?

Risposta: **no, è corretto**.

- nel repository `Update/Remove` sono comandi sul change tracker EF: non indicano “trovato/non trovato”
- nel service il `bool` rappresenta una regola applicativa: prima controllo se il record esiste, poi eseguo update/delete

In pratica:

1. il repository applica la modifica allo stato dell’entità
2. il service decide il risultato funzionale (`true/false`)
3. la Unit of Work salva con `SaveChangesAsync`

#### Quando fare refactoring

Conviene refactorare solo se vuoi una semantica più esplicita del semplice `bool`.

Opzioni consigliate:

1. usare un `OperationResult` (Success, NotFound, ValidationError, ecc.)
2. separare i metodi “Try” (`TryUpdateAsync`, `TryDeleteAsync`) da quelli che lanciano eccezione
3. mantenere `void` nel repository e migliorare solo il contratto service

#### Procedura di refactoring (step-by-step)

1. creare un tipo risultato applicativo (es. `ServiceResult`)
2. aggiornare `IGenericService` sostituendo `bool` con `ServiceResult`
3. aggiornare `GenericService` con i nuovi ritorni (`NotFound`, `Success`)
4. aggiornare i controller per mappare il risultato in HTTP status (`404`, `200`, `400`)
5. lasciare invariato `IGenericRepository` (a meno di esigenze specifiche)

Conclusione: non serve modificare `GenericRepository` solo perché il service ritorna `bool`; il disallineamento è intenzionale e architetturalmente corretto.

---

## 6) Come scrivere il MappingProfile (passo per passo)

File: `MovieManager.PL.API/Configurations/MappingProfile.cs`

### 6.1 Struttura base

1. creare classe `MappingProfile`
2. ereditare da `Profile` (AutoMapper)
3. nel costruttore definire le mappe con `CreateMap<,>()`

### 6.2 Mappe del progetto

- `CreateMap<Actor, ActorModel>().ReverseMap();`
- `CreateMap<Director, DirectorModel>().ReverseMap();`
- `CreateMap<Genre, GenreModel>().ReverseMap();`
- `CreateMap<Movie, MovieModel>().ReverseMap();`
- `CreateMap<Review, ReviewModel>().ReverseMap();`
- `CreateMap<MovieActor, MovieActorModel>().ReverseMap();`

### 6.3 Quando usare ReverseMap

`ReverseMap()` serve quando la conversione deve funzionare in entrambe le direzioni:

- entity -> model (lettura)
- model -> entity (create/update)

### 6.4 Mapping model -> entity e entity -> model (come avviene nel service)

Nel progetto il mapping viene usato in due direzioni principali.

## Entity -> Model

Usato nelle letture (es. `GetByIdAsync`, `GetAllAsync`):

- `_mapper.Map<TModel>(entity)`
- `_mapper.Map<IReadOnlyList<TModel>>(entities)`

Significato: prendo dati dal DAL e li converto nel formato usato dal BLL/API.

## Model -> Entity

Usato nelle scritture (es. `CreateAsync`):

- `_mapper.Map<TEntity>(model)`

Significato: prendo il model ricevuto dal layer applicativo e lo converto in entità persistibile dal repository.

## Model -> Entity esistente (update)

Usato negli aggiornamenti (`UpdateAsync`):

- `_mapper.Map(model, existingEntity)`

Significato: non creo una nuova entità, ma aggiorno quella già caricata dal database.

### 6.5 Metodi AutoMapper più usati e differenze

1. `Map<TDestination>(source)`
   - crea un nuovo oggetto `TDestination`
   - utile per letture o create

2. `Map(source, destination)`
   - aggiorna un oggetto già esistente
   - utile per update su entità tracciata da EF

3. `ReverseMap()` nella configurazione
   - evita di scrivere due mappe separate
   - abilita mapping in entrambe le direzioni

4. `ForMember(...)` (quando serve)
   - personalizza mapping di campi specifici
   - utile quando nomi o logica non coincidono 1:1

### 6.6 Cosa succede se manca una mappa

Se richiami `_mapper.Map<...>(...)` senza configurazione coerente in `MappingProfile`, AutoMapper solleva errore runtime di configurazione/mapping.

Per questo ogni coppia `Entity <-> Model` usata dal servizio deve essere configurata.

---

## 7) Registrazioni nel Program.cs e significato degli scope

File: `MovieManager.PL.API/Program.cs`

Ordine logico delle registrazioni:

1. `AddDbContext<MovieDbContext>(...)`
2. `AddScoped<IUnitOfWork, UnitOfWork>()`
3. `AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))`
4. registrazioni chiuse del GenericService:
   - `IGenericService<ActorModel> -> GenericService<Actor, ActorModel>`
   - `IGenericService<DirectorModel> -> GenericService<Director, DirectorModel>`
   - `IGenericService<GenreModel> -> GenericService<Genre, GenreModel>`
   - `IGenericService<MovieModel> -> GenericService<Movie, MovieModel>`
   - `IGenericService<ReviewModel> -> GenericService<Review, ReviewModel>`
5. repository e servizio dedicati per chiave composta:
   - `IMovieActorRepository -> MovieActorRepository`
   - `IMovieActorService -> MovieActorService`
6. `AddAutoMapper(typeof(Program).Assembly)`

### 7.1 Differenza tra AddScoped, AddTransient, AddSingleton

## AddScoped

- una sola istanza per richiesta HTTP
- ideale per servizi che usano `DbContext`
- nel progetto è la scelta corretta per repository/unitofwork/service

## AddTransient

- nuova istanza ogni volta che viene richiesta
- utile per servizi stateless leggeri
- meno adatta quando dipende da oggetti con lifecycle per-request

## AddSingleton

- unica istanza per tutta l’app
- non adatta per servizi legati a `DbContext`

Per questo nel progetto si usa **Scoped**.

### 7.2 AddAutoMapper

`AddAutoMapper(typeof(Program).Assembly)` dice ad AutoMapper di cercare i profili nell’assembly API, dove si trova `Configurations/MappingProfile.cs`.

---

## 8) Mini guida: Task, async e await

Nel progetto quasi tutti i metodi del service/repository sono asincroni. Questo è importante perché l’accesso a database è un’operazione I/O che può richiedere tempo.

### 8.1 Cos’è `Task`

`Task` rappresenta un’operazione che terminerà in futuro.

- `Task` = operazione senza valore di ritorno
- `Task<T>` = operazione che restituirà un valore `T`

Esempi presenti nel progetto:

- `Task<IReadOnlyList<TModel>> GetAllAsync(...)`
- `Task<bool> DeleteAsync(...)`

### 8.2 Cosa fa `async`

`async` indica che un metodo può usare `await` e che il suo risultato verrà impacchettato in un `Task`/`Task<T>`.

### 8.3 Cosa fa `await`

`await` aspetta in modo non bloccante il completamento di un `Task`.

- il thread non rimane occupato inutilmente durante l’attesa I/O
- quando l’operazione finisce, l’esecuzione riprende dal punto successivo

### 8.4 Differenza tra programmazione sincrona e asincrona

## Versione sincrona (concetto)

- una chiamata al DB blocca il thread fino al completamento
- con molte richieste concorrenti si saturano più facilmente i thread
- peggiora scalabilità e throughput lato server

## Versione asincrona (concetto)

- la chiamata al DB libera il thread durante l’attesa
- il thread può servire altre richieste
- migliore scalabilità in applicazioni web/API

### 8.5 Esempio pratico semplificato

Operazione “recupera tutti i film”.

- Sincrono: `var movies = repository.GetAll();`  
  Il thread resta bloccato fino al ritorno dei dati.

- Asincrono: `var movies = await repository.GetAllAsync();`  
  Il thread non resta bloccato nell’attesa I/O.

Nel progetto si usa l’approccio asincrono (`GetAllAsync`, `SaveChangesAsync`, ecc.) proprio per questo motivo.

---

## 9) Esempio di uso in un controller

Iniezione tipica:

- `IGenericService<MovieModel>` per operazioni sui film

Flusso tipico endpoint:

1. chiamata al metodo service (`GetAllAsync`, `CreateAsync`, ecc.)
2. gestione esito (`null` / `false` / risultato)
3. ritorno `ActionResult`

---

## 10) Errori comuni da evitare

1. dimenticare una mappa nel `MappingProfile`
2. usare `Singleton` con servizi che dipendono dal `DbContext`
3. non chiamare `SaveChangesAsync` nel service
4. usare `GenericService` con model che non implementano `IModelWithId`
5. registrare AutoMapper senza includere l’assembly dove sta il profilo

---

## 11) Checklist finale

1. `MovieManager.BLL` referenzia `MovieManager.DAL`.
2. pacchetto `AutoMapper` presente nel BLL.
3. model BLL creati e coerenti.
4. `IGenericService<TModel>` e `GenericService<TEntity, TModel>` presenti.
5. `MappingProfile` in `MovieManager.PL.API/Configurations`.
6. registrazioni DI in `Program.cs` presenti e con scope `Scoped`.
7. build soluzione senza errori.
