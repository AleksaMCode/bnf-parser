using ServiceStack;
using System.Collections.Generic;
using System.Linq;

namespace BNFParser
{
    public class Cities
    {
        public List<Dictionary<string, string>> data { get; set; }
        private readonly string key = "capital";
        public readonly static string CityNamesAPIurl = "https://countriesnow.space/api/v0.1/countries/capital";

        private void Trim()
        {
            data = data.Select(x => x.Where(y => key.Equals(y.Key) && !string.IsNullOrWhiteSpace(y.Value)).ToDictionary(y => y.Key, y => y.Value)).ToList();
        }

        public string[] GetCities(int count)
        {
            Trim();
            var cities = new List<string>();
            foreach (var city in data)
            {
                if (city.Count != 0)
                {
                    cities.Add(city[key]);

                    if (--count <= 0)
                    {
                        break;
                    }
                }
            }
            return cities.ToArray();
        }
    }

    class BigCityTerminalToken : TerminalToken
    {

        private readonly string[] cities;

        public BigCityTerminalToken(string name) : base(name)
        {
            cities = Cities.CityNamesAPIurl.GetJsonFromUrl().FromJson<Cities>().GetCities(200);
        }

        public bool IsItAMatch(string cityName)
        {
            foreach (string city in cities)
                if (string.Compare(city, cityName) == 0)
                    return true;
            return false;
        }

        public override int match(string cityName)
        {
            foreach (string city in cities)
                if (string.Compare(city, cityName) == 0)
                    return cityName.Length;
            return -1;
        }

        public override bool equals(object o)
        {
            if (o == null || !(o is BigCityTerminalToken))
                return false;

            return string.Compare(((BigCityTerminalToken)o).Name, Name) == 0;
        }
    }
}