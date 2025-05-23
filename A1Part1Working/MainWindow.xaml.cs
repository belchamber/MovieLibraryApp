﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace MovieLibraryApp
{
    public partial class MainWindow : Window
    {
        private MovieLibrary movieLibrary;

        // Constructor: initializes UI and loads movie list
        public MainWindow()
        {
            InitializeComponent();
            movieLibrary = new MovieLibrary();

            // Hardcoded default movies
            movieLibrary.AddMovie(new Movie(1, "Oppenheimer", "Christopher Nolan", "Drama", 2023, true));
            movieLibrary.AddMovie(new Movie(2, "Inception", "Christopher Nolan", "Sci-Fi", 2010, true));
            movieLibrary.AddMovie(new Movie(3, "Parasite", "Bong Joon-ho", "Thriller", 2019, true));
            movieLibrary.AddMovie(new Movie(4, "The Dark Knight", "Christopher Nolan", "Action", 2008, true));

            MoviesDataGrid.ItemsSource = movieLibrary.GetAllMovies();
        }

        // Called when the 'Add Movie' button is clicked
        private void BubbleSortButton_Click(object sender, RoutedEventArgs e)
        {
            movieLibrary.BubbleSortByTitle();
            RefreshGrid();
        }

        // Called when the 'Merge Sort' button is clicked – sorts movies by release year
        private void MergeSortButton_Click(object sender, RoutedEventArgs e)
        {
            movieLibrary.MergeSortByReleaseYear();
            RefreshGrid();
        }

        // Called when the 'Search by Title' button is clicked
        private void SearchByTitle_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text.Trim();
            if (string.IsNullOrEmpty(title))
            {
                MessageBox.Show("Please enter a title.");
                return;
            }

            var movie = movieLibrary.SearchByTitle(title);
            MoviesDataGrid.ItemsSource = movie != null ? new List<Movie> { movie } : movieLibrary.GetAllMovies();

            if (movie == null)
            {
                MessageBox.Show("Movie not found.");
            }
        }

        // Called when the 'Search by ID' button is clicked
        private void SearchByMovieID_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(MovieIDTextBox.Text, out int movieID))
            {
                MessageBox.Show("Invalid Movie ID. Please enter a number.");
                return;
            }

            var movie = movieLibrary.SearchByMovieID(movieID);
            MoviesDataGrid.ItemsSource = movie != null ? new List<Movie> { movie } : movieLibrary.GetAllMovies();

            if (movie == null)
            {
                MessageBox.Show("Movie not found.");
            }
        }

            // Called when the 'Binary Search by ID' button is clicked
        private void BinarySearchByID_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(MovieIDTextBox.Text, out int movieID))
            {
                MessageBox.Show("Invalid Movie ID. Please enter a number.");
                return;
            }

            var movie = movieLibrary.BinarySearchByMovieID(movieID);
            MoviesDataGrid.ItemsSource = movie != null ? new List<Movie> { movie } : movieLibrary.GetAllMovies();

            if (movie == null)
            {
                MessageBox.Show("Movie not found.");
            }
        }

        // Called when the 'Borrow Movie' button is clicked
        private void BorrowMovie_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(MovieIDTextBox.Text, out int movieID) || string.IsNullOrEmpty(UserNameTextBox.Text))
            {
                MessageBox.Show("Please enter both a valid Movie ID and a User Name.");
                return;
            }

            bool success = movieLibrary.BorrowMovie(movieID, UserNameTextBox.Text.Trim());
            MessageBox.Show(success ? "Movie borrowed successfully." : "Movie is currently unavailable and you were added to the waitlist.");
            RefreshGrid();
        }

        // Called when the 'Return Movie' button is clicked
        private void ReturnMovie_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(MovieIDTextBox.Text, out int movieID))
            {
                MessageBox.Show("Please enter a valid Movie ID.");
                return;
            }

            movieLibrary.ReturnMovie(movieID);
            RefreshGrid();
        }

        // Called when the 'Export' button is clicked – saves to JSON or XML
        private void ExportData_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                FileName = "Movies",
                DefaultExt = ".json",
                Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml"
            };

            if (dlg.ShowDialog() == true)
            {
                if (dlg.FileName.ToLower().EndsWith(".json"))
                    movieLibrary.ExportToJson(dlg.FileName);
                else
                    movieLibrary.ExportToXml(dlg.FileName);

                MessageBox.Show("Data exported successfully.");
            }
        }

        // Called when the 'Import' button is clicked – loads from JSON or XML

        private void ImportData_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml"
            };

            if (dlg.ShowDialog() == true)
            {
                if (dlg.FileName.ToLower().EndsWith(".json"))
                    movieLibrary.ImportFromJson(dlg.FileName);
                else
                    movieLibrary.ImportFromXml(dlg.FileName);

                RefreshGrid();
                MessageBox.Show("Data imported successfully.");
            }
        }

        // Updates input boxes and queue label when a movie is selected from the grid
        private void RefreshGrid()
        {
            MoviesDataGrid.ItemsSource = null;
            MoviesDataGrid.ItemsSource = movieLibrary.GetAllMovies();
        }

        // Updates input boxes and queue label when a movie is selected from the grid
        private void MoviesDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (MoviesDataGrid.SelectedItem is Movie selectedMovie)
            {
                // Update label
                NextInLineLabel.Content = !string.IsNullOrEmpty(selectedMovie.NextInQueue)
                    ? $"Next in line: {selectedMovie.NextInQueue}"
                    : "No one in queue.";

                // ✅ Populate search boxes
                TitleTextBox.Text = selectedMovie.Title;
                MovieIDTextBox.Text = selectedMovie.MovieID.ToString();
            }
        }

        // Clears input fields and resets the DataGrid to show all movies
        private void ResetList_Click(object sender, RoutedEventArgs e)
        {
            TitleTextBox.Clear();
            MovieIDTextBox.Clear();
            UserNameTextBox.Clear();
            NextInLineLabel.Content = string.Empty;
            MoviesDataGrid.ItemsSource = null;
            MoviesDataGrid.ItemsSource = movieLibrary.GetAllMovies();
        }
    }
}
