declare var process;
const version = "goal v" + process.env.VERSION;
console.log(version);

self.addEventListener('install', (e:any)=>
{
    console.log("Service Worker: Starting Installation");
    const install = async ()=>
    {
        await caches.open(version).then(async cache=>
        {
            await cache.add('/');
            console.log("/ added to cache");
            await fetch('/').then(async resp=>
            {
                let index = await resp.text();
                var split = index.split("\"");
                for (let key of split)
                {
                    if (key.indexOf(".png") != -1)
                    {
                        console.log(key + " added to cache");
                        await cache.add(key);
                    }
                }
            });
            await fetch('parcel-manifest.json').then(async resp=>
            {
                let files = await resp.json();
                for (let key in files)
                {
                    await cache.add(files[key]);
                    console.log(files[key] + " added to cache");
                }
            });

            await fetch("/manifest.webmanifest").then(async (resp)=>
            {
                let manifest = await resp.json();
                for (let key in manifest.icons)
                {
                    await cache.add(manifest.icons[key].src);
                    console.log(manifest.icons[key].src + " added to cache");
                }
            });

            await cache.add("/manifest.webmanifest");
            console.log("/manifest.webmanifest" + " added to cache");
        });

        console.log("Service Worker: Installation Done");
    }

    e.waitUntil(install());
});


self.addEventListener('activate', async (e:any)=>
{
    for (let key of await caches.keys())
    {
        if (key != version)
        {
            await caches.delete(key);
            console.log("Service Worker: Deleted cache:" + key);
        }
    }
    
    console.log("Service Worker: Active");
});

self.addEventListener('fetch', async (e:any)=>
{
    let req = e.request as Request;
    if (req.method != "GET")
        return;

    e.respondWith((async ()=> {
        let resp = await caches.match(req);
        if (resp)
            return resp;
        else
            return fetch(req);
        
    })());
});