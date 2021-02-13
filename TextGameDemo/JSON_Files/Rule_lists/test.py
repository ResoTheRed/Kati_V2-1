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
	

	
	#TODO: Friday - Sunday 12th-14th
	# Done # Get both the module system and the game to update character stats
	# Done # get short-term history and long-term history rules working 
	# Done # add negative, neutral, positive, lover, and rival button (auto effect player in that domain)
	# Done # make sure module tone branches are cycling correctly
	# Done # make sure module topics are cycling correctly
	#	make sure forced dialogue chains are working
	#	make sure response system is working
	#	Work all of the bugs out of the around town module
	#	copy around town module to fighting words,young love, questing, and lerin
	#	Add new module best_friends, competitors, and colleuges (just place holder text and around town rules for more variation)
	#	customize module rules
	
	#TODO: Monday - Thursday 15th - 19th
	# play test with all modules and fix bugs or rig it to by-pass bugs
	# add A TON of dialogue (good or bad) to each module and not add new rules
	# track different play throughs using all negative, positive, neutral, lover, disgust, and random
	# Open job interview game to 1583 students
	
	#TODO: 22th-25th
	# model data to fit into research
	# make graphs, charts, diagrams, and dialogue samples
	
	#TODO:
	# Write Paper