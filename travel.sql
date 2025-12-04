DROP DATABASE IF EXISTS `travel`;

CREATE DATABASE travel;

USE travel;

DROP USER IF EXISTS 'travel'@'localhost';
FLUSH PRIVILEGES;
CREATE USER travel@localhost identified by 'travel';
GRANT ALL PRIVILEGES ON travel.* TO 'travel'@'localhost';

CREATE TABLE user (
  id BIGINT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
  firstname VARCHAR(50) NOT NULL,
  lastname VARCHAR(50) NOT NULL,
  email VARCHAR(254) UNIQUE NOT NULL,
  username VARCHAR(50) UNIQUE NOT NULL,
  password VARCHAR(255) NOT NULL,
  created DATETIME DEFAULT CURRENT_TIMESTAMP() NOT NULL,
  status ENUM('active', 'inactive', 'suspended')
);

-- skapa index på user email
CREATE INDEX idx_user_email ON user(email);

INSERT INTO user (firstname, lastname, email, username, password, status) VALUES
('travel', 'traveller', 'traveller@test.test', 'traveluser', 'hashed_password_1', 'active'),
('inactive_travel', 'inactive_traveler', 'inactive_traveller@test.test', 'inactivetraveluser', 'hashed_password_1', 'inactive'),
('suspended_travel', 'suspended_traveler', 'suspended_traveller@test.test', 'suspendedtraveluser', 'hashed_password_1', 'suspended');