using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic.FileIO;
using RESTFestivals.CSVConverter.Converters;
using RESTFestivals.CSVConverter.Entity;

namespace RESTFestivals.CSVConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var path =
                @"E:\CodingProjects\RESTFestivals\RESTFestivals.CSVConverter\Scrape_Sitemaps\Scrape festivals_de\festivalplaner_scrape.csv";
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] {"#"};
                csvParser.SetDelimiters(new string[] {","});
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                //Extract Data from .CSV
                List<Festival_Unformatted> festivals_unformatted = new List<Festival_Unformatted>();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();
                    festivals_unformatted.Add(new Festival_Unformatted()
                    {
                        FestivalUrl = fields[1],
                        Title = fields[2],
                        Status = fields[4],
                        Date = fields[5],
                        DateZipCodeStateCountry = fields[6],
                        TableColOne = fields[7],
                        TableColTwo = fields[8],
                        Artist = fields[9],
                        Website = fields[11]
                    });
                }

                var festivalConverter = new Festival_de_Converter(festivals_unformatted);

                //How many festivals have a status
                if (festivalConverter.FormattedFestivals.Any(f => f.Status != null))
                {
                    Console.WriteLine(festivalConverter.FormattedFestivals.Count(f => f.Status != null) + " festivals have a Status");
                }
                
                //How many festivals contain one date 
                //How many festivals contain more than one date
                if (festivalConverter.FormattedFestivals.Any(f => f.Dates.Any()))
                {
                    Console.WriteLine("Out of " + festivalConverter.FormattedFestivals.Count() + ": " + festivalConverter.FormattedFestivals.Count(f => f.Dates.Count() > 0) +
                                      " festivals have a Date");
                    Console.WriteLine(festivalConverter.FormattedFestivals.Count(f => f.Dates.Count() > 1) + " festivals have more than one Date");

                    var festivalWithMoreThanOneDate = festivalConverter.FormattedFestivals.FirstOrDefault(f => f.Dates.Count() > 1);
                    Console.WriteLine(festivalWithMoreThanOneDate?.Dates.First());
                }
                
                //How many festivals contain a ZipCode, State, Street and Country
                if (festivalConverter.FormattedFestivals.Any(f =>
                    !string.IsNullOrWhiteSpace(f.Address.State) && !string.IsNullOrWhiteSpace(f.Address.ZipCode)))
                {
                    Console.WriteLine("Out of " + festivalConverter.FormattedFestivals.Count() + ": " +
                                      festivalConverter.FormattedFestivals.Count(f => !string.IsNullOrWhiteSpace(f.Address.State)) +
                                      " festivals have a State");

                    Console.WriteLine("Out of " + festivalConverter.FormattedFestivals.Count() + ": " +
                                      festivalConverter.FormattedFestivals.Count(f => !string.IsNullOrWhiteSpace(f.Address.ZipCode)) +
                                      " festivals have a ZipCode");
                    
                    Console.WriteLine("Out of " + festivalConverter.FormattedFestivals.Count() + ": " +
                                      festivalConverter.FormattedFestivals.Count(f => !string.IsNullOrWhiteSpace(f.Address.Location)) +
                                      " festivals have a Location");
                    
                    Console.WriteLine("Out of " + festivalConverter.FormattedFestivals.Count() + ": " +
                                      festivalConverter.FormattedFestivals.Count(f => !string.IsNullOrWhiteSpace(f.Address.Country)) +
                                      " festivals have a Country");
                }
                
                //How many festivals contain a URL
                if (festivalConverter.FormattedFestivals.Any(f => f.FestivalUrl.Any()))
                {
                    Console.WriteLine("Out of " + festivalConverter.FormattedFestivals.Count() + ": " + festivalConverter.FormattedFestivals.Count(f => f.FestivalUrl.Count() > 0) +
                                      " festivals have a FestivalUrl");
                }
                
                //How many festivals contain AverageExpectedVisitors
                if (festivalConverter.FormattedFestivals.Any(f => f.AverageExpectedVisitors != null))
                {
                    Console.WriteLine("Out of " + festivalConverter.FormattedFestivals.Count() + ": " + festivalConverter.FormattedFestivals.Count(f => f.AverageExpectedVisitors != null) +
                                      " festivals have a AverageExpectedVisitors Count");
                }
                
                //How many festivals contain IsCampingPossible
                if (festivalConverter.FormattedFestivals.Any(f => f.IsCampingPossible != null))
                {
                    Console.WriteLine("Out of " + festivalConverter.FormattedFestivals.Count() + ": " + festivalConverter.FormattedFestivals.Count(f => f.IsCampingPossible != null && f.IsCampingPossible == true) +
                                      " festivals have a IsCampingPossible = true");
                    
                    Console.WriteLine("Out of " + festivalConverter.FormattedFestivals.Count() + ": " + festivalConverter.FormattedFestivals.Count(f => f.IsCampingPossible != null && f.IsCampingPossible == false) +
                                      " festivals have a IsCampingPossible = false");
                }
                
                //How many festivals contain Email
                if (festivalConverter.FormattedFestivals.Any(f => f.Email != null))
                {
                    Console.WriteLine("Out of " + festivalConverter.FormattedFestivals.Count() + ": " + festivalConverter.FormattedFestivals.Count(f => f.Email != null) +
                                      " festivals have an Email");
                }
                
                //How many festivals contain a Phonenumber
                if (festivalConverter.FormattedFestivals.Any(f => f.PhoneNumber != null))
                {
                    Console.WriteLine("Out of " + festivalConverter.FormattedFestivals.Count() + ": " + festivalConverter.FormattedFestivals.Count(f => f.PhoneNumber != null) +
                                      " festivals have an PhoneNumber");
                }

                //How many festivals contain Artists 
                if (festivalConverter.FormattedFestivals.Any(f => f.Artists.Any()))
                {
                    Console.WriteLine("Out of " + festivalConverter.FormattedFestivals.Count() + ": " + festivalConverter.FormattedFestivals.Count(f => f.Artists.Any()) +
                                      " festivals have Artists");
                    Console.WriteLine(festivalConverter.FormattedFestivals.Count(f => f.Artists.Count() > 1) + " festivals have more than one Artists");
                }
                
                //How many festivals have a Website
                if (festivalConverter.FormattedFestivals.Any(f => f.Website != null))
                {
                    Console.WriteLine("Out of " + festivalConverter.FormattedFestivals.Count() + ": " + festivalConverter.FormattedFestivals.Count(f => f.Website != null) +
                                      " festivals have a Website");
                }
                
                //TODO: Replace/Clean all Console logs and replace with ILogger
                //TODO: Console log total number of festivals separately from the number of festivals with other occurences
                //TODO: Delete all festivals that have no Addresses
                
                Console.WriteLine("Conversion Completed");
            }
        }

        
        
        //TODO: Implement Unit Tests
        //Test for RemoveWhitespace
        // [Test]
        // [TestCase("123 123 1adc \n 222", "1231231adc222")]
        // public void RemoveWhiteSpace1(string input, string expected)
        // {
        //     string s = null;
        //     for (int i = 0; i < 1000000; i++)
        //     {
        //         s = input.RemoveWhitespace();
        //     }
        //     Assert.AreEqual(expected, s);
        // }
    }
}