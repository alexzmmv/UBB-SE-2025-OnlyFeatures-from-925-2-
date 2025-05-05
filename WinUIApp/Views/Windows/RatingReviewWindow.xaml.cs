// <copyright file="RatingReviewWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WinUIApp.Views.Windows
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using WinUIApp.Views.Windows;
    using WinUIApp.ViewModels;
    using WinUIApp.Models;
    using CommunityToolkit.WinUI;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the main window of the application.
    /// </summary>
    public sealed partial class RatingReviewWindow : Window
    {
        private const int MaxContentLength = 500;
        private readonly MainRatingReviewViewModel MainRatingReviewViewModel;
        private readonly int productId; // Add productId field

        public RatingReviewWindow(MainRatingReviewViewModel mainRatingReviewViewModel, int productId)
        {
            if (mainRatingReviewViewModel == null)
                throw new ArgumentNullException(nameof(mainRatingReviewViewModel));
            if (mainRatingReviewViewModel.RatingViewModel == null)
                throw new ArgumentException("RatingViewModel cannot be null", nameof(mainRatingReviewViewModel));
            if (mainRatingReviewViewModel.ReviewViewModel == null)
                throw new ArgumentException("ReviewViewModel cannot be null", nameof(mainRatingReviewViewModel));

            this.InitializeComponent();
            this.MainRatingReviewViewModel = mainRatingReviewViewModel;
            this.productId = productId; // Store the productId
            this.rootGrid.DataContext = mainRatingReviewViewModel;

            // Initialize ratings for this product
            mainRatingReviewViewModel.RatingViewModel.LoadRatingsForProduct(productId);
        }

        /// <summary>
        /// Handles the click event for the Add Review button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private async void AddReview_Click(object sender, RoutedEventArgs e)
        {
            if (this.MainRatingReviewViewModel.SelectedRating != null)
            {
                var reviewWindow = new ReviewWindow(
                    this.MainRatingReviewViewModel.Configuration,
                    this.MainRatingReviewViewModel.RatingViewModel,
                    this.MainRatingReviewViewModel.ReviewViewModel);
                reviewWindow.Activate();
            }
            else
            {
                await this.NoRatingSelectedDialog.ShowAsync();
            }
        }

        /// <summary>
        /// Handles the click event for the Add Rating button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void AddRating_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.MainRatingReviewViewModel == null)
                {
                    throw new InvalidOperationException("MainRatingReviewViewModel is null");
                }

                if (this.MainRatingReviewViewModel.RatingViewModel == null)
                {
                    throw new InvalidOperationException("RatingViewModel is null");
                }

                this.MainRatingReviewViewModel.ClearSelectedRating();
                // Pass the productId to RatingWindow constructor
                var ratingWindow = new RatingWindow(
                    this.MainRatingReviewViewModel.RatingViewModel,
                    this.productId);
                ratingWindow.Activate();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in AddRating_Click: {ex}");
                _ = this.ShowErrorDialogAsync($"Failed to open rating window: {ex.Message}");
            }
        }

        private async Task ShowErrorDialogAsync(string message)
        {
            await this.DispatcherQueue.EnqueueAsync(async () =>
            {
                var dialog = new ContentDialog
                {
                    Title = "Error",
                    Content = message,
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
            });
        }

        /// <summary>
        /// Handles the rating selection changed event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="eventArguments">The event arguments.</param>
        private void RatingSelection_Changed(object sender, RoutedEventArgs eventArguments)
        {
            if (sender is ListView listView)
            {
                this.MainRatingReviewViewModel.HandleRatingSelection(listView);
            }
        }
    }
}