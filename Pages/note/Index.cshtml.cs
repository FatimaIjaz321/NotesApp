using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace NotesApp.Pages.note
{
    public class IndexModel : PageModel
    {
        public List<Notesinfo> listnotesinfo = new List<Notesinfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=DESKTOP-QINOM21;Initial Catalog=mynotes;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM notes";
                    using SqlCommand cmd = new SqlCommand(sql, connection);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                           Notesinfo info = new Notesinfo();
                            info.id = "" + reader.GetInt32(0);
                            info.name = reader.GetString(1);
                            info.description = reader.GetString(2);
                            info.created_at = reader.GetDateTime(3).ToString();
                            listnotesinfo.Add(info);
                        }


                    }


                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }
    public class Notesinfo
    {
        public string id; 
        public string  name;
        public string description;
        public string  created_at;


    }
}
    

