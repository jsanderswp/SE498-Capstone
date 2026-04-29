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
    image_url varchar(255)  NOT NULL DEFAULT '',
    CONSTRAINT planets_pk PRIMARY KEY  (name)
);


INSERT INTO planets (name, solar_system, max_temp, min_temp, atmospheric_pressure, description, image_url) VALUES
('Vulcan', 'Vulcan System', 67.0, 18.0, 0.5, 'Arid desert world, home of the Vulcans, known for logic and stoicism.', '/images/planets/vulcan.jpg'),
('Qonos', 'Qonos System', 45.0, -10.0, 0.9, 'Harsh homeworld of the Klingon Empire, covered in dark continents and toxic seas.', '/images/planets/qonos.jpg'),
('Romulus', 'Romii System', 38.0, 5.0, 1.0, 'Lush twin world of the Romulan Star Empire, politically volatile and secretive.', '/images/planets/romulus.jpg'),
('Bajor', 'Bajoran System', 32.0, -5.0, 1.0, 'Spiritual homeworld of the Bajorans, recovering from Cardassian occupation.', '/images/planets/bajor.jpg'),
('Cardassia Prime', 'Cardassian System', 55.0, 10.0, 1.2, 'Resource-depleted militaristic homeworld of the Cardassian Union.', '/images/planets/cardassia-prime.jpg'),
('Betazed', 'Betazed System', 29.0, 8.0, 1.0, 'Peaceful world of telepathic humanoids, known for lush forests and open culture.', '/images/planets/betazed.jpg'),
('Ferenginar', 'Ferengi System', 20.0, 10.0, 1.4, 'Perpetually rain-soaked homeworld of the Ferengi, center of galactic commerce.', '/images/planets/ferenginar.jpg'),
('Risa', 'Risian System', 35.0, 22.0, 1.0, 'Pleasure planet with a perfectly controlled climate, a top tourist destination.', '/images/planets/risa.jpg'),
('Andoria', 'Andorian System', -15.0, -80.0, 0.9, 'Frozen moon of a gas giant, home of the blue-skinned, antennae-bearing Andorians.', '/images/planets/andoria.jpg'),
('Tellar Prime', 'Tellarite System', 25.0, -20.0, 1.1, 'Rugged homeworld of the argumentative Tellarites, founding members of the Federation.', '/images/planets/tellar-prime.jpg'),
('Trill', 'Trill System', 28.0, -3.0, 1.0, 'Serene world home to the joined Trill species and their symbiont pools.', '/images/planets/trill.jpg'),
('Denobula', 'Denobulan System', 40.0, 12.0, 1.3, 'Densely populated homeworld of the Denobulans, known for communal family structures.', '/images/planets/denobula.jpg'),
('Talos IV', 'Talosian System', 22.0, -8.0, 0.8, 'Forbidden world of the powerful Talosians, masters of illusion and mental projection.', '/images/planets/talos-iv.jpg'),
('Argelius II', 'Argelian System', 33.0, 18.0, 1.0, 'Hedonistic pleasure world known for its pacifist inhabitants and vibrant nightlife.', '/images/planets/argelius-ii.jpg'),
('Excalbia', 'Excalbian System', 500.0, 400.0, 0.0, 'Volcanic lava world inhabited by rock-like beings who study the concept of good and evil.', '/images/planets/excalbia.jpg'),
('Miri', 'Miri System', 26.0, 5.0, 1.0, 'Near-identical copy of Earth, devastated by a life-prolongation plague centuries ago.', '/images/planets/miri.jpg'),
('Genesis Planet', 'Mutara System', 45.0, 10.0, 0.7, 'Artificially created world born from the Genesis Device, unstable and short-lived.', '/images/planets/genesis-planet.jpg'),
('Neural', 'Neural System', 38.0, 12.0, 1.0, 'Primitive pre-industrial world caught in a proxy conflict between the Federation and Klingons.', '/images/planets/neural.jpg'),
('Magna Roma', 'Magna Roman System', 31.0, 8.0, 1.0, 'Parallel Earth where Roman civilization never fell, blending ancient culture with modern tech.', '/images/planets/magna-roma.jpg'),
('Melkoria', 'Melkorian System', 15.0, -40.0, 0.6, 'Remote icy frontier world used as a neutral meeting point between rival galactic powers.', '/images/planets/melkoria.jpg');
-- End of file.
