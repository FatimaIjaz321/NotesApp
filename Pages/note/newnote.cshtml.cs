using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace NotesApp.Pages.note
{
    public class newnoteModel : PageModel
    {
        public Notesinfo newnote = new Notesinfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {

        }
        public void OnPost()
        {
            newnote.name = Request.Form["name"];
            newnote.description = Request.Form["description"];
            if (string.IsNullOrEmpty(newnote.name) || string.IsNullOrEmpty(newnote.description))
            {
                errorMessage = "All fields are required";
                return;
            }
            try
            {
                string connectionString = "Data Source=DESKTOP-QINOM21;Initial Catalog=mynotes;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO notes" +
                        "(name,description) VALUES" +
                        "(@name,@description);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", newnote.name);
                        command.Parameters.AddWithValue("@description", newnote.description);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            newnote.name = "";
            newnote.description = "";
            successMessage = "Updated";
            Response.Redirect("/note/Index");
        }



    }
}


