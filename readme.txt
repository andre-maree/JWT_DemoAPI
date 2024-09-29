DemoAPI
= This is the .NET Web API project.
- Please use db.script to create the database, it will also insert navigation data.
- Set the database connection and other config items.
- Run DemoAPI, use the Swagger page to test the API endpoints.
- Create a login, then log in to get a token, and then use the Swagger auth button to include the token in the other endpoints.

BankHolidaysFunctionApp
- The process to automatically query the bank holidays service is built to be an example of a Durable Function serverless microservice.
- It can be deployed to the Function App serverless environments, or to the same App Service that is hosting the DemoAPI.
- To test the process locally, open another Visual Studio instance and set the BankHolidaysFunctionApp as the startup project.
- Set the url of the SaveBankHolidays service BankHolidaysFunction_HttpStart endpoint in the config of the DemoAPI project.
- Run BankHolidaysFunctionApp, use the StartBankHolidaysSaveProcess endpoint in the DemoAPI Swagger to start the process.
- The process is set to run every 15 seconds in debug, and every 12 hours for release.
- If needed, there is a ClearInstanceHistory endpoint in the BankHolidaysFunctionApp that can be used to reset the process.