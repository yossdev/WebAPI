BEGIN;


CREATE TABLE IF NOT EXISTS "JobTitle"
(
    id bigserial NOT NULL PRIMARY KEY,
    code text NOT NULL UNIQUE,
    name text NOT NULL,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp with time zone DEFAULT now()
);

CREATE TABLE IF NOT EXISTS "JobPosition"
(
    id bigserial NOT NULL PRIMARY KEY,
	code text NOT NULL UNIQUE,
    name text NOT NULL,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp with time zone NOT NULL DEFAULT now(),
    job_title_id bigint NOT NULL REFERENCES "JobTitle" (id) ON UPDATE NO ACTION ON DELETE RESTRICT
);

CREATE SEQUENCE IF NOT EXISTS seq_emp_nik
	INCREMENT 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1;

CREATE TABLE IF NOT EXISTS "Employee"
(
    id uuid NOT NULL DEFAULT uuid_generate_v4() PRIMARY KEY,
    nik text NOT NULL UNIQUE CHECK (nik ~ '^EMP-[0-9]+$') DEFAULT TO_CHAR(nextval('seq_emp_nik'::regclass), '"EMP-"fm000000'),
    name text NOT NULL,
    address text NOT NULL,
    created_at timestamp with time zone NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp with time zone NOT NULL DEFAULT now(),
    job_title_id bigint NOT NULL REFERENCES "JobTitle" (id) ON UPDATE NO ACTION ON DELETE NO ACTION,
    job_position_id bigint NOT NULL REFERENCES "JobPosition" (id) ON UPDATE NO ACTION ON DELETE NO ACTION
);


END;
