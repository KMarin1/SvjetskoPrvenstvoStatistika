# SvjetskoPrvenstvoStatistika

Desktop app for browsing World Cup results — pick a championship and a team, and see the
squad and match statistics. University project (Algebra, Object-Oriented Programming).

## Stack

C# · .NET · WinForms · Newtonsoft.Json · HttpClient

## Structure

```
PodatkovniSloj/                 data layer — API client, models, settings
SvjetskoPrvenstvoPobjedeForm/   WinForms UI
```

The solution is split in two on purpose. `PodatkovniSloj` is a class library that knows
how to fetch and deserialise data and nothing about the interface; the WinForms project
consumes it. Swapping the UI would not touch the data layer.

**`ApiService`** calls the World Cup API (`worldcup-vua.nullbit.hr`) for team results and
match data, async, and deserialises into `Data`, `Match` and `Player`.

**Settings** are read from a config file — which championship (men's or women's) and which
culture to run in — so the choice survives a restart instead of being hardcoded.

**`IgracControl`** is a custom user control for rendering one player, so the squad view is
a list of the same control rather than hand-built layout.

**`MyCulture`** handles localisation between Croatian and English.

## Running it

Open `SvjetskoPrvenstvoStatistika.sln` in Visual Studio and run the WinForms project. The
API is public, so there is nothing to configure.
