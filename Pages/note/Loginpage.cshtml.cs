using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotesApp.Pages.note
{
    public class LoginpageModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginpageModel> _logger;
        private readonly string _connectionString;

        public LoginpageModel(IConfiguration configuration, ILogger<LoginpageModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("mynotesConnection");
        }

        [BindProperty]
        public LoginPageModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool isValidUser;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string selectQuery = @"SELECT COUNT(*) FROM sign
                               WHERE email = @Email AND password = @Password";

                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Email", Input.Email);
                    command.Parameters.AddWithValue("@Password", Input.Password);

                    int count = (int)await command.ExecuteScalarAsync();
                    isValidUser = (count == 1);
                }
            }

            if (isValidUser)
            {
                return RedirectToPage("/note/Index");
            }
            else
            {
                // Invalid user credentials
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return Page();
            }
        }

        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    bool isValidUser;
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();

        //        string selectQuery = @"SELECT COUNT(*) FROM sign
        //                               WHERE email = @Email AND password = @Password";

        //        using (SqlCommand command = new SqlCommand(selectQuery, connection))
        //        {
        //            command.Parameters.AddWithValue("@Email", Input.Email);
        //            command.Parameters.AddWithValue("@Password", Input.Password);

        //            int count = (int)await command.ExecuteScalarAsync();
        //            isValidUser = (count == 1);
        //        }

        //        if (isValidUser)
        //        {
        //            string insertQuery = @"INSERT INTO sign (username, password)
        //                           VALUES (@Username, @Password)";

        //            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
        //            {
        //                insertCommand.Parameters.AddWithValue("@Username", Input.Email);
        //                insertCommand.Parameters.AddWithValue("@Password", Input.Password);

        //                await insertCommand.ExecuteNonQueryAsync();
        //            }
        //        }
        //    }

        //    if (isValidUser)
        //    {
        //        return RedirectToPage("/note/Index");
        //    }
        //    else
        //    {
        //        // Invalid user credentials
        //        ModelState.AddModelError(string.Empty, "Invalid username or password");
        //        return Page();
        //    }
       // }


        public async Task<IActionResult> OnPostSendEmailAsync()
        {
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

                        await smtpClient.SendMailAsync(message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email");
              
            }
            return RedirectToPage(".note/Index");
        }

        public class LoginPageModel
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }
        }
    }
}
