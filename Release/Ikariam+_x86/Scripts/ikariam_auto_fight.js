var cityIDScript = "";
var oldName = [];
function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
function getCityId(position){
	var cityID = getParameterByName("destinationCityId", $("#js_cityLocation" + position + "Link").attr('href'));
	if(cityID == null)
		cityID = getParameterByName("destinationCityId", $("#cityLocation" + position).attr('saved-href'));
	return cityID;
}

function writeScriptedData() {
	for(var i = 0; i < 17; i++)
		if(!$("#cityLocation" + i + "Scroll").hasClass("buildplace")){
			if(oldName[i] == null)
				oldName[i] = $("#js_cityLocation" + i + "TitleText").html();
			var getCityId2 = getCityId(i);
			$("#js_cityLocation" + i + "TitleText").html(oldName[i] + " | <a style=\"color: Blue;\" href=\"javascript: void(0);\" onclick=\"callbackObj.quickScript('" + getCityId2 + "');\">" + getCityId2 + "</a> | <a style=\"color: Blue;\" href=\"javascript: void(0);\" onclick=\"callbackObj.addCity('" + getCityId2 + "', '" + $("#js_cityLocation" + i + "LinkHover").attr("title") + "')\">Favorite</a>");
		}
}
