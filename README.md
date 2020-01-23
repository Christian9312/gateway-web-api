# Gateways

  

REST service (JSON/HTTP) for storing information about gateways and their associated devices.

  

## Building and Running

  

Change directory to the api directory (src\gateway-api)

### Restore command (this resolves all NuGet packages)

  

`dotnet restore`

  

### Build command

  

`dotnet build`

  

### Run command

  

`dotnet run`

  

Service will start on `https://localhost:5001`

  

## Testing

  

This repository contains an xUnit.NET test library. Change directory to the tests directory (src\gateway-api-tests)


### Restore command

  

`dotnet restore`

  

### Run tests

  

`dotnet test`

  
  

## Endpoints

  

### Gateways

  

#### Get all

  

Get to `/gateways`

  

#### Get one by id

  

Get to `/gateways/id`

  


#### Create gateway

  

POST to `/gateways`  


```
body:

{

   Name: 'string required',

   Address: 'string required'

}

```
#### Update gateway

PUT to `/gateways/id`  
```
body:

{

   name: 'string required',

   address: 'string required'

}
```


#### Delete gateway

DELETE  `/gateways/id`  







### Peripherals

  

#### Get all

  

Get to `/peripherals`

  

#### Get one by id

  

Get to `/peripherals/id`

  


#### Create peripheral

  

POST to `/peripherals`  


```
body:

{

   vendor: 'string',

   creationDate: 'dateTime required'
   
   status: `Offline/Online string required`
   
   gatewayId: `string required`

}


```
#### Update peripherals

PUT to `/peripherals/id`  
```
body:

{

   vendor: 'string',

   creationDate: 'dateTime required'
   
   status: `Offline/Online string required`
   
   gatewayId: `string required`

}
```


#### Delete peripherals

DELETE  `/peripherals/id`  



Build-automation in `.github\workflows\build-flow.yml` file in repo
