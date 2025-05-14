function requestNotificationPermission() {
    if (!("Notification" in window)) {
        return false;
    }

    if (Notification.permission !== "granted") {
        return Notification.requestPermission();
    }

    return true;
}

function showNotification(title, options) {
    if (!("Notification" in window)) {
        console.warn("This browser does not support notifications");
        return;
    }

    if (Notification.permission === "granted") {
        new Notification(title, options);
    } else if (Notification.permission !== "denied") {
        Notification.requestPermission().then(permission => {
            if (permission === "granted") {
                new Notification(title, options);
            }
        });
    }
}

// Make functions available to .NET
window.notification = {
    requestPermission: requestNotificationPermission,
    show: showNotification
};