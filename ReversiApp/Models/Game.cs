using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiApp.Models
{
    public class Game : IGame
    {
        public int GameID { get; set; }
        public string Omschrijving { get; set; }
        public string Token { get; set; }
        public ICollection<Speler> Spelers { get; set; }
        [NotMapped]
        public Kleur[,] Bord { get; set; }
        public Kleur AandeBeurt { get; set; }
        private List<string> stukkenTeSlaan;
        public Game()
        {
            stukkenTeSlaan = new List<string>();
            Bord = new Kleur[8, 8];
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    if (row == 3 && column == 3 || row == 4 && column == 4)
                    {
                        Bord[row, column] = Kleur.Wit;
                    }
                    else if (row == 3 && column == 4 || row == 4 && column == 3)
                    {
                        Bord[row, column] = Kleur.Zwart;
                    }
                    else
                    {
                        Bord[row, column] = Kleur.Geen;
                    }
                }
            }
        }
        public bool Afgelopen()
        {
            if (Pas())
            {
                return true;
            }
            return false;
        }

        public bool DoeZet(int rijZet, int kolomZet)
        {
            stukkenTeSlaan.Clear();
            if (ZetMogelijk(rijZet, kolomZet))
            {
                Bord[rijZet, kolomZet] = AandeBeurt;
                foreach (string stuk in stukkenTeSlaan)
                {
                    string[] values = stuk.Split(',');
                    int value1 = Int32.Parse(values[0]);
                    int value2 = Int32.Parse(values[1]);
                    Bord[value1, value2] = AandeBeurt;
                    Console.WriteLine($"{value1} {value2} wordt {AandeBeurt}");
                }
                AandeBeurt = (AandeBeurt == Kleur.Wit) ? Kleur.Zwart : Kleur.Wit;
                return true;
            }
            return false;
        }

        public Kleur OverwegendeKleur()
        {
            int aantalZwart = 0;
            int aantalWit = 0;
            for (int row = 0; row < Bord.GetLength(0); row++)
            {
                for (int column = 0; column < Bord.GetLength(1); column++)
                {
                    if (Bord[row, column] == Kleur.Zwart)
                    {
                        aantalZwart++;
                    }
                    if (Bord[row, column] == Kleur.Wit)
                    {
                        aantalWit++;
                    }
                }
            }
            if (aantalZwart > aantalWit) { return Kleur.Zwart; }
            else if (aantalWit > aantalZwart) { return Kleur.Wit; }
            return Kleur.Geen;
        }

        public bool Pas()
        {
            for (int row = 0; row < Bord.GetLength(0); row++)
            {
                for (int column = 0; column < Bord.GetLength(1); column++)
                {
                    if (Bord[row, column] == Kleur.Geen)
                    {
                        Console.WriteLine($"Row:{row} Column:{column}");
                        if (ZetMogelijk(row, column))
                        {
                            Console.WriteLine($"Row:{row} Column:{column} IS MOGELIJK");
                            return false;
                        }
                    }
                }
            }
            AandeBeurt = (AandeBeurt == Kleur.Wit) ? Kleur.Zwart : Kleur.Wit;
            Console.WriteLine($"Pas, {AandeBeurt} is nu aan de beurt");
            return true;
        }

        public bool ZetMogelijk(int rijZet, int kolomZet)
        {
            if (rijZet >= 0 && rijZet < 8 || kolomZet >= 0 && kolomZet < 8)
            {
                //kijkt op de plek vrij is
                if (Bord[rijZet, kolomZet] == Kleur.Geen)
                {
                    var rowLimit = Bord.GetLength(0) - 1;
                    var columnLimit = Bord.GetLength(1) - 1;
                    int neighBourNum = 1;
                    if (rijZet == 0 && kolomZet == 7) { neighBourNum = 5; }
                    else if (rijZet == 7 && kolomZet == 7) { neighBourNum = 1; }
                    else if (rijZet == 0 && kolomZet == 0) { neighBourNum = 6; }
                    else if (rijZet == 7 && kolomZet == 0) { neighBourNum = 2; }
                    else if (rijZet == 0) { neighBourNum = 4; }
                    else if (kolomZet == 7) { neighBourNum = 2; }


                    for (var x = Math.Max(0, rijZet - 1); x <= Math.Min(rijZet + 1, rowLimit); x++)
                    {
                        for (var y = Math.Max(0, kolomZet - 1); y <= Math.Min(kolomZet + 1, columnLimit); y++)
                        {
                            if (x != rijZet || y != kolomZet)
                            {
                                if (x >= 0 && y >= 0 && x < 8 && y < 8)
                                {
                                    //De eerste de beste neighbor die wordt gevonden
                                    if (Bord[x, y] != AandeBeurt && Bord[x, y] != Kleur.Geen)
                                    {
                                        if (kolomZet == 7 && rijZet > 0 && neighBourNum > 4)
                                        {
                                            neighBourNum++;
                                        }
                                        Console.WriteLine($"Neighbournum = {neighBourNum}");
                                        bool notEmpty = true;
                                        int count = 0;

                                        while (notEmpty)
                                        {
                                            count++;
                                            if (x >= 0 && y >= 0 && x < 8 && y < 8)
                                            {
                                                if (Bord[x, y] != AandeBeurt && Bord[x, y] != Kleur.Geen)
                                                {

                                                    Console.WriteLine($"{Bord[x, y]} x = {x} y = {y}");
                                                    stukkenTeSlaan.Add($"{x},{y}");
                                                    if (neighBourNum == 1) { x--; y--; }
                                                    else if (neighBourNum == 2) { x++; }
                                                    else if (neighBourNum == 3) { x--; y++; }
                                                    else if (neighBourNum == 4) { y--; }
                                                    else if (neighBourNum == 6) { y++; }
                                                    else if (neighBourNum == 7) { x++; y--; }
                                                    else if (neighBourNum == 8) { x++; }
                                                    else if (neighBourNum == 9) { x++; y++; }

                                                }
                                                else
                                                {

                                                    notEmpty = false;
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Zet niet mogelijk");
                                                return false;
                                            }
                                        } //end while
                                        Console.WriteLine(rijZet + " beginwaarde");
                                        Console.WriteLine(y + " eindwaarde");
                                        if (Bord[x, y] == AandeBeurt)
                                        {
                                            Console.WriteLine("Zet mogelijk");
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                }

                            }
                            neighBourNum++;
                        }
                    }
                }
            }
            Console.WriteLine("Zet niet mogelijk");
            return false; ;
        }
    }
}

