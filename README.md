# Car Rental API
## .NET, Azure SQL DataBase,  JWT Authentication, Entity Framework Core, NLog logging to file, Paging and filtering results

Project is **Web API backend application**, which provides functionalities for managing base of car rentals and the cars contained therein, addresses and base of user with authorization and authentication service.</br>

Provides endpoints for communication with using json models.

This project is **Azure SQL Database** ready. All you need is AzureDb CS.</br>
Projet contains **Swagger documentation** for endpoints and data schemas.</br>

Project contains users Authorization and Authentication. Mechanics are based on bearer token</br>
**Endpoints for user registration and login are:**</br>
>**POST**
/api/account/register</br>
>**POST**
/api/account/login</br>


**API delivers the following functionality :** </br>
**Rental Offices :**
>GET
/api/rentaloffices *- get all rental*</br>
POST
/api/rentaloffices *- add new rental*</br>
GET
/api/rentaloffices/{id} *- get rental with particular id*</br>
PUT
/api/rentaloffices/{id} *- modify rental with particular id   <sub> -  manager & admin roles allowed </sub>*</br> 
DELETE
>/api/rentaloffices/{id} *- delete rental with particular id <sub> -  manager & admin roles allowed </sub>*</br> 

**Cars :**
>GET
/all/cars *- get all cars in database*</br>
 GET /api/rentals/{rentalId}/cars *- get all **cars in** particular **rental***  </br> 
POST /api/rentals/{rentalId}/cars | *- add **new car to** particular **rental***</br>
GET /api/rentals/{rentalId}/cars/{carId} *- get particular **car from rental***</br>
DELETE
/api/rentals/{rentalId}/cars/{carId} *- delete particular **car from rental*** <sub> -  entity_author, manager & admin allowed </sub>*</br> 
PUT
/api/rentals/{rentalId}/cars/{carId} *- modify particular **car from rental*** <sub> -  entity_author, manager & admin allowed </sub>*</br> 


**UserManagemnet :** <sub>(restricted for administrator)</sub>
>POST
/api/users *- add **new user** directly by system administrator*</br> 
DELETE
/api/users/{userId} *- **delete user** by system administrator*</br>
GET
/api/users/{userId} *- get **informaion** about particular user*</br>
PUT
/api/users/{userId} *- **modify existing user** as system administrator</br>
GET
/api/users/all *- get **informaion** about **all** users particular user*</br>

GET endpoints use **filtration (searching)** if any Search Phrase is provided:</br>
\- searchPhrase - *string*

Results can be sorted if any SortBy column-name is provided (specific for endpoint): </br>
\- SortBy - *string*

GET endpoints use **pagination** and the following data should be provided: </br>
\- page number (default 1 - from start) - *int32* </br>
\- amount of items on page (default 20) - *int32*

**Paging** provides information useful to the frontend customer in the following model:</br>
> "items": [*list of items to display*] </br>
> "totalPages": *int*, </br>
> "itemFrom": *int*, </br>
> "itemTo": *int*, </br>
> "itemCount": *int* </br>


Passwords of every registered users are hashed and kept in database in unreadable form.

Project is using: </br>
\- Entity framework for Database querries </br>
\- JWT Bearer Authentication </br>
\- Self-implemented authorization </br>
\- Self-implemented paging and record filtration.
\- NLog logging to file </br>
\- Self-implemented middlewares </br>
\- X-unit Unit and Integration tests are work-in progress </br>
\- few additional training/demonstration usages like regexes etc. </br>
\- Validation for every key input data </br>
\- auto mapping and data-transfer objects
