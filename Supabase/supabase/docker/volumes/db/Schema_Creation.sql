drop table if exists  customer cascade;
drop table if exists location cascade;
drop table if exists cleaner cascade;
drop table if exists cleaning_type cascade;
drop table if exists cleaning_contract cascade;
drop table if exists cc_cleaner cascade;
drop table if exists cust_review_cleaner cascade;
drop table if exists cleaner_review_cust  cascade;
drop table if exists customer_payment cascade;
drop table if exists day_template cascade;
drop table if exists customer_payment cascade;
drop table if exists schedule cascade;
drop table if exists contract_schedule cascade;




Create table customer (
	id serial4 Not Null, 
	auth_id uuid Not Null,
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
	state text Not Null, 
	zipcode text Not Null,
	apartment_number text null,
	CONSTRAINT location_pk PRIMARY KEY (id)
);

Create table cleaner (
	id serial4 Not Null, 
	auth_id uuid Not Null,
	email text Not Null, 
	firstname text Not Null,
	surname text Not Null, 
	phone_number text Not Null, 
	bio text null, 
	verified bool null, 
	/* service_radius int4 null,
	*/
	hire_date timestamp Not Null,
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
	cust_id int4 REFERENCES customer (id) on delete set null,
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

CREATE TABLE cc_cleaner (
	id serial4 Not Null,
	contract_id int4 Not Null  REFERENCES cleaning_contract (id) on delete set null,
	cleaner_id int4 Not Null REFERENCES cleaner (id) on delete set null,
	CONSTRAINT cc_cleaner_pk PRIMARY KEY (id),
	unique (contract_id,cleaner_id)
);


CREATE TABLE cust_review_cleaner  (
	id serial4 Not Null,
	cust_id int4 Not Null REFERENCES customer (id) on delete set null,
	cleaner_id int4 Not Null REFERENCES cleaner (id) on delete set null,
	rating int4 Not Null constraint between1and5 check(rating <= 5 and rating >= 1), 
	review text Null, 
	CONSTRAINT cust_review_cleaner_pk PRIMARY KEY (id)
);

CREATE TABLE cleaner_review_cust  (
	id serial4 Not Null,
	cust_id int4 Not Null REFERENCES customer (id) on delete set null,
	cleaner_id int4 Not Null REFERENCES cleaner (id) on delete set null,
	rating int4 Not Null constraint between1and5 check(rating <= 5 and rating >= 1), 
	review text Null, 
	CONSTRAINT cleaner_review_cust_pk PRIMARY KEY (id)
);


CREATE TABLE customer_payment (
	id serial4 Not Null,
	cust_id int4 Not Null REFERENCES customer (id) on delete set null,
	contract_id int4 Not Null  REFERENCES cleaning_contract (id) on delete set null,
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
	CONSTRAINT schedule_pk PRIMARY KEY (id)
);

CREATE TABLE contract_schedule (
	id serial4 Not Null,
	contract_id int4 Not Null REFERENCES cleaning_contract (id) on delete set null, 
	schedule_id int4 Not Null REFERENCES schedule (id) on delete set null, 
	CONSTRAINT contract_schedule_pk PRIMARY KEY (id)
);