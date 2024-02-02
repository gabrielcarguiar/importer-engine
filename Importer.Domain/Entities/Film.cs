namespace Importer.Domain.Entities
{
    public class Film
    {
        public Film(string name, int timesWatched, Guid userId)
        {
            Name = name;
            TimesWatched = timesWatched;
            UserId = userId;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public int TimesWatched { get; private set; }
        public Guid UserId { get; private set; }
    }
}
