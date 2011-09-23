f = open("lungs.colpoints")
lines = f.readlines()
f.close()
w = 2027
f = open("new.txt", 'w')
lines = lines[5:46]
for line in reversed(lines):
	x, y = map(int, line.rstrip('\n').split(','))
	f.write(str(w - x) + ',' + str(y) + '\n')
f.close()