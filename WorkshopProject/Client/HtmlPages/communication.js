var webSocketClient;
var msgHandler;
var startPageHandler;

//todo back button stack

function start() {
    const urlAddress = "ws://localhost:8080/wot"
    console.log("connecting to server " + urlAddress + "\n");
    webSocketClient = new WebSocket(urlAddress);
    //when connection opens
    webSocketClient.onopen = function (event) {
        console.log("connected \n");
    }
    webSocketClient.onclose = function (event) {
        console.log("disconnected \n");
    }
    webSocketClient.onmessage = function (event) {
        var msg = JSON.parse(event.data);
        switch (msg.type) {
            case "notification":
                alert(msg.data);
                break;
            default:
                msgHandler(msg);
        }
    }
    webSocketClient.onsend = function (event) { }
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



