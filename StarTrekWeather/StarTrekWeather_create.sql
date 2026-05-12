-- Drop tables if they exist (so container restarts are clean)
DROP TABLE IF EXISTS user_pokemon_location_gyms;
DROP TABLE IF EXISTS user_pokemon_location_buildings;
DROP TABLE IF EXISTS user_pokemon_locations;
DROP TABLE IF EXISTS user_planets;
DROP TABLE IF EXISTS users;

-- users table
CREATE TABLE users (
    username VARCHAR(20) PRIMARY KEY,
    password_hash VARCHAR(255) NOT NULL
);

-- user_planets table
CREATE TABLE user_planets (
    users_username VARCHAR(20) NOT NULL,
    planets_name VARCHAR(40) NOT NULL,
    PRIMARY KEY (users_username, planets_name),
    CONSTRAINT fk_user
        FOREIGN KEY (users_username)
            REFERENCES users (username)
            ON DELETE CASCADE
);

-- user_pokemon_locations table
CREATE TABLE user_pokemon_locations (
    users_username VARCHAR(20) NOT NULL,
    location_name VARCHAR(100) NOT NULL,
    PRIMARY KEY (users_username, location_name),
    CONSTRAINT fk_user_pokemon_location
        FOREIGN KEY (users_username)
            REFERENCES users (username)
            ON DELETE CASCADE
);

-- user_pokemon_location_gyms table
CREATE TABLE user_pokemon_location_gyms (
    id SERIAL PRIMARY KEY,
    users_username VARCHAR(20) NOT NULL,
    location_name VARCHAR(100) NOT NULL,
    gym_name VARCHAR(100) NOT NULL,
    CONSTRAINT fk_user_pokemon_location_gym
        FOREIGN KEY (users_username, location_name)
            REFERENCES user_pokemon_locations (users_username, location_name)
            ON DELETE CASCADE
);

-- user_pokemon_location_buildings table
CREATE TABLE user_pokemon_location_buildings (
    id SERIAL PRIMARY KEY,
    users_username VARCHAR(20) NOT NULL,
    location_name VARCHAR(100) NOT NULL,
    building_name VARCHAR(100) NOT NULL,
        CONSTRAINT fk_user_pokemon_location_building
            FOREIGN KEY (users_username, location_name)
                REFERENCES user_pokemon_locations (users_username, location_name)
                ON DELETE CASCADE
);

-- Seed users
INSERT INTO users (username, password_hash)
VALUES
    ('kirk', 'hashed_kirk_password'),
    ('spock', 'hashed_spock_password'),
    ('picard', 'hashed_picard_password');

-- Seed user_planets (must reference existing users)
INSERT INTO user_planets (users_username, planets_name)
VALUES
    ('kirk', 'Earth'),
    ('kirk', 'Qo' || 'nos'),
    ('spock', 'Vulcan'),
    ('picard', 'Earth'),
    ('picard', 'Romulus');