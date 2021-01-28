// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {

    // Notificaton Module (client signalR)

    //-------------------------

    
    // Configure the client signalR authentication with JWT.

    //var hubUrl = "http://localhost:55764/NotificationHub";
    var hubUrl = "http://armaniousit-001-site13.ctempurl.com/NotificationHub";
    // var JWToken = localStorage.getItem("JWToken"); // we comment it, since we used cookie instead of localstorage.

    var token;
    var connection = new signalR.HubConnectionBuilder().withUrl(hubUrl, {
        accessTokenFactory: () => {
            $.ajax({
                type: 'Get',
                url: '/Account/GetJWTokenFromCookie',
                async: false,
                dataType: 'json',
                success: function (result) {
                    token = result;
                }
            });
            return token;
        }
    }).build();

    connection.on("NewNotification", (receiverId) => {

        connection.invoke("UpdateNotifications", receiverId);

    });

    connection.on("updateNotificationsBar", (newNotificationsCount) => {

        $("#notification-count").html(newNotificationsCount);

    });

    connection.start().catch(err => console.log(err.toString()));


    //-----------------------

    // we comment it, since we used cookie instead of localstorage.

    /*
    $("#logoutButton").on("click", function () {

        localStorage.removeItem("JWToken"); // that if we stored the token in localstorage, but since we stored it in cookie then we should expire the cookie

    });
    */

});