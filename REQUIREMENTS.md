# Requirements

This is the project brief for Object-Oriented Programming in .NET (OOP.NET) at Algebra, the brief this project was built against, paraphrased requirement by requirement. The submission and grading rules are left out because they are not about the software.

Each row is marked Met, Partly met or Not met. Paths are relative to the repository root.

| # | Part | Requirement | Status | Where it is implemented, or why not |
|---|------|-------------|--------|-------------------------------------|
| 1 | Solution | Three projects: a Windows Forms app, a WPF app, and a class library ("Podatkovni Sloj") that both apps use. | Partly met | `SvjetskoPrvenstvoPobjedeForm/` and `PodatkovniSloj/` exist; this repository has no WPF project. |
| 2 | Solution | Show statistics of the 2018 men's and 2019 women's World Cup from the API at worldcup-vua.nullbit.hr. | Met | `PodatkovniSloj/ApiService.cs` (`DefaultBaseAddress`), `PodatkovniSloj/Championship.cs` |
| 3 | Data layer | All data handling happens in the data layer, which both apps use. | Met | `PodatkovniSloj/`: loading, mapping, squad, rankings and settings; `MainForm` only binds the results. |
| 4 | Data layer | Read team results from `/men/teams/results` and `/women/teams/results` (FIFA code, wins, losses and so on). | Met | `ApiService.GetTeamsAsync`, `PodatkovniSloj/Models/TeamResult.cs` |
| 5 | Data layer | Read full match details from `/men/matches` and `/women/matches`. | Met | `ApiService.GetMatchesAsync`, `PodatkovniSloj/Models/Match.cs` and the other model records |
| 6 | Data layer | Read one team's matches from `/matches/country?fifa_code=XXX`. | Met | `ApiService.GetTeamMatchesAsync` |
| 7 | Data layer | Parse and map the fetched data. | Met | `PodatkovniSloj/WorldCupJson.cs`, `PodatkovniSloj/Models/` |
| 8 | Data layer | Prepare the data for the client apps. | Met | `PodatkovniSloj/Squad.cs`, `PodatkovniSloj/Rankings.cs`, `TeamResult.DisplayName` |
| 9 | Data layer | Save data to text files. | Met | `ConfigService.Save` in `PodatkovniSloj/Config/ConfigService.cs` |
| 10 | Data layer | Read data from text files. | Met | `ConfigService.Load` |
| 11 | Data layer | Load the same data from JSON files downloaded from the API, as an alternative to the API. | Met | `PodatkovniSloj/JsonFileService.cs`; files go in folders named like the API paths (`men/teams/results.json`, `men/matches.json`). |
| 12 | Data layer | A configuration file chooses API or JSON files, and the data layer checks that choice when it loads data. | Met | `datasource` and `jsondirectory` keys in `ConfigService`, `PodatkovniSloj/DataSourceFactory.cs` |
| 13 | Client apps | Handle every possible exception; the app must not crash after start-up and shows a message instead (for example when the API returns an error). | Met | `MainForm.LoadTeamsAsync`, `MainForm.btnSubmit_Click`, `Program.Main` (`ThreadException` handler); data errors arrive as `WorldCupDataException`. |
| 14 | Client apps | No hard-coded paths to resources; use relative paths, so the app runs on any machine with .NET without code changes. | Met | `ConfigService.DefaultFilePath` (per-user application data folder), `DataSourceFactory` (JSON folder relative to the app folder) |
| 15 | Windows Forms | Fetch API data asynchronously; the main thread only draws and does simple work. | Met | `MainForm.LoadTeamsAsync` awaits `IWorldCupDataSource`; the library uses `ConfigureAwait(false)`. |
| 16 | Windows Forms | Show a loading animation while waiting for a result. | Met | `pbLoading` (marquee progress bar) and `MainForm.SetLoading` |
| 17 | Windows Forms | At start-up, a form for choosing the championship (men or women) and the language. | Met | `ddlChampionshipType` and `ddlCulture` in `SvjetskoPrvenstvoPobjedeForm/MainForm.cs` |
| 18 | Windows Forms | The language choice localises the app; Croatian and English are offered. | Met | `SvjetskoPrvenstvoPobjedeForm/Properties/Strings.resx` and `Strings.en.resx`, `UiCulture.Apply`, `MainForm.ApplyTexts` |
| 19 | Windows Forms | Save the championship and language to a text file and use them on every later start. | Met | `ConfigService`, `Program.LoadSettings` |
| 20 | Windows Forms | If the settings file does not exist, ask the user for championship and language again. | Partly met | A missing file gives the defaults in the same window, where the user changes and saves them; there is no separate first-run prompt. |
| 21 | Windows Forms | Choose the favourite national team from a ComboBox filled from the API. | Met | `cbRepresentation`, `MainForm.LoadTeamsAsync` |
| 22 | Windows Forms | List the teams as "NAME (FIFA_CODE)". | Met | `TeamResult.DisplayName`, bound as `cbRepresentation.DisplayMember` |
| 23 | Windows Forms | Save the favourite team to a text file and use it on later starts. | Met | `UserSettings.Representation`, `representation` key in `ConfigService` |
| 24 | Windows Forms | Choose three favourite players; all players are the starting eleven plus the substitutes of the team's first match. | Partly met | The player list is computed by `Squad.FromFirstMatch`; the window has no player selection. |
| 25 | Windows Forms | Show each player in a user control with name, shirt number, position, captain flag and a star for favourites. | Not met | Needs the favourite-players view, which is not built. |
| 26 | Windows Forms | Favourites in one panel and the other players in another; move players by context menu and by drag and drop, several at once. | Not met | Needs the favourite-players view, which is not built. |
| 27 | Windows Forms | Save the favourite players to a text file and use them on later starts. | Not met | Needs the favourite-players view, which is not built. |
| 28 | Windows Forms | Let the user set a picture for each player, stored inside the solution for both apps, with a default picture when none is set. | Not met | Needs the player user control, which is not built. |
| 29 | Windows Forms | Rank the chosen team's players by goals and by yellow cards, with full name, picture and count. | Partly met | `Rankings.Goals` and `Rankings.YellowCards` compute the lists; no window shows them and there are no pictures. |
| 30 | Windows Forms | Rank the chosen team's matches by attendance, with location, attendance, home team and away team. | Partly met | `Rankings.Attendance` computes the list; no window shows it. |
| 31 | Windows Forms | Print the rankings on a chosen printer (a PDF is enough), with every row visible. | Not met | Needs the rankings view, which is not built. |
| 32 | Windows Forms | Change the championship and language later through the settings form and save the confirmed choice to a text file. | Met | `MainForm.btnSubmit_Click`, `ConfigService.Save`; the main window is the settings form. |
| 33 | Windows Forms | Ask for confirmation before applying new settings; Enter confirms and Esc cancels. | Met | `MainForm.Confirm` (OK is the default button, Esc maps to Cancel), `AcceptButton = btnSubmit` |
| 34 | Windows Forms | Ask for confirmation before the app closes, with the same keys. | Met | `MainForm.OnFormClosing` |
| 35 | WPF | Fetch data asynchronously, show a loading animation, and keep the window responsive in every size. | Not met | This repository has no WPF project. |
| 36 | WPF | Use the championship and language set in the Windows Forms app, and apply changes made in WPF back to it. | Not met | This repository has no WPF project; the shared settings store it would use is `ConfigService`. |
| 37 | WPF | Start-up window for championship and language (if not set yet) and the window mode: full screen or one of at least three sizes, saved to a text file and asked again if the file is missing. | Not met | This repository has no WPF project. |
| 38 | WPF | Load the favourite team set in Windows Forms, allow changing it, pick an opponent it played, both shown as "NAME (FIFA_CODE)", and show that match's result. | Not met | This repository has no WPF project. |
| 39 | WPF | Buttons next to both teams open a team window with a 0.5 s animation: name, FIFA code, games played, won, lost and drawn, goals for, against and difference. | Not met | This repository has no WPF project; the figures are in `TeamResult`. |
| 40 | WPF | Draw both starting line-ups on a pitch, players placed by position (Goalie, Defender, Midfield, Forward) as user controls with name, shirt number and the picture set in Windows Forms. | Not met | This repository has no WPF project. |
| 41 | WPF | Clicking a player opens a window with a different 0.3 s animation: name, shirt number, picture, position, captain flag, and goals and yellow cards in that match. | Not met | This repository has no WPF project. |
| 42 | WPF | A settings window with the same confirmation and keys, saving to a text file, and a confirmation before closing. | Not met | This repository has no WPF project. |
