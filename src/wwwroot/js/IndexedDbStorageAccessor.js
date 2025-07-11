export function initialize() {
    let ticketSystemDb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

    ticketSystemDb.onupgradeneeded = function () {
        let db = ticketSystemDb.result;

        if (!db.objectStoreNames.contains('products')) {
            db.createObjectStore('products', { keyPath: 'id' });
        }

        if (!db.objectStoreNames.contains('types')) {
            db.createObjectStore('types', { keyPath: 'id' });
        }
    }
}

export function set(collectionName, value) {
    let ticketSystemDb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

    ticketSystemDb.onsuccess = function () {
        let transaction = ticketSystemDb.result.transaction(collectionName, "readwrite");
        let collection = transaction.objectStore(collectionName);
        collection.put(value);
    }
}

export async function get(collectionName, id) {
    return new Promise((resolve, reject) => {
        const ticketSystemDb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

        ticketSystemDb.onerror = function (event) {
            reject(event.target.error);
        };

        ticketSystemDb.onsuccess = function () {
            try {
                const db = ticketSystemDb.result;
                const transaction = db.transaction(collectionName, "readonly");
                const objectStore = transaction.objectStore(collectionName);
                const request = objectStore.get(id);

                request.onsuccess = function (event) {
                    resolve(event.target.result);
                };

                request.onerror = function (event) {
                    reject(event.target.error);
                };

                transaction.oncomplete = function () {
                    db.close();
                };
            } catch (error) {
                reject(error);
            }
        };
    });
}

export async function getAll(collectionName) {
    return new Promise((resolve) => {
        let results = [];
        let ticketSystemDb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

        ticketSystemDb.onsuccess = function () {
            let transaction = ticketSystemDb.result.transaction(collectionName, "readonly");
            let objectStore = transaction.objectStore(collectionName);

            objectStore.openCursor().onsuccess = function (event) {
                let cursor = event.target.result;

                if (cursor) {
                    results.push(cursor.value);
                    cursor.continue();
                } else {
                    resolve(results);
                }
            };
        }
    });
}

export async function del(collectionName, id) {
    return new Promise((resolve, reject) => {
        let ticketSystemDb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

        ticketSystemDb.onsuccess = function () {
            let transaction = ticketSystemDb.result.transaction(collectionName, "readwrite");
            let objectStore = transaction.objectStore(collectionName);
            let request = objectStore.delete(id);

            request.onsuccess = function () {
                resolve(true);
            };

            request.onerror = function (event) {
                reject(event.target.error);
            };
        };

        ticketSystemDb.onerror = function (event) {
            reject(event.target.error);
        };
    });
}

let CURRENT_VERSION = 3;
let DATABASE_NAME = "TrackItemSystemDB";