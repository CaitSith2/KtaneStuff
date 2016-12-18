﻿using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

[assembly: AssemblyTitle("KtaneStuff")]
[assembly: AssemblyDescription("Contains some ancillary code used in the creation of some Keep Talking and Nobody Explodes mods.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("KtaneStuff")]
[assembly: AssemblyCopyright("Copyright © Timwi 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("95055383-2e25-42be-97b7-e1411a695e1d")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

namespace KtaneStuff
{
    class Program
    {
        static void Main(string[] args)
        {
            try { Console.OutputEncoding = Encoding.UTF8; }
            catch { }

            Ktane.SimonScreamsGenerateSmallTable();
            //Modeling.TheClock.Do();

            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
