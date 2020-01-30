declare var process;
const version = "goal 1.0.3-" + process.env.BUILD;
console.log(version);

self.addEventListener('install', async (e:any)=>
{
    console.log("Service Worker: Starting Installation");
    await caches.open(version).then(async cache=>
    {
        await cache.add('/');
        console.log("/ added to cache");
        await fetch('/').then(async resp=>
        {
          
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

        await cache.add("favicon-32x32.147bdf61.png");
        await cache.add("favicon-16x16.7444da75.png");
    });

    console.log("Service Worker: Installation Done");
});


self.addEventListener('activate', async (e:any)=>
{
    console.log("Service Worker: Active");
   /* await caches.open("goal").then(async (cache)=>
    {
        caches.delete("goal");
    });*/
});

self.addEventListener('fetch', async (e:any)=>
{
   e.respondWith(caches.match(e.request));
});