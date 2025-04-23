using Xunit;
using MovieLibraryApp;

namespace MovieLibraryTests
{
    public class UnitTest1   // First test
    {
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
public void SearchByTitle_ShouldWork(string title, int id)
{
    var lib = new MovieLibrary();
    lib.AddMovie(new Movie(id, title, "Dir", "Genre", 2020, true));
    var result = lib.SearchByTitle(title);
    Assert.NotNull(result);
    Assert.Equal(id, result.MovieID);
}

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


    }
}

