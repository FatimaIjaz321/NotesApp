using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace NotesApp.Pages.note
{
    public class EditModel : PageModel
    {
        public Notesinfo newnote = new Notesinfo();
        public string errorMessage = "";
        public string successMessage = "";
      
        public void OnGet( string id)
        {
         

            try
            {
              
                string connectionString = "Data Source=DESKTOP-QINOM21;Initial Catalog=mynotes;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM notes " +
                        "WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id); 

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                              
                                newnote.id = ""+ reader.GetInt32(0);
                                newnote.name = reader.GetString(1);
                                newnote.description = reader.GetString(2);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost()
        {
            newnote.id = Request.Form["id"];
            newnote.name = Request.Form["name"];
            newnote.description = Request.Form["description"];
            if (string.IsNullOrEmpty(newnote.id) || string.IsNullOrEmpty(newnote.name) || string.IsNullOrEmpty(newnote.description))
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
                    string sql = "UPDATE notes SET name = @name, description = @description " +
                        "WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", newnote.name);
                        command.Parameters.AddWithValue("@description", newnote.description);
                        command.Parameters.AddWithValue("@id", int.Parse(newnote.id));

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/note/Index");
        }
    }
}
