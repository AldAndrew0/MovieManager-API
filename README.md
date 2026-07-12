# Movie Manager API 🎬

Un'architettura software N-Tier (PL, BLL, DAL) per la gestione di un catalogo cinematografico, sviluppata in .NET 10 e C# con Entity Framework Core.

## Struttura del Progetto
La soluzione è divisa in 4 layer:
* **MovieManager.DAL (Data Access Layer):** Contiene i modelli EF, il DbContext, le migrazioni e il Generic Repository.
* **MovieManager.BLL (Business Logic Layer):** Contiene i modelli di dominio, le interfacce e i Generic Service, usando AutoMapper per la conversione.
* **MovieManager.PL.API (Presentation Layer):** L'API RESTful (Controller) che espone gli endpoint JSON.
* **MovieManager.PL.MVC (Presentation Layer):** L'interfaccia grafica utente (se configurata come progetto di avvio).

## Come avviare il progetto in locale

### 1. Prerequisiti
* .NET 10 SDK
* SQL Server (es. SQL Express o LocalDB)
* Visual Studio 2022

### 2. Configurazione Database
Il progetto non include il database fisico. Per crearlo sul tuo server locale, segui questi passi:

1. Apri il file `appsettings.json` all'interno del progetto **MovieManager.PL.API**.
2. Modifica la stringa di connessione `MovieDbString` inserendo il nome del tuo Server SQL locale (es. `Server=IL-TUO-SERVER\\SQLEXPRESS;...`).
3. Apri la **Console di Gestione Pacchetti** (Package Manager Console) in Visual Studio.
4. Assicurati che il "Progetto predefinito" a tendina sia impostato su **MovieManager.DAL**.
5. Lancia il comando per costruire il database:
   ```powershell
   Update-Database -Project MovieManager.DAL -StartupProject MovieManager.PL.API