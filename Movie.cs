using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace MovieLibraryApp
{
    public class Movie
    {
        public int MovieID { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }
        public bool Availability { get; set; }

        [XmlArray("WaitingQueue")]
        [XmlArrayItem("string")]
        public List<string> WaitingQueueList { get; set; } = new List<string>();

        [XmlIgnore]
        [JsonIgnore]
        public Queue<string> WaitingQueue
        {
            get => new Queue<string>(WaitingQueueList ?? new List<string>());
            set => WaitingQueueList = value != null ? new List<string>(value) : new List<string>();
        }

        [XmlIgnore, JsonIgnore] public int WaitingCount => WaitingQueue?.Count ?? 0;
        [XmlIgnore, JsonIgnore] public string CheckedOutTo { get; set; }
        [XmlIgnore, JsonIgnore] public string NextInQueue => WaitingQueue.Count > 0 ? WaitingQueue.Peek() : "";

        public Movie() { }

        public Movie(int movieID, string title, string director, string genre, int releaseYear, bool availability)
        {
            MovieID = movieID;
            Title = title;
            Director = director;
            Genre = genre;
            ReleaseYear = releaseYear;
            Availability = availability;
        }
    }
}