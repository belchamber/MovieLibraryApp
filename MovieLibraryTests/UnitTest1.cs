using Xunit;
using MovieLibraryApp;
using System.Collections.Generic;

namespace MovieLibraryTests
{
    public class UnitTest1
    {
        // Tests that a movie can be added and retrieved by ID
        [Fact]
        public void AddMovie_ShouldBeRetrievableByID()
        {
            var library = new MovieLibrary();
            var movie = new Movie(1, "Interstellar", "Nolan", "Sci-Fi", 2014, true);

            library.AddMovie(movie);
            var retrieved = library.SearchByMovieID(1);

            Assert.NotNull(retrieved);
            Assert.Equal("Interstellar", retrieved.Title);
        }

        [Theory]
        [InlineData("Inception", 1)]
        [InlineData("Avatar", 2)]
    
        // Tests title-based movie search
        public void SearchByTitle_ShouldWork(string title, int id)
        {
            var lib = new MovieLibrary();
            lib.AddMovie(new Movie(id, title, "Dir", "Genre", 2020, true));
            var result = lib.SearchByTitle(title);

            Assert.NotNull(result);
            Assert.Equal(id, result.MovieID);
        }

        // Tests that bubble sort - sorts titles correctly
        [Fact]
        public void BubbleSort_ShouldSortTitles()
        {
            var lib = new MovieLibrary();
            lib.AddMovie(new Movie(1, "Zebra", "D", "G", 2000, true));
            lib.AddMovie(new Movie(2, "Apple", "D", "G", 2001, true));

            lib.BubbleSortByTitle();
            var sorted = lib.GetAllMovies();

            Assert.Equal("Apple", sorted[0].Title);
            Assert.Equal("Zebra", sorted[1].Title);
        }

        // Tests that merge sort sorts movies by year
        [Fact]
        public void MergeSort_ShouldSortByYear()
        {
            var lib = new MovieLibrary();
            lib.AddMovie(new Movie(1, "Old", "D", "G", 1990, true));
            lib.AddMovie(new Movie(2, "New", "D", "G", 2020, true));

            lib.MergeSortByReleaseYear();
            var sorted = lib.GetAllMovies();

            Assert.Equal(1990, sorted[0].ReleaseYear);
            Assert.Equal(2020, sorted[1].ReleaseYear);
        }

        // Tests borrowing a movie that is available
        [Fact]
        public void BorrowAvailableMovie_ShouldWork()
        {
            var lib = new MovieLibrary();
            var m = new Movie(1, "A", "D", "G", 2000, true);
            lib.AddMovie(m);

            bool result = lib.BorrowMovie(1, "User");

            Assert.True(result);
            Assert.False(m.Availability);
            Assert.Equal("User", m.CheckedOutTo);
        }

        // Tests borrowing a movie that is already borrowed adds user to queue
        [Fact]
        public void BorrowUnavailable_ShouldAddToQueue()
        {
            var m = new Movie(2, "B", "D", "G", 2000, false)
            {
                CheckedOutTo = "User1"
            };
            var lib = new MovieLibrary();
            lib.AddMovie(m);

            bool result = lib.BorrowMovie(2, "User2");

            Assert.False(result);
            Assert.Equal("User2", m.NextInQueue);
        }

        // Tests that returning a movie with a waiting list assigns it to the next user
        [Fact]
        public void Return_WithWaitingList_ShouldAssignNext()
        {
            var m = new Movie(3, "C", "D", "G", 2000, false)
            {
                CheckedOutTo = "A",
                WaitingQueueList = new List<string> { "B", "C" }
            };
            var lib = new MovieLibrary();
            lib.AddMovie(m);

            lib.ReturnMovie(3);

            Assert.False(m.Availability);
            Assert.Equal("B", m.CheckedOutTo);
            Assert.Single(m.WaitingQueue);
        }

        // Tests that returning a movie with no queue makes it available
        [Fact]
        public void Return_NoQueue_ShouldMakeAvailable()
        {
            var m = new Movie(4, "D", "D", "G", 2000, false)
            {
                CheckedOutTo = "User"
            };
            var lib = new MovieLibrary();
            lib.AddMovie(m);

            lib.ReturnMovie(4);

            Assert.True(m.Availability);
            Assert.Null(m.CheckedOutTo);
        }

        // Tests that adding a movie with an existing ID replaces the old one
        [Fact]
        public void DuplicateID_ShouldOverwrite()
        {
            var lib = new MovieLibrary();
            lib.AddMovie(new Movie(5, "Old", "D", "G", 1990, true));
            lib.AddMovie(new Movie(5, "New", "D", "G", 2020, true));

            var movie = lib.SearchByMovieID(5);
            Assert.Equal("New", movie.Title);
        }

        // Tests borrowing from an empty library fails
        [Fact]
        public void BorrowFromEmptyLibrary_ShouldFail()
        {
            var lib = new MovieLibrary();
            bool result = lib.BorrowMovie(999, "User");

            Assert.False(result);
        }

         // Tests returning an invalid movie ID doesn't crash the system
        [Fact]
        public void ReturnInvalidID_ShouldNotCrash()
        {
            var lib = new MovieLibrary();
            var ex = Record.Exception(() => lib.ReturnMovie(12345));

            Assert.Null(ex);
        }
        
       // Tests hashtable throws error for missing key
        [Fact]
        public void InvalidKeyInHashTable_ShouldThrow()
        {
            var map = new MyHashTable<int, string>();

            Assert.Throws<KeyNotFoundException>(() => map.Get(1));
        }
    }
}
