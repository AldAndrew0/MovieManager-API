# MovieManager - Guida operativa: Generic Repository + Unit of Work

Questa guida descrive come realizzare in `MovieManager.DAL`:

1. `MovieDbContext`
2. `IGenericRepository<T>` + `GenericRepository<T>`
3. `IUnitOfWork` + `UnitOfWork`

---

## 1) Prerequisiti

Nel progetto `MovieManager.DAL` aggiungere il pacchetto:

- `Microsoft.EntityFrameworkCore`

Target framework del progetto: `net10.0`.

---

## 2) DbContext

Creare il file: `MovieManager.DAL/Data/MovieDbContext.cs`

Classe: `MovieDbContext : DbContext`

Costruttore:

- `MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)`

DbSet da dichiarare:

- `DbSet<Movie> Movies`
- `DbSet<Genre> Genres`
- `DbSet<Director> Directors`
- `DbSet<Actor> Actors`
- `DbSet<MovieActor> MovieActors`
- `DbSet<Review> Reviews`

Configurazioni minime consigliate in `OnModelCreating`:

- Chiave composta su `MovieActor`: `{ MovieId, ActorId }`
- Relazione `MovieActor -> Movie` (many-to-one)
- Relazione `MovieActor -> Actor` (many-to-one)
- Lunghezza e required su campi principali (`Title`, `Name`, `FirstName`, `LastName`, `ReviewerName`)
- Vincolo score recensione: da 1 a 10

---

## 3) Interfaccia Generic Repository

Creare il file: `MovieManager.DAL/Repositories/Interfaces/IGenericRepository.cs`

Namespace: `MovieManager.DAL.Repositories.Interfaces`

Metodi richiesti:

- `Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)`
- `Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)`
- `Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)`
- `Task AddAsync(T entity, CancellationToken cancellationToken = default)`
- `void Update(T entity)`
- `void Remove(T entity)`

Nota: il repository **non** deve chiamare `SaveChanges`.

---

## 4) Implementazione Generic Repository

Creare il file: `MovieManager.DAL/Repositories/GenericRepository.cs`

Classe: `GenericRepository<T> : IGenericRepository<T> where T : class`

Campi privati:

- `MovieDbContext _context`
- `DbSet<T> _dbSet`

Regole implementative:

- In lettura usare `AsNoTracking()` dove opportuno
- `GetByIdAsync` può usare `FindAsync`
- `AddAsync` aggiunge solo al contesto
- `Update` e `Remove` cambiano lo stato dell'entità

---

## 5) Interfaccia Unit of Work

Creare il file: `MovieManager.DAL/Repositories/Interfaces/IUnitOfWork.cs`

Metodi richiesti:

- `IGenericRepository<T> Repository<T>() where T : class`
- `Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)`

La UoW deve implementare `IDisposable`.

---

## 6) Implementazione Unit of Work

Creare il file: `MovieManager.DAL/Repositories/UnitOfWork.cs`

Classe: `UnitOfWork : IUnitOfWork`

Campi suggeriti:

- `MovieDbContext _context`
- `Dictionary<Type, object> _repositories`

Comportamento:

- `Repository<T>()` restituisce un repository dalla cache o ne crea uno nuovo
- `SaveChangesAsync()` chiama `_context.SaveChangesAsync()`
- `Dispose()` libera il `DbContext`

---

## 7) Come registrare in DI (quando si passa al layer web)

In `Program.cs` del progetto web:

- Registrare il `DbContext` con il provider scelto (SQL Server, SQLite, ecc.)
- Registrare `IUnitOfWork` -> `UnitOfWork`

Esempio logico:

- `services.AddDbContext<MovieDbContext>(...)`
- `services.AddScoped<IUnitOfWork, UnitOfWork>()`

---

## 8) Verifica finale

1. Compilare la soluzione.
2. Verificare assenza di errori.
3. Controllare che il repository non faccia `SaveChanges` diretto.
4. Verificare che `SaveChangesAsync` sia centralizzato in `UnitOfWork`.

---

## 9) Entità coinvolte (ripasso rapido)

- `Movie`
- `Genre`
- `Director`
- `Actor`
- `MovieActor`
- `Review`

Queste entità devono già esistere nella cartella `MovieManager.DAL/Entities`.
