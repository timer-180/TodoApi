This is API service to manage ToDo List.<br>
To create required DB, use <code>Add-Migration InitialCreate</code> and <code>Update-Database</code> commands.<br>
Configure <code>TodoApiConnection</code> connection string to DB in appsettings.json file.<br>
Configure <code>profiles:http:applicationUrl</code> property in Properties/launchSettings.json file.<br>
ToDo API url is <code>[applicationUrl]/api/TodoItems/</code><br>
This API service can be used by <a href="https://github.com/timer-180/TodoClient">ToDo Client</a> application.<br>
