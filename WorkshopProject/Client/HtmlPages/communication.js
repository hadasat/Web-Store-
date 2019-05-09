var webSocketClient;
var msgHandler;
var startPageHandler;

//todo back button stack

function start() {
    console.log("start");
    const urlAddress = "ws://localhost:8080/wot";
    console.log("connecting to server " + urlAddress + "\n");
    var isOldConnection= false;

    if (typeof(Storage) === "undefined") {
        alert("this web browser is supported");
    }
    if (sessionStorage.connectionId){
        isOldConnection = true;
        webSocketClient = new WebSocket(urlAddress,"old");
    }else{
        isOldConnection = false;
        webSocketClient = new WebSocket(urlAddress,"new");
    }


    //when connection opens
    webSocketClient.onopen = function (event) {
        if (isOldConnection) {
            var connectionId = Number(sessionStorage.connectionId);
            console.log("establosh old connection: " + connectionId);
            webSocketClient.send("old id is:"+connectionId);
        }
        console.log("connected \n");
    }

    webSocketClient.onclose = function (event) {
        console.log("disconnected \n");
    }

    webSocketClient.onmessage = function (event) {
        console.log("received message: "+event.data);
        var msg = JSON.parse(event.data);
        switch (msg.type) {
            case "setId":
                sessionStorage.connectionId = msg.id;
                console.log("got new id "+msg.id);
                break;
            case "notification":
                alert(msg.data);
                break;
            default:
                msgHandler(msg);
        }
    }
    webSocketClient.onsend = function (event) {
        console.log("sent message "+event.data)
    }
}
start();

function setUserId(){
    var localId = localStorage.getItem("userId");
    localId? 
        sendRequest("updateId","set" ,localId) :
        sendRequest("updateId","get",null);
}
setUserId();

//ofir - functions I use in the body scripts 
function updateHandler (newHandler){
    msgHandler = newHandler;
}

function sendRequest (type,info,data){
    var newMsg = {
        type : type,
        info : info,
        data : data
    };
    webSocketClient.send(JSON.stringify(newMsg));
}



