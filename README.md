Clinic Database Management System

This repository contains a comprehensive database management system designed and implemented as part of the “Working with Data” coursework. The project involved database modeling, normalization, SQL programming, data population, and secure application development for a medical facility information system.

Project Overview

The goal was to design and develop a robust, normalized relational database to manage clinic operations efficiently. The system tracks detailed information related to medical staff, patients, appointments, prescriptions, and medical records, while ensuring secure interactions through a user-friendly application.

Main Features

✅ Database Specification & ER Modeling
	•	Developed a detailed specification for clinic management, covering:
	•	Medical staff (doctors and nurses)
	•	Patients
	•	Appointments
	•	Prescriptions and drugs
	•	Medical records
	•	Designed a comprehensive Entity-Relationship (ER) diagram to visualize the data model clearly, including:
	•	Weak and strong entity relationships
	•	One-to-many and many-to-many relationships

✅ Normalization & Relational Schema
	•	Converted ER diagrams into relational schemas (tables) clearly indicating primary and foreign keys.
	•	Applied Boyce-Codd Normal Form (BCNF) normalization to optimize database performance and integrity.

✅ SQL Implementation & Database Population
	•	Implemented the database using Microsoft SQL Server with clearly defined constraints.
	•	Populated the database with realistic data using automated data-generation tools and manual SQL scripts.

✅ Complex Database Queries & Operations
	•	Implemented complex SQL queries to handle critical operations such as:
	•	Counting patient visits based on doctor specialization
	•	Reporting prescriptions issued by doctors along with associated drugs
	•	Managing and updating appointment attendance status

✅ Aggregate Queries & Advanced SQL Functions
	•	Developed SQL aggregate queries for statistical analysis:
	•	Listing appointments for specific doctors
	•	Determining the maximum appointments attended by patients
	•	Counting and categorizing appointments by doctors’ specialization
	•	Included a non-existence query to identify patients without appointments.

✅ Secure Clinic Application (C#)
	•	Built a user-friendly clinic management application using C# integrated with the SQL Server database.
	•	Features include:
	•	Appointment booking
	•	Database management (insert, update, delete operations)
	•	Analytical query execution and results visualization
	•	Implemented robust error handling with try-catch blocks and user-friendly error messages via Message Boxes.
	•	Ensured protection against SQL Injection attacks using parameterized queries.

Repository Structure

├── ER-Diagram/
├── Relational-Schema-Design/
├── SQL-Scripts/
├── Data-Population/
├── Complex-and-Aggregate-Queries/
└── Clinic-App-(C#)/

Technologies Used
	•	Database: Microsoft SQL Server
	•	Programming: SQL, C# (.NET Framework)
	•	Data Modeling: ER diagrams, BCNF normalization
	•	Security: Parameterized Queries (SQL Injection protection)

Skills Demonstrated
	•	Database Modeling and Normalization
	•	SQL Programming and Complex Query Design
	•	Application Development and Database Connectivity
	•	Data Analysis and Reporting
	•	Security and Exception Handling

This project clearly illustrates my ability to model, develop, and secure complex databases and applications, preparing me for real-world data management roles.
