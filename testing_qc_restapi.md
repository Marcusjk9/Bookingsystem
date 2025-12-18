### Postman testing mothods ### OBS INTERNT TESTING

# Skapa Databas & Tabeller:
För att skapa databas använder vi MySQL Workbench, för tabeller i postman DELETE /db   // rensar tabeller


# Admins
POST/admin // skapa admin, plus att alla uppgifter ska fyllas i body rutan
POST/login/admin  // loga in admin, email & password i body
GET/admin/users // testa admins behörighet att lista ut alla users
GET/admin/emplyees // **OBS**testa admins behörighet att lista ut alla anställda OBS***FUNKAR EJ*** MÅSTE KOLLA UPP



# users:
POST/login // testa user login
DELETE/login // testa user logga ut






# Employees:
POST/employees // skapa nytt employee, plus alla uppgifter ska fyllas i body rutan
POST/login/employee // testa employees login
GET/employees // visa lista med employees i databasen (visar null om man inte har gjort POST/login/employee innan)
DELETE/login/employee  //logga ut employee







