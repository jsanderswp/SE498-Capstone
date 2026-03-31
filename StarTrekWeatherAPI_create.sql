-- Created by Redgate Data Modeler (https://datamodeler.redgate-platform.com)
-- Last modification date: 2026-03-31 02:21:50.957

-- tables
-- Table: planets
CREATE TABLE planets (
    name varchar(40)  NOT NULL,
    solar_system varchar(40)  NOT NULL,
    max_temp float(1)  NOT NULL,
    min_temp float(1)  NOT NULL,
    description varchar(255)  NOT NULL,
    CONSTRAINT planets_pk PRIMARY KEY  (name)
);

-- End of file.

