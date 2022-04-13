let map;
let dotNet;

let legalIcon = new L.Icon({
    iconUrl: '../images/legal-event.png',
    iconSize: [35, 35],
    iconAnchor: [12, 12]
});

let illegalIcon = new L.Icon({
    iconUrl: '../images/illegal-event.png',
    iconSize: [35, 35],
    iconAnchor: [12, 12]
});

let newIcon = new L.Icon({
    iconUrl: '../images/alert-event.png',
    iconSize: [35, 35],
    iconAnchor: [12, 12],
    className: 'blinking'
});

window.leafletJsFunctions = {
    initialize: function (dotnetHelper) {
        dotNet = dotnetHelper;
        map = L.map('map', {maxBoundsViscosity: 0}).setView([40.866667, 4.566667], 3);

        L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=pk.eyJ1IjoiZm11Y25qYWsiLCJhIjoiY2t4ZjdscTJlMHYxaTJ1cXZkN2Q3NWx3ZCJ9.wWSCOAZCNdWoYgfetf5-tw', {
            maxZoom: 15,
            minZoom: 3,
            id: 'mapbox/dark-v10',
            tileSize: 512,
            zoomOffset: -1
        }).addTo(map);

        let southWest = L.latLng(-80.98155760646617, -185), northEast = L.latLng(89.99346179538875, 190);
        let bounds = L.latLngBounds(southWest, northEast);

        map.setMaxBounds(bounds);
        map.on('drag', function () {
            map.panInsideBounds(bounds, {animate: false});
        });

        map.on('click', function (e) {
            return dotnetHelper.invokeMethodAsync('ShowLatLonAsync', e.latlng.toString());
        });
    }
};

let mapMarkers = []

function loadLatestEvents(eventsFromDb) {
    for (let i = 0; i < mapMarkers.length; i++) {
        map.removeLayer(mapMarkers[i]);
    }

    eventsFromDb.forEach(item => {
        if (item.isIllegal && !item.isDismissed) {
            let marker = L.marker([item.latitude, item.longitude], {icon: newIcon}).addTo(map).on('click', () => onMarkerClick(item));
            mapMarkers.push(marker);
        } else if (!item.isIllegal && !item.isDismissed) {
            let marker = L.marker([item.latitude, item.longitude], {icon: newIcon}).addTo(map).on('click', () => onMarkerClick(item));
            mapMarkers.push(marker);
        } else if (item.isIllegal && item.isDismissed) {
            let marker = L.marker([item.latitude, item.longitude], {icon: illegalIcon}).addTo(map).on('click', () => onMarkerClick(item));
            mapMarkers.push(marker);
        } else if (!item.isIllegal && item.isDismissed) {
            let marker = L.marker([item.latitude, item.longitude], {icon: legalIcon}).addTo(map).on('click', () => onMarkerClick(item));
            mapMarkers.push(marker);
        }
    });
}

function loadNewEvent(newEvent) {
    let marker = L.marker([newEvent.latitude, newEvent.longitude], {icon: newIcon}).addTo(map).on('click', () => onMarkerClick(newEvent));
    mapMarkers.push(marker);
    map.flyTo([newEvent.latitude, newEvent.longitude], 5);
}

function loadExistingEvent(existingEvent) {
    if (existingEvent.isIllegal) {
        L.marker([existingEvent.latitude, existingEvent.longitude], {icon: illegalIcon}).addTo(map).on('click', () => onMarkerClick(existingEvent));
    } else {
        L.marker([existingEvent.latitude, existingEvent.longitude], {icon: legalIcon}).addTo(map).on('click', () => onMarkerClick(existingEvent));
    }
    map.flyTo([existingEvent.latitude, existingEvent.longitude], 5);
}

function flyToEvent(latitude, longitude) {
    map.flyTo([latitude, longitude], 5);
}

function onMarkerClick(e) {
    return dotNet.invokeMethodAsync('OpenEventDetailsModalAsync', e);
}