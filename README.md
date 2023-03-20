# CarRentalAPI
## Work with: Azure-located DataBase server, Swagger Documentation, .NET, Entity frameworks.

The **API-CRUD type application** provides functionalities for managing car rentals through the HTTP protocol.</br>
</br>
**Database** for this project is located in **Azure SQL Server**:</br>
contains 3 tables in relations one-to-one (addresses-rental offices) and one-to-many. (rental offices - cars)</br>
</br>
Projet contains **Swagger documentation** for endpoints and schemas.</br>
</br>
</br>
**API has the following functionality:**</br>
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
