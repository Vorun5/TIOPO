using System;
using System.Diagnostics;
using System.IO;

namespace triangle_test
{
    class Program
    {
        static void Main()
        {
            String outputFile = "result.txt";
            String triangleApp = @"C:\Users\user\source\repos\triangle\bin\Debug\netcoreapp3.1\triangle.exe";
            Console.WriteLine("Вводите путь до файла с тестами: ");
            var pathToInpFile = Console.ReadLine();
            if (!File.Exists(pathToInpFile))
            {
                Console.WriteLine("Ошбика при открытии файла " + pathToInpFile);
                return;
            }
            var inputStream = new StreamReader(pathToInpFile);
            var outputStream = new StreamWriter(outputFile);
            var str = inputStream.ReadLine();
            while(str != null)
            {
                try
                {

                    var buf = str.Split(':');
                    var args = buf[0];
                    var processStartInfo = new ProcessStartInfo
                    {
                        FileName = triangleApp,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        Arguments = args,
                    };
                    Process process = Process.Start(processStartInfo);
                    process.WaitForExit();
                    string programOutput = process.StandardOutput.ReadLine();
                    programOutput.Trim();
                    var expectedResult = buf[1].Trim();
                    var result = expectedResult == programOutput ? "succes" : "error";
                    Console.WriteLine(args + programOutput + " = " + expectedResult +  " : " + result);
                    outputStream.WriteLine(result);
                    str = inputStream.ReadLine();
                } catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            inputStream.Close();
            outputStream.Close();
        }
    }
}
