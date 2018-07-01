RateApplication - Skills

How to run:
RateApplication.Backend.exe [service name (default="RateApplication/api/")] [port (default=8080)] [SQLite database (default=skills.sqlite)]
RateApplication.Frontend.exe [service name (default=RateApplication/frontend/)] [port (default=80)] [api url (default=http://localhost:8080/RateApplication/api)]

Both backend and frontend are windows executables which self host the Skills API and frontend html page, respectively.
The frontend html page can be accessed at ../RateApplication/frontend/skills.html once RateApplication.Frontend.exe is up and running 