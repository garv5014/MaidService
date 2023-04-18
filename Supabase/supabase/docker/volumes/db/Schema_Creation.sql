SET
    search_path TO public;

DROP SCHEMA IF EXISTS public CASCADE;

CREATE SCHEMA public;

SET
    search_path TO public;

grant usage on schema public to postgres,
anon,
authenticated,
service_role;

--grant all privileges on all tables in schema public to postgres
--anon,
--authenticated,
--service_role;

grant all privileges on all functions in schema public to postgres,
anon,
authenticated,
service_role;

grant all privileges on all sequences in schema public to postgres,
anon,
authenticated,
service_role;

alter default privileges in schema public grant all on tables to postgres,
anon,
authenticated,
service_role;

alter default privileges in schema public grant all on functions to postgres,
anon,
authenticated,
service_role;

alter default privileges in schema public grant all on sequences to postgres,
anon,
authenticated,
service_role;


drop table if exists  customer cascade;
drop table if exists location cascade;
drop table if exists cleaner cascade;
drop table if exists cleaning_type cascade;
drop table if exists cleaning_contract cascade;
drop table if exists cleaner_assingments cascade;
drop table if exists cust_review_cleaner cascade;
drop table if exists cleaner_review_cust  cascade;
drop table if exists customer_payment cascade;
drop table if exists day_template cascade;
drop table if exists customer_payment cascade;
drop table if exists schedule cascade;
drop table if exists cleaner_availability cascade;




Create table customer (
	id serial4 Not Null, 
	auth_id uuid NULL REFERENCES auth.users,
	email text Not Null, 
	firstname text Not Null,
	surname text Not Null, 
	phone_number text Not Null, 
	CONSTRAINT customer_pk PRIMARY KEY (id),
	unique(auth_id)
);

Create table location (
	id serial4 NOT NULL,
	address text Not Null,
	city text Not Null,
	state text Not Null, 
	zipcode text Not Null,
	apartment_number text null,
	CONSTRAINT location_pk PRIMARY KEY (id)
);

Create table cleaner (
	id serial4 Not Null, 
	auth_id uuid Null REFERENCES auth.users,
	email text Not Null, 
	firstname text Not Null,
	surname text Not Null, 
	phone_number text Not Null, 
	bio text null, 
	verified bool null, 
	/* service_radius int4 null,
	*/
	hire_date date default CURRENT_DATE,
	CONSTRAINT cleaner_pk PRIMARY key (id),
	unique(auth_id)
);

Create TABLE cleaning_type(
	id serial4 Not Null,
	type text Not Null, 
	description text Not Null,
	constraint cleaning_type_pk PRIMARY KEY (id)
);

CREATE TABLE cleaning_contract (
	id serial4 Not Null,
	cust_id int4 Not Null REFERENCES customer (id) on delete set null,
	date_completed date Null, 
	schedule_date timestamp Not Null, 
	cost money Not Null,
	requested_hours Interval Not Null constraint notneg check (requested_hours > '0 hours'), 
	est_sqft int4 Not Null, 
	num_of_cleaners int4 Not Null constraint morethan1cleaner check (num_of_cleaners > 0),
	notes text Null,
	location_id int4 Not Null REFERENCES location (id) on delete set null,
	cleaning_type_id int4 Not Null REFERENCES cleaning_type (id) on delete set null,
	CONSTRAINT cleaning_contract_pk PRIMARY KEY (id)
);



CREATE TABLE cust_review_cleaner  (
	id serial4 Not Null,
	cust_id int4 Not Null REFERENCES customer (id) on delete set null,
	cleaner_id int4 Not Null REFERENCES cleaner (id) on delete set null,
	rating int4 Not Null constraint between1and5 check(rating <= 5 and rating >= 1), 
	review text Null, 
	CONSTRAINT cust_review_cleaner_pk PRIMARY KEY (cust_id, cleaner_id)
);

CREATE TABLE cleaner_review_cust  (
	id serial4 Not Null,
	cust_id int4 Not Null REFERENCES customer (id) on delete set null,
	cleaner_id int4 Not Null REFERENCES cleaner (id) on delete set null,
	rating int4 Not Null constraint between1and5 check(rating <= 5 and rating >= 1), 
	review text Null, 
	CONSTRAINT cleaner_review_cust_pk PRIMARY KEY (cust_id, cleaner_id)
);


CREATE TABLE customer_payment (
	id serial4 Not Null,
	cust_id int4 Not Null REFERENCES customer (id) ,
	contract_id int4 Not Null  REFERENCES cleaning_contract (id) ,
	amount_paid money Not Null, 
	received_date date Not Null,
	CONSTRAINT customer_receipt_pk PRIMARY KEY (id)
);

CREATE TABLE day_template(
	id serial4 Not Null,
	start_time time Not Null, 
	duration Interval Not Null, 
	CONSTRAINT day_template_pk PRIMARY KEY (id)
);

CREATE TABLE schedule (
	id serial4 Not Null,
	schedule_date DATE Not Null,
	start_time TIME Not Null,
	duration INTERVAL Not NULL,
	CONSTRAINT schedule_pk PRIMARY KEY (id),
	constraint schedule_uniq unique (schedule_date, start_time)
);

CREATE TABLE cleaner_availability (
	id serial4 Not Null,
	cleaner_id int4 Not Null REFERENCES cleaner (id) , 
	schedule_id int4 Not Null REFERENCES schedule (id) , 
	CONSTRAINT contract_schedule_pk PRIMARY KEY (cleaner_id, schedule_id, id ),
	unique (id)
);												 

CREATE TABLE cleaner_assignments( 
	id serial4 Not Null,
	contract_id int4 Not Null  REFERENCES cleaning_contract (id) on delete set null,
	cleaner_availability_id int4 Not Null REFERENCES cleaner_availability (id) on delete set null,
	CONSTRAINT cleaner_assingments_pk PRIMARY KEY (contract_id, cleaner_availability_id)
);

-- Functions
CREATE OR REPLACE procedure  public.MakeScheduleForASpecificDay(target_day date)
 LANGUAGE plpgsql
AS $function$
declare
template_curs cursor for select * from public.day_template ;
begin
	for r in template_curs 
	loop
		INSERT INTO public.schedule (schedule_date, start_time, duration)
			VALUES(target_day, r.start_time, r.duration);
	end loop;
end;
$function$
;

CREATE OR REPLACE procedure  MakeScheduleForTheNextMonth(start_date date default current_date)
 LANGUAGE plpgsql
AS $function$
declare
template_curs cursor for select * from public.day_template ;
interval_offset interval = interval '0 days';
begin
	for counter in 0..28 loop
		for r in template_curs 
		loop
			INSERT INTO public.schedule (schedule_date, start_time, duration)
				VALUES(start_date + interval_offset, r.start_time, r.duration);
			
		end loop;
		interval_offset = interval_offset + interval '1 days';
	end loop; 
end;
$function$
;