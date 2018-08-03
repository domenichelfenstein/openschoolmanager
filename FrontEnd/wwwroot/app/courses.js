var loadCourses = () => {
    fetch("../api/courses")
        .then(r => r.json())
        .then(json => {
            var old = document.querySelector("ul.courses");
            if (old) {
                old.remove();
            }

            var ul = document.createElement("UL");
            ul.classList.add("courses");
            for (var i = 0; i < json.length; i++) {
                var li = document.createElement("LI");
                li.innerText = json[i].name;
                ul.appendChild(li);
            }
            document.body.appendChild(ul);
        });
}

loadCourses();

document.querySelector("button").onclick = () => {
    var input = document.querySelector("input");
    var name = input.value;
    postData("../api/courses", { name: name })
        .then(() => {
            input.value = "";
            loadCourses();
        });
}

const postData = (url = ``, data = {}) => {
    // Default options are marked with *
    return fetch(url, {
        method: "POST", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, cors, *same-origin
        cache: "no-cache", // *default, no-cache, reload, force-cache, only-if-cached
        credentials: "same-origin", // include, same-origin, *omit
        headers: {
            "Content-Type": "application/json; charset=utf-8",
            // "Content-Type": "application/x-www-form-urlencoded",
        },
        redirect: "follow", // manual, *follow, error
        referrer: "no-referrer", // no-referrer, *client
        body: JSON.stringify(data), // body data type must match "Content-Type" header
    })
        .then(response => response.json()) // parses response to JSON
        .catch(error => console.error(`Fetch Error =\n`, error));
};