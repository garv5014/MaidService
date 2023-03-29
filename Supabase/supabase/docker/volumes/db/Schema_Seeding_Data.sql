-- Sample rows for customer table
INSERT INTO customer (auth_id, email, firstname, surname, phone_number) VALUES
('5b5e2c5d-9e37-49a2-bb54-8c453740134f', 'johndoe@example.com', 'John', 'Doe', '555-123-4567'),
('7c8e38aa-3a97-4813-a6c7-fcfa9bfe2767', 'janedoe@example.com', 'Jane', 'Doe', '555-987-6543'),
('18bba30e-bc28-4f56-aeed-4c4a4f578da1', 'bobsmith@example.com', 'Bob', 'Smith', '555-555-5555');


-- Location
INSERT INTO location (address, state, zipcode, apartment_number)
VALUES ('123 Main St', 'CA', '90210', null),
       ('456 Elm St', 'NY', '10001', 'Apt 2B'),
       ('789 Oak St', 'TX', '75001', null);

-- Sample rows for cleaner table
INSERT INTO cleaner (auth_id, email, firstname, surname, phone_number, bio, verified, hire_date) VALUES
('8e882e4d-1de4-4eb4-8bdf-69d00e3e3ca3', 'janejones@example.com', 'Jane', 'Jones', '555-444-5555', 'I have been cleaning professionally for 5 years', true, '2022-01-01'),
('c3c34d3b-f16e-47f9-ae31-64f92d8c01d6', 'petersmith@example.com', 'Peter', 'Smith', '555-666-7777', null, false, '2022-02-15'),
('5b5e2c5d-9e37-49a2-bb54-8c453740134f', 'johnsmith@example.com', 'John', 'Smith', '555-555-1234', 'I am thorough and efficient', true, '2022-03-01');

-- Cleaning Type
INSERT INTO cleaning_type (type, description)
VALUES ('Standard Cleaning', 'General cleaning of floors, surfaces, and bathrooms'),
       ('Deep Cleaning', 'Includes cleaning of hard-to-reach areas and appliances'),
       ('Move-in/Move-out Cleaning', 'Thorough cleaning of entire house or apartment');

-- Cleaning Contract
INSERT INTO cleaning_contract (cust_id, date_completed, schedule_date, cost, requested_hours, est_sqft, num_of_cleaners, notes, location_id, cleaning_type_id)
VALUES                        (1, '2023-03-17', '2023-03-17 10:00:00', 250.00, '4 hours', 1000, 2, 'Please pay in cash', 1, 1),
                              (2, null, '2023-03-18 14:00:00', 150.00, '2 hours', 500, 1, null, 2, 2),
                              (3, null, '2023-03-19 12:00:00', 200.00, '3 hours', 750, 1, 'Please bring your own cleaning supplies', 3, 3),
                              (1, null, (select NOW() + INTERVAL '2 days'), 100.00, '2 hours', 500, 1, null, 1, 1),
                              (1, null, (select NOW() + INTERVAL '3 days'), 120.00, '4 hours', 250, 1 ,'Do not be mean', 2, 2);

-- CC Cleaner
INSERT INTO cc_cleaner (contract_id, cleaner_id)
VALUES (1, 1),
       (1, 2),
       (2, 2);

-- Cust Review Cleaner
INSERT INTO cust_review_cleaner (cust_id, cleaner_id, rating, review)
VALUES (1, 1, 4, 'Alice did a great job!'),
       (2, 2, 3, 'Bob was okay.');
       /* (3, 1, 5, 'Charlie was amazing!'); */

-- Cleaner Review Cust
INSERT INTO cleaner_review_cust (cust_id, cleaner_id, rating, review)
VALUES (1, 1, 5, 'John was a great customer!'),
       (2, 2, 2, 'Jane was difficult to work with.');
       /* (3, 1, 4, 'Bob was a pleasure to work with.'); */

       -- Insert statements for table 'customer_payment'
INSERT INTO customer_payment (cust_id, contract_id, amount_paid, received_date)
VALUES (1, 1, 200.00, '2023-03-17'), 
       (1, 1, 50.00, '2023-03-18');
       
-- Insert statements for table 'day_template'
INSERT INTO day_template (start_time, duration)
VALUES ('08:00:00', '2 hours'), 
       ('10:00:00', '2 hours'), 
       ('12:00:00', '2 hours'),
       ('03:00:00', '2 hours');
       
-- Insert statements for table 'schedule'
INSERT INTO schedule (date, start_time, duration)
VALUES ('2023-03-17', '08:00:00', '2 hours'), 
       ('2023-03-18', '10:00:00', '2 hours'), 
       ('2023-03-19','12:00:00', '2 hours');

-- Conract_schedule
INSERT INTO contract_schedule (contract_id, schedule_id)
VALUES (1, 1),
       (1, 2),
       (2, 3);





