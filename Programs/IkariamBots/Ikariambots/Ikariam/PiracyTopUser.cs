namespace Ikariam
{
    public class PiracyTopUser
    {
        public string place, oldPlace, avatarId, capturePoints, name, distance, cityId, islandId, server;

        public PiracyTopUser(string place, string oldPlace, string avatarId, string capturePoints, string name, string distance, string cityId, string islandId, string server)
        {
            this.place = place;
            this.oldPlace = oldPlace;
            this.avatarId = avatarId;
            this.capturePoints = capturePoints;
            this.name = name;
            this.distance = distance.Split('.')[0];
            this.cityId = cityId;
            this.islandId = islandId;
            this.server = server;
        }
    }
}
