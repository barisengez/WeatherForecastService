# Business Notes

- Skipped unit support for temperature as it is not requested
- No specific business requirements were given in the case of adding forecast for the same day. It is set to overwrite that day.
  This is considered a workaround so always HTTP Status Code Created (201) is returned if successful
- Skipped making human friendly temperature descriptions configurable as it is not requested
- Temperatures are stored and treated as integers as floating point precision is not requested
- No rules are specified about what to do in case of missing forecasts in the upcoming week. 
  Assumed the next 7 available forecasts from today are to be returned regardless of their dates.

# Technical Notes

- Skipped Authorization/Authentication as it is not requested
- Skipped integration tests as it is not requested
- Skipped using MediatR to keep it simple, as CQRS pattern is not requested 
- Skipped any caching mechanism as it is not requested

- Applied db migrations in docker-compose for easy demonstration purposes. 
  Normally it should be run seperately from the app run to prevent concurrent runs in case of starting multiple instances of the app at the same time