/*var maxDelay = 30;
var minDelay = 10;
var crew = false;
var mission = 1;

var extra = ":eq(" + (mission-1) + ")";
var timer_is_on = false;
var snd = new Audio("data:audio/wav;base64,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qJrx4KOQjahZxWbcZgztj2c49nKmkId44S71j0c8eV9yDK6uPRzx5X18eDvjvQ6yKo9ZSS6l//8elePK/Lf//IInrOF/FvDoADYAGBMGb7FtErm5MXMlmPAJQVgWta7Zx2go+8xJ0UiCb8LHHdftWyLJE0QIAIsI+UbXu67dZMjmgDGCGl1H+vpF4NSDckSIkk7Vd+sxEhBQMRU8j/12UIRhzSaUdQ+rQU5kGeFxm+hb1oh6pWWmv3uvmReDl0UnvtapVaIzo1jZbf/pD6ElLqSX+rUmOQNpJFa/r+sa4e/pBlAABoAAAAA3CUgShLdGIxsY7AUABPRrgCABdDuQ5GC7DqPQCgbbJUAoRSUj+NIEig0YfyWUho1VBBBA//uQZB4ABZx5zfMakeAAAAmwAAAAF5F3P0w9GtAAACfAAAAAwLhMDmAYWMgVEG1U0FIGCBgXBXAtfMH10000EEEEEECUBYln03TTTdNBDZopopYvrTTdNa325mImNg3TTPV9q3pmY0xoO6bv3r00y+IDGid/9aaaZTGMuj9mpu9Mpio1dXrr5HERTZSmqU36A3CumzN/9Robv/Xx4v9ijkSRSNLQhAWumap82WRSBUqXStV/YcS+XVLnSS+WLDroqArFkMEsAS+eWmrUzrO0oEmE40RlMZ5+ODIkAyKAGUwZ3mVKmcamcJnMW26MRPgUw6j+LkhyHGVGYjSUUKNpuJUQoOIAyDvEyG8S5yfK6dhZc0Tx1KI/gviKL6qvvFs1+bWtaz58uUNnryq6kt5RzOCkPWlVqVX2a/EEBUdU1KrXLf40GoiiFXK///qpoiDXrOgqDR38JB0bw7SoL+ZB9o1RCkQjQ2CBYZKd/+VJxZRRZlqSkKiws0WFxUyCwsKiMy7hUVFhIaCrNQsKkTIsLivwKKigsj8XYlwt/WKi2N4d//uQRCSAAjURNIHpMZBGYiaQPSYyAAABLAAAAAAAACWAAAAApUF/Mg+0aohSIRobBAsMlO//Kk4soosy1JSFRYWaLC4qZBYWFRGZdwqKiwkNBVmoWFSJkWFxX4FFRQWR+LsS4W/rFRb/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////VEFHAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAU291bmRib3kuZGUAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAMjAwNGh0dHA6Ly93d3cuc291bmRib3kuZGUAAAAAAAAAACU=");
var CaptchaID = 0;

function getBase64Image(img) {
	var canvas = document.createElement("canvas");
	canvas.width = img.width;
	canvas.height = img.height;
	var ctx = canvas.getContext("2d");
	ctx.drawImage(img, 0, 0);
	var dataURL = canvas.toDataURL("image/png");
	return dataURL.replace(/^data:image\/(png|jpg);base64,/, "");
}

function alertPlaces2(){
	var lis = document.getElementById("pirateHighscore").getElementsByTagName("li");
	var text = "";
	for(var i = 0; i < lis.length; i++) {
		text += "Place: " + lis[i].getElementsByClassName("place")[0].innerHTML.replace(/ /g, "").replace(".", "");
		text += ", Capture points: " + lis[i].getElementsByClassName("pirateBooty")[0].innerHTML;
		text += ", User: " + lis[i].getElementsByClassName("short_text80")[0].innerHTML;
		text += "<br />";
	}
	$("#pirateFortress .buildingDescription p").html("<div style=\"direction: ltr; text-align: left;\">" + text + "</div>");
}

function alertPlaces(){
	if(document.getElementById("pirateFortress") === null) {
		$("#js_CityPosition17Link").click();
		setTimeout(function() {
			if(document.getElementById("pirateFortress") === null)
				alert("You don't have pirate's fortress in this city!", "Error");
			else 
				alertPlaces2();
		}, 2000);
	} else 
		alertPlaces2();
}

function startPiracy(){
	alert("This option is no longer available. Use \"Massive piracy use\" from the launcher.");
	
	alert("Pssttt.. \"Ikariam+ Massive piracy use\" is better in any aspect, use it in the launcher!");
	if(!timer_is_on)
		return;
	if(document.getElementById("pirateFortress") === null)
		$("#js_CityPosition17Link").click();
	setTimeout(function() {
		if(document.getElementById("pirateFortress") === null)
			alert("You don't have pirate's fortress in this city!", "Error");
		else {
			$("#js_mainBoxHeaderTitle").html("<div style=\"direction: ltr;\">Scripted by Ikariam+</div>");
			setMission();
		}
	}, 2000);
	
}

function setMission(){
	if(!timer_is_on)
		return;
	if(crew)
		$("#js_tabBootyQuest").click();
	setTimeout(function() {
		$(".capture" + extra).click();
		setTimeout(function() {
			if($("#captcha").length || window,document.getElementById("captcha") != null){
				if(UserAPIKEY == ""){
					snd.play();
					alert("Enter captcha! When finish press F3.", "Alert");
					timer_is_on = false;
					return;
				} else {
					setTimeout(function() {
						var base64ImageDecoded = getBase64Image(document.getElementsByClassName("captchaImage")[0]);
						//callbackObj.log("Uploading captcha - " + base64ImageDecoded.substring(0,10));
						if(base64ImageDecoded != undefined && base64ImageDecoded != "") {
							uploadCaptcha(base64ImageDecoded, function(CaptchaId){
								if(CaptchaId == "Fail") {
									setTimeout(function() {
										//callbackObj.log("Failed uploading captcha, retring... ");
										startPiracy();
									}, 1000);
								}
								//callbackObj.log("Captcha id: " + CaptchaId.toString());
								CaptchaID = CaptchaId.toString();
								//callbackObj.log("CaptchaID: " + CaptchaID.toString());
								setTimeout(function() {
									getData(CaptchaID.toString(), function(CaptchaVal){
										//callbackObj.log("Captcha  " + CaptchaID.toString() + " value = " + CaptchaVal.toString());
										$("#captcha").val(CaptchaVal.toString());
										setTimeout(function() {
											$("#pirateCaptureBox .content .centerButton .button").click();
											setTimeout(function() {
												//callbackObj.log("Captcha *didnt* worked: " + ($("#captcha").length || window,document.getElementById("captcha") != null).toString());
												if($("#captcha").length || window,document.getElementById("captcha") != null)
													captchaWorked(CaptchaID.toString(), false);
												else
													captchaWorked(CaptchaID.toString(), true);
												setTimeout(function() {
													startPiracy();
												}, Math.floor((Math.random() * maxDelay * 1000) + minDelay * 1000));
											}, 15000);
										}, 3000);
									});
								}, 75000);
							});
							//document.write(image64bit);
						} else {
							setTimeout(function() {
								//callbackObj.log("Failed getting captcha, retring... ");
								startPiracy();
							}, 1000);
						}
					}, 4000);
				}
			} else if(crew && $(".pirateHeader .resources .capturePoints .value").html() != '0')
				setCrew();
			else
				setTimeout(function() {
					startPiracy();
				}, Math.floor((Math.random() * maxDelay * 1000) + minDelay * 1000));
		}, 1000);
	}, 2000);
}

function setCrew(){
	if(!timer_is_on)
		return;
	$("#js_tabCrew").click();
	setTimeout(function() {
		$("#CPToCrewSliderMax").click();
		$("#CPToCrewSubmit").click();
		setTimeout(function() {
			startPiracy();
		}, Math.floor((Math.random() * maxDelay * 1000) + minDelay * 1000));
	}, 2000);
}

$("html").keyup(function(event) {
	if(event.which == 113) { //F2
        stopCount();
        return false;
    } else if(event.which == 114) { //F3
		startCount();
		return false;
	} else if(event.which == 115) { //F4
        alertPlaces();
        return false;
    }
});

function startCount() {
	alert("The script is started", "Alert");
	timer_is_on = true;
	$("body").append("<div style=\"width: 100%; height: 100%; position: fixed; background: rgba(0,0,0,0.5); z-index: 999999; text-align: center; color: #FFFFFF\" id=\"canceluserInput\">A script is running<br /><a href=\"index.php?view=city\">Abort script</a></div>");
	startPiracy();
}

function stopCount() {
    alert("The script is aborted", "Alert");
	$("#canceluserInput").remove();
	timer_is_on = false;
}

function uploadCaptcha(Base64Img, callback){
	$.ajax({
		url : 'https://www.9kw.eu/index.cgi',
		async: true,
		cache: false,
		type : 'post',
		data :  {action: "usercaptchaupload", "file-upload-01": Base64Img, nomd5: 1, maxtimeout: 900, base64: 1, apikey: UserAPIKEY, time: $.now(), source: UserAPISource},
		dataType : 'text',
		success: function(data, textStatus, request){
			callback(data);
		}, error: function(xhr, textStatus, errorThrown){
			callback('Fail');
		}
	});
}

function getData(CID, callback){
	$.ajax({
		url : 'https://www.9kw.eu/index.cgi',
		async: true,
		cache: false,
		type : 'get',
		data :  {action: "usercaptchacorrectdata", id: CID, apikey: UserAPIKEY, source: UserAPISource},
		dataType : 'text',
		success: function(data, textStatus, request){
			callback(data);
		}, error: function(xhr, textStatus, errorThrown){
			callback('h6gab1d4');
		}
	});
}

function captchaWorked(CID, worked){
	num = 2
	if(worked)
		num = 1
	$.ajax({
		url : 'https://www.9kw.eu/index.cgi',
		async: true,
		cache: false,
		type : 'get',
		data :  {action: "usercaptchacorrectback", correct: num, id: CID, apikey: UserAPIKEY, source: UserAPISource},
		dataType : 'text'
	});
}*/
function startPiracy(){
	alert("This option is no longer available. Use \"Massive piracy use\" from the launcher.");
}