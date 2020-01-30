import * as React from 'react';
import * as ReactDom from 'react-dom';
declare var require;
declare var process;
console.log(process.env.PATH);

if('serviceWorker' in navigator) 
{
    navigator.serviceWorker
    .register('./sw.ts')
    .then(function() { console.log('Service Worker Registered'); });
}

const Index = ()=>
{


    return <div>hello world!</div>
}


ReactDom.render(<Index/>, document.getElementById("main"));