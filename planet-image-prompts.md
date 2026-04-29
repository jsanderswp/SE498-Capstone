# TrekTemp — Planet Background Image Prompts

Use these prompts to generate the 20 planet background images for the PlanetInfo page. The shared style guide at the top is what keeps them looking like they belong to the same app. Paste it before each individual planet prompt, or set it as a system/style reference if your generator supports that.

---

## Shared Style Guide (paste before every prompt)

Cinematic full-disc view of an alien planet seen from low orbit, centered in frame, planet fills roughly 70% of the height. Deep space backdrop with a dense, slightly dim starfield — stars are small and crisp, not glowing flares. Dramatic rim lighting from a single off-screen star at the upper left, leaving a soft terminator and a darker limb on the lower right. Photorealistic, NASA-photograph quality, subtle atmospheric glow on the lit edge, faint cloud or dust detail where appropriate. 16:9 aspect ratio, 1920x1080. No spacecraft, no text, no UI elements, no people, no logos, no constellations, no nebulae overpowering the planet. Color grading is rich but slightly desaturated, with deep blacks and a cool cinematic tone. The image will be used as a full-bleed website background, so leave the upper-left and lower-right quadrants relatively uncluttered for text legibility.

**Negative prompt (for SD/Midjourney `--no`):** text, watermark, signature, ui, hud, spacecraft, nebula, galaxy spirals, cartoon, illustration, painting, low resolution, jpeg artifacts, blurry, oversaturated.

**Midjourney suffix (optional):** `--ar 16:9 --style raw --v 6`

---

## Per-Planet Prompts

### 1. Vulcan → `vulcan.jpg`
A scorching arid desert world. Endless red-orange and rust-brown continents, deep canyon networks, vast dune seas, almost no surface water — just a few dry salt-flat scars. Thin, hazy copper-tinted atmosphere barely visible at the limb. A sense of stillness and ancient discipline.

### 2. Qonos → `qonos.jpg`
A harsh, brooding homeworld. Dark volcanic continents in near-black basalt and deep crimson, riven by glowing magma fissures. Toxic seas the color of green-yellow sulfur. A roiling, smoke-streaked atmosphere with bands of dirty cloud. Dangerous and warlike in feel.

### 3. Romulus → `romulus.jpg`
A lush, secretive world with rich emerald-green continents, sapphire oceans, and sweeping white cloud bands. Slightly more shadow than light — a planet that looks beautiful but watchful. Faint hints of a twin world (Remus) as a small dim companion in the far distance.

### 4. Bajor → `bajor.jpg`
A spiritual, recovering world. Warm amber and ochre continents, turquoise inland seas, soft wispy clouds. Sunlight catches it like a sunrise — golden and forgiving. A few visible mountain ranges and broad fertile valleys. Peaceful, hopeful tone.

### 5. Cardassia Prime → `cardassia-prime.jpg`
A resource-stripped militaristic world. Pale tan and dust-grey continents scarred by dark industrial blights and dried-out riverbeds. Thin, smoggy ochre atmosphere. Sparse, shrunken seas in muted grey-blue. Severe, depleted, uneasy.

### 6. Betazed → `betazed.jpg`
A peaceful, telepathic homeworld. Vibrant deep-green continents covered in unbroken forest canopy, broad clear blue oceans, soft pearlescent cloud swirls. The atmospheric limb glows a faint lilac-pink. Open, gentle, alive.

### 7. Ferenginar → `ferenginar.jpg`
A perpetually storm-soaked world. Almost no land visible — surface is hidden under endless dense grey-green stormbands and swirling cyclones. Brief breaks reveal slick brown continents and bronze-tinted oceans. Heavy, opulent, commercial.

### 8. Risa → `risa.jpg`
A perfectly climate-controlled pleasure world. Vivid turquoise oceans, scattered tropical archipelagos in jade and white-sand crescents, immaculate thin cloud lacework. Clean, bright, almost too perfect — a postcard planet.

### 9. Andoria → `andoria.jpg`
A frozen ice moon orbiting a vast gas giant. The planet itself is a brilliant pale blue-white sphere of cracked glacial sheets and shadowed crevasse networks. The enormous gas giant looms partially in frame at the upper right, its banded amber-and-cream cloud belts dwarfing the moon. Cold, sharp, beautiful.

### 10. Tellar Prime → `tellar-prime.jpg`
A rugged, weathered world. Mottled grey-brown continents with sharp tectonic ridges, deep canyon scars, and dark slate seas. Thick, heavy atmosphere with restless cloud cover. Stocky, blunt, tough character.

### 11. Trill → `trill.jpg`
A serene, balanced world. Soft sage-green continents, calm clear teal oceans, gentle white spiral cloud systems. A few large freshwater lake-pool networks visible — implying the symbiont caves below. Quietly contemplative.

### 12. Denobula → `denobula.jpg`
A densely populated homeworld. Continents in warm yellow-brown and terracotta, threaded with visible river deltas. Thin atmospheric haze in pale gold. Subtle suggestion of dense settlement — not city lights, but textured surface variation.

### 13. Talos IV → `talos-iv.jpg`
A forbidden, mysterious world. Muted slate-grey and deep purple continents, dim violet-tinted seas, low ambient light, an eerie soft glow that almost feels unnatural. Clouds form impossibly geometric patterns in places. Quiet, unsettling, watchful.

### 14. Argelius II → `argelius-ii.jpg`
A hedonistic pleasure world. Warm pink-gold continents, clear amber-tinged seas, lush coastal greenery, soft sunset-coloured cloud bands. The whole sphere feels lit by an eternal golden hour. Inviting and decadent.

### 15. Excalbia → `excalbia.jpg`
A volcanic lava world. Surface dominated by glowing magma rivers in incandescent orange-red, broken by jagged black basalt continents. Thick black smoke plumes drift across the limb. Distant rim lit a furnace orange. Hellish, primal, ancient.

### 16. Miri → `miri.jpg`
A near-identical copy of Earth. Familiar blue oceans, green-and-tan continents in vaguely Earth-like shapes, white cloud swirls — but slightly wrong. Continents are subtly distorted, color is just a touch desaturated, and there's an unmistakable sense of emptiness, as if no one is home.

### 17. Genesis Planet → `genesis-planet.jpg`
An artificially created, unstable world. Continents in violently saturated greens and oranges that don't quite blend, oceans in unreal turquoise. Visible energy seams and tectonic glow lines as if the surface is still forming. Slight chromatic shimmer at the limb. Beautiful and clearly wrong.

### 18. Neural → `neural.jpg`
A primitive pre-industrial Earth-like world. Broad green-and-gold continents, deep blue oceans, scattered cumulus cloud, no signs of cities or technology. Pristine and untouched, almost prehistoric.

### 19. Magna Roma → `magna-roma.jpg`
A parallel Earth. Continents arranged like Earth's, but slightly shifted, colored in olive-greens, terracotta, and warm Mediterranean blues. Faint orderly grid patterns suggest sprawling Roman-style civilization. Familiar yet alternate.

### 20. Melkoria → `melkoria.jpg`
A remote icy frontier world. Pale silver-blue ice sheets, dark cracked seams of frozen ocean, sparse rocky outcrops in cold grey. Thin pale atmosphere with a faint aurora ring at the pole. Lonely, remote, neutral.

---

## Workflow tips

- Generate one planet at a time and keep the style guide block identical across all runs — that's what enforces visual consistency.
- If your generator supports image references / style references, generate Vulcan first, then use it as a style reference for the other 19. That tightens the look further.
- Aim for roughly 1920x1080, save as `.jpg` quality 80, and target 200–400 KB per file.
- Save to `StarTrekWeather/wwwroot/images/planets/` using the slugified filenames listed under each planet above.
