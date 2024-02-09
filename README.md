# SimpleRecords
Sample C# .NET Framework 4.8 project based on a sample project created as part of an interview process.  The base project has since been modified to have a few more features.  MS Tests was used for the unit test framework.  Other unit test frameworks can be used, I just chose MS Tests.  Different versions of user interfaces and API backend code are being planned and developed.  More features may be added depending on simplicity of the new features.  This repository is meant to be simple, requiring little time and effort to review.
## SimpleRecords.ConsoleApp
- A console application.
- The executable has one argument: the file name to import.
    
    `SimpleRecords.ConsoleApp.exe import.csv` will import `import.csv` from the directory SimpleRecords.ConsoleApp.exe is located in.
    
    `SimpleRecords.ConsoleApp.exe C:\files\import.txt` will import `import.txt` from the `C:\files` folder.
    
    `SimpleRecords.ConsoleApp.exe` will import `C:\Users\%USERNAME%\Documents\nameslist.csv`.
- When specifying a file to import, the file type (comma, space, or pipe delimited) will automatically be detected.
- A flat file is used to store the backend data.  A database is preferred; however a flat file allows for the most compatibility and ease of use.
## SimpleRecords.ConsoleApp.Tests
Contains all unit tests for the console application.
## SimpleRecords.API
RESTful API for viewing and adding name records.  When posting a new record, the body can be in any of the three delimited formats: comma, space, or pipe.
## SimpleRecords.ConsoleApp.Tests
Contains all unit tests for the REST APIs
## SimpleRecords.Common
A library of common code used by the console application and the API.
## SimpleRecords.Common.Tests
Test units for the common code.
## Logging
Log4Net is used to create logs for both the REST API and console application.  By default, logs are stored in `C:\SimpleRecords\Logs`.  If any errors or unusual behavior occurs, the logas should help figure out what is going on.

To change the log location, modify `log4net.config` for the desired application.  The file location can be changed on line 9 for the API and line 36 for the console application.  The line looks like the following: `<param name="File" value="C:/SimpleRecords/logs/apilog.txt" />`.
## Data Persistence
Ann addition to the original project.  By default the location of the database is `C:\SimpleRecords\Data`.  To change the location, modify the `SimpleRecords.API.dll.config` or `SimpleRecords.ConsoleApp.exe.config` and change the the line `<add key="DBFileLocation" value="C:/SimpleRecords/Data/database.json" />`.

The database is a json file, negating the need to install anything special to use.  Normally a database such as MySQL or SQL Serever would be used, a json file is effective for demonstration.  Modifications can be easily made to use a database.
## CI/CD
Implemented and demonstrated in the Repo.
## Why C# on .NET Framework?
.NET Core, or even Node.JS could work.  Since most jobs I apply for requires knowledge of the Encompass SDK, C# on .NET Framework 4.8 seemed like a safe bet to allow others to understand the code easily.  However, .NET Core and Node.JS API server code, as well as HTML/JavaScript/CSS, React/REdux, WinForms, WPF, and WinUI3 user interfaces are under development.   

## Resources
In the releases there is a resources zip file.  A sample json database file, a Postman collection, and sample names.csv can be found there.  With each new release, a new resources zip file is created.
