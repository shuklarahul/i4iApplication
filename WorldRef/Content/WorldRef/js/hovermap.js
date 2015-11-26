function FindLocation(latitude, longitude) {
            $("#dvMap").show();
            var latlng = new google.maps.LatLng(latitude, longitude);
            var mapOptions = {
                zoom: 6,
                center: latlng,
                mapTypeControl: true,
                scrollwheel: true,
                mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.DROPDOWN_MENU },
                navigationControl: true,
                navigationControlOptions: { style: google.maps.NavigationControlStyle.SMALL },
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                mapTypeControlOptions: {
                    style: google.maps.MapTypeControlStyle.DROPDOWN_MENU,
                    position: google.maps.ControlPosition.TOP_CENTER
                },

            };
            map = new google.maps.Map(document.getElementById('dvMap'), mapOptions);

            var companyMarker = new google.maps.Marker({
                position: latlng,
                map: map,
                flat: true
            });


        }
        function GetRouteLocation(id) {
            $.ajax({
                url: "/WorldRef/ShowProjectLocation",
                type: 'POST',
                data: JSON.stringify({ "id": id }),
                contentType: 'application/json; charset=utf-8',
                success: function (response) {
                    if (response == "" || response == null) {
                        return false;
                    }
                    else {
                        var location = response;
                        var geocoder = new google.maps.Geocoder();
                        var address = location;
                        geocoder.geocode({ 'address': address }, function (results, status) {
                            if (status == google.maps.GeocoderStatus.OK) {
                                var latitude = results[0].geometry.location.lat();
                                var longitude = results[0].geometry.location.lng();
                                FindLocation(latitude, longitude);

                                var foo = $("#abc1").html($("#dvMap"));
                                $.fancybox(foo);
                            } else {
                                alert("Request failed.")
                            }

                        });
                    }
                },
                error: function () {
                    alert("Failed! Please try again.");
                }
            });


        }