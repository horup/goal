/*fetch('parcel-manifest.json').then(resp=>
{
    console.log(v);
    self.addEventListener('install', (e:any)=>
    {
        caches.open('goal').then(cache=>
        {
            return cache.addAll(
                [
                    
                ]
            )
        });
        console.log("Service Worker Installed");
    });

    self.addEventListener('fetch', (e:any)=>
    {
        e.respondWith(caches.match(e.request));
    // console.log("Fetching " + JSON.stringify(e));
    });
});*/


self.addEventListener('install', async (e:any)=>
{
    console.log("Service Worker: Starting Installation");
    await caches.open('goal').then(async cache=>
    {
        await cache.add('/');
        console.log("/ added to cache");
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

});


self.addEventListener('fetch', async (e:any)=>
{
   e.respondWith(caches.match(e.request));
});