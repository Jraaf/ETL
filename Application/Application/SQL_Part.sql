create database ETL
use ETL
drop table Trips
create table Trips(
id int primary key identity,
tpep_pickup_datetime date,
tpep_dropoff_datetime date,
passenger_count int,
trip_distance float,
store_and_fwd_flag varchar(5),
PULocationID int,
DOLocationID int,
fare_amount money,
tip_amount money
)

select * from Trips

CREATE INDEX PULocationID_TipAmount ON Trips(PULocationID, tip_amount);
