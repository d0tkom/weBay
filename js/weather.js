var api = "b24a32fad4c07068e95f74be1b84acd6";
var xhr = new XMLHttpRequest();
navigator.geolocation.getCurrentPosition(showMap);
function showMap(position) {
    // Show a map centered at position
    lat = position.coords.latitude;
    lon = position.coords.longitude;
    //code has been rewritten to asynchronous HTTP request by following this example: https://developer.mozilla.org/en-US/docs/Web/API/XMLHttpRequest/Synchronous_and_Asynchronous_Requests
    xhr.open("GET", "http://api.openweathermap.org/data/2.5/weather?lat=" + lat + "&lon=" + lon + "&id=524901&APPID=" + api, true);
    xhr.onload = function (e) {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                var weather = JSON.parse(xhr.responseText);
                var temp = weather.main.temp - 273.15;
                var temp = temp.toFixed(0);
                var city = weather.name;
                document.getElementById("spanWeather").innerHTML = "The temperature in " + city + " is: " + temp + " °C. ";
            } else {
                console.error(xhr.statusText);
            }
        }
    };
    xhr.onerror = function (e) {
        console.error(xhr.statusText);
    };
    xhr.send(null);
}



