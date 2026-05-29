USE `coworkingdb`;

-- Clear any existing data safely by disabling foreign keys temporarily
SET FOREIGN_KEY_CHECKS = 0;
TRUNCATE TABLE `Status`;
TRUNCATE TABLE `Features_has_Workstation`;
TRUNCATE TABLE `Features`;
TRUNCATE TABLE ` Booking`;
TRUNCATE TABLE `Customer`;
TRUNCATE TABLE `Workstation`;
TRUNCATE TABLE `Area`;
SET FOREIGN_KEY_CHECKS = 1;

-- -----------------------------------------------------
-- Sample Data for Table `Area`
-- -----------------------------------------------------
INSERT INTO `Area` (`idArea`, `Name`, `Info`) VALUES
(1, 'Open Space Deck', 'Lively environment with hot desks, communal tables, and high-speed Wi-Fi.'),
(2, 'Quiet Focus Zone', 'Strictly silent room designed for deep focus, software development, and writing.'),
(3, 'Executive Meeting Wing', 'Private meeting rooms and boardrooms equipped with conference tech.');

-- -----------------------------------------------------
-- Sample Data for Table `Workstation`
-- -----------------------------------------------------
INSERT INTO `Workstation` (`Id`, `Description`, `Opening`, `Closing`, `MaxReservations`, `idArea`) VALUES
(1, 'Hot Desk - Window View 01', '08:00:00', '20:00:00', 1, 1),
(2, 'Hot Desk - Communal Table 05', '08:00:00', '20:00:00', 1, 1),
(3, 'Dedicated Coder Pod 12', '00:00:00', '23:59:59', 1, 2),
(4, 'Dedicated Designer Pod 14', '00:00:00', '23:59:59', 1, 2),
(5, 'Brainstorming Room A (Large)', '09:00:00', '18:00:00', 12, 3),
(6, 'Huddle Space B (Small)', '09:00:00', '21:00:00', 4, 3);

-- -----------------------------------------------------
-- Sample Data for Table `Customer`
-- -----------------------------------------------------
INSERT INTO `Customer` (`Id`, `Name`, `Address`, `Email`) VALUES
(1, 'Alice Smith', '123 Cyber Way, Milan', 'alice.smith@devmail.com'),
(2, 'Bob Jones', '456 Design Road, Florence', 'bob.jones@creativeagency.it'),
(3, 'Charlie Brown', '789 Startup Blvd, Rome', 'contact@charlieventures.com'),
(4, 'Diana Prince', '11 Amazonia Ave, Turin', 'diana@justicecorp.org');

-- -----------------------------------------------------
-- Sample Data for Table ` Booking` (Note the leading space matching your schema)
-- -----------------------------------------------------
INSERT INTO ` Booking` (`Id`, `DateStart`, `DateEnd`, `idClient`, `idWorkstation`, `Notes`) VALUES
(101, '2026-06-01 09:00:00', '2026-06-01 17:00:00', 1, 1, 'Prefers a location next to an extension cord adapter.'),
(102, '2026-06-01 10:00:00', '2026-06-01 13:00:00', 2, 5, 'Client presentation sync meeting.'),
(103, '2026-06-02 08:00:00', '2026-06-05 18:00:00', 3, 3, 'Full-week booking for crunch time project deployment.'),
(104, '2026-06-03 14:00:00', '2026-06-03 16:00:00', 4, 6, 'Quick interview loop panel session.');

-- -----------------------------------------------------
-- Sample Data for Table `Features`
-- -----------------------------------------------------
INSERT INTO `Features` (`idFeatures`, `Name`, `Description`) VALUES
(1, 'Ergonomic Chair', 'High-end Steelcase ergonomic task chair designed for long hours of seated work.'),
(2, 'Dual 4K Monitors', 'Two 27-inch 4K USB-C monitors with power delivery enabled.'),
(3, '4K Ultra-Wide Projector', 'Ultra short throw laser projector mapped onto a primary presentation wall.'),
(4, 'Interactive Smartboard', 'Touchscreen white-board with digital export capabilities via QR code.'),
(5, 'Wired Gigabit Ethernet', 'Dedicated RJ45 LAN port routing clean 1Gbps fiber connections.');

-- -----------------------------------------------------
-- Sample Data for Table `Features_has_Workstation`
-- -----------------------------------------------------
INSERT INTO `Features_has_Workstation` (`Features_idFeatures`, `Workstation_Id`) VALUES
(1, 1), -- Ergonomic chair at Hot Desk 01
(1, 3), -- Ergonomic chair at Coder Pod 12
(1, 4), -- Ergonomic chair at Designer Pod 14
(2, 3), -- Dual Monitors at Coder Pod 12
(2, 4), -- Dual Monitors at Designer Pod 14
(5, 3), -- Gigabit LAN at Coder Pod 12
(3, 5), -- Projector in Brainstorming Room A
(4, 5); -- Smartboard in Brainstorming Room A

-- -----------------------------------------------------
-- Sample Data for Table `Status`
-- -----------------------------------------------------
INSERT INTO `Status` (`Id`, `Status`, `idBooking`) VALUES
(1, 'Confirmed', 101),
(2, 'Awaiting Payment', 102),
(3, 'Confirmed', 103),
(4, 'Cancelled by User', 104);