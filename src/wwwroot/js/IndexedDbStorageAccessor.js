export function initialize()
{
    let ticketSystemDb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

    ticketSystemDb.onupgradeneeded = function () 
    {
        let db = ticketSystemDb.result;

        const productsStore = db.createObjectStore("products", { keyPath: "id" });

        productsStore.createIndex("name", "name", { unique: false });
        
    }
}

export function set(collectionName, value)
{
    let ticketSystemDb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);

    ticketSystemDb.onsuccess = function ()
    {
        let transaction = ticketSystemDb.result.transaction(collectionName, "readwrite");
        let collection = transaction.objectStore(collectionName);
        collection.put(value);
    }
}

export async function get(collectionName, id)
{
    let request = new Promise((resolver) => {
        let ticketSystemDb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);
        
        ticketSystemDb.onsuccess = function ()
        {
            let transaction = ticketSystemDb.result.transaction(collectionName, "readonly");
            let collection = transaction.objectStore(collectionName);
            let result = collection.get(id);

            result.onsuccess = function (event) {
                resolver(event.result);
            }
        }
    });

    let result = await request;
    return result;
}

export async function getAll(collectionName) {
    return new Promise((resolve) => {
        let results = [];
        let ticketSystemDb = indexedDB.open(DATABASE_NAME, CURRENT_VERSION);
        
        ticketSystemDb.onsuccess = function() {
            let transaction = ticketSystemDb.result.transaction(collectionName, "readonly");
            let objectStore = transaction.objectStore(collectionName);
            
            objectStore.openCursor().onsuccess = function(event) {
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
        
        ticketSystemDb.onsuccess = function() {
            let transaction = ticketSystemDb.result.transaction(collectionName, "readwrite");
            let objectStore = transaction.objectStore(collectionName);
            let request = objectStore.delete(id);
            
            request.onsuccess = function() {
                resolve(true);
            };
            
            request.onerror = function(event) {
                reject(event.target.error);
            };
        };
        
        ticketSystemDb.onerror = function(event) {
            reject(event.target.error);
        };
    });
}

let CURRENT_VERSION = 1;
let DATABASE_NAME = "TrackItemSystemDB";