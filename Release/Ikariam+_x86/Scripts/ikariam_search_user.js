/*clearInterval(init.periodicalDataRefresh);
callbackObj.createReport();
var usersSplit = UserName.split(",");
var Islands;
var maxIslands = 0;
var txt = "";
var IslandsSoFar = 0;
var matchesSoFar = 0;
var ms_start = Date.now();

$("body").html("<div style=\"background-color: black; color: White; direction: ltr; text-align: left;\"><div id=\"percentages\" style=\"direction: ltr;\">Please wait . . .</div><div style=\"direction: ltr;\"><a href=\"index.php?view=city\">Exit Script</a></div><br /><div id=\"places\" style=\"direction: ltr;\"></div><br /><div id=\"stats\" style=\"direction: ltr;\"></div></div>");

function writePlaces(places){
	var percentage = (IslandsSoFar*100)/maxIslands;
	var IslandDataFromAjax = JSON.parse(places);
	if(IslandDataFromAjax[0][1].backgroundData == "")
		return;
	var cities = IslandDataFromAjax[0][1].backgroundData.cities;
	for(var i = 0; i < cities.length; i++){
		for(var j = 0; j < usersSplit.length; j++){
			if(cities[i].id != -1 && cities[i].type == "city"){
				var wonder = "";
				if(IslandDataFromAjax[0][1].backgroundData.wonder == "1")
					wonder = "Hephaestus (" + IslandDataFromAjax[0][1].backgroundData.wonderLevel + ")";
				else if(IslandDataFromAjax[0][1].backgroundData.wonder == "2")
					wonder = "Hades (" + IslandDataFromAjax[0][1].backgroundData.wonderLevel + ")";
				else if(IslandDataFromAjax[0][1].backgroundData.wonder == "3")
					wonder = "Demeter (" + IslandDataFromAjax[0][1].backgroundData.wonderLevel + ")";
				else if(IslandDataFromAjax[0][1].backgroundData.wonder == "4")
					wonder = "Athena (" + IslandDataFromAjax[0][1].backgroundData.wonderLevel + ")";
				else if(IslandDataFromAjax[0][1].backgroundData.wonder == "5")
					wonder = "Hermes (" + IslandDataFromAjax[0][1].backgroundData.wonderLevel + ")";
				else if(IslandDataFromAjax[0][1].backgroundData.wonder == "6")
					wonder = "Ares (" + IslandDataFromAjax[0][1].backgroundData.wonderLevel + ")";
				else if(IslandDataFromAjax[0][1].backgroundData.wonder == "7")
					wonder = "Poseidon (" + IslandDataFromAjax[0][1].backgroundData.wonderLevel + ")";
				else if(IslandDataFromAjax[0][1].backgroundData.wonder == "8")
					wonder = "Colossus  (" + IslandDataFromAjax[0][1].backgroundData.wonderLevel + ")";
				
				var tradeGood = "";
				if(IslandDataFromAjax[0][1].backgroundData.tradegood == "1")
					tradeGood = "Vineyard (" + IslandDataFromAjax[0][1].backgroundData.tradegoodLevel + ")";
				else if(IslandDataFromAjax[0][1].backgroundData.tradegood == "2")
					tradeGood = "Quarry (" + IslandDataFromAjax[0][1].backgroundData.tradegoodLevel + ")";
				else if(IslandDataFromAjax[0][1].backgroundData.tradegood == "3")
					tradeGood = "Crystal mine (" + IslandDataFromAjax[0][1].backgroundData.tradegoodLevel + ")";
				else if(IslandDataFromAjax[0][1].backgroundData.tradegood == "4")
					tradeGood = "Sulphur pit (" + IslandDataFromAjax[0][1].backgroundData.tradegoodLevel + ")";
			
				var ally = "None";
				if(cities[i].ownerAllyTag != "")
					ally = cities[i].ownerAllyTag;
				
				if(opt == "UserName"){
					if(cities[i].ownerName.toLowerCase() == usersSplit[j].toLowerCase()){
						txt += (maxIslands - matchesSoFar + 1).toString() + ") " + cities[i].ownerName + " (" + cities[i].ownerAllyTag + ") From the city \"" + cities[i].name + "\" found in: " + IslandDataFromAjax[0][1].backgroundData.xCoord + ":" + IslandDataFromAjax[0][1].backgroundData.yCoord;
						if(cities[i].state == "vacation")
							txt += " - in vacation!";
						txt += "<br />";
						maxIslands++;
						searchSpy(cities[i].id, i, cities, IslandDataFromAjax, tradeGood, ally, wonder, function(spyResult, i, cities, IslandDataFromAjax, tradeGood, ally, wonder) {
							IslandsSoFar++;
							var spyResultFromAjax = JSON.parse(spyResult);
							var totalBuildings = "";
							if(spyResultFromAjax[0][1].backgroundData != ""){
								var buildings = spyResultFromAjax[0][1].backgroundData.position;
								if(spyResultFromAjax[0][1].backgroundData.id == cities[i].id) {
									for(var b = 0; b < buildings.length; b++)
										totalBuildings += buildings[b].building + ":" + buildings[b].level + ",";
									if(buildings.length > 0)
										totalBuildings = totalBuildings.slice(0,-1);
								}
							}
							var row = [IslandDataFromAjax[0][1].backgroundData.xCoord, IslandDataFromAjax[0][1].backgroundData.yCoord, IslandDataFromAjax[0][1].backgroundData.name, cities[i].name, cities[i].ownerName, (cities[i].actions.toString() != "" && cities[i].actions.hasOwnProperty("piracy_raid")).toString(), ally, (cities[i].state == "vacation").toString(), wonder, tradeGood, "Saw Mill (" + IslandDataFromAjax[0][1].backgroundData.resourceLevel + ")", (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("occupation")).toString(), (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("fleetAction")).toString(), cities[i].id, IslandDataFromAjax[0][1].backgroundData.id, totalBuildings];
							callbackObj.addToReport(row.join() + "\n");
							var percentage = (IslandsSoFar*100)/maxIslands;
							$("#percentages").html(percentage + "%");
							$("#stats").html(IslandsSoFar + "/" + maxIslands + ", " + (Date.now() - ms_start) + "ms");
						});
					}
					if(cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("occupation") && cities[i].infos.occupation.name.toLowerCase().search(usersSplit[j].toLowerCase()) != -1){
						txt += (maxIslands - matchesSoFar + 1).toString() + ") " + cities[i].infos.occupation.name + " occupied city " + " \"" + cities[i].name + "\" in: " + IslandDataFromAjax[0][1].backgroundData.xCoord + ":" + IslandDataFromAjax[0][1].backgroundData.yCoord;
						if(cities[i].state == "vacation")
							txt += " - in vacation!";
						txt += "<br />";
						maxIslands++;
						searchSpy(cities[i].id, i, cities, IslandDataFromAjax, tradeGood, ally, wonder, function(spyResult, i, cities, IslandDataFromAjax, tradeGood, ally, wonder) {
							IslandsSoFar++;
							var spyResultFromAjax = JSON.parse(spyResult);
							var totalBuildings = "";
							if(spyResultFromAjax[0][1].backgroundData != ""){
								var buildings = spyResultFromAjax[0][1].backgroundData.position;
								if(spyResultFromAjax[0][1].backgroundData.id == cities[i].id) {
									for(var b = 0; b < buildings.length; b++)
										totalBuildings += buildings[b].building + ":" + buildings[b].level + ",";
									if(buildings.length > 0)
										totalBuildings = totalBuildings.slice(0,-1);
								}
							}
							var row = [IslandDataFromAjax[0][1].backgroundData.xCoord, IslandDataFromAjax[0][1].backgroundData.yCoord, IslandDataFromAjax[0][1].backgroundData.name, cities[i].name, cities[i].ownerName, (cities[i].actions.toString() != "" && cities[i].actions.hasOwnProperty("piracy_raid")).toString(), ally, (cities[i].state == "vacation").toString(), wonder, tradeGood, "Saw Mill (" + IslandDataFromAjax[0][1].backgroundData.resourceLevel + ")", (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("occupation")).toString(), (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("fleetAction")).toString(), cities[i].id, IslandDataFromAjax[0][1].backgroundData.id, totalBuildings];
							callbackObj.addToReport(row.join() + "\n");
							var percentage = (IslandsSoFar*100)/maxIslands;
							$("#percentages").html(percentage + "%");
							$("#stats").html(IslandsSoFar + "/" + maxIslands + ", " + (Date.now() - ms_start) + "ms");
						});
					}
					if(cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("fleetAction") && cities[i].infos.fleetAction.name.toLowerCase().search(usersSplit[j].toLowerCase()) != -1){
						txt += (maxIslands - matchesSoFar + 1).toString() + ") " + cities[i].infos.fleetAction.name + " occupied port " + " \"" + cities[i].name + "\" in: " + IslandDataFromAjax[0][1].backgroundData.xCoord + ":" + IslandDataFromAjax[0][1].backgroundData.yCoord;
						if(cities[i].state == "vacation")
							txt += " - in vacation!";
						txt += "<br />";
						maxIslands++;
						searchSpy(cities[i].id, i, cities, IslandDataFromAjax, tradeGood, ally, wonder, function(spyResult, i, cities, IslandDataFromAjax, tradeGood, ally, wonder) {
							IslandsSoFar++;
							var spyResultFromAjax = JSON.parse(spyResult);
							var totalBuildings = "";
							if(spyResultFromAjax[0][1].backgroundData != ""){
								var buildings = spyResultFromAjax[0][1].backgroundData.position;
								if(spyResultFromAjax[0][1].backgroundData.id == cities[i].id) {
									for(var b = 0; b < buildings.length; b++)
										totalBuildings += buildings[b].building + ":" + buildings[b].level + ",";
									if(buildings.length > 0)
										totalBuildings = totalBuildings.slice(0,-1);
								}
							}
							var row = [IslandDataFromAjax[0][1].backgroundData.xCoord, IslandDataFromAjax[0][1].backgroundData.yCoord, IslandDataFromAjax[0][1].backgroundData.name, cities[i].name, cities[i].ownerName, (cities[i].actions.toString() != "" && cities[i].actions.hasOwnProperty("piracy_raid")).toString(), ally, (cities[i].state == "vacation").toString(), wonder, tradeGood, "Saw Mill (" + IslandDataFromAjax[0][1].backgroundData.resourceLevel + ")", (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("occupation")).toString(), (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("fleetAction")).toString(), cities[i].id, IslandDataFromAjax[0][1].backgroundData.id, totalBuildings];
							callbackObj.addToReport(row.join() + "\n");
							var percentage = (IslandsSoFar*100)/maxIslands;
							$("#percentages").html(percentage + "%");
							$("#stats").html(IslandsSoFar + "/" + maxIslands + ", " + (Date.now() - ms_start) + "ms");
						});
					}
				} else if(opt == "City") {
					if(cities[i].name.toLowerCase().search(usersSplit[j].toLowerCase()) != -1){
						txt += (maxIslands - matchesSoFar + 1).toString() + ") " + cities[i].ownerName + " (" + cities[i].ownerAllyTag + ") From the city \"" + cities[i].name + "\" found in: " + IslandDataFromAjax[0][1].backgroundData.xCoord + ":" + IslandDataFromAjax[0][1].backgroundData.yCoord;
						if(cities[i].state == "vacation")
							txt += " - in vacation!";
						txt += "<br />";
						maxIslands++;
						searchSpy(cities[i].id, i, cities, IslandDataFromAjax, tradeGood, ally, wonder, function(spyResult, i, cities, IslandDataFromAjax, tradeGood, ally, wonder) {
							IslandsSoFar++;
							var spyResultFromAjax = JSON.parse(spyResult);
							var totalBuildings = "";
							if(spyResultFromAjax[0][1].backgroundData != ""){
								var buildings = spyResultFromAjax[0][1].backgroundData.position;
								if(spyResultFromAjax[0][1].backgroundData.id == cities[i].id) {
									for(var b = 0; b < buildings.length; b++)
										totalBuildings += buildings[b].building + ":" + buildings[b].level + ",";
									if(buildings.length > 0)
										totalBuildings = totalBuildings.slice(0,-1);
								}
							}
							var row = [IslandDataFromAjax[0][1].backgroundData.xCoord, IslandDataFromAjax[0][1].backgroundData.yCoord, IslandDataFromAjax[0][1].backgroundData.name, cities[i].name, cities[i].ownerName, (cities[i].actions.toString() != "" && cities[i].actions.hasOwnProperty("piracy_raid")).toString(), ally, (cities[i].state == "vacation").toString(), wonder, tradeGood, "Saw Mill (" + IslandDataFromAjax[0][1].backgroundData.resourceLevel + ")", (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("occupation")).toString(), (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("fleetAction")).toString(), cities[i].id, IslandDataFromAjax[0][1].backgroundData.id, totalBuildings];
							callbackObj.addToReport(row.join() + "\n");
							var percentage = (IslandsSoFar*100)/maxIslands;
							$("#percentages").html(percentage + "%");
							$("#stats").html(IslandsSoFar + "/" + maxIslands + ", " + (Date.now() - ms_start) + "ms");
						});
					}
					if(cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("occupation") && cities[i].infos.occupation.name.toLowerCase().search(usersSplit[j].toLowerCase()) != -1){
						txt += (maxIslands - matchesSoFar + 1).toString() + ") " + cities[i].infos.occupation.name + " occupied city " + " \"" + cities[i].name + "\" in: " + IslandDataFromAjax[0][1].backgroundData.xCoord + ":" + IslandDataFromAjax[0][1].backgroundData.yCoord;
						if(cities[i].state == "vacation")
							txt += " - in vacation!";
						txt += "<br />";
						maxIslands++;
						searchSpy(cities[i].id, i, cities, IslandDataFromAjax, tradeGood, ally, wonder, function(spyResult, i, cities, IslandDataFromAjax, tradeGood, ally, wonder) {
							IslandsSoFar++;
							var spyResultFromAjax = JSON.parse(spyResult);
							var totalBuildings = "";
							if(spyResultFromAjax[0][1].backgroundData != ""){
								var buildings = spyResultFromAjax[0][1].backgroundData.position;
								if(spyResultFromAjax[0][1].backgroundData.id == cities[i].id) {
									for(var b = 0; b < buildings.length; b++)
										totalBuildings += buildings[b].building + ":" + buildings[b].level + ",";
									if(buildings.length > 0)
										totalBuildings = totalBuildings.slice(0,-1);
								}
							}
							var row = [IslandDataFromAjax[0][1].backgroundData.xCoord, IslandDataFromAjax[0][1].backgroundData.yCoord, IslandDataFromAjax[0][1].backgroundData.name, cities[i].name, cities[i].ownerName, (cities[i].actions.toString() != "" && cities[i].actions.hasOwnProperty("piracy_raid")).toString(), ally, (cities[i].state == "vacation").toString(), wonder, tradeGood, "Saw Mill (" + IslandDataFromAjax[0][1].backgroundData.resourceLevel + ")", (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("occupation")).toString(), (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("fleetAction")).toString(), cities[i].id, IslandDataFromAjax[0][1].backgroundData.id, totalBuildings];
							callbackObj.addToReport(row.join() + "\n");
							var percentage = (IslandsSoFar*100)/maxIslands;
							$("#percentages").html(percentage + "%");
							$("#stats").html(IslandsSoFar + "/" + maxIslands + ", " + (Date.now() - ms_start) + "ms");
						});
					}
					if(cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("fleetAction") && cities[i].infos.fleetAction.name.toLowerCase().search(usersSplit[j].toLowerCase()) != -1){
						txt += (maxIslands - matchesSoFar + 1).toString() + ") " + cities[i].infos.fleetAction.name + " occupied port " + " \"" + cities[i].name + "\" in: " + IslandDataFromAjax[0][1].backgroundData.xCoord + ":" + IslandDataFromAjax[0][1].backgroundData.yCoord;
						if(cities[i].state == "vacation")
							txt += " - in vacation!";
						txt += "<br />";
						maxIslands++;
						searchSpy(cities[i].id, i, cities, IslandDataFromAjax, tradeGood, ally, wonder, function(spyResult, i, cities, IslandDataFromAjax, tradeGood, ally, wonder) {
							IslandsSoFar++;
							var spyResultFromAjax = JSON.parse(spyResult);
							var totalBuildings = "";
							if(spyResultFromAjax[0][1].backgroundData != ""){
								var buildings = spyResultFromAjax[0][1].backgroundData.position;
								if(spyResultFromAjax[0][1].backgroundData.id == cities[i].id) {
									for(var b = 0; b < buildings.length; b++)
										totalBuildings += buildings[b].building + ":" + buildings[b].level + ",";
									if(buildings.length > 0)
										totalBuildings = totalBuildings.slice(0,-1);
								}
							}
							var row = [IslandDataFromAjax[0][1].backgroundData.xCoord, IslandDataFromAjax[0][1].backgroundData.yCoord, IslandDataFromAjax[0][1].backgroundData.name, cities[i].name, cities[i].ownerName, (cities[i].actions.toString() != "" && cities[i].actions.hasOwnProperty("piracy_raid")).toString(), ally, (cities[i].state == "vacation").toString(), wonder, tradeGood, "Saw Mill (" + IslandDataFromAjax[0][1].backgroundData.resourceLevel + ")", (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("occupation")).toString(), (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("fleetAction")).toString(), cities[i].id, IslandDataFromAjax[0][1].backgroundData.id, totalBuildings];
							callbackObj.addToReport(row.join() + "\n");
							var percentage = (IslandsSoFar*100)/maxIslands;
							$("#percentages").html(percentage + "%");
							$("#stats").html(IslandsSoFar + "/" + maxIslands + ", " + (Date.now() - ms_start) + "ms");
						});
					}
				} else if(opt = "Ally"){
					if(cities[i].ownerAllyId != "0" && cities[i].ownerAllyTag.toLowerCase() == usersSplit[j].toLowerCase()){

						searchSpy(cities[i].id, i, cities, IslandDataFromAjax, tradeGood, ally, wonder, function(spyResult, i, cities, IslandDataFromAjax, tradeGood, ally, wonder) {
							IslandsSoFar++;
							var spyResultFromAjax = JSON.parse(spyResult);
							var totalBuildings = "";
							if(spyResultFromAjax[0][1].backgroundData != ""){
								var buildings = spyResultFromAjax[0][1].backgroundData.position;
								if(spyResultFromAjax[0][1].backgroundData.id == cities[i].id) {
									for(var b = 0; b < buildings.length; b++)
										totalBuildings += buildings[b].building + ":" + buildings[b].level + ",";
									if(buildings.length > 0)
										totalBuildings = totalBuildings.slice(0,-1);
								}
							}
							var row = [IslandDataFromAjax[0][1].backgroundData.xCoord, IslandDataFromAjax[0][1].backgroundData.yCoord, IslandDataFromAjax[0][1].backgroundData.name, cities[i].name, cities[i].ownerName, (cities[i].actions.toString() != "" && cities[i].actions.hasOwnProperty("piracy_raid")).toString(), ally, (cities[i].state == "vacation").toString(), wonder, tradeGood, "Saw Mill (" + IslandDataFromAjax[0][1].backgroundData.resourceLevel + ")", (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("occupation")).toString(), (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("fleetAction")).toString(), cities[i].id, IslandDataFromAjax[0][1].backgroundData.id, totalBuildings];
							callbackObj.addToReport(row.join() + "\n");
							var percentage = (IslandsSoFar*100)/maxIslands;
							$("#percentages").html(percentage + "%");
							$("#stats").html(IslandsSoFar + "/" + maxIslands + ", " + (Date.now() - ms_start) + "ms");
						});
					}
					if(cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("occupation") && cities[i].infos.occupation.name.toLowerCase().search(usersSplit[j].toLowerCase()) != -1){

						searchSpy(cities[i].id, i, cities, IslandDataFromAjax, tradeGood, ally, wonder, function(spyResult, i, cities, IslandDataFromAjax, tradeGood, ally, wonder) {
							IslandsSoFar++;
							var spyResultFromAjax = JSON.parse(spyResult);
							var totalBuildings = "";
							if(spyResultFromAjax[0][1].backgroundData != ""){
								var buildings = spyResultFromAjax[0][1].backgroundData.position;
								if(spyResultFromAjax[0][1].backgroundData.id == cities[i].id) {
									for(var b = 0; b < buildings.length; b++)
										totalBuildings += buildings[b].building + ":" + buildings[b].level + ",";
									if(buildings.length > 0)
										totalBuildings = totalBuildings.slice(0,-1);
								}
							}
							var row = [IslandDataFromAjax[0][1].backgroundData.xCoord, IslandDataFromAjax[0][1].backgroundData.yCoord, IslandDataFromAjax[0][1].backgroundData.name, cities[i].name, cities[i].ownerName, (cities[i].actions.toString() != "" && cities[i].actions.hasOwnProperty("piracy_raid")).toString(), ally, (cities[i].state == "vacation").toString(), wonder, tradeGood, "Saw Mill (" + IslandDataFromAjax[0][1].backgroundData.resourceLevel + ")", (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("occupation")).toString(), (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("fleetAction")).toString(), cities[i].id, IslandDataFromAjax[0][1].backgroundData.id, totalBuildings];
							callbackObj.addToReport(row.join() + "\n");
							var percentage = (IslandsSoFar*100)/maxIslands;
							$("#percentages").html(percentage + "%");
							$("#stats").html(IslandsSoFar + "/" + maxIslands + ", " + (Date.now() - ms_start) + "ms");
						});
					}
					if(cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("fleetAction") && cities[i].infos.fleetAction.name.toLowerCase().search(usersSplit[j].toLowerCase()) != -1){
						txt += (maxIslands - matchesSoFar + 1).toString() + ") " + cities[i].infos.fleetAction.name + " occupied port " + " \"" + cities[i].name + "\" in: " + IslandDataFromAjax[0][1].backgroundData.xCoord + ":" + IslandDataFromAjax[0][1].backgroundData.yCoord;
						if(cities[i].state == "vacation")
							txt += " - in vacation!";
						txt += "<br />";
						maxIslands++;
						searchSpy(cities[i].id, i, cities, IslandDataFromAjax, tradeGood, ally, wonder, function(spyResult, i, cities, IslandDataFromAjax, tradeGood, ally, wonder) {
							IslandsSoFar++;
							var spyResultFromAjax = JSON.parse(spyResult);
							var totalBuildings = "";
							if(spyResultFromAjax[0][1].backgroundData != ""){
								var buildings = spyResultFromAjax[0][1].backgroundData.position;
								if(spyResultFromAjax[0][1].backgroundData.id == cities[i].id) {
									for(var b = 0; b < buildings.length; b++)
										totalBuildings += buildings[b].building + ":" + buildings[b].level + ",";
									if(buildings.length > 0)
										totalBuildings = totalBuildings.slice(0,-1);
								}
							}
							var row = [IslandDataFromAjax[0][1].backgroundData.xCoord, IslandDataFromAjax[0][1].backgroundData.yCoord, IslandDataFromAjax[0][1].backgroundData.name, cities[i].name, cities[i].ownerName, (cities[i].actions.toString() != "" && cities[i].actions.hasOwnProperty("piracy_raid")).toString(), ally, (cities[i].state == "vacation").toString(), wonder, tradeGood, "Saw Mill (" + IslandDataFromAjax[0][1].backgroundData.resourceLevel + ")", (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("occupation")).toString(), (cities[i].hasOwnProperty("infos") && cities[i].infos.hasOwnProperty("fleetAction")).toString(), cities[i].id, IslandDataFromAjax[0][1].backgroundData.id, totalBuildings];
							callbackObj.addToReport(row.join() + "\n");
							var percentage = (IslandsSoFar*100)/maxIslands;
							$("#percentages").html(percentage + "%");
							$("#stats").html(IslandsSoFar + "/" + maxIslands + ", " + (Date.now() - ms_start) + "ms");
						});
					}
				}
			}
		}
	}
	$("#percentages").html(percentage + "%");
	$("#places").html(txt);
	$("#stats").html(IslandsSoFar + "/" + maxIslands + ", " + (Date.now() - ms_start) + "ms");
}

function getAjaxIslands(){
	if(!callbackObj.isIslandExist('0')){
		$.ajax({
			url : 'index.php',
			async: true,
			cache: false,
			type : 'get',
			data :  {action: "WorldMap", 'function': "getJSONArea", x_min: -100, x_max: 100, y_min: -100, y_max: 100},
			dataType : 'text',
			success: function(res, textStatus, request){
				callbackObj.saveIsland('0', res);
				test1(res);
			}
		});
	} else {
		var dataFromFile = callbackObj.readIsland('0');
		if(dataFromFile != "None"){
			test1(dataFromFile);
		} else {
			$.ajax({
				url : 'index.php',
				async: true,
				cache: false,
				type : 'get',
				data :  {action: "WorldMap", 'function': "getJSONArea", x_min: -100, x_max: 100, y_min: -100, y_max: 100},
				dataType : 'text',
				success: function(res, textStatus, request){
					callbackObj.saveIsland('0', res);
					test1(res);
				}
			});
		}
	}
}

function searchSpy(cityID, i, cities, IslandDataFromAjax, tradeGood, ally, wonder, callback){
	$.ajax({
		url : 'index.php',
		async: true,
		cache: false,
		type : 'get',
		data :  {view: "noViewChange", 'action': "Options", 'function': "toggleCityBuildingNameOption", cityId: cityID, currentCityId: cityID, oldBackgroundView: "city", backgroundView: "city", ajax: 1},
		dataType : 'text',
		success: function(data, textStatus, request){
			callback(data, i, cities, IslandDataFromAjax, tradeGood, ally, wonder);
		}, error: function(xhr, textStatus, errorThrown){
			callback('[["updateGlobalData", {"backgroundData": ""}]]', i, cities, IslandDataFromAjax, tradeGood, ally, wonder);
		}
	});
}

function mockAjaxCall(islandId) {
	var deferred = jQuery.Deferred();
	if(!callbackObj.isIslandExist(islandId)){
		$.ajax({
			url : 'index.php',
			async: true,
			cache: false,
			type : 'get',
			data :  {view: "updateGlobalData", islandId: islandId, backgroundView: "island", currentIslandId: islandId, ajax: 1},
			dataType : 'text',
			success: function(data, textStatus, request){
				callbackObj.saveIsland(islandId, data);
				IslandsSoFar++;
				deferred.resolve(data);
				writePlaces(data);
			}
		});
	} else {
		
		var dataFromFile = callbackObj.readIsland(islandId);
		if(dataFromFile != "None"){
			IslandsSoFar++;
			deferred.resolve(dataFromFile);
		} else {
			$.ajax({
				url : 'index.php',
				async: true,
				cache: false,
				type : 'get',
				data :  {view: "updateGlobalData", islandId: islandId, backgroundView: "island", currentIslandId: islandId, ajax: 1},
				dataType : 'text',
				success: function(data, textStatus, request){
					callbackObj.saveIsland(islandId, data);
					IslandsSoFar++;
					deferred.resolve(data);
					writePlaces(data);
				}
			});
		}
	}

	return deferred;
}

function collectAllThatData() {
	var allCallsComplete = jQuery.Deferred();
	var callsPending = [];
	var callsComplete = [];
	var ms_interval = 250;

	var sum = -1;
	for(var x in Islands.data){
		for(var y in Islands.data[x]){
			
			callbackObj.log(Islands.data[x][y][0]);
			var promise = mockAjaxCall(Islands.data[x][y][0]);
			callsPending.push(promise);
			sum++;
			if(5 == sum)
				return;
		}
	}

	// As promises are resolved, add to callsComplete array.
	jQuery.each(callsPending, function(n, callPending) {
		jQuery.when(callPending).done(function(x) {
			callsComplete.push();
			
		});
	});

	// At defined interval, compare callsComplete to callsPending to determine
	// if all our calls are complete.
	var intervalId = setInterval(function() {
		console.log(callsComplete.length, 'calls complete');
		if ( callsComplete.length >= callsPending.length ) {
			clearInterval(intervalId);
			allCallsComplete.resolve(callsComplete);
		}
	}, ms_interval);

	// Return a deferred object for calling script to use.
	return allCallsComplete;
}

setTimeout(function(){ 
	getAjaxIslands();
}, 1000);

function test1(res){
	Islands = JSON.parse(res);
	for(var x in Islands.data){
		for(var y in Islands.data[x]){
			maxIslands++;
		}
	}
	matchesSoFar = maxIslands;
	var allThatDataCollected = collectAllThatData();
	jQuery.when(allThatDataCollected).then(function() {
		console.log('Data has been collected!');
		//WritePlaces(t);
	});
}*/
alert("Please update to a newer version of Ikariam+ in order to use this option");
