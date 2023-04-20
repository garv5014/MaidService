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
	date DATE Not Null,
	start_time TIME Not Null,
	duration INTERVAL Not NULL,
	CONSTRAINT schedule_pk PRIMARY KEY (id),
	constraint schedule_uniq unique (date, start_time)
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
			INSERT INTO public.schedule (date, start_time, duration)
				VALUES(start_date + interval_offset, r.start_time, r.duration);
			
		end loop;
		interval_offset = interval_offset + interval '1 days';
	end loop; 
end;
$function$
;



CREATE OR replace FUNCTION PUBLIC.getavailableslotsforacontract(target_cleaner_id integer,contract_id integer)
returns TABLE (id integer,"date" date,start_time time,duration interval) 
language plpgsql 
AS $function$
DECLARE 
schedule_id INTEGER;
target_date date;
BEGIN
  SELECT cc2.schedule_date
  INTO   target_date
  FROM   cleaning_contract cc2
  WHERE  (
                cc2.id = contract_id);
  
  RETURN query
  (
             select     s.id         AS id,
                        s.date       AS "date",
                        s.start_time AS start_time,
                        s.duration   AS duration
             FROM       schedule s
             INNER JOIN cleaner_availability ca
             ON         (
                                   s.id = ca.schedule_id
                        AND        s.date = target_date
                        AND        ca.cleaner_id = target_cleaner_id)
             LEFT JOIN  cleaner_assignments ca2
             ON         (
                                   ca2.cleaner_availability_id = ca.id)
             EXCEPT
             SELECT     s2.id         AS id,
                        s2.date       AS "date",
                        s2.start_time AS start_time,
                        s2.duration   AS duration
             FROM       cleaning_contract cc
             INNER JOIN cleaner_assignments cass
             ON         (
                                   cc.id = cass.contract_id)
             INNER JOIN cleaner_availability ca3
             ON         (
                                   cass.cleaner_availability_id = ca3.id)
             INNER JOIN schedule s2
             ON         (
                                   s2.id = ca3.schedule_id));
END;$function$;



CREATE OR REPLACE FUNCTION public.getallassignedslotsforacleaner(target_cleaner_id integer)
 RETURNS TABLE(id integer, cust_id integer, date_completed date, schedule_date timestamp without time zone, cost money, requested_hours interval, est_sqft integer, num_of_cleaners integer, notes text, location_id integer, cleaning_type_id integer, start_time time without time zone)
 LANGUAGE plpgsql
AS $function$
BEGIN
	RETURN query
	(
select cc.id as id,
		   cc.cust_id as cust_id,
		   cc.date_completed as date_completed,
		   cc.schedule_date as schedule_date,
		   cc."cost" as "cost",
		   cc.requested_hours as requested_hours,
		   cc.est_sqft as est_sqft,
		   cc.num_of_cleaners as num_of_cleaners,
		   cc.notes as notes,
		   cc.location_id as location_id ,
		   cc.cleaning_type_id as cleaning_type_id,
		   min(s.start_time) as start_time
	from cleaning_contract cc
	left join cleaner_assignments ca on (cc.id = ca.contract_id)
	left join cleaner_availability ca2 on (ca.cleaner_availability_id = ca2.id)
	left join schedule s on (ca2.schedule_id = s.id)
	where (ca2.cleaner_id = target_cleaner_id)
	group by 1,2,3,4,5,6,7,8,9,10,11
	having( cc.schedule_date > now())
	order by cc.schedule_date, start_time);
END;
$function$
;