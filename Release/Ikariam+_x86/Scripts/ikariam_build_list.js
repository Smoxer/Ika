var buildList = [];

function start(){
	$("#js_CityPosition" + buildList[0] + "Link").click();
	setTimeout(function() {
		if(!$("#js_buildingUpgradeButton").hasClass("disabled")) {
			$("#js_buildingUpgradeButton").click();
			buildList.shift();
		}
		setTimeout(function(){
			start();
		}, 10000);
	}, 3000);
}

function alertQueue(){
	var txt = "";
	for(var i = 0; i < buildList.length; i++)
		txt += $("#js_CityPosition" + buildList[i] + "Link").attr("title") + "\n\r";
	callbackObj.alert(txt, "Build list queue");
}

function display(){
	for(var i = 0; i < 19; i++) {
		if(!$("#position" + i).hasClass("buildingGround") && !$("#position" + i).hasClass("lockedPosition")){
			$("#js_CityPosition" + i + "Scroll").removeClass("invisible");
			$("#js_CityPosition" + i + "ScrollName").html($("#js_CityPosition" + i + "ScrollName").html().split(' | ')[0] + " | " + i.toString() + " | <a href=\"javascript: void(0);\" onclick=\"buildList.push('" + i.toString() + "'); alert('Added to queue');\">Add to queue</a>");//callbackObj.addToQueue('" + bgViewData.currentCityId + "', '" + i + "');
		}
	}
}
display();

setInterval("display()", 1000);