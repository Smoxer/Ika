namespace Ikariam
{
    public class City
    {
        public enum BUILDINGS : int
        {
            TOWN_HALL = 0,
            TRADING_PORT = 3,
            ACADEMY = 4,
            SHIPYARD = 5,
            BARRACKS = 6,
            WAREHOUSE = 7,
            TOWN_WALL = 8,
            TAVERN = 9,
            MUSEUM = 10,
            PALACE = 11,
            EMBASSY = 12,
            TRADING_POST = 13,
            WORKSHOP = 15,
            HIDEOUT = 16,
            GOVERNOR_RESIDENCE = 17,
            FORESTER_HOUSE = 18,
            STONEMASON = 19,
            GLASSBLOWER = 20,
            WINEGROWER = 21,
            ALCHEMIST_TOWER = 22,
            CARPENTER = 23,
            ARCHITECT_OFFICE = 24,
            OPTICIAN = 25,
            WINE_PRESS = 26,
            FIREWORK_TEST_AREA = 27,
            TEMPLE = 28,
            DUMP = 29,
            PIRATE_FORTRESS = 30,
            BLACK_MARKET = 31,
            SEA_CHART_ARCHIVE = 32
        };
        public enum POSITIONS : int
        {
            CITY_HALL = 0,
            RIGHT_PORT = 1,
            LEFT_PORT = 2,
            FREE_SPACE_3 = 3,
            FREE_SPACE_4 = 4,
            FREE_SPACE_5 = 5,
            FREE_SPACE_6 = 6,
            FREE_SPACE_7 = 7,
            FREE_SPACE_8 = 8,
            FREE_SPACE_9 = 9,
            FREE_SPACE_10 = 10,
            FREE_SPACE_11 = 11,
            FREE_SPACE_12 = 12,
            BUREAUCRACY_SPACE = 13,
            WALL = 14,
            FREE_SPACE_15 = 15,
            FREE_SPACE_16 = 16,
            PIRATE_FORTRESS = 17,
            FREE_SPACE_18 = 18
        };

        /// <summary>
        /// City ID
        /// </summary>
        public string ID;

        /// <summary>
        /// City name
        /// </summary>
        public string Name;

        /// <summary>
        /// City coordinates
        /// </summary>
        public string Cords;

        /// <summary>
        /// City's island ID
        /// </summary>
        public string IslandId;

        /// <summary>
        /// If city has pirate fortress
        /// </summary>
        public bool HasPirate;

        /// <summary>
        /// Initialize city
        /// </summary>
        public City(string Name, string ID, string Cords, string IslandId, bool HasPirate)
        {
            this.Name = Name;
            this.ID = ID;
            this.Cords = Cords;
            this.IslandId = IslandId;
            this.HasPirate = HasPirate;
        }
    }
}
