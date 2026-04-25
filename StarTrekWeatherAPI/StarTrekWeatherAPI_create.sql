-- Created by Redgate Data Modeler (https://datamodeler.redgate-platform.com)
-- Last modification date: 2026-03-31 02:21:50.957

-- tables
-- Table: planets
CREATE TABLE planets (
    name varchar(40)  NOT NULL,
    solar_system varchar(40)  NOT NULL,
    max_temp float(1)  NOT NULL,
    min_temp float(1)  NOT NULL,
    atmospheric_pressure float(1)  NOT NULL DEFAULT 0.0,
    description varchar(255)  NOT NULL,
    CONSTRAINT planets_pk PRIMARY KEY  (name)
);


INSERT INTO planets (name, solar_system, max_temp, min_temp, atmospheric_pressure, description) VALUES
('Vulcan', 'Vulcan System', 67.0, 18.0, 0.5, 'Arid desert world, home of the Vulcans, known for logic and stoicism.'),
('Qonos', 'Qonos System', 45.0, -10.0, 0.9, 'Harsh homeworld of the Klingon Empire, covered in dark continents and toxic seas.'),
('Romulus', 'Romii System', 38.0, 5.0, 1.0, 'Lush twin world of the Romulan Star Empire, politically volatile and secretive.'),
('Bajor', 'Bajoran System', 32.0, -5.0, 1.0, 'Spiritual homeworld of the Bajorans, recovering from Cardassian occupation.'),
('Cardassia Prime', 'Cardassian System', 55.0, 10.0, 1.2, 'Resource-depleted militaristic homeworld of the Cardassian Union.'),
('Betazed', 'Betazed System', 29.0, 8.0, 1.0, 'Peaceful world of telepathic humanoids, known for lush forests and open culture.'),
('Ferenginar', 'Ferengi System', 20.0, 10.0, 1.4, 'Perpetually rain-soaked homeworld of the Ferengi, center of galactic commerce.'),
('Risa', 'Risian System', 35.0, 22.0, 1.0, 'Pleasure planet with a perfectly controlled climate, a top tourist destination.'),
('Andoria', 'Andorian System', -15.0, -80.0, 0.9, 'Frozen moon of a gas giant, home of the blue-skinned, antennae-bearing Andorians.'),
('Tellar Prime', 'Tellarite System', 25.0, -20.0, 1.1, 'Rugged homeworld of the argumentative Tellarites, founding members of the Federation.'),
('Trill', 'Trill System', 28.0, -3.0, 1.0, 'Serene world home to the joined Trill species and their symbiont pools.'),
('Denobula', 'Denobulan System', 40.0, 12.0, 1.3, 'Densely populated homeworld of the Denobulans, known for communal family structures.'),
('Talos IV', 'Talosian System', 22.0, -8.0, 0.8, 'Forbidden world of the powerful Talosians, masters of illusion and mental projection.'),
('Argelius II', 'Argelian System', 33.0, 18.0, 1.0, 'Hedonistic pleasure world known for its pacifist inhabitants and vibrant nightlife.'),
('Excalbia', 'Excalbian System', 500.0, 400.0, 0.0, 'Volcanic lava world inhabited by rock-like beings who study the concept of good and evil.'),
('Miri', 'Miri System', 26.0, 5.0, 1.0, 'Near-identical copy of Earth, devastated by a life-prolongation plague centuries ago.'),
('Genesis Planet', 'Mutara System', 45.0, 10.0, 0.7, 'Artificially created world born from the Genesis Device, unstable and short-lived.'),
('Neural', 'Neural System', 38.0, 12.0, 1.0, 'Primitive pre-industrial world caught in a proxy conflict between the Federation and Klingons.'),
('Magna Roma', 'Magna Roman System', 31.0, 8.0, 1.0, 'Parallel Earth where Roman civilization never fell, blending ancient culture with modern tech.'),
('Melkoria', 'Melkorian System', 15.0, -40.0, 0.6, 'Remote icy frontier world used as a neutral meeting point between rival galactic powers.');
-- End of file.