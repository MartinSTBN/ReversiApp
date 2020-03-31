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
        public Speler Speler  { get; set; }
        [NotMapped]
        public Kleur[,] Bord { get; set; }
        public Kleur AandeBeurt { get; set; }
        public int aantalGeslagenDoorWit { get; set; }
        public int aantalGeslagenDoorZwart { get; set; }
        public string SpelerDieWiltJoinen { get; set; }
        public string JoinAccepteerStatus { get; set; }
        public List<string> stukkenTeSlaan;
        public Game()
        {
            stukkenTeSlaan = new List<string>();
            Bord = new Kleur[10, 10];
            for (int row = 0; row < 10; row++)
            {
                for (int column = 0; column < 10; column++)
                {
                    if (row == 4 && column == 4 || row == 5 && column == 5)
                    {
                        Bord[row, column] = Kleur.Wit;
                    }
                    else if (row == 4 && column == 5 || row == 5 && column == 4)
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
            neighBourPieces.Clear();
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

            if (rijZet >= 0 && rijZet <= 8 || kolomZet >= 0 && kolomZet <= 8)
            {
                //kijkt op de plek vrij is
                if (Bord[rijZet, kolomZet] == Kleur.Geen)
                {
                    Console.WriteLine("Plek vrij");
                    var rowLimit = Bord.GetLength(0) - 1;
                    var columnLimit = Bord.GetLength(1) - 1;
                    int neighBourNum = 1;
                    var count = 0;
                    Console.WriteLine(neighBourNum);
                    for (var x = Math.Max(0, rijZet - 1); x <= Math.Min(rijZet + 1, rowLimit); x++)
                    {
                        for (var y = Math.Max(0, kolomZet - 1); y <= Math.Min(kolomZet + 1, columnLimit); y++)
                        {
                            Console.WriteLine($"Neighbournum = {neighBourNum}");
                            if (x != rijZet || y != kolomZet)
                            {
                                if (x >= 0 && y >= 0 && x <= 8 && y <= 8)
                                {
                                    Console.WriteLine($"{x},{y} = {Bord[x, y]}");

                                    //De eerste de beste neighbor die wordt gevonden
                                    if (Bord[x, y] != AandeBeurt && Bord[x, y] != Kleur.Geen)
                                    {
                                        Console.WriteLine($"Neighbour gevonden op positie {x} {y} met kleur {Bord[x, y]}");
                                        Console.WriteLine($"{x} {y}");
                                        Console.WriteLine($"Neighbournum = {neighBourNum}");

                                        NeighbourCheck(neighBourNum, x, y);


                                    }
                                    count++;
                                }

                            }
                            neighBourNum++;
                        }
                    }

                    if (neighBourPieces.Count > 0)
                    {
                        foreach (var item in neighBourPieces)
                        {
                            foreach (var value in item.NeighBours)
                            {
                                stukkenTeSlaan.Add(value);
                            }
                        }
                        Console.WriteLine("Zet mogelijk");
                        return true;
                    }
                }
            }
            Console.WriteLine("Zet niet mogelijk");
            return false;
        }
        private List<NeighBour> neighBourPieces = new List<NeighBour>();
        public void NeighbourCheck(int neighBourNum, int x, int y)
        {
            bool notEmpty = true;
            int count = 0;

            NeighBour neighBour = new NeighBour();
            neighBour.NeighBours = new List<string>();
            var previousColor = AandeBeurt;
            while (notEmpty)
            {
                Console.WriteLine("Start loop");
                if (Bord[x, y] != AandeBeurt && Bord[x, y] != Kleur.Geen)
                {
                    Console.WriteLine($"{Bord[x, y]} x = {x} y = {y} Added to neighBours");
                    neighBour.NeighBours.Add($"{x},{y}");
                    if (neighBourNum == 1) { x--; y--; }
                    else if (neighBourNum == 2) { x--; }
                    else if (neighBourNum == 3) { x--; y++; }
                    else if (neighBourNum == 4) { y--; }
                    else if (neighBourNum == 6) { y++; }
                    else if (neighBourNum == 7) { x++; y--; }
                    else if (neighBourNum == 8) { x++; }
                    else if (neighBourNum == 9) { x++; y++; }
                    previousColor = Bord[x, y];
                    count++;
                }
                else
                {
                    if (previousColor != AandeBeurt && Bord[x, y] == Kleur.Geen)
                    {
                        Console.WriteLine("EEN EN DAN GEEN");
                        Console.WriteLine("COUNT = " + count);
                        for (int i = 0; i < count; i++)
                        {
                            neighBour.NeighBours.RemoveAt(neighBour.NeighBours.Count - 1);
                        }


                    }
                    Console.WriteLine("Stop loop");
                    notEmpty = false;

                }
            } //end while
            Console.WriteLine(neighBour.NeighBours.Count);
            if (neighBour.NeighBours.Count > 0)
            {
                neighBourPieces.Add(neighBour);
            }
        }
    }
    public class NeighBour
    {
        public List<string> NeighBours { get; set; }
    }
}

