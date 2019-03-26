# core-api-client
Base API client for Thorium Rest based micro services. 

Each call carries additional headers for the following: 

- Time zone offset
  The time zone offset is in minutes. This allows you to detect the time zone of an end user in your back end services. This may be neccesary if you need to generate documents.
- Current User Id
  The user id of the current user. Depending on what you pick, this can be any text.
- Context
  A string to describe the operating context of a request. This can be used to denote a tenant in multi tenant systems for example.



