using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Candidate_Service;
using Candidate_BusinessObjects;

namespace CandidateManagement_WPF_TDC
{
    public partial class MainWindow : Window
    {
        private readonly Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private bool isProcessing = false;
        private IHRAccountService hRAccountService;

        // Define role constants
        private const int ROLE_ADMIN = 1;
        private const int ROLE_MANAGER = 2;
        private const int ROLE_STAFF = 3;

        public MainWindow()
        {
            InitializeComponent();
            EmailTextBox.TextChanged += OnEmailTextChanged;
            hRAccountService = new HRAccountService();
            ProgramComboBox.SelectedIndex = 0; // Set default selection
        }

        private void OnEmailTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(EmailTextBox.Text) && !emailRegex.IsMatch(EmailTextBox.Text))
            {
                ShowError("Please enter a valid email address");
            }
            else
            {
                ClearError();
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (isProcessing) return;

            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                    string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                    ProgramComboBox.SelectedItem == null)
                {
                    ShowError("Please fill in all fields");
                    return;
                }

                if (!emailRegex.IsMatch(EmailTextBox.Text))
                {
                    ShowError("Please enter a valid email address");
                    return;
                }

                isProcessing = true;
                SetLoginButtonState(true);

                try
                {
                    var (isAuthenticated, userRole) = await hRAccountService.AuthenticateAsync(EmailTextBox.Text, PasswordBox.Password);

                    if (isAuthenticated)
                    {
                        // Get selected program
                        var selectedProgram = ((ComboBoxItem)ProgramComboBox.SelectedItem).Content.ToString();

                        // Check if user has permission for the selected program
                        bool hasPermission = CheckProgramPermission(userRole, selectedProgram);

                        if (!hasPermission)
                        {
                            ShowError("You don't have permission to access this program");
                            return;
                        }

                        // Open the selected program
                        switch (selectedProgram)
                        {
                            case "Candidate Profile Management":
                                var candidateProfileWindow = new CandidateProfileWindow();
                                candidateProfileWindow.Show();
                                break;

                            case "Job Posting Management":
                                var jobPostingWindow = new JobPostingWindow(userRole);
                                jobPostingWindow.Show();
                                break;

                            default:
                                ShowError("Invalid program selection");
                                return;
                        }

                        MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                    else
                    {
                        ShowError("Invalid email or password");
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"An error occurred during authentication: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                ShowError($"An error occurred: {ex.Message}");
            }
            finally
            {
                isProcessing = false;
                SetLoginButtonState(false);
            }
        }

        private bool CheckProgramPermission(int userRole, string selectedProgram)
        {
            switch (selectedProgram)
            {
                case "Candidate Profile Management":
                    return userRole == ROLE_ADMIN;

                case "Job Posting Management":
                    return userRole == ROLE_MANAGER || userRole == ROLE_STAFF;

                default:
                    return false;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Are you sure you want to exit?",
                "Confirm Exit",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;

            var shakeAnimation = new DoubleAnimation
            {
                From = -5,
                To = 5,
                Duration = TimeSpan.FromMilliseconds(50),
                AutoReverse = true,
                RepeatBehavior = new RepeatBehavior(3)
            };

            ErrorMessage.RenderTransform = new TranslateTransform();
            ErrorMessage.RenderTransform.BeginAnimation(TranslateTransform.XProperty, shakeAnimation);
        }

        private void ClearError()
        {
            ErrorMessage.Text = string.Empty;
        }

        private void SetLoginButtonState(bool isProcessing)
        {
            var loginButton = (Button)FindName("LoginButton");
            if (loginButton != null)
            {
                loginButton.Content = isProcessing ? "Signing In..." : "Sign In";
                loginButton.IsEnabled = !isProcessing;
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            EmailTextBox.Focus();
        }
    }
}