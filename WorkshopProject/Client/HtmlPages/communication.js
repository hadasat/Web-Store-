var webSocketClient;
var msgHandler;
var startPageHandler;
var signoutHandler;

//back button stack

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

   
    webSocketClient.onsend = function (event) {
        console.log("sent message "+event.data)
    }

    webSocketClient.onmessage = function (event) {
        console.log("received message: "+event.data);
        var msg = JSON.parse(event.data);
        switch (msg.type) {
            case "setId":
                sessionStorage.connectionId = msg.data;
                console.log("got new id "+msg.data);
                break;
            case "notification":
                alert(msg.data);
                break;
            default:
                handleMessage(msg);
        }
    }
}
start();

function handleMessage(res){
    var handle = responseHandlers[res.requestId];
    res.info==='success' ? handle.resolve(JSON.parse(res.data)) :
    alert(msg.data); 
}


var messageId=0;
var responseHandlers={};
function sendRequest (type,info,data){
    return new Promise(function(resolve, reject) {
    var newMsg = {
        id  : messageId,
        type : type,
        info : info,
        data : data
    };
    webSocketClient.send(JSON.stringify(newMsg));
    responseHandlers[messageId] = {resolve: resolve , reject: reject};
    messageId++;  
});
}



