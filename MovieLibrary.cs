using System;
using System.Collections.Generic;
using System.Linq;

namespace MovieLibraryApp
{
    public class MovieLibrary
    {
        private readonly MyHashTable<int, Movie> movieHashtable = new();
        private LinkedList<Movie> Movies = new();

        // Add a movie to the library
        public void AddMovie(Movie movie)
        {
            Movies.AddLast(movie);
            movieHashtable.Insert(movie.MovieID, movie);
        }

        // Search methods

        public Movie SearchByTitle(string title)
        {
            return Movies.FirstOrDefault(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public Movie SearchByMovieID(int movieID)
        {
            return movieHashtable.ContainsKey(movieID) ? movieHashtable.Get(movieID) : null;
        }

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
    }
}
