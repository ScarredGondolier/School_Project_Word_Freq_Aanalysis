using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        try
        {
            string programovyAdresar = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Console.WriteLine("Zadajte názov súboru (bez prípony):");
            string nazovSuboru = Console.ReadLine();

            //https://learn.microsoft.com/en-us/dotnet/api/system.io.path.combine?view=net-8.0
            //https://www.csharptutorial.net/csharp-file/csharp-path/
            string cesta_k_suboru = Path.Combine(programovyAdresar, $"{nazovSuboru}.txt");

            if (File.Exists(cesta_k_suboru))
            {
                // Inicializácia slovníka pre počítanie slov
                Dictionary<string, int> pocitadloSlov = new Dictionary<string, int>();

                // Čítanie súboru riadok po riadku
                //https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader?view=net-8.0

                using (StreamReader sr = new StreamReader(cesta_k_suboru, Encoding.UTF8))
                {
                    string riadok;
                    while ((riadok = sr.ReadLine()) != null)
                    {
                        // https://learn.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.split?view=net-8.0
                        //Delenie vstupných reťazcov na pole podreťazcov na pozíciách definovaných zhodou regulárneho výrazu.
                        string[] slova = Regex.Split(riadok, @"\W+", RegexOptions.IgnorePatternWhitespace);

                        //počítadlo slov, ktore su dlhsie ako 3 pismena
                        foreach (string slovo in slova)
                        {
                            if (slovo.Length > 3)
                            {
                                if (pocitadloSlov.ContainsKey(slovo))
                                    pocitadloSlov[slovo]++;
                                else
                                    pocitadloSlov[slovo] = 1;
                            }
                        }
                    }
                }

                // Zoradenie slov podľa počtu výskytov vzostupne
                //https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.orderbydescending?view=net-8.0
                var zoradeneSlova = pocitadloSlov.OrderByDescending(x => x.Value);

                // Výpis počtu vyskytov slov s viac ako 3 písmenami
                Console.WriteLine("Počet slov s viac ako 3 písmenami:");
                foreach (var kvp in zoradeneSlova)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }

                // Získať nazov suboru od používateľa
                //subor, ktory je vysledkom, bude .csv
                Console.WriteLine("Zadajte názov súboru:");
                string nazov = Console.ReadLine();

                // Uloženie zoznamu do súboru CSV s pocitadlom
                //ak by sme chceli viackrat spustit, nech sa vytvori novy csv
                int cislo = 1;
                string cestaCsvSuboru;
                do
                {
                    cestaCsvSuboru = Path.Combine(programovyAdresar, $"pocetnost_slov_{nazov}_{cislo}.csv");
                    cislo++;
                } while (File.Exists(cestaCsvSuboru));

                //https://learn.microsoft.com/en-us/dotnet/api/system.io.streamwriter?view=net-8.0
                using (StreamWriter sw = new StreamWriter(cestaCsvSuboru, false, Encoding.UTF8))
                {
                    sw.WriteLine("Nájdené slovo; Počet výskytu");
                    foreach (var kvp in zoradeneSlova)
                    {
                        sw.WriteLine($"{kvp.Key};{kvp.Value}");
                    }
                }

                Console.WriteLine($"Zoznam bol uložený do súboru {cestaCsvSuboru}");
            }
            else
            {
                Console.WriteLine("Súbor neexistuje.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Chyba: {ex.Message}");
        }
    }
}






