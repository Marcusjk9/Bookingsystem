### countries

INSERT INTO countries (id, name) VALUES
(1, 'Sverige'),
(2, 'Tyskland'),
(3, 'Storbritannien'),
(4, 'Frankrike');


### cities

INSERT INTO cities (id, country, name) VALUES
(1, 1, 'Stockholm'),
(2, 1, 'Göteborg'),
(3, 1, 'Malmö'),
(4, 1, 'Uppsala'),
(5, 1, 'Lund'),
(6, 2, 'Berlin'),
(7, 3, 'London'),
(8, 4, 'Paris');


### users

INSERT INTO users
(firstname, lastname, password, email, phone, nationalidnumber, street, streetnumber, city, country, status)
VALUES
('Anna', 'Svensson', 'pass123', 'anna@grupp2.com', '+46701234501', '8501011234', 'Sveavägen', '12', 1, 1, 'active'),
('Johan', 'Karlsson', 'pass123', 'johan@grupp2.com', '+46701234502', '9002022345', 'Kungsgatan', '45', 1, 1, 'active'),
('Maria', 'Lindberg', 'pass123', 'maria@grupp2.com', '+46701234503', '9203033456', 'Avenyn', '7', 2, 1, 'pending'),
('Erik', 'Nilsson', 'pass123', 'erik@grupp2.com', '+46701234504', '8804044567', 'Möllevångsgatan', '3', 3, 1, 'active'),
('Sara', 'Johansson','pass123', 'sara@grupp2.com', '+46701234505', '9305055678', 'Vaksalagatan', '21', 4, 1, 'inactive'),
('Lars', 'Berg', 'pass123', 'lars@grupp2.com', '+46701234506', '8106066789', 'Lilla Fiskaregatan', '9', 5, 1, 'active'),
('Emma', 'Holm', 'pass123', 'emma@grupp2.com', '+46701234507', '9707077890', 'Nybrogatan', '18', 1, 1, 'active'),
('Michael', 'Schmidt', 'pass123', 'michael@grupp2.com', '+4915112345678', '7008081234', 'Hauptstrasse', '10', 6, 2, 'active'),
('James', 'Brown', 'pass123', 'james@grupp2.com', '+447911234567', '6809092345', 'Baker Street', '221B', 7, 3, 'active'),
('Claire', 'Dubois', 'pass123', 'claire@grupp2.com', '+33661234567', '7501013456', 'Rue de Rivoli', '5', 8, 4, 'pending');


### admins



### employees

INSERT INTO employees
(firstname, lastname, password, role, status, office, email, phone, nationalidnumber, street, streetnumber, city, country)
VALUES 
('Niklas', 'Andersson', 'emp123', 'support', 'active', 'halmstad', 'niklas@grupp2.com', '+46701111111', '8201011234', 'Stationsgatan', '1', 1, 1),
('Karin', 'Eriksson', 'emp123', 'manager', 'active', 'stockholm', 'karin@grupp2.com', '+46702222222', '8502022345', 'Vasagatan', '10', 1, 1),
('Oskar', 'Persson', 'emp123', 'administrator','pending', 'berlin', 'oskar@grupp2.com', '+491511234000', '9003033456', 'Friedrichstrasse', '50', 6, 2),
('Linda', 'Karlsson', 'emp123', 'tech', 'active', 'london', 'linda@grupp2.com', '+447911111222', '8804044567', 'Oxford Street', '100', 7, 3),
('Marcus', 'Johansson', 'emp123', 'support', 'inactive', 'stockholm', 'marcus@grupp2.com', '+46703333333', '9305055678', 'Birger Jarlsgatan', '25', 1, 1);