using System.Runtime.InteropServices;

namespace Materia;

public sealed partial class RawInput
{
    [LibraryImport("user32.dll", SetLastError = true)]
    private static partial int GetRawInputData(nint hRawInput, uint uiCommand, out RawInputData pData, ref int pcbSize, int cbSizeHeader);

    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputData
    {
        public RawInputDataHeader header;
        public RawInputDataData data;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RawInputDataHeader
    {
        public uint type;
        public uint size;
        public nint device;
        public nint param;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct RawInputDataData
    {
        [FieldOffset(0)] public RawMouseData mouseData;
        [FieldOffset(0)] public RawKeyboardData keyboardData;
        [FieldOffset(0)] public RawHIDData hidData;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct RawMouseData
    {
        [FieldOffset(0x00)] public ushort flags;
        [FieldOffset(0x04)] public uint buttons;
        [FieldOffset(0x04)] public ushort buttonFlags;
        [FieldOffset(0x06)] public ushort buttonData;
        [FieldOffset(0x08)] public uint rawButtons;
        [FieldOffset(0x0C)] public int lastX;
        [FieldOffset(0x10)] public int lastY;
        [FieldOffset(0x14)] public uint extraInformation;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RawKeyboardData
    {
        public ushort makeCode;
        public ushort flags;
        private readonly ushort reserved;
        public ushort vKey;
        public uint message;
        public ulong extraInformation;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RawHIDData
    {
        public int sizeHid;
        public int count;
        public byte rawData;
    }

    public RawInputData? Data { get; }

    public unsafe RawInput(nint hRawInput)
    {
        var size = sizeof(RawInputData);
        var copiedBytes = GetRawInputData(hRawInput, 0x10000003, out var data, ref size, sizeof(RawInputDataHeader));
        if (copiedBytes > 0)
            Data = data;
    }
}