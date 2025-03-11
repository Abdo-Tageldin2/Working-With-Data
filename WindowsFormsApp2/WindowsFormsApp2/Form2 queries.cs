using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form2_queries : Form
    {
        private string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=Test;Integrated Security=True";
        private TextBox dynamicTextBox;
        private Label labelDescription;
        private Label labelInputRequirement;
        private ComboBox comboBoxSpecialization;

        public Form2_queries()
        {
            InitializeComponent();
            labelDescription = new Label();
            labelDescription.AutoSize = true;
            labelDescription.Location = new System.Drawing.Point(10, 50);
            labelDescription.Text = "";
            this.Controls.Add(labelDescription);

            labelInputRequirement = new Label();
            labelInputRequirement.AutoSize = true;
            labelInputRequirement.Location = new System.Drawing.Point(10, 80);
            labelInputRequirement.Text = "";
            panel1.Controls.Add(labelInputRequirement); // Add labelInputRequirement to panel1

            comboBoxSpecialization = new ComboBox();
            comboBoxSpecialization.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSpecialization.Location = new System.Drawing.Point(150, 40);
            comboBoxSpecialization.Size = new System.Drawing.Size(150, 20);
            comboBoxSpecialization.Visible = false; // Initially invisible
            panel1.Controls.Add(comboBoxSpecialization); // Add comboBoxSpecialization to panel1
        }

        private void Form2_queries_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Query 1");
            comboBox1.Items.Add("Query 2");
            comboBox1.Items.Add("Query 3");
            comboBox1.Items.Add("Query 4");
            comboBox1.Items.Add("Query 5");
            comboBox1.Items.Add("Query 6"); // Add the new query option

            // Add specialization options
            comboBoxSpecialization.Items.Add("Cardiology");
            comboBoxSpecialization.Items.Add("Orthopedics");
            comboBoxSpecialization.Items.Add("Neurology");
            comboBoxSpecialization.Items.Add("Dermatology");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear previous input controls
            if (dynamicTextBox != null)
            {
                panel1.Controls.Remove(dynamicTextBox);
                dynamicTextBox.Dispose();
            }
            labelInputRequirement.Text = "";

            // Hide comboBoxSpecialization by default
            comboBoxSpecialization.Visible = false;

            // Check which query is selected and add input control accordingly
            if (comboBox1.SelectedIndex == 0) // Query 1
            {
                dynamicTextBox = new TextBox();
                dynamicTextBox.Name = "textBoxDoctorID";
                dynamicTextBox.Location = new System.Drawing.Point(150, 10);
                dynamicTextBox.Size = new System.Drawing.Size(150, 20); // Set the size of the TextBox
                panel1.Controls.Add(dynamicTextBox);

                // Show query description
                labelDescription.Text = "Retrieve appointments for a specific doctor.";

                // Display input requirement
                labelInputRequirement.Text = "Enter Doctor ID:";
                labelInputRequirement.Location = new System.Drawing.Point(10, 10); // Adjust the location of labelInputRequirement
            }
            else if (comboBox1.SelectedIndex == 1) // Query 2
            {
                // Hide the TextBox
                // Clear the description
                labelDescription.Text = "Retrieve the patient with the maximum number of attended appointments.";
            }
            else if (comboBox1.SelectedIndex == 2) // Query 3
            {
                // Hide the TextBox
                // Clear the description
                labelDescription.Text = "Retrieve the count of appointments for each doctor with their specialization.";
            }
            else if (comboBox1.SelectedIndex == 3) // Query 4
            {
                // Hide the TextBox
                // Clear the description
                labelDescription.Text = "Retrieve patients who have no appointments.";
            }
            else if (comboBox1.SelectedIndex == 4) // Query 5
            {
                // Hide the TextBox
                // Clear the description
                labelDescription.Text = "Display the total number of prescriptions issued by each doctor and list each drug prescribed.";
            }
            else if (comboBox1.SelectedIndex == 5) // Query 6
            {
                dynamicTextBox = new TextBox();
                dynamicTextBox.Name = "textBoxPatientID";
                dynamicTextBox.Location = new System.Drawing.Point(150, 10);
                dynamicTextBox.Size = new System.Drawing.Size(150, 20); // Set the size of the TextBox
                panel1.Controls.Add(dynamicTextBox);

                labelDescription.Text = "Retrieve the number of visits for a patient with a specific doctor specialization.";

                labelInputRequirement.Text = "Enter Patient ID:";
                labelInputRequirement.Location = new System.Drawing.Point(10, 10); // Adjust the location of labelInputRequirement

                // Show comboBoxSpecialization
                comboBoxSpecialization.Visible = true;
                comboBoxSpecialization.SelectedIndex = -1; // Reset the selection
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "";

            // Check which query the user wants to execute
            if (comboBox1.SelectedIndex == 0) // Query 1
            {
                query = "SELECT a.Appointment_ID AS Appointment_ID, p.Name AS Patient_Name, a.Attendacne_status AS Attendance_Status " +
                        "FROM Appointments AS a " +
                        "JOIN Patients AS p ON a.Patient_ID = p.Patient_ID " +
                        "WHERE a.Doctor_ID = @DoctorID " +
                        "ORDER BY a.Appointment_ID";
            }
            else if (comboBox1.SelectedIndex == 1) // Query 2
            {
                query = "SELECT p.Name AS Patient_Name, p.Patient_ID AS Patient_ID, MAX(a.Appointment_Count) AS Max_Appointments_Attended " +
                        "FROM ( " +
                        "    SELECT Patient_ID, COUNT(*) AS Appointment_Count " +
                        "    FROM Appointments " +
                        "    WHERE Attendacne_status = 'Present' " + // Filter to include only attended appointments
                        "    GROUP BY Patient_ID " +
                        ") AS a " +
                        "JOIN Patients AS p ON a.Patient_ID = p.Patient_ID " +
                        "GROUP BY p.Name, p.Patient_ID";
            }
            else if (comboBox1.SelectedIndex == 2) // Query 3
            {
                query = "SELECT ms.Name AS Doctor_Name, ds.Doctor_specialization AS Specialization, COUNT(a.Appointment_ID) AS Appointment_Count " +
                        "FROM Appointments AS a " +
                        "JOIN Doctor AS d ON a.Doctor_ID = d.Doctor_ID " +
                        "JOIN Medical_staff AS ms ON d.SSN = ms.SSN " +
                        "JOIN Doctor_specialization AS ds ON d.Doctor_ID = ds.Doctor_ID " +
                        "GROUP BY ms.Name, ds.Doctor_specialization";
            }
            else if (comboBox1.SelectedIndex == 3) // Query 4
            {
                query = "SELECT p.Name AS Patient_Name, p.Patient_ID AS Patient_ID " +
                        "FROM Patients AS p " +
                        "WHERE NOT EXISTS ( " +
                        "    SELECT 1 " +
                        "    FROM Appointments AS a " +
                        "    WHERE a.Patient_ID = p.Patient_ID " +
                        ")";
            }
            else if (comboBox1.SelectedIndex == 4) // Query 5
            {
                query = "SELECT d.Doctor_ID AS Doctor_ID, ms.Name AS Doctor_Name, pd.Drugs AS Drug, COUNT(p.Prescription_ID) AS Total_Prescriptions " +
                        "FROM Prescription AS p " +
                        "JOIN Appointments AS a ON p.Appointment_ID = a.Appointment_ID AND p.Patient_ID = a.Patient_ID " +
                        "JOIN Doctor AS d ON a.Doctor_ID = d.Doctor_ID " +
                        "JOIN Medical_staff AS ms ON d.SSN = ms.SSN " +
                        "JOIN Prescription_Drugs AS pd ON p.Prescription_ID = pd.Prescription_ID " +
                        "GROUP BY d.Doctor_ID, ms.Name, pd.Drugs";
            }
            else if (comboBox1.SelectedIndex == 5) // Query 6
            {
                query = "SELECT COUNT(*) AS Num_Visits " +
                        "FROM Appointments AS A " +
                        "JOIN Doctor AS D ON A.Doctor_ID = D.Doctor_ID " +
                        "JOIN Doctor_specialization AS DS ON D.Doctor_ID = DS.Doctor_ID " +
                        "WHERE A.Patient_ID = @PatientID " +
                        "AND DS.Doctor_specialization = @Specialization";
            }

            // Execute the selected query
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        if (comboBox1.SelectedIndex == 0) // Query 1
                        {
                            int doctorID;
                            if (int.TryParse(dynamicTextBox.Text, out doctorID))
                            {
                                command.Parameters.AddWithValue("@DoctorID", doctorID);
                            }
                            else
                            {
                                MessageBox.Show("Invalid Doctor ID");
                                return;
                            }
                        }
                        else if (comboBox1.SelectedIndex == 5) // Query 6
                        {
                            int patientID;
                            if (int.TryParse(dynamicTextBox.Text, out patientID))
                            {
                                command.Parameters.AddWithValue("@PatientID", patientID);
                            }
                            else
                            {
                                MessageBox.Show("Invalid Patient ID");
                                return;
                            }

                            if (comboBoxSpecialization.SelectedItem != null)
                            {
                                command.Parameters.AddWithValue("@Specialization", comboBoxSpecialization.SelectedItem.ToString());
                            }
                            else
                            {
                                MessageBox.Show("Please select a specialization");
                                return;
                            }
                        }

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dataGridView1.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
    }
}
