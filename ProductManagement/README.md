# ProductManagement
## Produktverwaltung - Beispiel

Die Aufgabe besteht darin eine einfache, datenbankgestützte, Produktverwaltung mit einem Web-Frontend und C# Backend zu entwickeln. Es soll dabei Wert auf ein strukturiertes Vorgehen und saubere Architektur gelegt werden. Abgesehen von der Grundfunktionalität ist der weitere Funktionsumfang sowie das UI-Design zweitrangig.

Die Aufgabe soll dazu dienen die Programmiererfahrung anhand der Architektur, Design Patterns und eingesetzter Frameworks zu bewerten. Das Ergebnis darf also overengineered sein. Da die ausgeschriebene Stelle zum großen Teil die Weiterentwicklung bestehender Software beinhaltet, ist es natürlich unser Interesse, dass auf Technologien gesetzt wird, die bei uns zum Einsatz kommen (siehe dazu Technologien & Frameworks). Dies ist aber keine Voraussetzung, da die Einarbeitung in neue Technologien zum Alltag in der Programmierung gehört.

---

### Anforderungen & Rahmenbedingungen

* **Rahmenbedingungen**: Es werden durch uns keine Zugangsdaten zu einer Datenbank bereitgestellt. Es bietet sich somit eine lokal in Docker gehostete Datenbank an.
* **Abgabe**: Abzugeben ist ein oder mehrere Git-Repositories (URL oder git archive), welche den vollständigen Quellcode und eventuell eine Historie enthalten. Die notwendigen Schritte zum Kompilieren und Starten der Projekte müssen in einer einfachen README aufgeführt sein. Es sollte möglich sein die Projekte über eine CLI (PowerShell, bash, …) zu kompilieren und starten.
* **Nachbereitung**: Vorgesehen ist ein kurzes Meeting nach der Abgabe, um auf die Architektur, Besonderheiten und eventuelle Probleme oder Fehler einzugehen. Dies kann auf Wunsch auch in Textform an uns erfolgen.
* **Zeit**: Nicht immer läuft alles nach Plan. Sollte die vorgegebene Zeit nicht eingehalten werden können, so ist das nicht schlimm. Wir bitten nur um eine kurze Information diesbezüglich.

---

### Technologien & Frameworks

-   **Allgemein**
    -   Visual Studio
    -   REST / JSON
    -   Docker
    -   Aspire
-   **Frontend**
    -   Angular 
    -   Blazor
-   **Backend**
    -   C# und .NET 8.0 oder neuer (Vorgabe!)
    -   ASP.NET Core (Vorgabe!)
    -   Entity Framework Core
-   **Datenbank**
    -   Postgres

---

### Projektstruktur

Die Struktur deines Backends folgt den gewählten Architekturstilen.

### 📁 Clean Architecture
Eine klassische Clean Architecture mit klarer Trennung der Verantwortlichkeiten.

Domain\ - **Kernlogik, Entitäten, Schnittstellen**  
├── Entities\
└── Interfaces\
Application\ - **Anwendungslogik, CQRS, DTOs, Resources (.resx).**  
├── Interfaces\
├── Dtos\
├── Features\
├── Resources\
└── Validations\
Infrastructure\ - **Implementierungen von Schnittstellen aus der Domain/Application, Datenbankzugriff**  
├── Persistence\
├── Configurations\
└── Migrations\
ProductsApi\ - **Einstiegspunkt (API), Endpunkte, Dependency Injection,...**  
├── Endpoints\
├── Filters\
├── Interfaces\
└── Program.cs  
Tests\ - **NUnit-Testprojekt für Unit- und Integrationstests**

### Erweitert
Solution\
├── ApiGateway - ** YarpProxy **  
├── Aspire - ** Aspire, AppHost **  
├── Backend - ** s.o. **  
└── Frontend - ** Angular, Blazor **  

--


### Erste Schritte

1.  **Voraussetzungen installieren**
    * .NET 9 SDK und das Aspire-Workload:
       - dotnet workload install aspire
    * Das Aspire CLI Tool:
       - dotnet tool install --global aspire.cli --prerelease
    * Docker (Desktop)
    * Kiota
       - dotnet tool install --global Microsoft.OpenApi.Kiota
    
2.  **Starten des Projekts**
    * Wechsle in das Solution-Hauptverzeichnis 
    * Starte das Projekt mit dem .NET SDK (empfohlen):
       - dotnet build Aspire\AppHost
       - dotnet run --project Aspire\AppHost
    * Alternativ mit dem Aspire CLI (verwendet einen zufälligen Port):
       - aspire run

---
### Wichtige URLs

-   **Aspire Dashboard**: https://localhost:17198
-   **Products-Api**: https://localhost:7294, http://localhost:5044
-   **Angular**: http://localhost:4200
-   **Blazor**: https://localhost:7193, http://localhost:5010
-   **Grafana**: http://localhost:3000/

### Im Projekt umgesetzt:
- Verwendung von Aspire anstelle von Docker.Compose
- Validierung hinzugefügt
- Opentelemetry/Metrics hinzugefügt
- Datenbankanbindung mit EF-Core (MigrationService für die Migration der Datenbank) 
- CQRS 
- Policy hinzugefügt. (Secure by default / Secure by Design)
- Pagination bei der Query hinzugefügt
- OpenApi - Dokument (Microsoft.Extensions.ApiDescription.Server)
- Kiota-Http-Client für leichteren Zugriff auf die Api generieren lassen
- Blazor-UI hinzugefügt
- Angular-UI hinzugefügt
- Yarp-Proxy hinzugefügt
- Internationalisierung / Lokalisierung (en, de)

### Copyright und Lizenzen

-   ** dotnet-project-licenses **  
    - dotnet tool install --global dotnet-project-licenses  
    - dotnet-project-licenses --input . --export-license-texts --output-directory Licenses

### Fehlerbehandlung

-   ** Docker **  
    - docker network prune -f
