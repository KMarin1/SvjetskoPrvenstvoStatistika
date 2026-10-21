namespace PodatkovniSloj.Tests;

/// <summary>A fresh folder for one test, deleted with everything in it afterwards.</summary>
internal sealed class TempDirectory : IDisposable
{
    public TempDirectory() => Path = Directory.CreateTempSubdirectory("SvjetskoPrvenstvoTests-").FullName;

    public string Path { get; }

    public string Combine(params string[] parts) => System.IO.Path.Combine([Path, .. parts]);

    public void Dispose() => Directory.Delete(Path, recursive: true);
}
