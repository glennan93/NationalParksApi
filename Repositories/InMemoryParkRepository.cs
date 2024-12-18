//using System.Collections.Generic;
//using System.Linq;
//using NationalParksApi.Models;

//namespace NationalParksApi.Repositories
//{
//    public class InMemoryParkRepository : IParkRepository
//    {
//        private readonly List<NationalPark> _parks;

//        //public InMemoryParkRepository()
//        //{
//        //    _parks = new List<NationalPark>
//        //    {
//        //            new NationalPark(1,  "Acadia National Park",                    "Maine",                      1919),
//        //            new NationalPark(2,  "American Samoa National Park",            "American Samoa",             1988),
//        //            new NationalPark(3,  "Arches National Park",                    "Utah",                       1971),
//        //            new NationalPark(4,  "Badlands National Park",                  "South Dakota",               1978),
//        //            new NationalPark(5,  "Big Bend National Park",                  "Texas",                      1944),
//        //            new NationalPark(6,  "Biscayne National Park",                  "Florida",                    1980),
//        //            new NationalPark(7,  "Black Canyon of the Gunnison National Park","Colorado",                 1999),
//        //            new NationalPark(8,  "Bryce Canyon National Park",              "Utah",                       1928),
//        //            new NationalPark(9,  "Canyonlands National Park",               "Utah",                       1964),
//        //            new NationalPark(10, "Capitol Reef National Park",              "Utah",                       1971),
//        //            new NationalPark(11, "Carlsbad Caverns National Park",          "New Mexico",                 1930),
//        //            new NationalPark(12, "Channel Islands National Park",           "California",                 1980),
//        //            new NationalPark(13, "Congaree National Park",                  "South Carolina",             2003),
//        //            new NationalPark(14, "Crater Lake National Park",               "Oregon",                     1902),
//        //            new NationalPark(15, "Cuyahoga Valley National Park",           "Ohio",                       2000),
//        //            new NationalPark(16, "Death Valley National Park",              "California, Nevada",         1994),
//        //            new NationalPark(17, "Denali National Park & Preserve",         "Alaska",                     1917),
//        //            new NationalPark(18, "Dry Tortugas National Park",              "Florida",                    1992),
//        //            new NationalPark(19, "Everglades National Park",                "Florida",                    1947),
//        //            new NationalPark(20, "Gates of the Arctic National Park & Preserve","Alaska",                1980),
//        //            new NationalPark(21, "Gateway Arch National Park",              "Missouri",                   2018),
//        //            new NationalPark(22, "Glacier Bay National Park & Preserve",    "Alaska",                     1980),
//        //            new NationalPark(23, "Glacier National Park",                   "Montana",                    1910),
//        //            new NationalPark(24, "Grand Canyon National Park",              "Arizona",                    1919),
//        //            new NationalPark(25, "Grand Teton National Park",               "Wyoming",                    1929),
//        //            new NationalPark(26, "Great Basin National Park",               "Nevada",                     1986),
//        //            new NationalPark(27, "Great Sand Dunes National Park & Preserve","Colorado",                  2004),
//        //            new NationalPark(28, "Great Smoky Mountains National Park",     "Tennessee, North Carolina",  1934),
//        //            new NationalPark(29, "Guadalupe Mountains National Park",       "Texas",                      1972),
//        //            new NationalPark(30, "Haleakalā National Park",                 "Hawaii",                     1961),
//        //            new NationalPark(31, "Hawaiʻi Volcanoes National Park",         "Hawaii",                     1916),
//        //            new NationalPark(32, "Hot Springs National Park",               "Arkansas",                   1921),
//        //            new NationalPark(33, "Indiana Dunes National Park",             "Indiana",                    2019),
//        //            new NationalPark(34, "Isle Royale National Park",               "Michigan",                   1940),
//        //            new NationalPark(35, "Joshua Tree National Park",               "California",                 1994),
//        //            new NationalPark(36, "Katmai National Park & Preserve",         "Alaska",                     1980),
//        //            new NationalPark(37, "Kenai Fjords National Park",              "Alaska",                     1980),
//        //            new NationalPark(38, "Kings Canyon National Park",              "California",                 1940),
//        //            new NationalPark(39, "Kobuk Valley National Park",              "Alaska",                     1980),
//        //            new NationalPark(40, "Lake Clark National Park & Preserve",     "Alaska",                     1980),
//        //            new NationalPark(41, "Lassen Volcanic National Park",           "California",                 1916),
//        //            new NationalPark(42, "Mammoth Cave National Park",              "Kentucky",                   1941),
//        //            new NationalPark(43, "Mesa Verde National Park",                "Colorado",                   1906),
//        //            new NationalPark(44, "Mount Rainier National Park",             "Washington",                 1899),
//        //            new NationalPark(45, "New River Gorge National Park & Preserve","West Virginia",              2020),
//        //            new NationalPark(46, "North Cascades National Park",            "Washington",                 1968),
//        //            new NationalPark(47, "Olympic National Park",                   "Washington",                 1938),
//        //            new NationalPark(48, "Petrified Forest National Park",          "Arizona",                    1962),
//        //            new NationalPark(49, "Pinnacles National Park",                 "California",                 2013),
//        //            new NationalPark(50, "Redwood National Park",                   "California",                 1968),
//        //            new NationalPark(51, "Rocky Mountain National Park",            "Colorado",                   1915),
//        //            new NationalPark(52, "Saguaro National Park",                   "Arizona",                    1994),
//        //            new NationalPark(53, "Sequoia National Park",                   "California",                 1890),
//        //            new NationalPark(54, "Shenandoah National Park",                "Virginia",                   1935),
//        //            new NationalPark(55, "Theodore Roosevelt National Park",        "North Dakota",               1978),
//        //            new NationalPark(56, "Virgin Islands National Park",            "U.S. Virgin Islands",        1956),
//        //            new NationalPark(57, "Voyageurs National Park",                 "Minnesota",                  1975),
//        //            new NationalPark(58, "White Sands National Park",               "New Mexico",                 2019),
//        //            new NationalPark(59, "Wind Cave National Park",                 "South Dakota",               1903),
//        //            new NationalPark(60, "Wrangell–St. Elias National Park & Preserve","Alaska",                 1980),
//        //            new NationalPark(61, "Yellowstone National Park",               "Wyoming, Montana, Idaho",    1872),
//        //            new NationalPark(62, "Yosemite National Park",                  "California",                 1890),
//        //            new NationalPark(63, "Zion National Park",                      "Utah",                       1919)
//        //    };
//        //}

//        public IEnumerable<NationalPark> GetAll() => _parks;

//        public NationalPark? GetById(int id) => _parks.FirstOrDefault(p => p.Id == id);


//        public NationalPark Add(NationalPark park)
//        {
//            int newId = _parks.Any() ? _parks.Max(p => p.Id) + 1 : 1;
//            var newPark = new NationalPark(newId, park.Name, park.State, park.YearEstablished);
//            _parks.Add(newPark);
//            return newPark;
//        }

//        public NationalPark? Update(int id, NationalPark park)
//        {
//            var index = _parks.FindIndex(p => p.Id == id);
//            if (index == -1)
//                return null;

//            // Modify the existing park directly
//            _parks[index].Name = park.Name;
//            _parks[index].State = park.State;
//            _parks[index].YearEstablished = park.YearEstablished;

//            // ID remains the same
//            return _parks[index];
//        }


//        public bool Delete(int id)
//        {
//            var park = GetById(id);
//            if (park == null) return false;
//            _parks.Remove(park);
//            return true;
//        }

//    }
//}