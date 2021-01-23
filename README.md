<h2 align="center"> ConnectingDepartments & EffortsTrackingSystem </h2>
Web Application that enable communication between different departments of the organization, Which means an employee in any department can request task from another employee in any other department.
It allows the departments admins to assign tasks to their employees, also provides a dashboard to view the statistics of the tasks.
Also it tracks the employees efforts (The tasks that the employee had completed) in each department.

## Technologies

- Asp.Net Core Web API (As Back-End)
- Asp.Net Core 3 (As Front-End)
- Swagger (for api documentation)
- SignalR
- EntityFramework Core
- SqlServer
- JQuery
- Bootstrap

## Application Functions

======= Department Admin =======

- Manage Department Categories.
- Manage the employees in his department.
- Add employees to his department.
- Manage the tasks that are requested from his department.
- Assign a Task to one of his department employees.
- Request a task from other employees.
- Reports about the tasks requested from his department or the tasks that his department requested from other departments.

======= Department Employee =======

- Request a task from other employees
- Manage the tasks that are requested by other employees from him.
- Manage the tasks that he requested from other employees.

= Note: We can request tasks from employees that belong only to HR and Graphic Design departments.


## Application Components

- <b>The Solution folder will contain:-</b>
  1. "EffortTrackingSystem.API" (Back-End) project which implemented using .Net Core WebApi.
  2. "EffortTrackingSystem.Web" (Front-End) project which implemented using .Net Core 3.
  3. "EffortTrackingSystem.Models" project that contains the Models and Dtos.
  4. "EffortTrackingSystem.bak" database backup file (Restore it in sql server).

- <b>all the accounts of the application has password "123asd"</b>
  
  1. HR Department Admin:-<br>
  hradmin@evapharma.com<br>
  123asd

  2. Graphic Design Department Admin:-<br>
  gdadmin@evapharma.com<br>
  123asd

## Application Live
- 

