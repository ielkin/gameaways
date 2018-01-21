//var map;
//var markersArray = [];
//var defaultLat = $("#DefaultLat").val();
//var defaultLng = $("#DefaultLng").val();
//var defaultPointId = $("#DefaultPointId").val();
//var defaultZoom = 6,
//    cityLevelZoom = 11,
//    buildingLevelZoom = 16;

////$(document).ready(function onDocumentReady() {
////    initialize();

////    if (defaultPointId != 0) {
////        defaultZoom = buildingLevelZoom;
////        $(".point-details").load("/RecyclePoints/RecyclePointDetails/" + defaultPointId);
////    }
////    else {
////        if (navigator.geolocation) {
////            navigator.geolocation.getCurrentPosition(function (position) {
////                defaultZoom = cityLevelZoom;

////                defaultLat = Number(position.coords.latitude);
////                defaultLng = Number(position.coords.longitude);

////                map.setCenter(new google.maps.LatLng(defaultLat, defaultLng));
////                map.setZoom(cityLevelZoom);
////            },
////            function (error) {
////                switch (error.code) {
////                    //case error.PERMISSION_DENIED:
////                    //    alert("User denied the request for Geolocation.");
////                    //    break;
////                    //case error.POSITION_UNAVAILABLE:
////                    //    alert("Location information is unavailable.");
////                    //    break;
////                    //case error.TIMEOUT:
////                    //    alert("The request to get user location timed out.");
////                    //    break;
////                    //case error.UNKNOWN_ERROR:
////                    //    alert("An unknown error occurred.");
////                    //    break;
////                }
////            });
////        }
////    }
////});

//function initialize() {
//    var mapProp = {
//        center: new google.maps.LatLng(defaultLat, defaultLng),
//        zoom: defaultZoom,
//        mapTypeId: google.maps.MapTypeId.ROADMAP,
//    };

//    map = new google.maps.Map(document.getElementById("googleMap"), mapProp);

//    $.getJSON("/RecyclePoints/GetListOfRecyclePoints", null, function (data) {
//        for (var i = 0; i < data.length; i++) {
//            var marker = new google.maps.Marker({
//                position: new google.maps.LatLng(data[i].Lat, data[i].Lng),
//                info: "<div>" + data[i].PointName + "</br>" + data[i].Street + "<span id='point-id' style='display: none'>" + data[i].ID + "</span></div>",
//                title: data[i].PointName,
//                icon: "/Content/Images/maps-pin-green.png",
//                map: map
//            });

//            markersArray[i] = marker;

//            var infowindow = new google.maps.InfoWindow({ maxWidth: 300 });

//            google.maps.event.addListener(marker, 'click', function () {
//                $(".point-details").load("/RecyclePoints/RecyclePointDetails/" + Number($(this.info).find("#point-id").text()));
//                infowindow.setContent(this.info);
//                infowindow.open(map, this);
//            });
//        }
//    });
//}

//$(".filter-container>.accept-button").click(function () {
//    $(this).toggleClass("button-selected");
//    clearOverlays();

//    var acceptedMaterials = new Array();

//    $(".button-selected").each(function (index, element) {
//        acceptedMaterials[index] = Number($(element).attr("data-val"));
//    });

//    var filter = {
//        acceptedMaterials: acceptedMaterials,
//        searchString: $("#searchString").val()
//    }

//    $.ajax({
//        url: '/RecyclePoints/GetListOfRecyclePoints',
//        data: filter,
//        traditional: true,
//        success: function (result) {
//            for (var i = 0; i < result.length; i++) {
//                for (var j = 0; j < markersArray.length; j++) {
//                    if (result[i].Name == markersArray[j].title) {
//                        markersArray[j].setVisible(true)
//                    }
//                }
//            }
//        }
//    });
//});

//$(".filter-container>.filter-search").click(function () {
//    $(this).toggleClass("button-selected");
//    clearOverlays();

//    var acceptedMaterials = new Array();

//    $(".button-selected").each(function (index, element) {
//        acceptedMaterials[index] = Number($(element).attr("data-val"));
//    });

//    var filter = {
//        acceptedMaterials: acceptedMaterials,
//        searchString: $("#searchString").val()
//    }

//    $.getJSON("/RecyclePoints/GetListOfRecyclePoints", filter, function (data) {
//        for (var i = 0; i < data.length; i++) {
//            for (var j = 0; j < markersArray.length; j++) {
//                if (data[i].PointName == markersArray[j].title) {
//                    markersArray[j].setVisible(true)
//                }
//            }
//        }
//    });
//});

//$("#searchString").keypress(function (event) {
//    if (event.charCode == 13) {
//        $(".filter-container>.filter-search").trigger("click");
//    }
//});

//$("#searchString").keyup(function () {
//    if ($("#searchString").val() != "") {
//        $(".clear-search").css("top", $(".filter-search").position().top + 8);
//        $(".clear-search").css("left", $(".filter-search").position().left - 55);
//        $(".clear-search").show();
//    }
//    else {
//        $(".clear-search").hide();
//    }
//});

//$(".clear-search").click(function () {
//    $("#searchString").val("");
//    $(".clear-search").hide();
//    $("#searchString").focus();
//    $(".filter-container>.filter-search").trigger("click");
//});

//clearOverlays = function () {
//    for (var i = 0; i < markersArray.length; i++) {
//        markersArray[i].setVisible(false);
//    }
//}