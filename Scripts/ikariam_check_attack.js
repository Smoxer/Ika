function checkAttackAlly(){
	if($("#embassyGeneralAttacksToAlly .content table .alt").length){
		callbackObj.hardAlert("There is an attack on one of your ally friends!");
		clearInterval(checkI);
	}
	ajaxHandlerCall('?view=embassyGeneralAttacksToAlly&backgroundView=city&templateView=embassyGeneralAttacksToAlly&currentTab=tabEmbassy');
}

function checkAttackMe(){
	if($("#js_GlobalMenu_military").hasClass("normalalert")){
		callbackObj.scriptedAttacked(bgViewData.currentCityId);
		callbackObj.hardAlert("There is an attack on you!");
		clearInterval(checkI);
	}
}
$("body").append("<div style=\"width: 100%; height: 100%; position: fixed; background: rgba(0,0,0,0.5); z-index: 999999; text-align: center; color: #FFFFFF\" id=\"canceluserInput\">A script is running<br /><a href=\"index.php?view=city\">Abort script</a></div>");