# Time Rate Provider

## Run
To run locally using ```dotnet```:
1. Ensure working directory is ProviderApi
2. ```dotnet restore```
3. ```dotnet run```

To run locally using ```docker```:
1. Ensure working directory is ProviderApi
2. ```docker build -t providerapi .```
3. ```docker run -d -p 5000:80 providerapi```

To run tests locally:
1. Ensure working directory is ProviderApi.Tests
2. ```dotnet test```

## Use

Healthcheck, metrics and ping URLS:
- ```/health```
- ```/metrics```
- ```/ping```

Sample curls:
- ```curl localhost:5000/api/rate?begin=2017-08-09T10:00:00Z&end=2017-08-09T11:30:00Z```
- ```curl localhost:5000/metrics```

## Modify

Use a different static data file:
Update ProviderApi/data/sample.json with desired json file and restart

## Notes:

- Lots of assumptions about requirements: GET vs POST, format of return object (JSON vs plaintext), how to handle invalid ranges vs. nonexistant ranges
- Deployment script not included but docker file created to ease deployment in existing container management system
- Test coverage very low; mostly as an example
