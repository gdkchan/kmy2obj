using System;
using System.IO;

using kmy2obj.Mdl3D;
using kmy2obj.Mdl3D.Formats;

namespace kmy2obj
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[kmy2obj] Metal Gear Solid The Twin Snakes model converter");
            Console.WriteLine("Made by gdkchan");
            Console.WriteLine("Version 0.1.4");
            Console.Write(Environment.NewLine);

            if (args.Length > 0)
            {
                foreach (string Argument in args)
                {
                    if (File.Exists(Argument) && Path.GetExtension(Argument).ToLower() == ".kmy")
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(string.Format("[{0}] ", Path.GetFileName(Argument)));

                        try
                        {
                            Model Mdl = KMY.FromFile(Argument);
                            OBJ.ToFile(Mdl, Path.GetFileNameWithoutExtension(Argument) + ".obj");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Converted successfully!");
                        }
                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Conversion failed!");
                        }

                        Console.Write(Environment.NewLine);
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Drag and drop files on this executable!");
            }

            Console.ResetColor();
        }
    }
}
