using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {
        ComboBox patientComboBox = new ComboBox();
        ComboBox doctorComboBox = new ComboBox();
        ComboBox attendanceComboBox = new ComboBox();

        public Form2()
        {
            InitializeComponent();
            LoadPanel(); // Call LoadPanel() when the form loads
        }

        private void LoadPatients()
        {
            // Connect to the database and fetch patient data
            string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=Test;Integrated Security=True";
            string query = "SELECT Patient_ID, Name FROM Patients";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int patientID = reader.GetInt32(0);
                    string patientName = reader.GetString(1);
                    patientComboBox.Items.Add(new { ID = patientID, Name = patientName });
                }
            }
        }

        private void LoadDoctors()
        {
            // Connect to the database and fetch doctor data
            string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=Test;Integrated Security=True";
            string query = "SELECT Doctor.Doctor_ID, Medical_staff.Name, Doctor_specialization.Doctor_specialization " +
                           "FROM Doctor " +
                           "INNER JOIN Medical_staff ON Doctor.SSN = Medical_staff.SSN " +
                           "INNER JOIN Doctor_specialization ON Doctor.Doctor_ID = Doctor_specialization.Doctor_ID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int doctorID = reader.GetInt32(0);
                    string doctorName = reader.GetString(1);
                    string doctorSpecialization = reader.GetString(2);
                    doctorComboBox.Items.Add(new { ID = doctorID, Name = doctorName, Specialization = doctorSpecialization });
                }
            }
        }

        private void LoadAttendanceStatus()
        {
            string[] attendanceStatuses = { "Present", "Absent" };
            foreach (string status in attendanceStatuses)
            {
                attendanceComboBox.Items.Add(status);
            }
        }

        private void bookButton_Click(object sender, EventArgs e)
        {
            try
            {
                int patientID = ((dynamic)patientComboBox.SelectedItem).ID;
                int doctorID = ((dynamic)doctorComboBox.SelectedItem).ID;
                string attendanceStatus = attendanceComboBox.Text;

                // Generate a unique Appointment_ID
                int appointmentID = GenerateUniqueAppointmentID();

                // Insert appointment data into the database
                string connectionString = @"Data Source=.\SQLExpress;Initial Catalog=Test;Integrated Security=True";
                string query = "INSERT INTO Appointments (Appointment_ID, Patient_ID, Doctor_ID, Attendacne_status) " +
                               "VALUES (@AppointmentID, @PatientID, @DoctorID, @AttendanceStatus)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@AppointmentID", appointmentID);
                    command.Parameters.AddWithValue("@PatientID", patientID);
                    command.Parameters.AddWithValue("@DoctorID", doctorID);
                    command.Parameters.AddWithValue("@AttendanceStatus", attendanceStatus);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    MessageBox.Show("Appointment booked successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private int GenerateUniqueAppointmentID()
        {
            Random rnd = new Random();
            return rnd.Next(10000, 99999); // Generate a random 5-digit number
        }

        private void LoadPanel()
        {
            LoadPatients();
            LoadDoctors();
            LoadAttendanceStatus();

            // Create dropdowns for patients, doctors, and attendance status inside the panel
            int labelX = 20;
            int comboX = 150;
            int y = 20;

            // Patient dropdown
            Label patientLabel = new Label();
            patientLabel.Text = "Patient:";
            patientLabel.Location = new System.Drawing.Point(labelX, y);
            panel1.Controls.Add(patientLabel);

            patientComboBox.Location = new System.Drawing.Point(comboX, y);
            patientComboBox.Width = 300; // Increased width
            panel1.Controls.Add(patientComboBox);

            y += 30;

            // Doctor dropdown
            Label doctorLabel = new Label();
            doctorLabel.Text = "Doctor:";
            doctorLabel.Location = new System.Drawing.Point(labelX, y);
            panel1.Controls.Add(doctorLabel);

            doctorComboBox.Location = new System.Drawing.Point(comboX, y);
            doctorComboBox.Width = 400; // Increased width
            panel1.Controls.Add(doctorComboBox);

            y += 30;

            // Attendance status dropdown
            Label attendanceLabel = new Label();
            attendanceLabel.Text = "Attendance Status:";
            attendanceLabel.Location = new System.Drawing.Point(labelX, y);
            panel1.Controls.Add(attendanceLabel);

            attendanceComboBox.Location = new System.Drawing.Point(comboX, y);
            attendanceComboBox.Width = 200;
            panel1.Controls.Add(attendanceComboBox);

            y += 30;

            // Book button
            Button bookButton = new Button();
            bookButton.Text = "Book Appointment";
            bookButton.Location = new System.Drawing.Point(150, y);
            bookButton.Click += new System.EventHandler(this.bookButton_Click);
            panel1.Controls.Add(bookButton);
        }
    }
}
