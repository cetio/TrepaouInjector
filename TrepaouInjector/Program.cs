using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ManualMapInjection;
using Microsoft.Win32;

namespace TrepaouInjector
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("TREPAOU INJECTOR by cet#5090");
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.Write("Steam DLL (if applicable): ");
            string preqPath = Console.ReadLine();
            Console.Write("CSGO DLL: ");
            string path = Console.ReadLine();

            if (!File.Exists(path))
                return;
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName == "csgo" || process.ProcessName == "steam" || process.ProcessName == "steamwebhelper" || 
                    process.ProcessName == "steamservice")
                    process.Kill();
            }

            Process proc = Process.Start(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Valve\Steam", "InstallPath", null) + "\\Steam.exe");

            Console.WriteLine("Waiting on prerequisites...");
            if (File.Exists(preqPath))
                ManMap.ManualMap(proc, preqPath);
            System.Threading.Thread.Sleep(3000);
            ManMap.ManualMap(proc, Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]) + "\\VAC.dll");

            Process.Start("steam://rungameid/730");
            Console.WriteLine("Waiting on CSGO...");
            while (Process.GetProcessesByName("csgo").Length < 1 || Process.GetProcessesByName("csgo")[0].HandleCount < 1480)
                System.Threading.Thread.Sleep(100);
            proc = Process.GetProcessesByName("csgo")[0];

            bool status = ManMap.ManualMap(proc, path);

            Console.ForegroundColor = status ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine(status ? "Successfully injected!" : "Injection failed!");
            Console.ReadKey();
        }
    }
}
