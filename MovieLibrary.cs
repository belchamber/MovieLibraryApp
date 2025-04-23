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

        // Bubble Sort by Title
        public void BubbleSortByTitle()
        {
            var list = Movies.ToList();
            for (int i = 0; i < list.Count - 1; i++)
                for (int j = 0; j < list.Count - i - 1; j++)
                    if (string.Compare(list[j].Title, list[j + 1].Title) > 0)
                        (list[j], list[j + 1]) = (list[j + 1], list[j]);
            Movies = new LinkedList<Movie>(list);
        }

        // Merge Sort by Release Year
        public void MergeSortByReleaseYear()
        {
            Movies = new LinkedList<Movie>(MergeSort(Movies.ToList()));
        }

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
    }
}
