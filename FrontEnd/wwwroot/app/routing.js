var routes = [
    { from: /courses\/.*/,      to: "app/pages/course.html" },
    { from: /students\/.*/,   to: "app/pages/students.html" },
    { from: /takeselfie\/.*/,   to: "app/pages/takeselfie.html" },
    { from: /^$/,               to: "app/pages/courses.html" },
    { from: /.*/,               to: "app/pages/pagenotfound.html" }
];

var getRoute = function(input) {
    for (let i = 0; i < routes.length; i++) {
        const r = routes[i];
        const result = r.from.exec(input);
        if(result) {
            return r.to;
        }
    }

    return undefined;
}

function getAnchor() {
    var currentUrl = document.URL,
	urlParts   = currentUrl.split('#');
		
    return (urlParts.length > 1) ? urlParts[1] : "";
}

var route = function(route) {
    var actualRoute = getRoute(route);
    if(actualRoute == undefined) {
        return;
    }

    var app = document.querySelector(".app");
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function() {
        if(xhttp.readyState != 4) {
            return;
        }
    
        app.innerHTML = xhttp.responseText;
        var scripts = app.querySelectorAll("script");
        for (let i = 0; i < scripts.length; i++) {
            const script = scripts[i];
            if(script.src) {
                var imported = document.createElement('script');
                imported.src = script.src;
                document.head.appendChild(imported);
            } else {
                eval(script.innerHTML);
            }
        }
    }
    xhttp.open("GET", actualRoute, true);
    xhttp.send();
}

var urlHandler = function() {
    route(getAnchor());
}

window.onhashchange = urlHandler;

urlHandler();