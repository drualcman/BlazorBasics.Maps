const DEFAULT_SETTINGS = {
    enableHighAccuracy: true,
    timeoutMilliseconds: 15000,
    maximumAgeMilliseconds: 0,
    fallbackToLowAccuracy: true,
    lowAccuracyTimeoutMilliseconds: 10000,
    lowAccuracyMaximumAgeMilliseconds: 60000
};

// GeolocationPositionError codes, see https://developer.mozilla.org/docs/Web/API/GeolocationPositionError
const BROWSER_PERMISSION_DENIED = 1;
const BROWSER_POSITION_UNAVAILABLE = 2;
const BROWSER_TIMEOUT = 3;

// Must stay in step with GeolocationFailureReason
const REASON_NONE = 0;
const REASON_PERMISSION_DENIED = 1;
const REASON_POSITION_UNAVAILABLE = 2;
const REASON_TIMEOUT = 3;
const REASON_NOT_SUPPORTED = 4;
const REASON_INSECURE_CONTEXT = 5;
const REASON_UNKNOWN = 6;

// The browser is free to answer neither callback, and a getCurrentPosition without a
// timeout then waits for a fix that never arrives. Every request carries a timeout, and
// this is how long the watchdog waits on top of it before giving up on the browser.
const WATCHDOG_MARGIN_MILLISECONDS = 2000;

function failure(reason, message) {
    return {
        isSuccess: false,
        latitude: 0,
        longitude: 0,
        accuracy: 0,
        failureReason: reason,
        failureMessage: message
    };
}

function success(position) {
    return {
        isSuccess: true,
        latitude: position.coords.latitude,
        longitude: position.coords.longitude,
        accuracy: typeof position.coords.accuracy === 'number' ? position.coords.accuracy : 0,
        failureReason: REASON_NONE,
        failureMessage: null
    };
}

function fromBrowserError(error) {
    if (!error) return failure(REASON_UNKNOWN, 'The geolocation api failed without saying why.');

    switch (error.code) {
        case BROWSER_PERMISSION_DENIED:
            return failure(REASON_PERMISSION_DENIED,
                error.message || 'The user or the browser denied the request for geolocation.');
        case BROWSER_POSITION_UNAVAILABLE:
            return failure(REASON_POSITION_UNAVAILABLE,
                error.message || 'The location information is unavailable.');
        case BROWSER_TIMEOUT:
            return failure(REASON_TIMEOUT,
                error.message || 'The request to get the user location timed out.');
        default:
            return failure(REASON_UNKNOWN, error.message || 'An unknown geolocation error occurred.');
    }
}

function isAvailable() {
    if (typeof navigator === 'undefined') {
        return failure(REASON_NOT_SUPPORTED, 'There is no navigator in this context.');
    }
    if (typeof window !== 'undefined' && window.isSecureContext === false) {
        return failure(REASON_INSECURE_CONTEXT,
            'The page is not a secure context, the browser hides the geolocation api. Serve it over https or from localhost.');
    }
    if (!navigator.geolocation || typeof navigator.geolocation.getCurrentPosition !== 'function') {
        return failure(REASON_NOT_SUPPORTED,
            'This browser does not expose the geolocation api. Inside an iframe it also needs allow="geolocation", and a Permissions-Policy header can take it away.');
    }
    return null;
}

function requestOnce(enableHighAccuracy, timeoutMilliseconds, maximumAgeMilliseconds) {
    return new Promise(resolve => {
        let isSettled = false;
        let watchdog = 0;

        const finish = result => {
            if (isSettled) return;
            isSettled = true;
            clearTimeout(watchdog);
            resolve(result);
        };

        watchdog = setTimeout(
            () => finish(failure(REASON_TIMEOUT,
                `The geolocation api did not answer within ${timeoutMilliseconds} ms.`)),
            timeoutMilliseconds + WATCHDOG_MARGIN_MILLISECONDS);

        try {
            navigator.geolocation.getCurrentPosition(
                position => finish(success(position)),
                error => finish(fromBrowserError(error)),
                {
                    enableHighAccuracy: enableHighAccuracy,
                    timeout: timeoutMilliseconds,
                    maximumAge: maximumAgeMilliseconds
                });
        } catch (error) {
            finish(failure(REASON_UNKNOWN, error && error.message ? error.message : String(error)));
        }
    });
}

// A high accuracy request is a gps request, and a gps fix indoors is a fix that does not
// arrive. When it is the timeout or a missing fix that stopped it, the network provider
// is asked instead: a coarse position is worth more than no position at all.
function isWorthRetryingWithLowAccuracy(result) {
    return result.failureReason === REASON_TIMEOUT
        || result.failureReason === REASON_POSITION_UNAVAILABLE;
}

async function readPosition(options) {
    const settings = Object.assign({}, DEFAULT_SETTINGS, options || {});

    const unavailable = isAvailable();
    if (unavailable) return unavailable;

    const first = await requestOnce(
        settings.enableHighAccuracy,
        settings.timeoutMilliseconds,
        settings.maximumAgeMilliseconds);

    if (first.isSuccess) return first;
    if (!settings.enableHighAccuracy || !settings.fallbackToLowAccuracy) return first;
    if (!isWorthRetryingWithLowAccuracy(first)) return first;

    const second = await requestOnce(
        false,
        settings.lowAccuracyTimeoutMilliseconds,
        settings.lowAccuracyMaximumAgeMilliseconds);

    return second.isSuccess ? second : first;
}

// navigator.permissions is missing on Safari before 16, and reading query off it throws
// before any promise exists, so the whole call is guarded and not only its rejection.
async function readPermissionState() {
    try {
        if (typeof navigator === 'undefined'
            || !navigator.permissions
            || typeof navigator.permissions.query !== 'function') {
            return 'unknown';
        }

        const status = await navigator.permissions.query({ name: 'geolocation' });
        return status && status.state ? status.state : 'unknown';
    } catch (error) {
        return 'unknown';
    }
}

function getPositionAsync() {
    return readPosition().then(result => {
        if (!result.isSuccess) throw new Error(result.failureMessage);
        return { latitude: result.latitude, longitude: result.longitude };
    });
}

function checkGeolocationPermission() {
    return readPermissionState().then(state => state === 'granted');
}

export { readPosition, readPermissionState, getPositionAsync, checkGeolocationPermission }
