import json

f = open("../AroundTown.json","r")

data = json.load(f)
dict = {}

for key in data:
	for tone in data[key]:
		for dialogue in data[key][tone]:
			for req in data[key][tone][dialogue]["req"]:
				dict[req] = key

for k,v in dict.items():
	print(str(k)+" : "+str(v))
	
