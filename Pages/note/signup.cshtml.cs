using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace NotesApp.Pages.note
{
    public class signupModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<signupModel> _logger;
        private readonly string _connectionString;

        public signupModel(IConfiguration configuration, ILogger<signupModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("mynotesConnection");
        }

        [BindProperty]
        public SignUpInputModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Insert the user into the database
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"INSERT INTO sign (username, email, password) 
                                       VALUES (@Username, @Email, @Password)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", Input.Username);
                    command.Parameters.AddWithValue("@Email", Input.Email);
                    command.Parameters.AddWithValue("@Password", Input.Password);

                    command.ExecuteNonQuery();
                }
            }

            // Send confirmation email
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 465;

            // SSL/TLS parameters
            string smtpUsername = "Hosinfosystem123@gmail.com";
            string smtpPassword = "Project@123890768";

            try
            {
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.EnableSsl = true;

                    // Set authentication credentials
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                    // Send an email
                    using (MailMessage message = new MailMessage())
                    {
                        message.Subject = "Test Email";
                        message.Body = "Hello, this is a test email.";
                        message.From = new MailAddress("Hosinfosystem123@gmail.com");
                        message.To.Add("fatimaworks915@gmail.com");

                        smtpClient.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email");
                // Handle the exception as per your application's requirements
                // You can display an error message or redirect to an error page
            }

            // Redirect to a confirmation page or display a success message
            return RedirectToPage("/note/Loginpage");
        }
    }

    public class SignUpInputModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
