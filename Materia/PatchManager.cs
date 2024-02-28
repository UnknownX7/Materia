using System.Globalization;

namespace Materia;

public sealed class PatchManager : IDisposable
{
    private readonly List<AsmPatch> patches = new();

    public AsmPatch CreatePatch(nint address, IReadOnlyCollection<byte?> bytes)
    {
        var patch = new AsmPatch(address, bytes);
        patches.Add(patch);
        return patch;
    }

    public AsmPatch CreatePatch(nint address, string bytesString) => CreatePatch(address, ParseByteString(bytesString));
    public AsmPatch CreatePatch(string sig, IReadOnlyCollection<byte?> bytes) => CreatePatch(SigScanner.GameAssembly.Scan(sig), bytes);
    public AsmPatch CreatePatch(string sig, string bytesString) => CreatePatch(sig, ParseByteString(bytesString));

    private static byte?[] ParseByteString(string bytesString)
    {
        bytesString = bytesString.Replace(" ", string.Empty);

        var bytes = new byte?[bytesString.Length / 2];
        for (int i = 0; i < bytesString.Length; i += 2)
        {
            var s = bytesString.Substring(i, 2);
            bytes[i / 2] = s != "??" ? byte.Parse(s, NumberStyles.AllowHexSpecifier) : null;
        }
        return bytes;
    }

    public void Dispose()
    {
        foreach (var patch in patches)
            patch.Dispose();
    }
}