# opening-hours

This project is built on ASP .NET CORE 5.0

## Steps to run
1. Ensure you've .NET CORE 5.0 SDK install.
2. Clone the repository and navigate to the directory.
3. Open the solution file (OpeningHours.API.sln) in visual studio or visual studio code.
4. Run the project, this automatically opens the swagger documentation where available endpoints can be found.

## Available endpoints
There are two endpoints, the first one accepts application/json format data and return line separated string of more human readable data. The other endpoint accept file in json format and return the same thing as the first.

Sample file can be found in the project root directory.

You can also use the sample below to test the first endpoint.

```javascript
{ 
"friday" : [ 
      { 
      "type" : "open", 
      "value" : 64800 
      } 
    ], 
"saturday": [ 
      { 
      "type" : "close", 
      "value" : 3600 
      }, 
      { 
      "type" : "open", 
      "value" : 32400 
      }, 
      { 
      "type" : "close", 
      "value" : 39600 
      }, 
      { 
      "type" : "open", 
      "value" : 57600 
      }, 
      { 
      "type" : "close", 
      "value" : 82800
      } 
  ] 
} 
