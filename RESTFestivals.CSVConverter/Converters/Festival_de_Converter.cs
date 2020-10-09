using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using RESTFestivals.CSVConverter.Entity;
using RESTFestivals.CSVConverter.Helpers;

namespace RESTFestivals.CSVConverter.Converters
{
    public class Festival_de_Converter
    {
        private int festivalsUnformattedCount;

        public List<Festival> FormattedFestivals { get; set; }
            = new List<Festival>();

        private readonly List<Festival_Unformatted> _festivalsUnformatted
            = new List<Festival_Unformatted>();
        
        public Festival_de_Converter(List<Festival_Unformatted> festivalsUnformatted)
        {
            if (festivalsUnformatted.Count <= 0)
            {
                //TODO: Throw exception or log?
            }
            else
            {
                _festivalsUnformatted = festivalsUnformatted;
            
                ExtractFestivals();
            }
        }
        
        private void ExtractFestivals()
            {
                festivalsUnformattedCount = _festivalsUnformatted.Count;
                var festivalsUnformattedBuffer = _festivalsUnformatted;
                
                while (festivalsUnformattedCount > 0)
                {
                    var festivalTitle = festivalsUnformattedBuffer.First().Title;
                    var festivalsWithSameTitle =
                        festivalsUnformattedBuffer.Where(fu => fu.Title == festivalTitle).ToList();

                    var festival = new Festival()
                    {
                        Address = new Address()
                    };

                    //Add Title
                    festival.Title = festivalTitle;
                    
                    //Extract Status
                    ExtractStatusColumn(festivalsWithSameTitle, festival);

                    //Extract Date/s
                    ExtractDates(festivalsWithSameTitle, festival);

                    //Extract Address information from multiple line field
                    ExtractAddress(festivalsWithSameTitle, festival);

                    //Extract url
                    festival.FestivalUrl = festivalsWithSameTitle.First().FestivalUrl;

                    //Extract Expected visitors
                    ExtractExpectedVisitors(festivalsWithSameTitle, festival);

                    //Extract Camping Option
                    ExtractCampingOption(festivalsWithSameTitle, festival);

                    //Extract Email and Phone Number
                    ExtractEmailAndTelephone(festivalsWithSameTitle, festival);

                    //Extract Artists
                    ExtractArtists(festivalsWithSameTitle, festival);

                    //Extract Festival Website
                    ExtractFestivalWebsite(festivalsWithSameTitle, festival);

                    FormattedFestivals.Add(festival);

                    festivalsUnformattedBuffer.RemoveAll(fub => fub.Title == festivalTitle);
                    festivalsUnformattedCount = festivalsUnformattedBuffer.Count();
                }
            }

        private void ExtractFestivalWebsite(List<Festival_Unformatted> festivalsWithSameTitle, Festival festival)
        {
            var festivalWebsiteString =
                festivalsWithSameTitle.FirstOrDefault(f => !string.IsNullOrEmpty(f.Website))?.Website;

            festival.Website = WebsiteRegex.MatchWebsite(festivalWebsiteString);

            if (festival.Website == null)
            {
                //TODO: Log festival does not contain website
            }
        }

        private void ExtractArtists(List<Festival_Unformatted> festivalsWithSameTitle, Festival festival)
        {
            var festivalWithArtistsCollection = festivalsWithSameTitle.Where(f => !string.IsNullOrEmpty(f.Artist));

            foreach (var festivalWithArtists in festivalWithArtistsCollection)
            {
                festival.Artists.Add(new Artists()
                {
                    Name = festivalWithArtists.Artist
                });
            }
        }

        private void ExtractEmailAndTelephone(List<Festival_Unformatted> festivalsWithSameTitle, Festival festival)
        {
            var hotlineKontaktString = festivalsWithSameTitle
                .First(f => f.TableColOne == "Info-Hotline:" || f.TableColOne == "Kontakt")
                .TableColTwo;

            if (!string.IsNullOrWhiteSpace(hotlineKontaktString))
            {
                //Extract Email
                festival.Email = EmailRegex.MatchEmail(hotlineKontaktString);

                //Extract Phone Number
                hotlineKontaktString = RemoveCharacters.RemoveWhitespaces(hotlineKontaktString);

                hotlineKontaktString = RemoveCharacters.RemoveSpecialCharacters(hotlineKontaktString, new []{'+'});

                festival.PhoneNumber = TelephoneRegex.MatchTelephone(hotlineKontaktString);
            }
        }

        private void ExtractCampingOption(List<Festival_Unformatted> festivalsWithSameTitle, Festival festival)
        {
            var campingPossibleString = festivalsWithSameTitle.FirstOrDefault(f => f.TableColOne == "Camping")
                ?.TableColTwo;

            if (campingPossibleString != null)
            {
                campingPossibleString = campingPossibleString.ToLower();

                if (campingPossibleString == "nein")
                {
                    festival.IsCampingPossible = false;
                }

                if (campingPossibleString == "ja" || campingPossibleString == "vorhanden")
                {
                    festival.IsCampingPossible = true;
                }
            }
        }

        private void ExtractExpectedVisitors(List<Festival_Unformatted> festivalsWithSameTitle, Festival festival)
        {
            var expectedVisitorsString = festivalsWithSameTitle.FirstOrDefault(f => f.TableColOne == "Besucher")
                ?.TableColTwo;

            if (!string.IsNullOrWhiteSpace(expectedVisitorsString))
            {
                festival.AverageExpectedVisitors = RemoveCharacters.RemoveAllExceptNumeric<int>(expectedVisitorsString);
            }
        }

        private void ExtractAddress(List<Festival_Unformatted> festivalsWithSameTitle, Festival festival)
        {
            var splitZipStateAddressCountrys =
                Regex.Split(festivalsWithSameTitle.First().DateZipCodeStateCountry, @"\s{2,}");

            if (splitZipStateAddressCountrys.Length == 4)
            {
                splitZipStateAddressCountrys = splitZipStateAddressCountrys.Skip(1).ToArray();

                for (int i = 0; i < splitZipStateAddressCountrys.Length; i++)
                {
                    //Extract Zip and State
                    if (i == 0)
                    {
                        var splitZipCodeAndProvince = splitZipStateAddressCountrys[i].Split("-");

                        for (int j = 0; j < splitZipCodeAndProvince.Length; j++)
                        {
                            splitZipCodeAndProvince[j] = RemoveCharacters.RemoveWhitespaces(splitZipCodeAndProvince[j]);

                            //Extract Zip
                            if (j == 0)
                            {
                                festival.Address.ZipCode = splitZipCodeAndProvince[j];
                            }

                            //Extract State
                            if (j >= 1)
                            {
                                if (!string.IsNullOrEmpty(festival.Address.State))
                                {
                                    festival.Address.State += " ";
                                }

                                festival.Address.State += splitZipCodeAndProvince[j];
                            }
                        }
                    }

                    //Extract Street
                    if (i == 1)
                    {
                        festival.Address.Location = splitZipStateAddressCountrys[i];
                    }

                    //Extract Country
                    if (i == 2)
                    {
                        festival.Address.Country = splitZipStateAddressCountrys[i];
                    }
                }
            }
        }

        private void ExtractDates(List<Festival_Unformatted> festivalsWithSameTitle, Festival festival)
        {
            var date = festivalsWithSameTitle.First().Date;

            //Parse Festivals with only one date
            DateTime parsedDate1;
            if (DateTime.TryParse(date, out parsedDate1))
            {
                festival.Dates.Add(new FestivalDate()
                {
                    Date = parsedDate1
                });
            }

            //Parse Festivals with more than one Date

            var splitDates = date.Split("-");
            for (int i = 0; i < splitDates.Length; i++)
            {
                splitDates[i] = RemoveCharacters.RemoveWhitespaces(splitDates[i]);
            }

            if (splitDates.Length > 1)
            {
                if (DateTime.TryParse(splitDates[1], out parsedDate1))
                {
                    splitDates[0] += parsedDate1.Date.Year;
                    festival.Dates.Add(new FestivalDate()
                    {
                        Date = parsedDate1
                    });
                }

                if (DateTime.TryParse(splitDates[0], out parsedDate1))
                {
                    festival.Dates.Add(new FestivalDate()
                    {
                        Date = parsedDate1
                    });
                }
            }
        }

        private void ExtractStatusColumn(List<Festival_Unformatted> festivalsWithSameTitle, Festival festival)
        {
            var status = festivalsWithSameTitle.First().Status;
            if (status == "ABGESAGT")
            {
                festival.Status = FestivalStatus.Abgesagt;
            }

            if (status == "AUSVERKAUFT")
            {
                festival.Status = FestivalStatus.Ausverkauft;
            }

            if (status == "VERLEGT")
            {
                festival.Status = FestivalStatus.Verlegt;
            }
        }
    }
}