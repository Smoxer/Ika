var spiesCity = Citiesht.split(",");

function startSendSpies(cityToSearch){
	if(cityToSearch >= spiesCity.length) {
		$("#canceluserInput").remove();
		return;
	}
	ajaxHandlerCall('?view=sendSpy&isMission=1&destinationCityId=' + spiesCity[cityToSearch].toString() + '&backgroundView=island&templateView=cityDetails');
	
	setTimeout(function(){
		var totalSpies = parseInt($("span[id^='js_spyCount_']").html());
		var bestWinRate = 0;
		
		var finalSpies = 0;
		var finalDecoys = 0;

		for(var spies = 1; spies < totalSpies; spies++){
			for(var decoys = 0; decoys < totalSpies-1; decoys++){
				if(spies + decoys <= totalSpies){
					var LPWinRate = parseInt($("#info_chance_text").html().slice(0, -1));
					var LPDetectRate = parseInt($("#info_risk_text").html().slice(0, -1));
					
					$("input[id^='agents']").val(spies);
					$("input[id^='agents']").click();
					$("input[id^='decoys']").val(decoys);
					$("input[id^='decoys']").click();
					
					var PWinRate = parseInt($("#info_chance_text").html().slice(0, -1));
					var PDetectRate = parseInt($("#info_risk_text").html().slice(0, -1));
					
					if(bestWinRate < PWinRate && LPDetectRate != PDetectRate){
						bestWinRate = PWinRate;
						finalSpies = spies;
						finalDecoys = decoys;
					}
						
				}
			}
		}

		$("input[id^='agents']").val(finalSpies);
		$("input[id^='agents']").click();
		$("input[id^='decoys']").val(finalDecoys);
		$("input[id^='decoys']").click();
		
		$("#missionForm").submit();
		
		setTimeout(function() {startSendSpies(cityToSearch+1);}, 3000);
	}, 5000);
}

startSendSpies(0);
$("body").append("<div style=\"width: 100%; height: 100%; position: fixed; background: rgba(0,0,0,0.5); z-index: 999999; text-align: center; color: #FFFFFF\" id=\"canceluserInput\">A script is running<br /><a href=\"index.php?view=city\">Abort script</a></div>");