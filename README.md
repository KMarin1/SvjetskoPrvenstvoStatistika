# SvjetskoPrvenstvoStatistika

A Windows desktop app in C# and Windows Forms for the 2018 men's and 2019 women's FIFA World Cup, built on a separate data layer over the public API at [worldcup-vua.nullbit.hr](https://worldcup-vua.nullbit.hr).

The window lets you pick the language (Croatian or English), the championship and your national team, and remembers the choice. The team list comes from the API and is shown as "Croatia (CRO)". The data layer also loads every match and one team's matches, builds a team's squad from its first match, and ranks a team's players by goals and yellow cards and its matches by attendance. Those results are tested but not shown in the window yet: the favourite-players, rankings and printing views of the original brief are not built. [REQUIREMENTS.md](REQUIREMENTS.md) maps each requirement of the brief to the code.

The project started as coursework for Object-Oriented Programming in .NET at Algebra. In 2026 it was refactored: moved to SDK-style .NET 10, the data layer rewritten around System.Text.Json and a shared HttpClient, error handling and localisation added, and tests and CI introduced.

## Architecture

```
SvjetskoPrvenstvoPobjedeForm   Windows Forms app (net10.0-windows)
        |
        v
PodatkovniSloj                 data layer, class library (net10.0), no UI references
        ^
        |
PodatkovniSloj.Tests           xUnit tests for the data layer
```

`PodatkovniSloj` ("data layer"):

- `IWorldCupDataSource` is what the app asks for data. `ApiService` implements it over HTTP; `JsonFileService` reads the same data from JSON files saved from the API. `DataSourceFactory` picks one from the settings.
- `Models/` holds immutable records for the API's JSON: `TeamResult`, `Match`, `Team`, `TeamStatistics`, `TeamEvent`, `Player`, `Weather`. `WorldCupJson` holds the serializer options (snake_case names, nullable annotations enforced).
- `Squad` and `Rankings` turn matches into a team's player list and its rankings.
- `Config/` holds `UserSettings` and `ConfigService`, which reads and writes them as a text file.
- Any failure to load data, whether network, timeout, HTTP status, file or malformed JSON, reaches the caller as `WorldCupDataException`.

`SvjetskoPrvenstvoPobjedeForm`:

- `Program` loads the settings, applies the language, creates the one `HttpClient` the app uses, picks the data source and opens `MainForm`.
- `MainForm` loads the teams asynchronously with a loading bar, reports errors in a message box, asks before saving and before closing, and cancels a running load when it closes.
- UI text is in `Properties/Strings.resx` (Croatian) and `Strings.en.resx` (English).

## Settings

Settings are stored per user in `%LOCALAPPDATA%\SvjetskoPrvenstvoStatistika\config.txt` as `key=value` lines. The window sets the first three; the last two are set by editing the file.

| Key | Values | Default |
|-----|--------|---------|
| `championshiptype` | `men`, `women` | `men` |
| `culture` | `hr`, `en` | `hr` |
| `representation` | FIFA code of the chosen team, e.g. `CRO` | none |
| `datasource` | `api`, `json` | `api` |
| `jsondirectory` | folder with the JSON files, relative to the app folder or absolute | `data` |

With `datasource=json`, the files are expected where the API paths would put them: `men/teams/results.json`, `men/matches.json`, and the same under `women/`.

## Running

Requires Windows and the .NET 10 SDK.

```
dotnet run --project SvjetskoPrvenstvoPobjedeForm
```

The API is public and needs no key. The solution file `SvjetskoPrvenstvoStatistika.sln` opens in any IDE that supports .NET 10.

## Building and testing

```
dotnet build
dotnet test
```

The build treats every compiler and analyzer warning as an error (`Directory.Build.props`), so a successful build has no warnings. The tests use xUnit v3 on Microsoft.Testing.Platform, which `global.json` selects for `dotnet test`. They run without network access: HTTP goes through a fake `HttpMessageHandler` that returns hand-written JSON fixtures shaped like the real responses, and file tests use temporary folders.

GitHub Actions runs the same restore, build and test steps on `windows-latest` for every push and pull request (`.github/workflows/ci.yml`).

## Tech stack

- C# 14, .NET 10
- Windows Forms
- System.Text.Json, HttpClient
- xUnit v3, Microsoft.Testing.Platform
- GitHub Actions

## License

MIT, see [LICENSE](LICENSE).
