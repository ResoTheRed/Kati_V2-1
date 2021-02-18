import json

f = open("../AroundTown.json","r")

data = json.load(f)
dict = {}

for key in data:
	for tone in data[key]:
		for dialogue in data[key][tone]:
			for req in data[key][tone][dialogue]["lead to"]:
				dict[req] = key

for k,v in dict.items():
	print(str(k)+" : "+str(v))
	

	
	#TODO: Feb 12th-19th  
	# Done # Get both the module system and the game to update character stats 
	# Done # get short-term history and long-term history rules working 
	# Done # add negative, neutral, positive, lover, and rival override (auto effect player in that domain) 
	# Done # make sure module tone branches are cycling correctly 
	# Done # make sure module topics are cycling correctly 
	# Done # make sure forced dialogue chains are working 
	# Done # make sure response system is working 
	# Done # handle lead to problem 
	# Done # Handle response req problem
	# Done # Work the bugs out of the around town module 
	#	copy around town module setup to fighting words,young love, questing, and others 
	# 	play test with all modules and fix bugs or rig it to by-pass bugs 
	# 	add A TON of dialogue (good or bad) to each module 
	
	
	#TODO: 22th-25th
	# 	track different play throughs using all negative, positive, neutral, lover, disgust, and random 
	# 	Open job interview game to 1583 students 
	# 	model data to fit into research
	# 	make graphs, charts, diagrams, and dialogue samples
	
	#TODO: March 1st-15th
	# 	Write Paper
	# 	Submit paper by march 15th