The premise of this application is simple, after applying to a job you can add the resumé and or cover letter for reference and track response times and get statistics.

I had to re-upload the entire thing losing the commit history and issue tracking, as I made a mistake in my attempt to rename the old project files, accidentally adding files and info to the repo that shouldn't be public.

This could easily be solved by a Back- and front-end solution with SQLite, however I wish to spend my time while seeking a job productively, and applying more advanced subjects. 

Because this is a public repo I can't be keeping my information in the appsettings, so for that purpose I use an excluded folder with a json file, that supplies all the projects with their configurations.



Currently containing:
- [x] Docker-compose orchestration with secrets for important parts.
- [x] Local MSSQL & PostgreSQL servers  (PostgreSQL is what I have worked with the least, so I chose that for login and learned OpenIddict doesn't support it. But I kept both for the experience.)
- [x] ~~OpenIDConnect & OAuth2 with OpenIddict in AuthorizationServer.~~
- [x] ~~Login/user creation with encryption, routed through the Authorization server, so that the Login functionality remains un-exposed.~~\
- [x] Login using Windows account or Microsoft login
- [x] Caching to ensure user session is carried over, instead of logging in every time.       
- [x] OpenIDConnect, OAuth2 through Entra





Blazor front-end:
- [x] Upload PDF files to server
- [x] Preview applications in the app
- [ ] Show statistics such as how many has replied, response times
- [x] Anti forgery (is on by default so easy check)
- [x] Automatically gets Bearer token and inserts into HTTPClient requests

ASP.NET Back-end:
- [x] Use the Graph(entra) user Id as relation to applications.
- [x] Store applications in EF database.
- [x] Encrypt & Decrypt (AES + SHA256)
- [x] Access token validation with Entra specified scope
- [ ] Ensure files are updated correctly.
- [x] Cache for development purposes.
- [x] Bearer tokens used to validate requests.
- [x] Validate tokens in swagger

Potential additions:
- [ ] Use RabbitMQ/Message Brokers to integrate with email responses (Updating the status of applications automatically)
