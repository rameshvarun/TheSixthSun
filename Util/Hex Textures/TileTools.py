import os, sys, string, json
import math
import itertools
from PIL import Image
from PIL import ImageDraw
from PIL import ImageChops


"""
This module creates a grid of all possible terrain tile combinations.
It the getCoordinates function will return three coordinates in
 the tile grid corresponding to points on the requested triangle.
The makeTemplateTile method saves an image that can be used as a reference
 when designing the tiles.

The first, second, and third terrains are drawn as follows:
    |  
 B  |  C
   / \
  / A \
 /     \

"""

# List of PNGs representing the different types of terrain.
# The shape of each should be consistant. See template for
# the appropriate shape of a hex/triangle/sliver
HEXSRC = [
	"red.png",		#1
	"blue.png",		#2
	"yellow.png",	#3
	"orange.png",	#4
	"green.png",	#5
	"purple.png"	#6
]

# The size of a source image
SRC_SIZE = 256
# The size of a tile in the tile grid
TILE_SIZE = 85


# Determines the pixel coordinates of specified tile on the tile grid
def computeCoordinates(x, y, z):

	a, b, c = 0, 0, 0

	# calculate a:
	j = 21
	for i in range(x):
		a = a + j
		j = j - (6 - i)

	# calculate b:
	for i in range(y):
		if i < x :
			continue
		b = b + (6 - i)

	# calculate c:
	c = z - y


	# The tile grid coordinates:
	imgSize = TILE_SIZE	
	tileX = ((a + b + c) % 7) * imgSize
	tileY = ((a + b + c) / 7) * imgSize

	# The points of a proportional triangle:
	points = []
	t = 0.0
	while (t < math.pi * 2.0):
		p = (math.sin(t) * 0.45 + 0.5, math.cos(t) * 0.45 + 0.5)
		points.append((p[0] * imgSize, p[1] * imgSize))
		t += math.pi * (2.0 / 3.0);

	# The points of the requested triangle:
	p1 = (tileX + points[0][0], tileY + points[0][1])
	p2 = (tileX + points[2][0], tileY + points[2][1])
	p3 = (tileX + points[1][0], tileY + points[1][1])


	#print "tile ", (x, y, z), " is at ", (tileX, tileY)
	return [p1, p2, p3]


# Uses the computeCoordinates function
# to return the corrected coordinates of a tile
def getCoordinates(x, y, z):
	#third index of each element specifies original order
	triples = [	[x, 'A', 0],
				[y, 'B', 1],
				[z, 'C', 2]	]

	sd = sorted(triples)

	coords = computeCoordinates(sd[0][0], sd[1][0], sd[2][0])

	sd[0][1] = coords[0]
	sd[1][1] = coords[1]
	sd[2][1] = coords[2]

	# sort by [2]
	resorted = sorted(sd, key=lambda s: s[2])
	coordinates = [resorted[0][1], resorted[1][1], resorted[2][1]]

	#print coordinates
	return coordinates


# Outputs one tile made of the terrains specified by a, b, and c
def makeTile(srcImgs, a, b, c):

	#Copies images. Otherwsise images are distorted as the program runs
	imgA = ImageChops.duplicate(srcImgs[a])
	imgB = ImageChops.duplicate(srcImgs[b])
	imgC = ImageChops.duplicate(srcImgs[c])

	#rotates counterclockwise
	imgB = imgB.rotate(240);
	imgC = imgC.rotate(120);

	#Accounts for overlaying transparency
	imgA.paste(imgB, None, imgB)
	imgA.paste(imgC, None, imgC)

	return imgA


# Makes a geometric template for designing the real tiles
def makeTemplateTile():
	
	#make a new transparent image
	img = Image.new("RGB", (SRC_SIZE, SRC_SIZE), "#ffc600")
	draw = ImageDraw.Draw(img)
	
	#parameters
	cx = SRC_SIZE / 2
	cy = SRC_SIZE / 2
	crad = SRC_SIZE

	#The points of the (1) triangle and (2) 
	points1 = []
	points2 = []

	#Get points for an equilateral triangle
	t = 0.0
	while (t < math.pi * 2.0):
		p1 = (math.sin(t) * 0.45 + 0.5, math.cos(t) * 0.45 + 0.5)
		p2 = (math.sin(t + math.pi/3) * 0.45 + 0.5, math.cos(t + math.pi / 3) * 0.45 + 0.5)

		points1.append((p1[0] * img.size[0], p1[1] * img.size[1]))
		points2.append((p2[0] * img.size[0], p2[1] * img.size[1]))

		t += math.pi * (2.0 / 3.0);

	#draw the triangle
	draw.polygon(points1, fill="#ffc600")

	#the "dual" outline
	draw.line((points1[0][0], points1[0][1], points1[1][0], points1[1][1]), fill="#eab600", width=5)
	draw.line((points1[1][0], points1[1][1], points1[2][0], points1[2][1]), fill="#eab600", width=5)
	draw.line((points1[0][0], points1[0][1], points1[2][0], points1[2][1]), fill="#eab600", width=5)

	#the "hex" border
	for p in points2:
		draw.line((cx, cy) + p, fill="#ffffff", width=8)

	#save the template
	img.save("template.png")


if __name__=='__main__':

	# Make a new, transparent image
	img = Image.new("RGB", (SRC_SIZE, SRC_SIZE), "#ffc600")

	# Draw guide lines on it
	cx = SRC_SIZE / 2
	cy = SRC_SIZE / 2
	crad = SRC_SIZE

	# Not necessary for the rest of the main method:
	#makeTemplateTile()

	# Load the source images
	srcImgs = []
	for s in HEXSRC:
		img = Image.open(s)
		srcImgs.append(img)

	# Generate all 56 combinations of 6 indexes
	combos = list(itertools.combinations_with_replacement([0, 1, 2, 3, 4, 5], 3))
	n = len(HEXSRC)
	nRows = int(math.ceil(math.sqrt(len(combos))))
	nCols = int(math.floor(math.sqrt(len(combos))))
	#print nRows, nCols
	index = 0
	imgTileSet = Image.new("RGB", (TILE_SIZE * nCols, TILE_SIZE * nRows), "#ffffff")
	
	# Create the Tile Grid
	for combo in combos:
		a = combo[0]
		b = combo[1]
		c = combo[2]

		# Make this tile
		imgTile = makeTile(srcImgs, a, b, c)

		# Add to the image
		tileX = index % 7
		tileY = index / 7
		
		imgTile = imgTile.resize((TILE_SIZE, TILE_SIZE), Image.ANTIALIAS)

		#imgTile.save("debug/tile%d_%d_%d.png" % (a, b, c))
		box = (tileX * TILE_SIZE, tileY * TILE_SIZE, (tileX + 1) * TILE_SIZE, (tileY + 1) * TILE_SIZE)
		imgTileSet.paste(imgTile, box, imgTile)

		#print (a, b, c), " index: ", index, (tileX, tileY),	"\tbox", box
		index += 1

	imgTileSet.save("tileset.png")
