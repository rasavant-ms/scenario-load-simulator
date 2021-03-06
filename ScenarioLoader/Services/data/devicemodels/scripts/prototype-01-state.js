// Copyright (c) Microsoft. All rights reserved.

/*global log*/
/*jslint node: true*/

"use strict";

var center_latitude = 47.612514;
var center_longitude = -122.204184;

// Default state
var state = {
    online: true,
    temperature: 65.0,
    temperature_unit: "F",
    pressure: 150.0,
    pressure_unit: "psig",
    latitude: center_latitude,
    longitude: center_longitude,
    moving: false
};

/**
 * Restore the global state using data from the previous iteration.
 *
 * @param previousState The output of main() from the previous iteration
 */
function restoreState(previousState) {
    // If the previous state is null, force a default state
    if (previousState !== undefined && previousState !== null) {
        state = previousState;
    } else {
        log("Using default state");
    }
}

/**
 * Simple formula generating a random value around the average
 * in between min and max
 */
function vary(avg, percentage, min, max) {
    var value = avg * (1 + ((percentage / 100) * (2 * Math.random() - 1)));
    value = Math.max(value, min);
    value = Math.min(value, max);
    return value;
}

/**
 * Generate a random geolocation at some distance (in miles)
 * from a given location
 */
function varylocation(latitude, longitude, distance) {
    // Convert to meters, use Earth radius, convert to radians
    var radians = (distance * 1609.344 / 6378137) * (180 / Math.PI);
    return {
        latitude: latitude + radians,
        longitude: longitude + radians / Math.cos(latitude * Math.PI / 180)
    };
}

/**
 * Entry point function called by the simulation engine.
 *
 * @param context        The context contains current time, device model and id
 * @param previousState  The device state since the last iteration
 */
/*jslint unparam: true*/
function main(context, previousState) {

    // Restore the global state before generating the new telemetry, so that
    // the telemetry can apply changes using the previous function state.
    restoreState(previousState);

    // 65 +/- 1%,  Min 35, Max 100
    state.temperature = vary(65, 1, 35, 100);

    // 150 +/- 5%,  Min 50, Max 300
    state.pressure = vary(150, 5, 50, 300);

    // 0.1 miles around some location
    if (state.moving) {
        var coords = varylocation(center_latitude, center_longitude, 0.1);
        state.latitude = coords.latitude;
        state.longitude = coords.longitude;
    }

    return state;
}
