using System;
using System.Data;
using System.Data.SqlClient;

namespace TestAppLab6
{
    public partial class Form : System.Windows.Forms.Form
    {
        private static SqlConnection connection;
        private string text = $"SELECT Student.STUDENT_ID, Student.STUDENT_SURNAME, Student.STUDENT_NAME, Student.STUDENT_PATRONUMIC, STUDENT_NUM_RECORD_BOOK, STUDENT_GROUP FROM Student";

        public Form()
        {
            InitializeComponent();
            connection = new SqlConnection("Data Source = MSI-LAPTOP-PC; Initial Catalog = TestBD; Integrated Security = true; Connect Timeout = 1;");
            Reload(text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlCommand sqlCommand = new SqlCommand("add", connection))
            {
                connection.Open();
                sqlCommand.CommandType = CommandType.StoredProcedure;

                string[] FIO = new string[2];
                FIO = textBox1.Text.Split();

                sqlCommand.Parameters.Add(new SqlParameter("@STUDENT_SURNAME", SqlDbType.Char, 25));
                sqlCommand.Parameters["@STUDENT_SURNAME"].Value = FIO[0];

                sqlCommand.Parameters.Add(new SqlParameter("@STUDENT_NAME", SqlDbType.Char, 25));
                sqlCommand.Parameters["@STUDENT_NAME"].Value = FIO[1];

                sqlCommand.Parameters.Add(new SqlParameter("@STUDENT_PATRONUMIC", SqlDbType.Char, 25));
                sqlCommand.Parameters["@STUDENT_PATRONUMIC"].Value = FIO[2];

                sqlCommand.Parameters.Add(new SqlParameter("@STUDENT_NUM_RECORD_BOOK", SqlDbType.NChar, 6));
                sqlCommand.Parameters["@STUDENT_NUM_RECORD_BOOK"].Value = textBox3.Text;

                sqlCommand.Parameters.Add(new SqlParameter("@STUDENT_GROUP", SqlDbType.Char, 12));
                sqlCommand.Parameters["@STUDENT_GROUP"].Value = textBox2.Text;

                sqlCommand.ExecuteNonQuery();
                connection.Close();
            }

            Reload(text);
        }

        private void Reload(string sql)
        {
            using (SqlCommand sqlCommand = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Load(dataReader);
                    dataGridView1.DataSource = dataTable;
                    dataReader.Close();
                }
                connection.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e) => Reload(text);

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (comboBox1.Text)
            {
                case "По фамилии (в алфавитном)":
                    Reload(text + " ORDER BY STUDENT_SURNAME");
                    break;
                case "По имени(в алфавитном)":
                    Reload(text + " ORDER BY STUDENT_NAME");
                    break;
                case "По группе":
                    Reload(text + " ORDER BY STUDENT_GROUP");
                    break;
            }
        }
    }
}