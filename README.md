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

  

Get to `/gateway`

  

#### Get one by id

  

Get to `/gateway/id`

  


#### Create gateway

  

POST to `/gateway`  


```
body:

{

   Name: 'string required',

   Address: 'string required'

}

```
#### Update gateway

PUT to `/gateway/id`  
```
body:

{

   name: 'string required',

   address: 'string required'

}
```


#### Delete gateway

DELETE  `/gateway/id`  







### Peripherals

  

#### Get all

  

Get to `/peripheral`

  

#### Get one by id

  

Get to `/peripheral/id`

  


#### Create peripheral

  

POST to `/peripheral`  


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

PUT to `/peripheral/id`  
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

DELETE  `/peripheral/id`  



Build-automation in `.github\workflows\build-flow.yml` file in repo
