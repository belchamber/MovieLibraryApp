using Xunit;
using MovieLibraryApp;

namespace MovieLibraryTests
{
    public class UnitTest1
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
    }
}

