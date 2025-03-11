using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private DataTable table; // DataTable variable to hold the data

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Populate the ComboBox with table names
            comboBox1.Items.Add("Nurse");
            comboBox1.Items.Add("Appointments");
            comboBox1.Items.Add("Assists");
            comboBox1.Items.Add("Doctor_specialization");
            comboBox1.Items.Add("Medical_records");
            comboBox1.Items.Add("Medical_staff");
            comboBox1.Items.Add("patients");
            comboBox1.Items.Add("prescription");
            comboBox1.Items.Add("prescription_Drugs");
            comboBox1.Items.Add("Doctor");

            // Set default selection
            comboBox1.SelectedIndex = 0;

            // Attach event handler for dataGridView1 selection change
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=Test;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string selectedTable = comboBox1.SelectedItem.ToString();
                    string query = $"SELECT * FROM {selectedTable}";

                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();

                    // Create a new data adapter based on the specified query
                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    // Create a new DataTable to hold the data
                    table = new DataTable(); // Store the DataTable in a class-level variable

                    // Fill the DataTable with data from the data adapter
                    adapter.Fill(table);

                    // Bind the DataTable to the DataGridView
                    dataGridView1.DataSource = table;

                    // Clear existing controls
                    panel1.Controls.Clear();

                    // Add TextBoxes for each column of the selected table
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        TextBox textBox = new TextBox();
                        textBox.Name = "textBox" + i;
                        textBox.Location = new System.Drawing.Point(10, 30 * i);
                        textBox.ReadOnly = true; // Set TextBox to read-only initially
                        panel1.Controls.Add(textBox);

                        Label label = new Label();
                        label.Text = table.Columns[i].ColumnName;
                        label.Location = new System.Drawing.Point(120, 30 * i + 5);
                        panel1.Controls.Add(label);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow row = dataGridView1.SelectedRows[0];

                // Populate textBoxes with corresponding values
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    TextBox textBox = (TextBox)panel1.Controls["textBox" + i];
                    textBox.Text = row.Cells[i].Value.ToString();
                    textBox.ReadOnly = false; // Allow editing
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if there is a selected row
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=Test;Integrated Security=True";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string selectedTable = comboBox1.SelectedItem.ToString();
                        string setValues = "";
                        foreach (Control control in panel1.Controls)
                        {
                            if (control is TextBox)
                            {
                                setValues += table.Columns[int.Parse(control.Name.Replace("textBox", ""))].ColumnName + " = '" + control.Text + "',";
                            }
                        }
                        // Remove the trailing comma
                        setValues = setValues.TrimEnd(',');

                        string primaryKeyColumnName = GetPrimaryKeyColumnName(selectedTable); // Get the primary key column name
                        string query = $"UPDATE {selectedTable} SET {setValues} WHERE {primaryKeyColumnName} = @{primaryKeyColumnName}";

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue($"@{primaryKeyColumnName}", dataGridView1.SelectedRows[0].Cells[0].Value);

                        connection.Open();

                        // Execute the query
                        command.ExecuteNonQuery();

                        // Refresh the DataTable from the database
                        button1_Click(sender, e);
                    }
                }
                else
                {
                    MessageBox.Show("No row is selected for updating.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }



        // Helper method to get the primary key column name of the selected table
        private string GetPrimaryKeyColumnName(string tableName)
        {
            string primaryKeyColumnName = table.Columns[0].ColumnName; // Get the first column name of the DataTable
            return primaryKeyColumnName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if there is a selected row
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=Test;Integrated Security=True";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string selectedTable = comboBox1.SelectedItem.ToString();
                        string primaryKeyColumnName = GetPrimaryKeyColumnName(selectedTable); // Get the primary key column name
                        string primaryKeyValue = dataGridView1.SelectedRows[0].Cells[primaryKeyColumnName].Value.ToString();
                        string query = $"DELETE FROM {selectedTable} WHERE {primaryKeyColumnName} = @{primaryKeyColumnName}";

                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue($"@{primaryKeyColumnName}", primaryKeyValue);

                        connection.Open();

                        // Execute the query
                        command.ExecuteNonQuery();

                        // Refresh the DataTable from the database
                        button1_Click(sender, e);
                    }
                }
                else
                {
                    MessageBox.Show("No row is selected for deletion.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=Test;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string selectedTable = comboBox1.SelectedItem.ToString();
                    string columns = "";
                    string values = "";
                    foreach (Control control in panel1.Controls)
                    {
                        if (control is TextBox)
                        {
                            columns += table.Columns[int.Parse(control.Name.Replace("textBox", ""))].ColumnName + ",";
                            values += "'" + control.Text + "',";
                        }
                    }
                    // Remove the trailing comma
                    columns = columns.TrimEnd(',');
                    values = values.TrimEnd(',');

                    string query = $"INSERT INTO {selectedTable} ({columns}) VALUES ({values})";

                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();

                    // Execute the query
                    command.ExecuteNonQuery();

                    // Refresh the DataGridView
                    button1_Click(sender, e);

                    // Clear the TextBoxes
                    foreach (Control control in panel1.Controls)
                    {
                        if (control is TextBox)
                        {
                            control.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            Form2_queries form2 = new Form2_queries();
            form2.Show();
            this.Hide();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();

        }
    }
}
