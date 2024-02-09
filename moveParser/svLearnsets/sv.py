import json
import re

output = {}
mons = []

def moveToJson(move):
    return "MOVE_" + re.sub('[- ]', '_', move.upper())

with open("sv.txt", "r+") as file:
    mons = file.read().split("\n\n")

for mon in mons:
    lines = mon.split("\n")
    monName = lines[1]
    monName = re.sub('\d* - ', '', monName)
    monName = re.sub('[ -]', '_', monName)
    output[monName.upper()] = {
        "LevelMoves": [],
        "PreEvoMoves": [],
        "TMMoves": [],
        "EggMoves": [],
        "TutorMoves": []
        # "EggMoves": []
    }
    for line in lines:
        if line != "" and line[0] == "-":
            if line[2] == "[":
                if line[5] == "]":
                    level = int(line[3:5])
                    moveName = moveToJson(line[7:])
                    levelMove = {
                        "Level": level,
                        "Move": moveName
                    }
                    output[monName.upper()]["LevelMoves"].append(levelMove)
                elif line[8] == "]":
                    output[monName.upper()]["TMMoves"].append(moveToJson(line[10:]))
            else:
                if "Egg Moves:" in lines and "Reminder:" in lines:
                    if lines.index(line) < lines.index("Reminder:"):
                        output[monName.upper()]["EggMoves"].append(moveToJson(line[2:]))
                    else:
                        output[monName.upper()]["PreEvoMoves"].append(moveToJson(line[2:]))
                elif "Egg Moves:" in lines and "Reminder:" not in lines:
                    output[monName.upper()]["EggMoves"].append(moveToJson(line[2:]))
                elif "Egg Moves:" not in lines and "Reminder:" in lines:
                    output[monName.upper()]["PreEvoMoves"].append(moveToJson(line[2:]))

with open("../db/gen/sv.json", "w", newline="\n") as file:
    file.write(json.dumps(output, indent=2))
