all: icon.ico

icon.ico: icon-16.png icon-24.png icon-32.png icon-48.png icon-57.png icon-72.png icon-96.png icon-114.png icon-128.png icon-144.png icon-256.png icon-512.png
	convert icon-16.png icon-24.png icon-32.png icon-48.png icon-57.png icon-72.png icon-96.png icon-114.png icon-128.png icon-144.png icon-256.png icon-512.png icon.ico

icon-16.png: icon-16.svg
	rsvg -w 16 -h 16 icon-16.svg icon-16.png

icon-24.png: icon-16.svg
	rsvg -w 24 -h 24 icon-16.svg icon-24.png

icon-32.png: icon-32.svg
	rsvg -w 32 -h 32 icon-32.svg icon-32.png

icon-48.png: icon-32.svg
	rsvg -w 48 -h 48 icon-32.svg icon-48.png

icon-57.png: icon-64.svg
	rsvg -w 57 -h 57 icon-64.svg icon-57.png

icon-64.png: icon-64.svg
	rsvg -w 64 -h 64 icon-64.svg icon-64.png

icon-72.png: icon-64.svg
	rsvg -w 72 -h 72 icon-64.svg icon-72.png

icon-96.png: icon-64.svg
	rsvg -w 96 -h 96 icon-64.svg icon-96.png

icon-114.png: icon-256.svg
	rsvg -w 114 -h 114 icon-256.svg icon-114.png

icon-128.png: icon-256.svg
	rsvg -w 128 -h 128 icon-256.svg icon-128.png

icon-144.png: icon-256.svg
	rsvg -w 144 -h 144 icon-256.svg icon-144.png

icon-256.png: icon-256.svg
	rsvg -w 256 -h 256 icon-256.svg icon-256.png

icon-512.png: icon-512.svg
	rsvg -w 512 -h 512 icon-512.svg icon-512.png

