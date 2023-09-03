
var sidebarurl = "https://<%=CURRENT_DOMAIN%>"; // Change as required
var sidebartitle = "-"; // Change as required
var url = this.location;
var title = document.title;
function bookmarksite() {
    if (window.sidebar && window.sidebar.addPanel) { // Firefox
        window.sidebar.addPanel(sidebartitle, sidebarurl, "");
    } else if (document.all) { // IE Favorite
        window.external.AddFavorite(url, title);
    } else if (window.opera && window.print) {
        // do nothing
    } else if (navigator.appName == "Netscape") {
        alert("Ctrl(컨트롤)+D키를 눌러서 등록하세요.");
    }
}

function OpenComMenu(url, p_w, p_h) {
    pop_win("win", url, 0, 0, p_w, p_h, 0, 0, 1, 1, 1);
}