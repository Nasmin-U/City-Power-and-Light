
## Entity Management Application with Microsoft Dataverse
.Net Core console application for managing entities in Microsoft Dataverse. Display and interact with accounts, contacts, and cases using CRUD operations.

---

### Table of Contents
1. [Features](#features)
2. [Technologies Used](#technologies-used)
3. [Setup and Installation](#setup-and-installation)
    - [Prerequisites](#prerequisites)
    - [Clone the Repository](#clone-the-repository)
    - [Configure the Application](#configure-the-application)
    - [Build the Application](#build-the-application)
    - [Run the Application](#run-the-application)
4. [Project Structure](#project-structure)
5. [Usage](#usage)
    - [CRUD Operations Workflow](#crud-operations-workflow)
6. [Customization](#customization)
7. [Logging](#logging)
8. [Example Output](#example-output)
9. [Contributing](#contributing)
10. [License](#license)
11. [Contact](#contact)

---

### Features
- **Accounts Management**: Create, update, and delete accounts with attributes like name, email, and phone number.
- **Contacts Management**: Handle contacts with attributes such as full name, email, company name, and business phone.
- **Case Management**: Perform CRUD operations on cases with attributes like title, priority, origin, and status.

---

### Technologies Used
- **C#**: Core programming language.
- **Microsoft Dataverse SDK**: For entity CRUD operations.
- **.NET Core**: Application framework.
- **Dependency Injection**: Used to initialize services.
- **Console-Based UI**: For displaying entity attributes.

---

### Setup and Installation

#### Prerequisites
1. .NET 6 or later installed.
2. Microsoft Dataverse environment.
3. A valid Dataverse connection string.

#### Clone the Repository
Find the local directory where you want to store the project and run:
```bash
git clone https://github.com/Nasmin-U/City-Power-and-Light.git
```

#### Configure the Application

The `appsettings.json` file is ignored in version control, follow these steps:

   - Create a file named `appsettings.json` in the root directory of the project.
   - Add the following configuration:
     ```json
     {
         "ConnectionStrings": {
             "Dataverse": "your-dataverse-connection-string"
         }
     }
     ```

   - Replace `your-dataverse-connection-string` with your Dataverse connection string.
   - Select the file and set the `Copy to Output Directory` property to `Copy if newer`.

---

### **Project Structure**

- **City.Helpers**: Utility classes for logging, validation, and configuration.
    - `ConsoleFormatter.cs`: Formats and displays entity attributes.
    - `EntityValidator.cs`: Validates entity attributes for CRUD operations.
    - `Utility.cs`: Handles configuration and Dataverse client initialization.

- **City.Services**: Core services for interacting with Dataverse.
    - `EntityService.cs`: Provides generic CRUD operations for entities.

- Entity-specific services for managing Accounts, Contacts, and Cases.
    - `AccountService.cs`: Handles account operations.
    - `ContactService.cs`: Manages contact operations.
    - `CaseService.cs`: Manages case operations.

- **Program.cs**: Entry point for the application. Initializes services and orchestrates operations.

---

### **Usage**
1. Run the application.
2. Select the entity operation you wish to perform (Accounts, Contacts, or Cases).
3. View detailed logs of CRUD operations directly in the console.

#### **CRUD Operations Workflow**
- **Create**: Entities are created with required attributes (validated).
- **Read**: Retrieve and display entity details.
- **Update**: Modify entity attributes such as priority or status.
- **Delete**: Clean up test entities created during operations.

---


### **Logging**
The application uses `ConsoleFormatter` for structured logging:
- **INFO**: Logs successful operations.
- **ERROR**: Logs validation or CRUD failures.
- **SUCCESS**: Logs completed operations, such as entity creation or deletion.
---

### **Example Output**

```
*************************** CASE (INCIDENT) CRUD OPERATIONS ***************************

INFO: Creating Account and Contact for Cases
INFO: Creating Cases
INFO: Reading Case for Account
title: Account Case Title
ticketnumber: 12345
prioritycode: High (1)
caseorigincode: Phone Call (1)
customerid: account (ID)
statuscode: Active (1)
createdon: 06/12/2024 11:56:50
```

---

### **Contributing**
1. Fork the repository.
2. Create a feature branch:
```bash
git checkout -b feature/new-feature
```
3. Commit your changes:
```bash
git commit -m "Add new feature"
```
4. Push to the branch:
```bash
git push origin feature/new-feature
```
5. Create a pull request.

---

### **License**
This project is licensed under the MIT License. See `LICENSE` for details.

---

### **Contact**
For issues or feature requests, please open an issue in the repository or contact the project maintainers.

---

Let me know if you'd like further refinements or additional sections!