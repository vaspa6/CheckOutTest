# CheckOutTest
Technical Test for CheckOut

## Run Web APP
- Open cmd/powershell in ./CheckOutTest\CheckOutTest.Web
- Run command dotnet run
- Head to https://localhost:5001/index.html for a UI of the endpoints available
- There is an Authorize button on the top righ hand side of the webPage which need an API key to auth (key: 1f964f4c-ee29-4c75-bf05-56f750c6fa95)

## Run Tests
- WebAPI tests
  - Open cmd/powershell in ./CheckOutTest\CheckOutTest.Web.Test
  - Run command dotnet test
- Manager tests
  - Open cmd/powershell in ./CheckOutTest\CheckOutTest.Core.Test
  - Run command dotnet test

## Assumptions
- Merchant will be calling the gateway in a RESTful manner
- We don't want to store CVV information in our data store for compliance reasons
- 3D security is not used
- Use of greater separation between data objects for future-proofing from the assumption made that this is MVP for this project

## Improvements
- Automated API tests ran on a build pipeline/release process (PostMan for example)
- Implementation of Fluent Validator (or similar library) instead of the custom data annotations for validation
- Introduces additional entities
- Add more unit tests
- Addition of specific exceptions
- Improve Authorisation (via Cloud services or Identity server)
- Implementing an Adapter pattern to keep interactions with a Bank generic and build adapter specifically for every single 3rd part bank
- Implementing encryption for recieved data (against sniffing attacks)
- Docker image

## Cloud
For a Cloud deployment/setup, I would use an API Management service (both Azure and AWS provide very similar services) which will be the point of contact for the API endpoints.
This will give the users of the API a very friendly way to see the endpoints documented and run manual tests for them and also will let them manage Auth keys/secrets. With the use
of the above-mentioned services expandability of the API becomes a lot easier due to being able to point specific endpoint/set of endpoints to different applications/backends and
different data stores. The backend can be deployed either to app services or containers, preferably behind a traffic manager and a load balancer (again both Azure and AWS have great
services for those)

## Comments
In this project, I have implemented Entity framework as a framework of choice to interact with the DB, but Dapper or any other framework could be used instead.
Very little of the compliance that Visa Program, MasterCard Program and Amex rules have been followed here. In a real-life scenario from my experience, there will be
a lot of data that will have to be masked, encrypted under specific instruction or not stored at all.
