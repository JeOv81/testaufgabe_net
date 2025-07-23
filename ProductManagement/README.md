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
-   **Frontend**
    -   Angular (bevorzugt)
-   **Backend**
    -   C# und .NET 8.0 oder neuer (Vorgabe!)
    -   ASP.NET Core (Vorgabe!)
    -   Entity Framework Core (bevorzugt)
-   **Datenbank**
    -   Postgres
    -   Redis

---

### Projektstruktur

Die Struktur deines Backends folgt den gewählten Architekturstilen.

### 📁 Clean Architecture
Eine klassische Clean Architecture mit klarer Trennung der Verantwortlichkeiten.

Domain\ - **Kernlogik, Entitäten, Schnittstellen und Value Objects.**

├── Entities\
└── Interfaces\
Application\ - **Anwendungslogik, Use Cases, DTOs und Service-Schnittstellen.**

├── Interfaces\
├── Dtos\
└── Services\
Infrastructure\ - **Implementierungen von Schnittstellen aus der Domain/Application, Datenbankzugriff, externe Services.**

├── Persistence\
├── Repositories\
└── Services\
ProductsApi\ - **Einstiegspunkt (API), Endpunkte (Controller), Dependency Injection.**

├── Endpoints\
└── Program.cs
Tests\ - **NUnit-Testprojekt für Unit- und Integrationstests.**

--


### Erste Schritte

1.  **Voraussetzungen installieren**
    * .NET 9 SDK und das Aspire-Workload:
            dotnet workload install aspire
    * Das Aspire CLI Tool:
            dotnet tool install --global aspire.cli --prerelease
    * Docker (Desktop)
    
2.  **Starten des Projekts**
    * Wechsle in das Solution-Hauptverzeichnis (cd ProductManagement).
    * Starte das Projekt mit dem .NET SDK (empfohlen):
        - dotnet build Aspire\AppHost
        - dotnet run --project Aspire\AppHost
    * Alternativ mit dem Aspire CLI (verwendet einen zufälligen Port):
        - aspire run

---
### Wichtige URLs

-   **Aspire Dashboard**: https://localhost:17198
-   **Products-Api**: https://localhost:7294, http://localhost:5044


### Was wurde gemacht:
- Validierung hinzugefügt
- Opentelemetry/Metrics hinzugefügt
- Datenbankanbindung mit EF-Core (MigrationService für die Migration der Datenbank) / verwendete Datenbank: Postgres
- CQRS 
- Policy zum absichern der Endpunkte. Diese sollten secure by default sein, damit nicht schlimmes passieren kann.
- Verwendung von Aspire anstelle von Docker.Compose
- Pagination bei der Query hinzugefügt