using Candidate_BusinessObjects;
using Candidate_Service;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CandidateManagement_WPF_TDC
{
    public partial class JobPostingWindow : Window
    {
        private readonly int? roleid;
        private readonly IJobpostService jobpostService;

        public JobPostingWindow(int? roleid)
        {
            InitializeComponent();
            this.roleid = roleid;
            jobpostService = new JobPostService();

            // Configure permissions based on role
            ConfigureRolePermissions();

            // Load data immediately after initialization
            LoadJobPostings();
        }

        private void ConfigureRolePermissions()
        {
            switch (roleid)
            {
                case 1: // Admin role ID
                    btn_delete.IsEnabled = true;
                    break;
                case 2: // Manager role ID
                    btn_add.IsEnabled = true;
                    btn_delete.IsEnabled = true;
                    break;
                default:
                    btn_add.IsEnabled = false;
                    btn_delete.IsEnabled = false;
                    break;
            }
        }

        private void LoadJobPostings()
        {
            try
            {
                var jobPostings = jobpostService.GetJobPostings();
                if (jobPostings != null)
                {
                    ListPost.ItemsSource = jobPostings;
                }
                else
                {
                    MessageBox.Show("No job postings found or error loading data.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading job postings: {ex.Message}");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshJobPostings();
        }

        private void RefreshJobPostings()
        {
            // Fetch job postings and bind to DataGrid
            ListPost.ItemsSource = jobpostService.GetJobPostings();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            JobPosting jobpost = CollectJobPostingData();
            if (jobpost == null) return;

            if (jobpostService.Create(jobpost))
            {
                MessageBox.Show("Job posting added successfully!");
                RefreshJobPostings();
            }
            else
            {
                MessageBox.Show("Failed to add job posting.");
            }
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            JobPosting jobpost = CollectJobPostingData();
            if (jobpost == null) return;

            if (jobpostService.UpdateJobpost(jobpost))
            {
                MessageBox.Show("Job posting updated successfully!");
                RefreshJobPostings();
            }
            else
            {
                MessageBox.Show("Failed to update job posting.");
            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if (ListPost.SelectedItem == null)
            {
                MessageBox.Show("Please select a job post to delete.");
                return;
            }

            // Get the full selected job posting
            JobPosting selectedJob = (JobPosting)ListPost.SelectedItem;

            // Confirm deletion
            MessageBoxResult result = MessageBox.Show(
                $"Are you sure you want to delete the job posting '{selectedJob.JobPostingTitle}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (jobpostService.DeleteJobPost(selectedJob))
                    {
                        MessageBox.Show("Job posting deleted successfully!");
                        RefreshJobPostings();
                        ClearInputFields();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete job posting. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting job posting: {ex.Message}");
                }
            }
        }

        private void btn_Search_Click_1(object sender, RoutedEventArgs e)
        {
            string searchText = txt_search.Text;
            if (!string.IsNullOrEmpty(searchText))
            {
                ListPost.ItemsSource = jobpostService.searchJobPostingByJobPostingTitle(searchText);
            }
            else
            {
                RefreshJobPostings();
            }
        }

        private void btn_Refresh_Click(object sender, RoutedEventArgs e)
        {
            ClearInputFields();
            RefreshJobPostings();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListPost.SelectedItem is JobPosting selectedJobPost)
            {
                txt_PostID.Text = selectedJobPost.PostingId;
                txt_PostID.IsReadOnly = true;
                txt_Jobtitle.Text = selectedJobPost.JobPostingTitle;
                date_Post.SelectedDate = selectedJobPost.PostedDate;

                // Update RichTextBox for Description
                txt_description.Document.Blocks.Clear();
                txt_description.Document.Blocks.Add(new Paragraph(new Run(selectedJobPost.Description)));
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private JobPosting CollectJobPostingData()
        {
            if (string.IsNullOrWhiteSpace(txt_PostID.Text) ||
                string.IsNullOrWhiteSpace(txt_Jobtitle.Text) ||
                !date_Post.SelectedDate.HasValue)
            {
                MessageBox.Show("Please fill in all required fields.");
                return null;
            }

            TextRange textRange = new TextRange(txt_description.Document.ContentStart, txt_description.Document.ContentEnd);
            return new JobPosting
            {
                PostingId = txt_PostID.Text,
                JobPostingTitle = txt_Jobtitle.Text,
                Description = textRange.Text.Trim(),
                PostedDate = date_Post.SelectedDate.Value
            };
        }

        private void ClearInputFields()
        {
            txt_PostID.Text = string.Empty;
            txt_PostID.IsReadOnly = false;
            txt_Jobtitle.Text = string.Empty;
            date_Post.SelectedDate = null;
            txt_description.Document.Blocks.Clear();
        }

        private void btn_logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Confirm Logout",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Create and show the main window
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                // Close the current window
                this.Close();
            }
        }
    }
}