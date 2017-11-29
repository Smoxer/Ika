islands = islands.split(",");
islandsSearch = [];

function checkIsland(islandIndex){
	$.ajax({
		url : 'index.php',
		async: true,
		cache: false,
		type : 'get',
		data :  {view: "updateGlobalData", islandId: islandsSearch[islandIndex], backgroundView: "island", currentIslandId: islandsSearch[islandIndex], ajax: 1},
		dataType : 'text',
		success: function(data, textStatus, request){
			if(islandsSearch.length > islandIndex){
				var IslandDataFromAjax = JSON.parse(data);
				if(IslandDataFromAjax[0][1].backgroundData == "")
					checkIsland(islandIndex+1);
				var cities = IslandDataFromAjax[0][1].backgroundData.cities;
				var go = true;
				for(var i = 0; i < cities.length; i++){
					if(cities[i].id == -1 && cities[i].type == "buildplace"){
						ajaxHandlerCall('?view=colonize&islandId=' + islandsSearch[islandIndex] + '&position=' + i)
						go = false;
						setTimeout(function() { $("#transport").submit(); callbackObj.alert("A colony is going to create!"); }, 5000);
						break;
					}
				}
				if(go)
					checkIsland(islandIndex+1);
			}else
				setTimeout(function() { checkIsland(0); }, 5000);
		}
	});
}

function checkIslands(){
	$("body").append("<div style=\"width: 100%; height: 100%; position: fixed; background: rgba(0,0,0,0.5); z-index: 999999; text-align: center; color: #FFFFFF\" id=\"canceluserInput\">A script is running<br /><a href=\"index.php?view=city\">Abort script</a></div>");
	$.ajax({
		url : 'index.php',
		async: true,
		cache: false,
		type : 'get',
		data :  {action: "WorldMap", 'function': "getJSONArea", x_min: -100, x_max: 100, y_min: -100, y_max: 100},
		dataType : 'text',
		success: function(data, textStatus, request){
			AllIslands = JSON.parse(data);
			for(var x in AllIslands.data){
				for(var y in AllIslands.data[x]){
					if(jQuery.inArray(x + ":" + y, islands) != -1)
						islandsSearch.push(AllIslands.data[x][y][0]);
				}
			}
			checkIsland(0);
		}
	});
}

checkIslands();