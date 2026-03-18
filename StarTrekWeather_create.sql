-- Created by Redgate Data Modeler (https://datamodeler.redgate-platform.com)
-- Last modification date: 2026-03-18 07:36:54.528

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

-- Table: user_planets
CREATE TABLE user_planets (
    users_username varchar(20)  NOT NULL,
    planets_name varchar(40)  NOT NULL,
    CONSTRAINT user_planets_pk PRIMARY KEY  (users_username,planets_name)
);

-- Table: users
CREATE TABLE users (
    username varchar(20)  NOT NULL,
    password_hash varchar(255)  NOT NULL,
    CONSTRAINT users_pk PRIMARY KEY  (username)
);

-- foreign keys
-- Reference: user_planets_planets (table: user_planets)
ALTER TABLE user_planets ADD CONSTRAINT user_planets_planets
    FOREIGN KEY (planets_name)
    REFERENCES planets (name);

-- Reference: user_planets_users (table: user_planets)
ALTER TABLE user_planets ADD CONSTRAINT user_planets_users
    FOREIGN KEY (users_username)
    REFERENCES users (username);

-- End of file.

