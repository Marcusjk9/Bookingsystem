USE travel;

-- ==========================================
-- COUNTRIES
-- ==========================================
INSERT INTO countries (name) VALUES
('Sweden'),
('Germany'),
('United Kingdom'),
('France');

-- ==========================================
-- CITIES
-- ==========================================
INSERT INTO cities (country, name) VALUES
-- Sweden (country_id = 1)
(1, 'Stockholm'),
(1, 'Halmstad'),
(1, 'Gothenburg'),
(1, 'Malmö'),
-- Germany (country_id = 2)
(2, 'Berlin'),
(2, 'Munich'),
(2, 'Hamburg'),
-- United Kingdom (country_id = 3)
(3, 'London'),
(3, 'Manchester'),
(3, 'Edinburgh'),
-- France (country_id = 4)
(4, 'Paris'),
(4, 'Lyon'),
(4, 'Marseille');

-- ==========================================
-- USERS (6 test users)
-- ==========================================
INSERT INTO users (firstname, lastname, password, email, phone, nationalidnumber, street, streetnumber, city, country, status) VALUES
('Test', 'User', 'hashed_password_123', 'test.user@example.com', '+46701234567', '19900101-1234', 'Testgatan', '1', 1, 1, 'active'),
('John', 'Doe', 'hashed_password_456', 'john.doe@example.com', '+46702345678', '19850615-5678', 'Storgatan', '42', 2, 1, 'active'),
('Jane', 'Smith', 'hashed_password_789', 'jane.smith@example.com', '+44207123456', '19920320-9012', 'Oxford Street', '100', 8, 3, 'active'),
('Max', 'Mustermann', 'hashed_password_abc', 'max.mustermann@example.com', '+4930123456', '19880705-3456', 'Berliner Straße', '25', 5, 2, 'pending'),
('Alice', 'Testson', 'hashed_password_def', 'alice.test@example.com', '+33142345678', '19950812-7890', 'Rue de Test', '15', 11, 4, 'active'),
('Bob', 'Demo', 'hashed_password_ghi', 'bob.demo@example.com', '+46703456789', '19870430-2345', 'Demogatan', '7', 3, 1, 'inactive');

-- ==========================================
-- ADMINS (2 per role: god, mid, base)
-- ==========================================
INSERT INTO admins (firstname, lastname, password, role, status, office, email, phone, nationalidnumber, street, streetnumber, city, country) VALUES
('Super', 'Admin', 'admin_password_001', 'god', 'active', 'stockholm', 'super.admin@company.com', '+46708111111', '19800101-0001', 'Adminvägen', '1', 1, 1),
('Chief', 'Administrator', 'admin_password_002', 'god', 'active', 'halmstad', 'chief.admin@company.com', '+46708222222', '19810202-0002', 'Chefsgatan', '2', 2, 1),
('Mid', 'Manager', 'admin_password_003', 'mid', 'active', 'stockholm', 'mid.manager@company.com', '+46708333333', '19820303-0003', 'Mellangatan', '3', 1, 1),
('Middle', 'Boss', 'admin_password_004', 'mid', 'active', 'halmstad', 'middle.boss@company.com', '+46708444444', '19830404-0004', 'Mittenvägen', '4', 2, 1),
('Basic', 'Admin', 'admin_password_005', 'base', 'active', 'stockholm', 'basic.admin@company.com', '+46708555555', '19840505-0005', 'Basvägen', '5', 1, 1),
('Entry', 'Level', 'admin_password_006', 'base', 'pending', 'halmstad', 'entry.level@company.com', '+46708666666', '19850606-0006', 'Nybörjargatan', '6', 2, 1);

-- ==========================================
-- EMPLOYEES (2 per role: support, manager, administrator, tech)
-- ==========================================
INSERT INTO employees (firstname, lastname, password, role, status, office, email, phone, nationalidnumber, street, streetnumber, city, country) VALUES
('Support', 'Helpdesk', 'emp_password_001', 'support', 'active', 'stockholm', 'support.help@company.com', '+46709111111', '19860101-1001', 'Supportgatan', '10', 1, 1),
('Customer', 'Service', 'emp_password_002', 'support', 'active', 'halmstad', 'customer.service@company.com', '+46709222222', '19870202-1002', 'Servicegatan', '11', 2, 1),
('Team', 'Manager', 'emp_password_003', 'manager', 'active', 'stockholm', 'team.manager@company.com', '+46709333333', '19880303-1003', 'Chefsgatan', '12', 1, 1),
('Project', 'Lead', 'emp_password_004', 'manager', 'active', 'halmstad', 'project.lead@company.com', '+46709444444', '19890404-1004', 'Ledargatan', '13', 2, 1),
('System', 'Administrator', 'emp_password_005', 'administrator', 'active', 'stockholm', 'system.admin@company.com', '+46709555555', '19900505-1005', 'Systemvägen', '14', 1, 1),
('Network', 'Admin', 'emp_password_006', 'administrator', 'active', 'halmstad', 'network.admin@company.com', '+46709666666', '19910606-1006', 'Nätverksgatan', '15', 2, 1),
('Tech', 'Support', 'emp_password_007', 'tech', 'active', 'stockholm', 'tech.support@company.com', '+46709777777', '19920707-1007', 'Teknikvägen', '16', 1, 1),
('IT', 'Specialist', 'emp_password_008', 'tech', 'active', 'halmstad', 'it.specialist@company.com', '+46709888888', '19930808-1008', 'IT-gatan', '17', 2, 1);

-- ==========================================
-- HOTELS (5 hotels)
-- ==========================================
INSERT INTO hotels (name, description, street, streetnumber, city, country, frontdeskopen, frontdeskclose, checkin, checkout, distancetocitycenter) VALUES
('Grand Hotel Stockholm', 'Luxury waterfront hotel in the heart of Stockholm', 'Södra Blasieholmshamnen', '8', 1, 1, '06:00', '23:00', '15:00', '11:00', 500),
('Seaside Resort Halmstad', 'Beautiful beachfront resort with ocean views', 'Strandvägen', '1', 2, 1, '07:00', '22:00', '14:00', '10:00', 3000),
('Berlin Central Hotel', 'Modern hotel near Brandenburg Gate', 'Unter den Linden', '45', 5, 2, '00:00', '23:59', '15:00', '11:00', 1000),
('London Tower Hotel', 'Historic hotel with views of Tower Bridge', 'Tower Bridge Road', '100', 8, 3, '06:00', '23:00', '14:00', '10:00', 2000),
('Paris Eiffel Hotel', 'Charming boutique hotel near Eiffel Tower', 'Avenue de la Bourdonnais', '30', 11, 4, '07:00', '22:00', '16:00', '11:00', 1500);

-- ==========================================
-- CONVENIENCE (Hotel amenities)
-- ==========================================
INSERT INTO convenience (hotel, type) VALUES
-- Grand Hotel Stockholm (hotel_id = 1)
(1, 'wifi'),
(1, 'breakfast'),
(1, 'spa'),
(1, 'parking'),
-- Seaside Resort Halmstad (hotel_id = 2)
(2, 'wifi'),
(2, 'pool'),
(2, 'breakfast'),
(2, 'pet'),
(2, 'parking'),
-- Berlin Central Hotel (hotel_id = 3)
(3, 'wifi'),
(3, 'breakfast'),
(3, 'parking'),
-- London Tower Hotel (hotel_id = 4)
(4, 'wifi'),
(4, 'pool'),
(4, 'breakfast'),
(4, 'spa'),
-- Paris Eiffel Hotel (hotel_id = 5)
(5, 'wifi'),
(5, 'breakfast'),
(5, 'pet');

-- ==========================================
-- ROOMS (approximately 10 rooms per hotel)
-- ==========================================
-- Grand Hotel Stockholm (hotel_id = 1)
INSERT INTO rooms (roomnumber, size, hotel, type, singlebed, doublebed, maxoccupancy, priceperday, currency) VALUES
(101, 25, 1, 'standard', 2, 0, 2, 120, 'eur'),
(102, 30, 1, 'standard', 0, 1, 2, 130, 'eur'),
(103, 28, 1, 'standard', 2, 0, 2, 120, 'eur'),
(201, 35, 1, 'deluxe', 0, 1, 2, 180, 'eur'),
(202, 40, 1, 'deluxe', 1, 1, 3, 200, 'eur'),
(203, 38, 1, 'deluxe', 0, 1, 2, 190, 'eur'),
(301, 50, 1, 'suite', 0, 2, 4, 350, 'eur'),
(302, 55, 1, 'suite', 1, 2, 5, 400, 'eur'),
(303, 60, 1, 'suite', 0, 2, 4, 380, 'eur'),
(304, 45, 1, 'deluxe', 0, 1, 2, 210, 'eur');

-- Seaside Resort Halmstad (hotel_id = 2)
INSERT INTO rooms (roomnumber, size, hotel, type, singlebed, doublebed, maxoccupancy, priceperday, currency) VALUES
(101, 30, 2, 'standard', 2, 0, 2, 90, 'eur'),
(102, 32, 2, 'standard', 0, 1, 2, 95, 'eur'),
(103, 30, 2, 'standard', 2, 0, 2, 90, 'eur'),
(104, 35, 2, 'deluxe', 0, 1, 2, 140, 'eur'),
(201, 40, 2, 'deluxe', 0, 1, 2, 150, 'eur'),
(202, 42, 2, 'deluxe', 1, 1, 3, 170, 'eur'),
(203, 40, 2, 'deluxe', 0, 1, 2, 150, 'eur'),
(301, 60, 2, 'suite', 0, 2, 4, 280, 'eur'),
(302, 65, 2, 'suite', 1, 2, 5, 320, 'eur'),
(303, 55, 2, 'suite', 0, 2, 4, 300, 'eur');

-- Berlin Central Hotel (hotel_id = 3)
INSERT INTO rooms (roomnumber, size, hotel, type, singlebed, doublebed, maxoccupancy, priceperday, currency) VALUES
(101, 22, 3, 'standard', 1, 0, 1, 100, 'eur'),
(102, 28, 3, 'standard', 0, 1, 2, 120, 'eur'),
(103, 28, 3, 'standard', 0, 1, 2, 120, 'eur'),
(201, 32, 3, 'deluxe', 0, 1, 2, 160, 'eur'),
(202, 35, 3, 'deluxe', 1, 1, 3, 180, 'eur'),
(203, 32, 3, 'deluxe', 0, 1, 2, 160, 'eur'),
(301, 45, 3, 'suite', 0, 2, 4, 300, 'eur'),
(302, 50, 3, 'suite', 1, 2, 5, 350, 'eur'),
(303, 48, 3, 'suite', 0, 2, 4, 320, 'eur'),
(304, 40, 3, 'deluxe', 0, 1, 2, 170, 'eur');

-- London Tower Hotel (hotel_id = 4)
INSERT INTO rooms (roomnumber, size, hotel, type, singlebed, doublebed, maxoccupancy, priceperday, currency) VALUES
(101, 26, 4, 'standard', 2, 0, 2, 140, 'eur'),
(102, 30, 4, 'standard', 0, 1, 2, 150, 'eur'),
(103, 28, 4, 'standard', 2, 0, 2, 140, 'eur'),
(201, 38, 4, 'deluxe', 0, 1, 2, 200, 'eur'),
(202, 40, 4, 'deluxe', 1, 1, 3, 220, 'eur'),
(203, 38, 4, 'deluxe', 0, 1, 2, 200, 'eur'),
(301, 55, 4, 'suite', 0, 2, 4, 400, 'eur'),
(302, 60, 4, 'suite', 1, 2, 5, 450, 'eur'),
(303, 58, 4, 'suite', 0, 2, 4, 420, 'eur'),
(304, 42, 4, 'deluxe', 0, 1, 2, 210, 'eur');

-- Paris Eiffel Hotel (hotel_id = 5)
INSERT INTO rooms (roomnumber, size, hotel, type, singlebed, doublebed, maxoccupancy, priceperday, currency) VALUES
(101, 24, 5, 'standard', 1, 0, 1, 110, 'eur'),
(102, 28, 5, 'standard', 0, 1, 2, 130, 'eur'),
(103, 28, 5, 'standard', 0, 1, 2, 130, 'eur'),
(201, 35, 5, 'deluxe', 0, 1, 2, 180, 'eur'),
(202, 38, 5, 'deluxe', 1, 1, 3, 200, 'eur'),
(203, 35, 5, 'deluxe', 0, 1, 2, 180, 'eur'),
(301, 50, 5, 'suite', 0, 2, 4, 350, 'eur'),
(302, 55, 5, 'suite', 1, 2, 5, 400, 'eur'),
(303, 52, 5, 'suite', 0, 2, 4, 370, 'eur'),
(304, 40, 5, 'deluxe', 0, 1, 2, 190, 'eur');

-- ==========================================
-- ACTIVITIES
-- ==========================================
INSERT INTO activities (type) VALUES
('f1'),
('soccer'),
('climbing');

-- ==========================================
-- ACTIVITIES DESTINATIONS (Activities available at hotels)
-- ==========================================
INSERT INTO activitiesdestinations (activity, hotel, activityprice, currency) VALUES
-- Grand Hotel Stockholm
(2, 1, 150, 'eur'), -- soccer
(3, 1, 250, 'eur'), -- climbing
-- Seaside Resort Halmstad
(2, 2, 120, 'eur'), -- soccer
(3, 2, 180, 'eur'), -- climbing
-- Berlin Central Hotel
(1, 3, 800, 'eur'), -- F1
(2, 3, 100, 'eur'), -- soccer
-- London Tower Hotel
(1, 4, 1000, 'eur'), -- F1
(3, 4, 200, 'eur'), -- climbing
-- Paris Eiffel Hotel
(1, 5, 950, 'eur'), -- F1
(2, 5, 130, 'eur'), -- soccer
(3, 5, 220, 'eur'); -- climbing

-- ==========================================
-- BOOKINGS (Sample bookings)
-- ==========================================
INSERT INTO bookings (user, room, checkin, checkout, price, currency, status, message) VALUES
(1, 1, '2025-01-15', '2025-01-18', 360, 'eur', 'confirmed', 'Looking forward to my stay!'),
(2, 11, '2025-02-10', '2025-02-14', 380, 'eur', 'confirmed', 'Beach vacation'),
(3, 31, '2025-03-05', '2025-03-08', 420, 'eur', 'pending', 'Business trip to London'),
(4, 21, '2025-04-20', '2025-04-23', 480, 'eur', 'confirmed', NULL),
(5, 41, '2025-05-12', '2025-05-15', 390, 'eur', 'confirmed', 'Anniversary trip'),
(6, 5, '2025-06-01', '2025-06-03', 360, 'eur', 'cancelled', 'Plans changed');

-- ==========================================
-- COTRAVELLERS (Additional guests)
-- ==========================================
INSERT INTO cotravellers (bookingid, nationalidnumber, firstname, lastname, email, phone, room, checkin, checkout, price, currency) VALUES
(1, '19920505-5555', 'Sarah', 'Testson', 'sarah.test@example.com', '+46704567890', 2, '2025-01-15', '2025-01-18', 390, 'eur'),
(2, '19880915-6666', 'Emma', 'Doe', 'emma.doe@example.com', '+46705678901', 12, '2025-02-10', '2025-02-14', 380, 'eur'),
(5, '19931120-7777', 'Pierre', 'Dupont', 'pierre.dupont@example.com', '+33143456789', 42, '2025-05-12', '2025-05-15', 390, 'eur');

-- ==========================================
-- END OF TEST DATA
-- ==========================================