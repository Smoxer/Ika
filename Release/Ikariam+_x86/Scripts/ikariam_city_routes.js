var cityRoutes = $("#tabSendTransporter ul li");
for(route = 0; route < cityRoutes.length; route++){
	$("#" + cityRoutes[route].id).css({"height": "150", "text-align": "center"});
	$("#" + cityRoutes[route].id).append("<a class=\"button\" style=\"margin-top: 5px;\" href=\"javascript: void(0);\" onclick=\"ajaxHandlerCall('" + $("#" + cityRoutes[route].id + " a").attr("href") + "');\">Create route</a>");
}