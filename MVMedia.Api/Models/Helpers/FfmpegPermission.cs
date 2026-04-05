using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MVMedia.Api.Models.Helpers;

public static class FfmpegPermission
{
    public static void EnsureExecutable(string ffmpegPath)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return;

        if (!File.Exists(ffmpegPath))
            throw new FileNotFoundException($"FFMpeg não encontrado em: {ffmpegPath}");

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "chmod",
                Arguments = $"+x {ffmpegPath}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            }
        };

        process.Start();
        process.WaitForExit();
    }
}
