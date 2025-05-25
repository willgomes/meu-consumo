export function requestPermission() {
    if (!("Notification" in window)) {
        console.error("This browser does not support desktop notification");
        return false;
    }

    return Notification.requestPermission().then(permission => {
        return permission === "granted";
    });
}

export function show(title, options) {
    if (!("Notification" in window)) {
        console.error("This browser does not support desktop notification");
        return;
    }

    if (Notification.permission === "granted") {
        new Notification(title, options);
    }
    else if (Notification.permission !== "denied") {
        Notification.requestPermission().then(permission => {
            if (permission === "granted") {
                new Notification(title, options);
            }
        });
    }
}