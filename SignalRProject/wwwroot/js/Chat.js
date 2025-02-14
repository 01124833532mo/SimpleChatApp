document.addEventListener('DOMContentLoaded', function () {
    var userName = prompt("Please enter your name:");
    if (!userName) {
        alert("You must enter a name to join the chat.");
        return;
    }

    var messageInput = document.getElementById("messageInp");
    var sendMessageBtn = document.getElementById("sendMessageBtn");

    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/Chathub")
        .build();

    connection.start()
        .then(() => {
            console.log("Connection started.");
        })
        .catch(err => console.error("Connection error: ", err));

    sendMessageBtn.addEventListener('click', function () {
        var message = messageInput.value;
        if (message) {
            connection.invoke("Send", userName, message)
                .catch(err => console.error("Send error: ", err));
            displayMessage(userName, message, true);
            messageInput.value = '';
        }
    });

    connection.on("ReciveMessage", function (userName, message) {
        displayMessage(userName, message, false);
    });

    function displayMessage(userName, message, isSentByMe) {
        var messageContainer = document.createElement("div");
        messageContainer.classList.add("message");

        if (isSentByMe) {
            messageContainer.classList.add("sent");
        } else {
            messageContainer.classList.add("received");
        }

        var messageBubble = document.createElement("div");
        messageBubble.classList.add("message-bubble");
        messageBubble.textContent = message;

        var messageSender = document.createElement("div");
        messageSender.classList.add("message-sender");
        messageSender.textContent = userName;

        messageContainer.appendChild(messageSender);
        messageContainer.appendChild(messageBubble);
        document.getElementById("conversation").appendChild(messageContainer);

        document.getElementById("conversation").scrollTop = document.getElementById("conversation").scrollHeight;
    }
});