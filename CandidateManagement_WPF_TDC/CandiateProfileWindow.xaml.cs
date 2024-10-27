using Candidate_BusinessObjects;
using Candidate_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CandidateManagement_WPF_TDC
{
    
    public partial class CandidateProfileWindow : Window
    {
        private IHRCandidateService hRCandidateService;
        private IJobpostService jobpostService;
        // Add this field to cache the original list
        private List<CandidateProfile> _allCandidates;

        public CandidateProfileWindow()
        {
            InitializeComponent();
            hRCandidateService = new HRCandidateService();
            jobpostService = new JobPostService();
            HandleBeforeLoaded();
        }

        public void UpdateGridView()
        {
            _allCandidates = hRCandidateService.GetCandidateProfiles().ToList();
            ListViewCandidate.ItemsSource = _allCandidates;
        }

        private void HandleBeforeLoaded()
        {
            UpdateGridView();
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            CandidateProfile can = new CandidateProfile
            {
                CandidateId = txt_CanID.Text,
                Fullname = txt_fullname.Text
            };

            DateTime.TryParse(date_Birth.Text, out DateTime birthDate);
            can.Birthday = birthDate != DateTime.MinValue ? birthDate : DateTime.Now;

            TextRange textRange = new TextRange(txt_description.Document.ContentStart, txt_description.Document.ContentEnd);
            can.ProfileShortDescription = textRange.Text;

            can.ProfileUrl = txt_url.Text;

            var selectedPosting = cb_jobPosting.SelectedValue;
            if (selectedPosting == null)
            {
                MessageBox.Show("Please fill in the blank");
                return;
            }

            can.PostingId = selectedPosting.ToString();
            if (hRCandidateService.Create(can))
            {
                MessageBox.Show("Added Successfully!");
                UpdateGridView();
            }
            else
            {
                MessageBox.Show("Failed to add this entry!");
            }
        }

        private void Load_ListCandidate_Loaded(object sender, RoutedEventArgs e)
        {
            // Load all candidates and cache them
            _allCandidates = hRCandidateService.GetCandidateProfiles().ToList();
            ListViewCandidate.ItemsSource = _allCandidates;

            // Load job postings for both main form and search
            var jobPostings = jobpostService.GetJobPostings();

            // Setup main job posting combo
            this.cb_jobPosting.ItemsSource = jobPostings;
            this.cb_jobPosting.DisplayMemberPath = "JobPostingTitle";
            this.cb_jobPosting.SelectedValuePath = "PostingId";

            // Setup search job posting combo
            this.cb_SearchJobPosting.ItemsSource = new List<JobPosting>(jobPostings);
            this.cb_SearchJobPosting.DisplayMemberPath = "JobPostingTitle";
            this.cb_SearchJobPosting.SelectedValuePath = "PostingId";
        }

        private void btn_Search_Click_1(object sender, RoutedEventArgs e)
        {
            var searchResults = _allCandidates;

            // Search by ID
            if (!string.IsNullOrWhiteSpace(txt_SearchId.Text))
            {
                searchResults = searchResults.Where(c =>
                    c.CandidateId.ToLower().Contains(txt_SearchId.Text.ToLower())).ToList();
            }

            // Search by Name
            if (!string.IsNullOrWhiteSpace(txt_SearchName.Text))
            {
                searchResults = searchResults.Where(c =>
                    c.Fullname.ToLower().Contains(txt_SearchName.Text.ToLower())).ToList();
            }

            // Search by Job Posting
            if (cb_SearchJobPosting.SelectedValue != null)
            {
                var selectedPostingId = cb_SearchJobPosting.SelectedValue.ToString();
                searchResults = searchResults.Where(c =>
                    c.PostingId == selectedPostingId).ToList();
            }

            // Update ListView with search results
            ListViewCandidate.ItemsSource = searchResults;

            // Show results count
            MessageBox.Show($"Found {searchResults.Count} matching candidates.");
        }

        private void btn_update_Click(object sender, RoutedEventArgs e)
        {
            CandidateProfile can = new CandidateProfile
            {
                CandidateId = txt_CanID.Text,
                Fullname = txt_fullname.Text,
                Birthday = DateTime.Parse(date_Birth.Text)
            };

            TextRange textRange = new TextRange(txt_description.Document.ContentStart, txt_description.Document.ContentEnd);
            can.ProfileShortDescription = textRange.Text;
            can.ProfileUrl = txt_url.Text;

            var selectedPosting = cb_jobPosting.SelectedValue;
            if (selectedPosting == null)
            {
                MessageBox.Show("Please select a Job Posting");
                return;
            }

            can.PostingId = selectedPosting.ToString();
            if (hRCandidateService.UpdateCandidate(can))
            {
                MessageBox.Show("Update Successfully!");
                UpdateGridView();
            }
            else
            {
                MessageBox.Show("Update Error, Failed!");
            }
        }

        private void btn_ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            // Clear search fields
            txt_SearchId.Clear();
            txt_SearchName.Clear();
            cb_SearchJobPosting.SelectedIndex = -1;

            // Reset ListView to show all candidates
            ListViewCandidate.ItemsSource = _allCandidates;
        }
        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            CandidateProfile can = new CandidateProfile { CandidateId = txt_CanID.Text };
            if (hRCandidateService.DeleteCandidate(can))
            {
                MessageBox.Show("Delete Success");
                UpdateGridView();
            }
            else
            {
                MessageBox.Show("This entry can't be deleted!");
            }
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cb_jobPosting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Code for handling selection change, if needed.
        }

        private void ListViewCandidate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCandidate = (CandidateProfile)ListViewCandidate.SelectedItem;
            if (selectedCandidate != null)
            {
                // Update all fields with the selected candidate's information
                txt_CanID.Text = selectedCandidate.CandidateId;
                txt_CanID.IsReadOnly = true;
                txt_fullname.Text = selectedCandidate.Fullname;
                date_Birth.SelectedDate = selectedCandidate.Birthday;
                cb_jobPosting.SelectedValue = selectedCandidate.PostingId;
                txt_url.Text = selectedCandidate.ProfileUrl;

                // Update RichTextBox
                txt_description.Document.Blocks.Clear();
                txt_description.Document.Blocks.Add(new Paragraph(new Run(selectedCandidate.ProfileShortDescription)));
            }
        }

        private void btn_NewCandidate_Click(object sender, RoutedEventArgs e)
        {
            // Clear all fields and enable Candidate ID textbox
            txt_CanID.IsReadOnly = false;
            txt_CanID.Text = string.Empty;
            txt_fullname.Text = string.Empty;
            date_Birth.SelectedDate = DateTime.Now;
            cb_jobPosting.SelectedIndex = -1;
            txt_url.Text = string.Empty;
            txt_description.Document.Blocks.Clear();

            // Deselect any selected item in the ListView
            ListViewCandidate.SelectedItem = null;
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
