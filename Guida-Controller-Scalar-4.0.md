# Guida Controller API + Scalar in .NET 10 (MovieManager)

Questa guida spiega:

1. Come è stato configurato **Scalar** nel progetto.
2. Come creare un nuovo controller API nello stesso stile del codice esistente.
3. Come documentazione OpenAPI e UI Scalar leggono i decoratori/attributi.

---

## 1) Configurazione Scalar (già applicata)

Nel progetto API è stata configurata la pipeline moderna per `.NET 10`:

- OpenAPI nativo (`Microsoft.AspNetCore.OpenApi`)
- UI interattiva (`Scalar.AspNetCore`)

### 1.1 Package NuGet

File: `MovieManager.PL.API/MovieManager.PL.API.csproj`

- `Microsoft.AspNetCore.OpenApi`
- `Scalar.AspNetCore`

### 1.2 Program.cs

File: `MovieManager.PL.API/Program.cs`

- `builder.Services.AddOpenApi();`
- `app.MapOpenApi();`
- `app.MapScalarApiReference();`

Questo espone:

- Documento OpenAPI JSON: `/openapi/v1.json`
- UI interattiva Scalar: `/scalar`

### 1.3 Startup su HTTPS

File: `MovieManager.PL.API/Properties/launchSettings.json`

Nel profilo `https`:

- `"launchBrowser": true`
- `"launchUrl": "scalar"`

Quindi in avvio si apre direttamente la pagina Scalar.

---

## 2) Come funziona la documentazione (OpenAPI + Scalar)

Scalar non legge i controller direttamente: legge il documento OpenAPI generato dall'app.

Regola pratica:

- Se un attributo/decoratore finisce nel JSON OpenAPI, lo vedrai anche in Scalar.

Attributi utili (già usati nel progetto):

- `[ApiController]`
- `[Route("api/[controller]")]`
- `[Produces("application/json")]`
- `[ProducesResponseType(...)]`
- `[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]`

Non servono decoratori “specifici Scalar” per i controller.

---

## 3) Pattern controller usato nel progetto

I controller esistenti (`MoviesController`, `GenresController`, `DirectorsController`, `ReviewsController`, `ActorsController`) seguono tutti lo stesso schema:

1. Iniezione di un service (`IGenericService<TModel>`)
2. Endpoint CRUD async
3. `CancellationToken` su ogni endpoint
4. Response HTTP coerenti (200/201/204/400/404)
5. Metadata OpenAPI tramite `ProducesResponseType`

Per casi speciali (chiavi composte / logica custom), pattern dedicato come `MovieActorsController` + `IMovieActorService`.

---

## 4) Procedura passo-passo: creare un nuovo controller

Esempio su entità esistente: `Movie` (controller reale: `MoviesController`).

### Step 1 — Model

Verificare il model in BLL: `MovieModel`.

### Step 2 — Service

Per CRUD standard usare il generic service già registrato in `Program.cs`:

`builder.Services.AddScoped<IGenericService<MovieModel>, GenericService<Movie, MovieModel>>();`

Se in futuro servono regole business particolari, valutare un service dedicato.

### Step 3 — Controller

Controller esistente: `MoviesController` in `MovieManager.PL.API/Controllers` con:

- `[ApiController]`
- `[Route("api/[controller]")]`
- `[Produces("application/json")]`
- costruttore con `IGenericService<MovieModel>`

### Step 4 — Endpoint consigliati (già presenti)

- `GET /api/movies`
- `GET /api/movies/{id}`
- `POST /api/movies`
- `PUT /api/movies/{id}`
- `DELETE /api/movies/{id}`

### Step 5 — Contratti e status code

Per ogni endpoint aggiungere i `ProducesResponseType` coerenti.

Esempi tipici:

- GET list: `200`
- GET by id: `200`, `404`
- POST: `201`, `400`
- PUT: `204`, `400`, `404`
- DELETE: `204`, `404`

### Step 6 — Validazioni base controller

Per `PUT` verificare coerenza route/body:

- se `id` route diverso da `model.Id` → `BadRequest`

### Step 7 — Verifica in Scalar

Avvia il profilo HTTPS e apri `/scalar`:

- verifica presenza endpoint
- prova le chiamate con “Try it out”

---

## 5) Best practice adottate nel codice corrente

- Controller sottili: logica business nei service.
- Async + `CancellationToken` ovunque.
- Risposte HTTP consistenti e prevedibili.
- OpenAPI nativo .NET 10, senza dipendere da Swashbuckle.

---

## 6) Troubleshooting rapido

### Non vedo endpoint in Scalar

Controllare che:

- ambiente sia `Development`
- in `Program.cs` ci siano `app.MapOpenApi();` e `app.MapScalarApiReference();`
- il controller abbia route corretta e sia nel progetto API

### La UI non si apre in startup

Controllare `launchSettings.json` profilo `https`:

- `launchBrowser = true`
- `launchUrl = scalar`

### Modifiche non riflesse durante debug

Se il debugger era attivo durante le modifiche, riavviare l'app (o usare hot reload se sufficiente).

---

## 7) Endpoint utili del progetto

- UI Scalar: `https://localhost:7182/scalar`
- OpenAPI JSON: `https://localhost:7182/openapi/v1.json`

(Le porte dipendono dal `launchSettings.json` attivo.)
