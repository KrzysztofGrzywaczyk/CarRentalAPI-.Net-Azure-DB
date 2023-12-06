# CarRentalAPI
## Azure SQL DataBase, NLog logging to file, .NET, Entity Framework Core, Middleware for Errors handling

The **API-CRUD type application** provides functionalities for managing car rentals through the HTTP protocol.</br>
</br>
**Database** for this project is located in **Azure SQL Server**:</br>
contains 3 tables in relations one-to-one (addresses-rental offices) and one-to-many. (rental offices - cars)</br>
Project contains self-written **Middleware for errors handling**
</br>
Projet contains **Swagger documentation** for endpoints and schemas.</br>
</br>
</br>
**API delivers the following functionality:**</br>
</br>
Getting list of **all** new rental offices:<br>
**GET**<br>
**​/api​/rentaloffices**<br>
</br>
Adding new rental offices:<br>
**POST**<br>
**​/api​/rentaloffices**<br>
</br>
Getting particular rental office by id:<br>
**GET**<br>
**​/api​/rentaloffices​/{id}**<br>
</br>
Edit particular rental office indicated by id:<br>
**PUT**<br>
**​/api​/rentaloffices​/{id}**<br>
</br>
Delete particular rental office indicated by id:<br>
**DELETE**<br>
**​/api​/rentaloffices​/{id}**<br>
