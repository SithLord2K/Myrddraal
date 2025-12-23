using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Myrddraal.Services
{
    public class BladeRunner
    {
        public async Task<string> ExecuteBladeAsync(string toolName, string arguments)
        {
            var result = "";
            var processInfo = new ProcessStartInfo();

            // 1. OS DETECTION: Switch logic based on where the app is running
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                // KALI LINUX MODE
                processInfo.FileName = "/bin/bash";
                // -c tells bash to read the command from the string
                processInfo.Arguments = $"-c \"{toolName} {arguments}\"";
            }
            else
            {
                // WINDOWS DEV MODE
                processInfo.FileName = "cmd.exe";
                processInfo.Arguments = $"/C {toolName} {arguments}";
            }

            // 2. CONFIGURE OUTPUT STREAMS
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.UseShellExecute = false;
            processInfo.CreateNoWindow = true;

            // Optional: Set a working directory if you have scripts in a specific folder
            // processInfo.WorkingDirectory = "..."; 

            try
            {
                // 3. LAUNCH THE TOOL
                using (var process = new Process { StartInfo = processInfo })
                {
                    process.Start();

                    // Read output asynchronously to prevent UI freezing
                    var outputTask = process.StandardOutput.ReadToEndAsync();
                    var errorTask = process.StandardError.ReadToEndAsync();

                    await process.WaitForExitAsync();

                    var output = await outputTask;
                    var error = await errorTask;

                    // 4. FORMAT OUTPUT
                    if (!string.IsNullOrEmpty(error))
                    {
                        // Some tools write warnings to Stderr, so we include both
                        result = $"[STDERR]:\n{error}\n\n[STDOUT]:\n{output}";
                    }
                    else
                    {
                        result = output;
                    }
                }
            }
            catch (Exception ex)
            {
                result = $"[SYSTEM FAILURE]: {ex.Message}";
            }

            return result;
        }
    }
}