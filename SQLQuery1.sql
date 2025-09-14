use booking;

select * from bookings;

select * from Trips;
insert into Trips (Destination, BaseFare) values ('Nairobi', 38250.00);
insert into Trips (Destination, BaseFare) values ('Lagos', 45900);
insert into Trips (Destination, BaseFare) values ('Paris', 63050);



select * from Customers;
insert into Customers (FirstName, LastName, Email, CustomerType, LoyaltyPoints) values ('John', 'Doe', 'John@email.com', 'VIP', 0);
