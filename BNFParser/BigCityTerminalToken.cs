using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNFParser
{
    class BigCityTerminalToken : TerminalToken
    {
        private readonly string[] cityArray =
            {
            "Valley Stream", "Great Neck Plaza", "Clifton Springs", "Canton", "Plattsburgh", "Goshen", "Syracuse", "Port Jervis", "Williston Park", "New Hempstead", "Port Chester", "Canandaigua", "Lockport", "Saratoga Springs",
            "Rome", "Penn Yan", "Mechanicville", "Corning", "Cobleskill", "Wappingers Falls", "Colonie", "Airmont", "North Syracuse", "Ellenville", "Albion", "Fredonia", "Bath", "Pelham Manor", "Utica", "Lake Grove", "North Hills",
            "Suffern", "New Rochelle", "Newark", "Warwick", "Chestnut Ridge", "Dobbs Ferry", "Monticello", "Ossining", "Hamburg", "Middletown", "Lindenhurst", "Geneseo", "Lynbrook", "Schenectady", "Buffalo", "Walden", "Tarrytown",
            "Queens", "Wesley Hills", "Glens Falls", "Freeport", "Hastings-on-Hudson", "Mastic Beach", "Horseheads", "Pelham", "Malone", "Red Hook", "Cohoes", "Batavia", "Pleasantville", "White Plains", "Bronxville", "Massapequa Park",
            "Bayville", "Watertown", "Salamanca", "Mount Kisco", "Medina", "Elmsford", "Rye Brook", "Coxsackie", "Westbury", "Northport", "Catskill", "Auburn", "Saranac Lake", "Newburgh", "New Square", "Potsdam", "Sea Cliff", "Floral Park",
            "East Hills", "Amityville", "Williamsville", "Rockville Centre", "Croton-on-Hudson", "Kingston", "Elmira", "Monroe", "Amsterdam", "Peekskill", "Ogdensburg", "Hornell", "Herkimer", "New Paltz", "Albany", "Kiryas Joel", "Olean",
            "Farmingdale", "Niagara Falls", "Lackawanna", "West Haverstraw", "Endicott", "Rochester", "Ilion", "Manorhaven", "Babylon", "Watervliet", "Hempstead", "Ravena", "New York", "Scarsdale", "Bronx", "Fulton", "Briarcliff Manor",
            "Irvington", "Washingtonville", "Poughkeepsie", "Mineola", "Patchogue", "Johnson City", "Brockport", "Tonawanda", "Mamaroneck", "East Rochester", "Depew", "Rye", "Haverstraw", "Harrison", "Cortland", "Norwich", "Hudson Falls",
            "Honeoye Falls", "Garden City", "Port Jefferson", "Baldwinsville", "Ballston Spa", "Fairport", "Oswego", "Woodbury", "Hilton", "North Tonawanda", "Jamestown", "Webster", "Sidney", "East Rockaway", "Kaser", "Oneonta", "Owego",
            "Kenmore", "Long Beach", "Tuckahoe", "Valatie", "Troy", "Nyack", "Great Neck", "Chester", "Attica", "Ithaca", "Larchmont", "Rensselaer", "Cedarhurst", "Yonkers", "Lawrence", "Brooklyn", "East Aurora", "Massena", "Binghamton",
            "Spring Valley", "Oneida", "Gowanda", "Kings Point", "Lancaster", "Hudson", "Fort Plain", "Sleepy Hollow", "Dunkirk", "Manhattan", "Beacon", "Geneva", "Wellsville", "Mount Vernon", "Johnstown", "Solvay", "Glen Cove", "Scotia",
            "Gloversville", "Staten Island", "New Hyde Park", "Exeter", "Clarion", "Millersville", "Camp Hill", "Canonsburg", "Grove City", "Cresson", "Quakertown", "Hatboro", "Folcroft", "Connellsville"
        };

        public BigCityTerminalToken(string name) : base(name) { }

        public bool IsItAMatch(string cityName)
        {
            foreach (string city in cityArray)
                if (string.Compare(city, cityName) == 0)
                    return true;
            return false;
        }

        public override int match(string cityName)
        {
            foreach (string city in cityArray)
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