// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {

    // Notificaton Module (client signalR)

    //-------------------------

                                                                                                        // Configure the client signalR authentication with JWT.
    var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:55764/NotificationHub", { accessTokenFactory: () => localStorage.getItem("JWToken") }).build();

    connection.on("NewNotification", (receiverId) => {

        connection.invoke("UpdateNotifications", receiverId);

    });

    connection.on("updateNotificationsBar", (newNotificationsCount) => {

        $("#notification-count").html(newNotificationsCount);

    });

    connection.start().catch(err => console.log(err.toString()));


    //-----------------------

    $("#logoutButton").on("click", function () {

        localStorage.removeItem("JWToken");

    });

});