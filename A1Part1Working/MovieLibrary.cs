using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace MovieLibraryApp
{
    public class MovieLibrary
    {
        private readonly MyHashTable<int, Movie> movieHashtable = new();
        private LinkedList<Movie> Movies = new();

        public void AddMovie(Movie movie)
        {
            Movies.AddLast(movie);
            movieHashtable.Insert(movie.MovieID, movie);
        }


        // Search functionality for title and ID

        public Movie SearchByTitle(string title) => Movies.FirstOrDefault(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        public Movie SearchByMovieID(int movieID) => movieHashtable.ContainsKey(movieID) ? movieHashtable.Get(movieID) : null;

        public Movie BinarySearchByMovieID(int movieID)
        {
            var sorted = Movies.OrderBy(m => m.MovieID).ToList();
            int low = 0, high = sorted.Count - 1;
            while (low <= high)
            {
                int mid = (low + high) / 2;
                if (sorted[mid].MovieID == movieID) return sorted[mid];
                if (sorted[mid].MovieID < movieID) low = mid + 1; else high = mid - 1;
            }
            return null;
        }

        public bool BorrowMovie(int movieID, string userName)
        {
            if (!movieHashtable.ContainsKey(movieID)) return false;
            var movie = movieHashtable.Get(movieID);
            if (movie.Availability)
            {
                movie.Availability = false;
                movie.CheckedOutTo = userName;
                return true;
            }

            var queue = movie.WaitingQueue;
            queue.Enqueue(userName);
            movie.WaitingQueue = queue; // Force update so NextInQueue and WaitingCount refresh

            return false;
        }

        public void ReturnMovie(int movieID)
        {
            if (!movieHashtable.ContainsKey(movieID)) return;
            var movie = movieHashtable.Get(movieID);
            if (movie.Availability) return;

            var queue = movie.WaitingQueue;
            if (queue.Count > 0)
            {
                string next = queue.Dequeue();
                movie.Availability = false;
                movie.CheckedOutTo = next;
                MessageBox.Show($"'{movie.Title}' reassigned to: {next}");
            }
            else
            {
                movie.Availability = true;
                movie.CheckedOutTo = null;
            }

            movie.WaitingQueue = queue; 
        }

        public void BubbleSortByTitle()
        {
            var list = Movies.ToList();
            for (int i = 0; i < list.Count - 1; i++)
                for (int j = 0; j < list.Count - i - 1; j++)
                    if (string.Compare(list[j].Title, list[j + 1].Title) > 0)
                        (list[j], list[j + 1]) = (list[j + 1], list[j]);
            Movies = new LinkedList<Movie>(list);
        }

        public void MergeSortByReleaseYear() => Movies = new LinkedList<Movie>(MergeSort(Movies.ToList()));

        private List<Movie> MergeSort(List<Movie> list)
        {
            if (list.Count <= 1) return list;
            int mid = list.Count / 2;
            var left = MergeSort(list.GetRange(0, mid));
            var right = MergeSort(list.GetRange(mid, list.Count - mid));
            return Merge(left, right);
        }

        private List<Movie> Merge(List<Movie> left, List<Movie> right)
        {
            var result = new List<Movie>();
            int i = 0, j = 0;
            while (i < left.Count && j < right.Count)
                result.Add(left[i].ReleaseYear <= right[j].ReleaseYear ? left[i++] : right[j++]);
            result.AddRange(left.GetRange(i, left.Count - i));
            result.AddRange(right.GetRange(j, right.Count - j));
            return result;
        }

        public List<Movie> GetAllMovies() => Movies.ToList();

        public void ExportToJson(string path) => File.WriteAllText(path, JsonConvert.SerializeObject(Movies.ToList(), Formatting.Indented));

        public void ImportFromJson(string path)
        {
            var movies = JsonConvert.DeserializeObject<List<Movie>>(File.ReadAllText(path));
            foreach (var m in movies)
            {
                m.Availability = true; 
                m.CheckedOutTo = null; 
                m.WaitingQueueList = new();
                AddMovie(m);
            }
        }

        public void ExportToXml(string path)
        {
            var serializer = new XmlSerializer(typeof(List<Movie>));
            using var writer = new StreamWriter(path);
            serializer.Serialize(writer, Movies.ToList());
        }

        public void ImportFromXml(string path)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<Movie>));
                using var fs = new FileStream(path, FileMode.Open);
                var movies = (List<Movie>)serializer.Deserialize(fs);
                foreach (var m in movies)
                {
                    m.Availability = true; 
                    m.CheckedOutTo = null; 
                    m.WaitingQueueList = new();
                    AddMovie(m);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Import failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
