// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function sendEmailCode(button, email, url, resendText) {
    if (!email) {
        return;
    }
    button.setAttribute('disabled', 'true');
    let countingDown = 60;
    var interval = setInterval(() => {
        countingDown--;
        button.innerText = countingDown + 's';
        if (countingDown === 0) {
            button.removeAttribute('disabled');
            button.innerText = resendText;
            clearInterval(interval);
        }
    }, 1000);
    requestSendCode(email, url);
}

function requestSendCode(email, url) {
    var csrfToken = getCookie("CSRF-TOKEN");
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (xhttp.readyState == XMLHttpRequest.DONE) {
            if (xhttp.status != 200) {
                alert('Error:Send code!');
            }
        }
    };
    xhttp.open('POST', url, true);
    xhttp.setRequestHeader("Content-type", "application/json");
    xhttp.setRequestHeader("X-CSRF-TOKEN", csrfToken);
    xhttp.send(JSON.stringify({ "email": email }));
}